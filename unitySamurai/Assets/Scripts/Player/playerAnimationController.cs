using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class playerAnimationController : MonoBehaviour
{


    private float cooldowntime = 2f;
    private float nextFireTime = 0f;
    public static int noOfClicks = 0;
    private float lastClickedTime = 0f;
    float maxComboDelay = 1f;




    public GameObject rifle;
    public GameObject shield;
    public GameObject sword;
    private CharacterController characterController;


    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        bool _isGrounded = PlayerController._groundedPlayer;



        bool inputKeyPresses = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);
        bool isIdleShooting = rifle.activeSelf;
        bool isIdleShield = shield.activeSelf;
        bool isIdleSword = sword.activeSelf;

        bool isRunning = Input.GetKey(KeyCode.LeftShift) && inputKeyPresses;
        bool isJumping = Input.GetButtonDown("Jump") && !_isGrounded  && (isRunning || !inputKeyPresses || inputKeyPresses);
        bool isWalkShooting = (rifle.activeSelf && inputKeyPresses);
        bool isWalkSword = (sword.activeSelf && inputKeyPresses);

        bool isDodging = Input.GetKeyDown(KeyCode.E) && PlayerController.canDodge;

        animator.SetBool("isWalkSword", isWalkSword);
        animator.SetBool("isIdleSword", isIdleSword);
        animator.SetBool("isWalkShooting", isWalkShooting);
        animator.SetBool("isIdleShooting", isIdleShooting);
        animator.SetBool("isWalking", inputKeyPresses);
        animator.SetBool("isIdleShield", isIdleShield);
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isDodging", isDodging);



        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("hit1"))
        {
            animator.SetBool("hit1", false);
        }

        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("hit2"))
        {
            animator.SetBool("hit2", false);
        }

        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("hit3"))
        {
            animator.SetBool("hit3", false);
            noOfClicks = 0;
        }


        if (Time.time - lastClickedTime > maxComboDelay)
        {
            noOfClicks = 0;
        }

        if(Time.time > nextFireTime )
        {
            if(Input.GetMouseButtonDown(0))
            {
                onClick();
            }
        }
        // Reset other parameters to false when setting a specific one to true





    }


    void onClick()
    {
        lastClickedTime = Time.time;
        noOfClicks++;

        if(noOfClicks == 1 )
        {
            animator.SetBool("hit1", true);

        }
        noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);

        if(noOfClicks >= 2 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("hit1"))
        {
            if (SwordScript.enemy != null && SwordScript.enemy.CompareTag("MeleeEnemy")) SwordScript.enemy.GetComponent<EnemyScript>().TakeDamage(20f);
            else if (SwordScript.enemy != null && SwordScript.enemy.CompareTag("RangedEnemy")) SwordScript.enemy.GetComponent<RangedEnemy>().TakeDamage(10f);
            animator.SetBool("hit1", false);
            animator.SetBool("hit2", true);
        }


        if (noOfClicks >= 3 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("hit2"))
        {
            if (SwordScript.enemy != null && SwordScript.enemy.CompareTag("MeleeEnemy")) SwordScript.enemy.GetComponent<EnemyScript>().TakeDamage(20f);
            else if (SwordScript.enemy != null && SwordScript.enemy.CompareTag("RangedEnemy")) SwordScript.enemy.GetComponent<RangedEnemy>().TakeDamage(10f);

            animator.SetBool("hit2", false);
            animator.SetBool("hit3", true);

            noOfClicks = 0;
        }

    }
}
