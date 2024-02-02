using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class SwordScript : MonoBehaviour
{

    private float damage = 0.2f;
    private Animator animator;
    private AnimatorStateInfo lastInfoState;

    public GameObject LegPoint;

    public static bool canDamage = false;
    public static GameObject enemy;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GameObject.FindWithTag("Player").gameObject.GetComponent<Animator>();
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position, transform.up, out RaycastHit hit,5f) && !lastInfoState.Equals(animator.GetCurrentAnimatorStateInfo(0))
)
        {
            // Check if the hit object has the EnemyScript component
            EnemyScript enemyScript = hit.collider.GetComponent<EnemyScript>();
            RangedEnemy rangedEnemy = hit.collider.GetComponent<RangedEnemy>();
            // If the hit object has the EnemyScript component, apply damage
            if (enemyScript != null && Input.GetMouseButtonDown(0))
            {
                enemy = enemyScript.gameObject;
                canDamage = true;
            }
            else if(rangedEnemy != null && Input.GetMouseButtonDown(0))
            {
                enemy = rangedEnemy.gameObject;
                canDamage = true;
            }
        }
    }


    
}
