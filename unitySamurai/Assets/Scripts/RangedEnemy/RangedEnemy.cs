using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemy : MonoBehaviour
{
    public GameObject bullet;

    private NavMeshAgent agent;
    private GameObject target;
    private Transform playerTransform; // Reference to the player's transform
    private float nextDestinationTime = 0f;
    private float destinationCooldown = 5f; // Adjust this cooldown time as needed
    private float playerDetectionRange = 10f;
    public GameObject orientation;
    public GameObject shootpoint;
    private GameObject rifle;
    private float health = 100f;
    private Animator animator;
    private float targetTime = 1f;
    private float currentTimer = 0f;



    private void Start()
    {
        rifle = GameObject.FindWithTag("EnemyRifle");
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindWithTag("Player");
        animator = gameObject.GetComponent<Animator>();
        playerTransform = target.transform; // Assuming the target is the player's GameObject
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer < playerDetectionRange)
        {
            agent.isStopped = true;
            Shoot();
            LookAtPlayer(); // Look at the player
        }
        else if (Time.time >= nextDestinationTime)
        {
            agent.isStopped = false;
            MoveToRandomPoint();
            nextDestinationTime = Time.time + destinationCooldown;
        }
    }

    void MoveToRandomPoint()
    {
        Vector3 randomDestination = Random.insideUnitSphere * 15f;
        randomDestination += transform.position;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomDestination, out hit, 15f, NavMesh.AllAreas);

        // Set the destination for the NavMeshAgent
        agent.SetDestination(hit.position);
    }

    void LookAtPlayer()
    {
        Vector3 directionToPlayer = target.transform.position - transform.position;

        float angle = Vector3.Angle(shootpoint.transform.forward, directionToPlayer);

        Quaternion rotationToPlayer = Quaternion.LookRotation(directionToPlayer, transform.up);

        Quaternion additionalRotation = Quaternion.AngleAxis(angle + 24, transform.up);

        Quaternion finalRotation = rotationToPlayer * additionalRotation;

        transform.rotation = Quaternion.Slerp(transform.rotation, finalRotation, Time.deltaTime * 20f);
    }



    public void TakeDamage(float damage)
    {
        if(health <= 0 )
        {
            Die();
        }
        else
        {
            animator.SetTrigger("isHit");

            health -= damage;
        }
    }



    public void Die()
    {
        animator.SetTrigger("isDead");
        Destroy(gameObject, 3f);
    }


    public void Shoot()
    {

        if (currentTimer > targetTime  && !animator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
        {
            GameObject bullets = Instantiate(bullet, shootpoint.transform.position, rifle.transform.rotation);
            bullets.GetComponent<testBullet>().setInitialDirection((playerTransform.position - shootpoint.transform.position).normalized);
            currentTimer = 0f;
        }
        else
        {
            currentTimer += Time.deltaTime;
        }
        
    }

}
