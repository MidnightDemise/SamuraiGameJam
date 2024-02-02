using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] float health = 100;
    [SerializeField] GameObject hitVFX;
    [SerializeField] GameObject ragdoll;
    private GameObject shield;
    private AssetManager assetManager;

    [Header("Combat")]
    [SerializeField] float attackCD = 3f;
    [SerializeField] float attackRange = 1f;
    [SerializeField] float aggroRange = 4f;

    GameObject player;
    NavMeshAgent agent;
    Animator animator;
    Animator playerAnimator;
    float timePassed;
    float newDestinationCD = 0.5f;

    void Start()
    {
        assetManager = GameObject.Find("Assets").GetComponent<AssetManager>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
        playerAnimator = player.GetComponent<Animator>();

        shield = assetManager.Shield;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(animator);
       
        animator.SetFloat("Speed", agent.velocity.magnitude / agent.speed);

        if (player == null)
        {
            return;
        }

        if (timePassed >= attackCD)
        {
            if (Vector3.Distance(player.transform.position, transform.position) <= 3f)
            {
                animator.SetTrigger("Attack");

                if (shield.activeSelf)
                {
                    playerAnimator.SetTrigger("ShieldHit");
                }
                else
                {
                    playerAnimator.SetTrigger("hit");
                    player.GetComponent<PlayerController>().TakeDamage(3f);

                }
                
                timePassed = 0;
            }
        }
        timePassed += Time.deltaTime;

        if (newDestinationCD <= 0 && Vector3.Distance(player.transform.position, transform.position) <= aggroRange)
        {
            newDestinationCD = 0.5f;
            agent.SetDestination(player.transform.position + new Vector3( 0,0,1));
        }
        newDestinationCD -= Time.deltaTime;
        transform.LookAt(player.transform);
    }

 

    void Die()
    {
        animator.SetTrigger("Dead");
        Destroy(this.gameObject,4f);
    }

    public void TakeDamage(float damageAmount)
    {
        

        if (health <= 0)
        {
            Die();
        }
        else
        {
            health -= damageAmount;
            animator.SetTrigger("Damage");

        }
    }
    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, aggroRange);
    }

}
