using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _rb;
    private Transform _camera;

    [SerializeField] float moveSpeed = 1;
    public float acceleration = 30f;
    public float deceleration = 30f;
    private float targetSpeed;
    private float currentSpeed;
    private float maxSpeed = 17f;
    public bool isRamming;
    public bool BeingKnockedBack;
    private float RamStunTime = 2f;
    private float RamStunTimer = 0;

    private Vector2 inputVector;

    [SerializeField] float turnSmoothTime = 0.36f;
    float turnsmoothVelocity;
    
    
    [SerializeField] private LayerMask groundlayer;
    [SerializeField] private float CheckSphereRadius = 0.5f;
    [SerializeField]private float CheckHeight = 0.2f;

    void Start()
    {
        isRamming = false;
        _rb = GetComponent<Rigidbody>();
        _camera = Camera.main.transform;
        GameManager.call.playerRefList.Add(this);
    }

    void FixedUpdate()
    {
        if(BeingKnockedBack == true)
        {
            isRamming = false;
        }
        IsGrounded();
        if(BeingKnockedBack == false)
        {
            MovePlayer();
        }
        Debug.Log(currentSpeed);
        if(currentSpeed >= maxSpeed)
        {
            isRamming = true;
        }
        else if(currentSpeed != maxSpeed) 
        {
        
            isRamming = false;
        }
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

    void MovePlayer()
    {
        Vector2 inputDir = inputVector.normalized;
        if (inputDir != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + _camera.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnsmoothVelocity, turnSmoothTime);
        }

        targetSpeed = moveSpeed * inputDir.magnitude;
        float rate = inputDir != Vector2.zero ? acceleration : deceleration;
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, rate * Time.fixedDeltaTime);
        _rb.Move(transform.position + transform.forward * currentSpeed * Time.fixedDeltaTime, transform.rotation);
    }

    //keeping this incase we need it later, if we don't it'll be removed before the first build.
    private bool IsGrounded()
    {
        RaycastHit hit;
        if(BeingKnockedBack == true)
        {
            RamStunTimer = RamStunTimer + Time.deltaTime;
            if(RamStunTimer >= RamStunTime)
            {
            BeingKnockedBack = false;
                RamStunTimer = 0;
            }
        }
        return Physics.SphereCast(transform.position, CheckSphereRadius, -transform.up, out hit, CheckHeight, groundlayer);
    }

    public void KnockBack(float KnockForce, Vector3 Direction)
    {       
        _rb.AddForce(Direction * KnockForce, ForceMode.Impulse);
    }
}
