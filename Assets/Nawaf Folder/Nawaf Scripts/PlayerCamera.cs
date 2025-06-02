using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float mouseSensitivity = 50f;
    public Transform playerBody;
    public float smoothTime = 0.05f;

    float xRotation = 0f;
    float currentMouseX = 0f;
    float currentMouseY = 0f;
    float mouseXVelocity = 0f;
    float mouseYVelocity = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float targetMouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float targetMouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        currentMouseX = Mathf.SmoothDamp(currentMouseX, targetMouseX, ref mouseXVelocity, smoothTime);
        currentMouseY = Mathf.SmoothDamp(currentMouseY, targetMouseY, ref mouseYVelocity, smoothTime);

        xRotation -= currentMouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * currentMouseX);
    }
}