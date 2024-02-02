using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Third : MonoBehaviour
{

    [Header("Refrences")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Transform combatLookAt;
    public Transform cinemachineTransform;



    public Rigidbody rb;


    public float dodgeForce = 150f;
    public float jumpForce = 10f;

    public float rotationSpeed;
    public float moveSpeed;
    public float targetMoveSpeed;
    private float oldSpeed;

    public static CameraStyle currentStyle;
    private bool disableInput;
    public enum CameraStyle
    {
        Basic,
        Combat
    }



    private void Start()
    {
        currentStyle = CameraStyle.Basic;
        oldSpeed = moveSpeed;
        CutsceneManager.Instance.OnCutScenePlaying += Instance_OnCutScenePlaying;
        CutsceneManager.Instance.OnCutSceneStop += Instance_OnCutSceneStop;
    }



    // Update is called once per frame
    private void Update()
    {
        if (disableInput) return;
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        //Calculates the direction facing of the player and normalizes it so we only get direction
        PlayerRotation(horizontalInput , verticalInput );

        Vector3 moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        MovePlayer(moveDirection);

        if (Input.GetKeyDown(KeyCode.Space))
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
        if (Input.GetKeyDown(KeyCode.LeftControl))
            rb.AddForce(player.transform.forward * dodgeForce, ForceMode.Impulse);

    }

    private void MovePlayer(Vector3 moveDirection)
    {
        // Applying force to move the player

        if(Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = Mathf.Lerp(moveSpeed, targetMoveSpeed, 3f * Time.deltaTime);
            rb.velocity = new Vector3(moveDirection.x * moveSpeed, rb.velocity.y, moveDirection.z * moveSpeed);

        }
        else
        { // going back to original speed
            moveSpeed = Mathf.Lerp(moveSpeed, oldSpeed, 3f * Time.deltaTime);
            rb.velocity = new Vector3(moveDirection.x * moveSpeed, rb.velocity.y, moveDirection.z * moveSpeed);
        }
    }



    private void PlayerRotation(float horizontalInput , float verticalInput)
    {
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        if (currentStyle == CameraStyle.Basic)
        {

            //Calculating the inputs from the mouse and then having a vector that multiples the forward vector of the calculation with the inputs so we another vector where we have to lerp to

            Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

            //checking if inputs are pressed then only slerp so that no bug occurs
            if (inputDir != Vector3.zero)
            {
                //setting the rotation of the playerObj 
                player.forward = Vector3.Lerp(player.forward, inputDir, rotationSpeed * Time.deltaTime);

            }
        }
        else if (currentStyle == CameraStyle.Combat)
        {
            Vector3 dir = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
            orientation.forward = dir.normalized;

            player.forward = dir.normalized;
        }
       
    }

    private void Instance_OnCutSceneStop(object sender, System.EventArgs e)
    {
        disableInput = false;
    }

    private void Instance_OnCutScenePlaying(object sender, System.EventArgs e)
    {
        disableInput = true;
        playerObj.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}

