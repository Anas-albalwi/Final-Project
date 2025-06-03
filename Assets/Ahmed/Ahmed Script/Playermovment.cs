using UnityEngine;


public class Playermovment : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float acceleration = 10f;
    public float mouseSensitivity = 2f;
    public float jumpForce = 5f;
    public Transform cameraTransform;
    public LayerMask groundMask;

    private Rigidbody rb;
    private float xRotation = 0f;
    private bool isGrounded;

    private Vector3 currentVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Mouse Look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        // Ground Check
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f, groundMask);

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        // Input
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        Vector3 targetDirection = (transform.right * moveX + transform.forward * moveZ).normalized;
        Vector3 targetVelocity = targetDirection * moveSpeed;

        // Smooth Movement
        Vector3 velocity = Vector3.Lerp(rb.linearVelocity, targetVelocity + Vector3.up * rb.linearVelocity.y, Time.fixedDeltaTime * acceleration);
        rb.linearVelocity = velocity;
    }
}