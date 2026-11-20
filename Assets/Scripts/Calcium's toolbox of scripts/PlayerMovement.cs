using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;



public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _rb;
    private Transform _camera;
    private BoxCollider _ramcollider;
   
    [SerializeField] float moveSpeed = 1;
    public float acceleration = 30f;
    public float deceleration = 30f;
    private float currentSpeed;
    public bool isRamming;
    public bool BeingKnockedBack;

    private Vector2 inputVector;

    [SerializeField] float turnSmoothTime = 0.36f;
    float turnsmoothVelocity;
    
    
    [SerializeField] private LayerMask groundlayer;
    [SerializeField] private float CheckSphereRadius = 0.5f;
    [SerializeField]private float CheckHeight = 0.2f;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _camera = Camera.main.transform;
        _ramcollider = GetComponent<BoxCollider>();
    }

    void FixedUpdate()
    {
        IsGrounded();
        if(BeingKnockedBack == false)
        {
            MovePlayer();
        }
        Debug.Log(currentSpeed);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            inputVector = context.ReadValue<Vector2>();
        }

        if (context.canceled)
        {
            inputVector = Vector2.zero;
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        KnockBack(10);
    }
   

    //'borrowed' the movement script from the 3d platformer, will edit for this game.
    void MovePlayer()
    {
        Vector2 inputDir = inputVector.normalized;
        if (inputDir != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + _camera.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnsmoothVelocity, turnSmoothTime);
        }

        float targetSpeed = moveSpeed * inputDir.magnitude;
        float rate = inputDir != Vector2.zero ? acceleration : deceleration;
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, rate * Time.fixedDeltaTime);
        _rb.Move(transform.position + transform.forward * currentSpeed * Time.fixedDeltaTime, transform.rotation);
    }

    //keeping this incase we need it later, if we don't it'll be removed before the first build.
    private bool IsGrounded()
    {
        RaycastHit hit;
        BeingKnockedBack = false;
        return Physics.SphereCast(transform.position, CheckSphereRadius, -transform.up, out hit, CheckHeight, groundlayer);
    }

    public void KnockBack(float KnockForce)
    {       
        _rb.AddForce(-transform.forward * KnockForce, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && other != _rb)
        {
            if(other.GetComponent<PlayerMovement>() != null)
            {
                return;
            }
            else
            {
                return;
            }
        }
    }
}
