using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputActionReference _moveReference;

    [SerializeField] private float moveSpeed = 5f; // Player movement speed
    [SerializeField] private Transform cameraTransform; // Assign the camera in the inspector
    [SerializeField] private Animator animator;
    private Vector3 movement;
    private Rigidbody rb;

    private Vector2 _input;

    private void OnEnable()
    {
        // Subscribe to both the performed and canceled events
        _moveReference.action.performed += UpdateInput;
        _moveReference.action.canceled += UpdateInput;
    }

    private void OnDisable()
    {
        // Unsubscribe from both events when the object is disabled
        _moveReference.action.performed -= UpdateInput;
        _moveReference.action.canceled -= UpdateInput;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        _moveReference.action.Enable();
    }

    public void UpdateInput(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        // Convert movement to be relative to camera rotation
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        movement = (forward * _input.y + right * _input.x).normalized;
        animator.SetFloat("Speed", movement.magnitude);

        // Apply movement
        rb.linearVelocity = new Vector3(movement.x * moveSpeed, rb.linearVelocity.y, movement.z * moveSpeed);

        // Rotate player to face movement direction
        if (movement.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }
}