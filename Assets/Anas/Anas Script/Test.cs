using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class Test : MonoBehaviour
{
    Vector2 _moveAmt, _lookAmt;
    Rigidbody _rb;

    [Header("Speeds")]
    public float WalkSpeed = 5f;
    public float RotateSpeed = 180f;  // œ—Ã« /À«‰Ì…
    public float PitchSpeed = 180f;  // œ—Ã« /À«‰Ì…

    [Header("Vertical Look Limits")]
    public float MinPitch = -60f;
    public float MaxPitch = +60f;


    public Transform camPivot; // ‰› —÷ ≈‰ «·ﬂ«„Ì—« ÂÌ ÿ›· child ·Â–« «·‹ GameObject
    float _currentPitch = 0f;
    float _currentYaw;

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveAmt = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        _lookAmt = context.ReadValue<Vector2>();
    }

    void Awake()
    {

        // 2) Ã·» «·—ÌÃÌœ»ÊœÌ
        _rb = GetComponent<Rigidbody>();
    }


    
    void FixedUpdate()
    {
        // Õ—ﬂ… √„«„/Œ·›
        Vector3 forward = transform.forward * _moveAmt.y;
        Vector3 right = transform.right * _moveAmt.x;
        _rb.MovePosition(_rb.position + (forward + right) * WalkSpeed * Time.fixedDeltaTime);

        // œÊ—«‰ √›ﬁÌ ÕÊ· «·„ÕÊ— Y (Ì„Ì‰/Ì”«—)
        float yawDelta = _lookAmt.x * RotateSpeed * Time.fixedDeltaTime;
        _currentYaw += yawDelta; // Õ›Ÿ «·“«ÊÌ…
        _rb.MoveRotation(Quaternion.Euler(0f, _currentYaw, 0f));
    }

    void LateUpdate()
    {
        // œÊ—«‰ —√”Ì ÕÊ· «·„ÕÊ— X (›Êﬁ/ Õ )
        float pitchDelta = -_lookAmt.y * PitchSpeed * Time.deltaTime;
        _currentPitch = Mathf.Clamp(_currentPitch + pitchDelta, MinPitch, MaxPitch);

        //  ÿ»Ìﬁ «·œÊ—«‰ ⁄·Ï «·ﬂ«„Ì—« ›ﬁÿ (CameraPivot)
        if (camPivot != null)
        {
            camPivot.localRotation = Quaternion.Euler(_currentPitch, 0f, 0f);
        }
    }

}
