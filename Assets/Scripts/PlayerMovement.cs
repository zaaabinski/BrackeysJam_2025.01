using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputActionReference _moveReference;

    [SerializeField] private float moveSpeed = 5f; // Player movement speed
    [SerializeField] private Transform cameraTransform; // Assign the camera in the inspector
    
    private Vector3 movement;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        _moveReference.action.Enable();
    }

    void Update()
    {
        // Get input
        Vector2 input = _moveReference.action.ReadValue<Vector2>();

        // Convert movement to be relative to camera rotation
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();
        
        movement = (forward * input.y + right * input.x).normalized;

        // Rotate player to face movement direction
        if (movement.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    void FixedUpdate()
    {
        // Apply movement
        rb.linearVelocity = new Vector3(movement.x * moveSpeed, rb.linearVelocity.y, movement.z * moveSpeed);

        // Camera follows the player
        if (cameraTransform != null)
        {
            Vector3 camOffset = new Vector3(10, 8, -10); // Keeps the camera in position
            cameraTransform.position = transform.position + camOffset;
            cameraTransform.rotation = Quaternion.Euler(25, -45, 0); // Set fixed -45-degree angle
        }
    }
}