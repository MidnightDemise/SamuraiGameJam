using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rane : MonoBehaviour
{

    private NavMeshAgent agent;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
        bool isWalking = agent.velocity.magnitude > 0.5f ? true : false;

        animator.SetBool("isWalking", isWalking);
    }
}
