using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveAroundObject : MonoBehaviour
{
    public Transform combatLookAtPos;


    [SerializeField]
    private GameObject rifle;
    [SerializeField]
    private GameObject shield;
    [SerializeField]
    private GameObject sword;

    [SerializeField]
    private float _mouseSensitivity = 3.0f;

    private float _rotationY;
    private float _rotationX;

    [SerializeField]
    private Transform _target;

    [SerializeField]
    private Transform target;

    [SerializeField]
    private float _distanceFromTarget = 1.5f;

    [SerializeField]
    private float smoothness = 5f;

    private float lerpProgress = 0f;

    private Vector3 _currentRotation;
    private Vector3 _smoothVelocity = Vector3.zero;
    private Vector3 originalPos;

    [SerializeField]
    private float _smoothTime = 0.2f;

    [SerializeField]
    private Vector2 _rotationXMinMax = new Vector2(-20, 20);

    private float oldsen;
    private void Start()
    {


        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        originalPos = transform.position;
        oldsen = _mouseSensitivity;
    }

    void Update()
    {
        if(Rifle.isLerping)
        {
            _mouseSensitivity = 0.9f;
        }
        else
        {
            _mouseSensitivity = oldsen;
        }
        if ((rifle.activeSelf || shield.activeSelf || sword.activeSelf) && !Input.GetMouseButtonDown(1))
        {
            
                transform.position = Vector3.Lerp(transform.position, combatLookAtPos.position, smoothness * Time.deltaTime);

            
            float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity;

            _rotationY += mouseX;
            _rotationX -= mouseY;
            _rotationX = Mathf.Clamp(_rotationX, _rotationXMinMax.x, _rotationXMinMax.y);

            Vector3 nextRotation = new Vector3(_rotationX, _rotationY);
            _currentRotation = Vector3.SmoothDamp(_currentRotation, nextRotation, ref _smoothVelocity, _smoothTime);
            transform.localEulerAngles = _currentRotation;
            
            target.transform.rotation = transform.rotation;


        }
        else if(!Input.GetMouseButtonDown(1))
        {

            transform.position = Vector3.Lerp(transform.position, originalPos, 5f * Time.deltaTime);

            float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity;

            _rotationY += mouseX;
            _rotationX -= mouseY;

            // Apply clamping for x rotation 
            _rotationX = Mathf.Clamp(_rotationX, _rotationXMinMax.x, _rotationXMinMax.y);

            Vector3 nextRotation = new Vector3(_rotationX, _rotationY);

            // Apply damping between rotation changes
            _currentRotation = Vector3.SmoothDamp(_currentRotation, nextRotation, ref _smoothVelocity, _smoothTime);
            transform.localEulerAngles = _currentRotation;

            // Substract forward vector of the GameObject to point its forward vector to the target
            transform.position = _target.position - transform.forward * _distanceFromTarget;

        }
        
    }

    void handleRotation()
    {

    }
}