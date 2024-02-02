using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private float health = 100;
    private float currentTime;
    public static bool canDodge = false;

    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private GameObject Shield;
    [SerializeField]
    private GameObject rifle;
    [SerializeField]
    private GameObject isGroundedSphere;

    private CharacterController _controller;
    private bool isWalled;

    [SerializeField]
    private float _playerSpeed = 5f;

    [SerializeField]
    private float _rotationSpeed = 10f;

    [SerializeField]
    private Camera _followCamera;

    private Vector3 _playerVelocity;
    public static bool _groundedPlayer;

    [SerializeField]
    private float _jumpHeight = 1.0f;
    [SerializeField]
    private float _gravityValue = -9.81f;


    private Vector3 movementInput;
    private Vector3 movementDirection;
    private float oldspeed;

    [SerializeField]
    private bool disableInput;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
        //CutsceneManager.Instance.OnCutScenePlaying += Instance_OnCutScenePlaying;
        //CutsceneManager.Instance.OnCutSceneStop += Instance_OnCutSceneStop;
        oldspeed = _playerSpeed;
    }

    private void Update()
    {
        Debug.Log(health);
        Movement();
        //if (CanDodge(gameObject.transform,bullet.transform)) Dodge();

        CheckBulletDistance();

        if (canDodge) Dodge();

       
    }

    void Movement()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            _playerSpeed = Mathf.Lerp(_playerSpeed, 5f, 5f * Time.deltaTime);
        }
        else
        {
            _playerSpeed = Mathf.Lerp(_playerSpeed, oldspeed, 5f * Time.deltaTime);

        }
        Collider[] colliders = Physics.OverlapSphere(isGroundedSphere.transform.position, 0.3f, LayerMask.GetMask("Ground"));



        if (colliders.Length > 0)
        {
            _groundedPlayer = true;
            _playerVelocity.y = 0f;
        }


        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
       
        if(rifle.activeSelf)
        {

            movementInput = Quaternion.Euler(0, _followCamera.transform.eulerAngles.y, 0) * new Vector3(horizontalInput, 0, verticalInput);
            movementDirection = movementInput.normalized;
        }
        else
        {
            movementInput = Quaternion.Euler(0, _followCamera.transform.eulerAngles.y, 0) * new Vector3(horizontalInput, 0, verticalInput);
            movementDirection = movementInput.normalized;

        }






        _controller.Move(movementDirection * 3.5f * Time.fixedDeltaTime);

        if (movementDirection != Vector3.zero)
        {
            Quaternion desiredRotation = Quaternion.LookRotation(movementDirection, Vector3.up);

            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, _rotationSpeed * Time.deltaTime);
        }

        if (Input.GetButtonDown("Jump") && _groundedPlayer)
        {
            _playerVelocity.y += Mathf.Sqrt(_jumpHeight * -3.0f * _gravityValue);
        }
        

        _playerVelocity.y += _gravityValue * Time.deltaTime;
        _controller.Move(_playerVelocity * Time.deltaTime);

        _groundedPlayer = false;
    }





    private void Instance_OnCutSceneStop(object sender, System.EventArgs e)
    {
        disableInput = false;
    }

    private void Instance_OnCutScenePlaying(object sender, System.EventArgs e)
    {
        disableInput = true;
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
            isWalled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
            isWalled = false;
    }




    void CheckBulletDistance()
    {
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");

        foreach (GameObject bullet in bullets)
        {
            if (bullet != null)
            {
                float distanceToBullet = Vector3.Distance(transform.position, bullet.transform.position);

                // Check if the bullet is within the detection range
                if (distanceToBullet < 5f)
                
                     canDodge = true;

            }
        
        }
    }



    private void Dodge()
    {

        if(Input.GetKeyDown(KeyCode.E))
        {
            if (currentTime > 0.2f)
            {
                currentTime = 0f;
                canDodge = false;
            }
    
        }
        else
        {
            currentTime += Time.deltaTime;
        }




    }


    public void TakeDamage(float damage)
    {
       if(Shield.activeSelf)
        {
            animator.SetTrigger("ShieldHit");
            return;
        }
        
        if(health <= 0) { Destroy(gameObject); }

        health -= damage;
    }

  

}