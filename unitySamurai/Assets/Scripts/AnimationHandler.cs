using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    public Animator _Animator {  get; private set; }
    private  int vertical;
    private  int horizontal;
    private const float DAMP_TIME = 0.1f;

    [SerializeField] private PlayerCharacter character;


    public bool IsInteracting { get
        {
            return _Animator.GetBool("IsInteracting");
        }}

    private void Awake()
    {
        _Animator = GetComponent<Animator>();
        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");
    }

    private void OnAnimatorMove()
    {
        if (!IsInteracting) return;
        character._RigidBody.drag = 0;
        Vector3 deltaPos = _Animator.deltaPosition;
        deltaPos.y = 0;
        Vector3 velocity = deltaPos / Time.deltaTime;
        character._RigidBody.velocity = velocity;
    }

    public void PlayTargetAnimation(string targetAnim, bool isInteracting)
    {
        _Animator.applyRootMotion = isInteracting;
        _Animator.SetBool("IsInteracting", isInteracting);
        _Animator.CrossFade(targetAnim, 0.1f);
    }

    public void UpdateAnimatorMoveValue(Vector2 inputMoveVal)
    {
       
        float moveAmount = Mathf.Clamp01(Mathf.Abs(inputMoveVal.x) + Mathf.Abs(inputMoveVal.y));
        float verticalVal = Mathf.Abs(CalculateMoveAnimatorVal(moveAmount)) ;
        float horiztontalVal = CalculateMoveAnimatorVal(0);
        _Animator.SetFloat(vertical, verticalVal, DAMP_TIME, Time.deltaTime);
        _Animator.SetFloat(horizontal, horiztontalVal, DAMP_TIME, Time.deltaTime);
        

    }

    private float CalculateMoveAnimatorVal(float inputMoveVal)
    {
        float returnedVal;
        if (inputMoveVal > 0 && inputMoveVal < 0.55f)
            returnedVal = 0.5f;
        else if (inputMoveVal > 0.55f)
            returnedVal = 1f;
        else if (inputMoveVal < 0 && inputMoveVal > -0.55f)
            returnedVal = -0.5f;
        else if (inputMoveVal < -0.55f)
            returnedVal = -1f;
        else 
            returnedVal = 0f;
        return returnedVal;
    }

}
