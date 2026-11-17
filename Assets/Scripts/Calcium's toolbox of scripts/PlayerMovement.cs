using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;


public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _rb;
    private Transform _camera;
    
    [SerializeField] float moveSpeed = 1;
    [SerializeField] float Jumpforce = 1;

    private Vector2 inputVector;

    [SerializeField] float turnSmoothTime = 0.2f;
    float turnsmoothVelocity;
    
    [Header("Jump Ground Check")]
    [SerializeField] private LayerMask groundlayer;
    [SerializeField] private float CheckSphereRadius = 0.5f;
    [SerializeField]private float CheckHeight = 0.2f;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _camera = Camera.main.transform;
    }
    void FixedUpdate()
    {
        MovePlayer();
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

    public void OnJump(InputAction.CallbackContext context)
    {
        if (IsGrounded())
        {
            _rb.AddForce(Vector3.up * Jumpforce, ForceMode.Impulse);
        }
    }

    /// <summary>
    /// move the player in the relative direction from the camera with smooth player object rotation
    /// </summary>
    void MovePlayer()
    {
        Vector2 inputDir = inputVector.normalized;
        if (inputDir != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + _camera.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnsmoothVelocity, turnSmoothTime);
        }

        float targetSpeed = moveSpeed * inputDir.magnitude * Time.fixedDeltaTime;
        _rb.Move(transform.position + (transform.forward * targetSpeed), transform.rotation);
    }

    private bool IsGrounded()
    {
        RaycastHit hit;
        return Physics.SphereCast(transform.position, CheckSphereRadius, -transform.up, out hit, CheckHeight, groundlayer);
    }

    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.blueViolet;
        Gizmos.DrawWireSphere(transform.position + (-transform.up * CheckHeight), CheckSphereRadius);
    }
}
