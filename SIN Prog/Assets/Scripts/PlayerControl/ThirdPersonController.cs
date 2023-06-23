using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonController : MonoBehaviour
{
    //input fields
    private ThirdPersonActionMap _actionMap;
    private InputAction _move;

    //movement fields
    private Rigidbody _rb;
    public float movementForce = 1f;
    public float jumpForce = 5f;
    public float maxSpeed = 5f;
    private Vector3 _forceDirection = Vector3.zero;

    [SerializeField]
    private Camera playerCamera;
    private Animator animator;

    private LayerMask sandLayer;
    private static readonly int Walking = Animator.StringToHash("Walking");
    private static readonly int Jump = Animator.StringToHash("Jump");

    private void Awake()
    {
        _rb = this.GetComponent<Rigidbody>();
        _actionMap = new ThirdPersonActionMap();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _actionMap.OnFoot.Jump.performed += DoJump;
        _move = _actionMap.OnFoot.Move;
        _actionMap.OnFoot.Enable();
    }

    private void OnDisable()
    {
        _actionMap.OnFoot.Jump.performed -= DoJump;
        _actionMap.OnFoot.Disable();
    }

    private void FixedUpdate()
    {
        _forceDirection += GetCameraRight(playerCamera) * (_move.ReadValue<Vector2>().x * movementForce);
        _forceDirection += GetCameraForward(playerCamera) * (_move.ReadValue<Vector2>().y * movementForce);

        _rb.AddForce(_forceDirection, ForceMode.Impulse);
        //when let go of button, stop accelerate
        _forceDirection = Vector3.zero;

        if (_rb.velocity.y < 0f)
        {
            _rb.velocity -= Vector3.down * (Physics.gravity.y * Time.fixedDeltaTime);
        }

        Vector3 horizontalVelocity = _rb.velocity;
        horizontalVelocity.y = 0;
        if (horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed)
        {
            _rb.velocity = horizontalVelocity.normalized * maxSpeed + Vector3.up * _rb.velocity.y;
        }
        LookAt();
    }

    private void LookAt()
    {
        Vector3 direction = _rb.velocity;
        direction.y = 0;

        if (_move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
        {
            //WE ARE MOVING
            animator.SetBool(Walking, true);
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            var turnSpeed = 7f; //or whatever
            _rb.rotation = Quaternion.Slerp(_rb.rotation, targetRotation, Time.deltaTime * turnSpeed);

            //Added.normalized very late, maybe remove if it causes bugs
            //Before was: rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
        else
        {
            animator.SetBool(Walking, false);
            //WE ARENT MOVING
            //Idle Animation
            //stop cam rotation
            _rb.angularVelocity = Vector3.zero;
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
            animator.SetTrigger(Jump);
            _forceDirection += Vector3.up * jumpForce;
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