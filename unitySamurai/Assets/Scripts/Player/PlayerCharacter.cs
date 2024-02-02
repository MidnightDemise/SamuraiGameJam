using EazyCamera.Legacy;
using System;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public static PlayerCharacter Instance {  get; private set; }


    public event EventHandler<OnWeaponStateChangedArgs> OnWeaponStateChanged;
    public class OnWeaponStateChangedArgs: EventArgs
    {
        public WeaponState weaponState;
    }

    private Vector2 inputMoveDir;
    private Vector3 moveDirection;
    private Vector2 aimDirection;

    public Rigidbody _RigidBody {  get; private set; }
    private float rotSensitivity;
    public float defaultSensitivity;
    public float aimSensitivity;
    private Transform cameraObj;


    private float movementSpeed = 5f;
    private float rotationSpeed = 10f;
    private float aimRotationSpeed = 20f;

    [SerializeField] private Transform aimCamera;
    [SerializeField]private AnimationHandler _AnimationHandler;
    [SerializeField] private Rifle _Rifle;
    [SerializeField] private Transform staticMesh;
    [SerializeField] private Transform aimTransform;
    [SerializeField] private Transform headTransform;

    private PlayerControls _PlayerControls;

    public enum WeaponState
    {
        Sword,
        Gun,
        None
    }

    private WeaponState weaponState;

    private void Awake()
    {
        Instance = this;
        _RigidBody = GetComponent<Rigidbody>();
        cameraObj = Camera.main.transform;
        rotSensitivity = defaultSensitivity;
        weaponState = WeaponState.Sword;
        SwitchWeapon();


        Cursor.visible = false;
        Cursor.lockState= CursorLockMode.Locked;

    }

    private void Start()
    {
        _PlayerControls = InputManager.Instance.PlayerControls;

        _PlayerControls.Movement.WASD.performed +=
            (UnityEngine.InputSystem.InputAction.CallbackContext obj) => inputMoveDir = obj.ReadValue<Vector2>();
        _PlayerControls.Actions.L_Ctrl.performed += HandleDodge;
        _PlayerControls.Actions.Space.performed += Space_performed;
       _PlayerControls.Actions.RMB.started += RMB_started;
        _PlayerControls.Actions.RMB.canceled += RMB_canceled;
        _PlayerControls.Actions.Tab.started += Tab_started;
        _PlayerControls.Actions.LMB.started += LMB_started;
    }

    private void LMB_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        switch (weaponState)
        {
            case WeaponState.Sword:
                //Do Melee Attack
                _AnimationHandler.PlayTargetAnimation("SwordAttack", true);
                break;
            case WeaponState.Gun:
                //Do Rifle Attack
                if (!_AnimationHandler._Animator.GetBool("Aiming")) return;
                _AnimationHandler.PlayTargetAnimation("RifleAttack", true);
                _Rifle?.Shoot();
                break;
            case WeaponState.None:
            default:
                break;
        }
        OnWeaponStateChanged?.Invoke(this, new OnWeaponStateChangedArgs
        {
            weaponState = this.weaponState
        });
    }



    private void Update()
    {
        HandleMovement();
        if (!_AnimationHandler._Animator.GetBool("Aiming"))
            HandleMeshRotation(Time.deltaTime);
        else
            HandleAimingRotation();


        _AnimationHandler.UpdateAnimatorMoveValue(inputMoveDir);
    }


    #region Movement
    private void HandleMovement()
    {
        if (_AnimationHandler.IsInteracting) return;
        float horizontalMove = inputMoveDir.x;
        float verticalMove = inputMoveDir.y;

        moveDirection = cameraObj.forward * verticalMove;
        moveDirection += cameraObj.right * horizontalMove;
        moveDirection.Normalize();
        moveDirection.y = 0;

        moveDirection *= movementSpeed;

        _RigidBody.velocity = moveDirection;
    }
    #endregion
    #region Rotation
    private void HandleMeshRotation(float delta)
    {
        float horizontalMove = inputMoveDir.x;
        float verticalMove = inputMoveDir.y;
        

        Vector3 targetDir = cameraObj.forward * verticalMove;
        targetDir += cameraObj.right * horizontalMove;

        targetDir.Normalize();
        targetDir.y = 0;

        if(targetDir == Vector3.zero)
            targetDir = transform.forward;

        Quaternion desiredRotation = Quaternion.LookRotation(targetDir);


        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * delta * rotSensitivity);

       

    }
    private void HandleAimingRotation()
    {
        // Assuming aimDirection is a Vector2 where x is the horizontal input and y is the vertical input
        Vector3 aimDirection3D = new Vector3(aimDirection.x, 0f, 0f);

        // Ensure the aim direction is not zero before rotating
        if (aimDirection3D.magnitude > 0.1f)
        {
            // Calculate the desired rotation based on the aim direction
            Quaternion desiredRotation = Quaternion.LookRotation(aimDirection3D, Vector3.up);

            // Use Slerp for smooth rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, 0.5f * Time.deltaTime);
        }
    }

    private void HandleAimingMeshRotation(bool aim)
    {
        if (aim)
            staticMesh.localRotation = Quaternion.Euler(0f, 45f, 0f);
        else
            staticMesh.localRotation = Quaternion.Euler(Vector3.zero);

    }

    #endregion
    #region Weapon Switching
    private void SwitchWeapon()
    {
        switch (weaponState)
        {
            case WeaponState.Sword:
                weaponState = WeaponState.Gun;
                break;
            case WeaponState.Gun:
                weaponState = WeaponState.Sword; 
                break;
            case WeaponState.None:
            default:
                break;
        }
        ToggleWeapon();
    }

    private void ToggleWeapon()
    {
        switch (weaponState)
        {
            case WeaponState.Sword:
                _AnimationHandler._Animator.SetBool("Sword", true);
                _Rifle?.SetVisibility(false);
                break;
            case WeaponState.Gun:
                _Rifle?.SetVisibility(true);
                _AnimationHandler._Animator.SetBool("Sword", false);
                break;
            case WeaponState.None:
            default:
                break;
        }
        OnWeaponStateChanged?.Invoke(this, new OnWeaponStateChangedArgs
        {
            weaponState = this.weaponState
        });
    }
    #endregion

    #region Input Callbacks
    private void HandleDodge(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (_AnimationHandler.IsInteracting) return;
            transform.forward = new Vector3(cameraObj.forward.x, 0f, cameraObj.forward.z).normalized;
            _AnimationHandler.PlayTargetAnimation("DodgeRoll", true);
    }
    private void Tab_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        SwitchWeapon();
    }

    private void RMB_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (weaponState != WeaponState.Gun) return;
        rotSensitivity = defaultSensitivity;
        _AnimationHandler._Animator.SetBool("Aiming", false);
        aimCamera.gameObject.SetActive(false);
        HandleAimingMeshRotation(false);

    }

    private void RMB_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (weaponState != WeaponState.Gun) return;
        rotSensitivity = aimSensitivity;
        _AnimationHandler._Animator.SetBool("Aiming", true);
        aimCamera.gameObject.SetActive(true);
        _PlayerControls.Movement.Mouse.performed += Mouse_performed;
        HandleAimingMeshRotation(true);
    }

    private void Space_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (_AnimationHandler.IsInteracting) return;

        _AnimationHandler.PlayTargetAnimation("Jump", true);
    }
    private void Mouse_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        aimDirection = obj.ReadValue<Vector2>();
    }
    #endregion
}
