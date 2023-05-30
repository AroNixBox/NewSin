using MoreMountains.Tools;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonController : MonoBehaviour
{
    //input fields
    private ThirdPersonActionMap actionMap;
    private InputAction move;

    //movement fields
    private Rigidbody rb;
    [SerializeField]
    private float movementForce = 1f;
    [SerializeField]
    private float jumpForce = 5f;
    [SerializeField]
    private float maxSpeed = 5f;
    private Vector3 forceDirection = Vector3.zero;

    [SerializeField]
    private Camera playerCamera;
    private Animator animator;

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        actionMap = new ThirdPersonActionMap();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        actionMap.OnFoot.Jump.performed += DoJump;
        move = actionMap.OnFoot.Move;
        actionMap.OnFoot.Enable();
    }

    private void OnDisable()
    {
        actionMap.OnFoot.Jump.performed -= DoJump;
        actionMap.OnFoot.Disable();
    }

    private void FixedUpdate()
    {
        forceDirection += move.ReadValue<Vector2>().x * GetCameraRight(playerCamera) * movementForce;
        forceDirection += move.ReadValue<Vector2>().y * GetCameraForward(playerCamera) * movementForce;

        rb.AddForce(forceDirection, ForceMode.Impulse);
        //when let go of button, stop accelerate
        forceDirection = Vector3.zero;

        if (rb.velocity.y < 0f)
        {
            rb.velocity -= Vector3.down * Physics.gravity.y * Time.fixedDeltaTime;
        }

        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0;
        if (horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed)
        {
            rb.velocity = horizontalVelocity.normalized * maxSpeed + Vector3.up * rb.velocity.y;
        }
        LookAt();
    }

    private void LookAt()
    {
        Vector3 direction = rb.velocity;
        direction.y = 0;

        if (move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
        {
            //WE ARE MOVING
            animator.SetBool("Walking", true);
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            float turnSpeed = 7f; //or whatever
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, Time.deltaTime * turnSpeed);

            //Added.normalized very late, maybe remove if it causes bugs
            //Before was: rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
        else
        {
            animator.SetBool("Walking", false);
            //WE ARENT MOVING
            //Idle Animation
            //stop camrotation
            rb.angularVelocity = Vector3.zero;
        }
    }

    private Vector3 GetCameraForward(Camera playerCamera)
    {
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0;
        //we dont want movement to depend of cameraangle
        return forward.normalized;
    }

    private Vector3 GetCameraRight(Camera playerCamera)
    {
        Vector3 right = playerCamera.transform.right;
        right.y = 0;
        return right.normalized;
    }

    private void DoJump(InputAction.CallbackContext obj)
    {
        if (IsGrounded())
        {
            animator.SetTrigger("Jump");
            forceDirection += Vector3.up * jumpForce;
        }
    }

    private bool IsGrounded()
    {
        Ray ray = new Ray(this.transform.position + Vector3.up * 0.25f, Vector3.down);
        //0.3f bit higher than 0.25f, so we know when grounded
        //Add Tag from what we can jump
        if (Physics.Raycast(ray, out RaycastHit hit, 0.3f))
        {
            return true;
        }
        else
        {
            return false;
        }

    }
}