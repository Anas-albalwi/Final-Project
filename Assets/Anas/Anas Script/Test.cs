using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class Test : MonoBehaviour
{
    PlayerInput _playerInput;
    InputAction _moveAction, _lookAction;
    Vector2 _moveAmt, _lookAmt;
    Rigidbody _rb;

    [Header("Speeds")]
    public float WalkSpeed = 5f;
    public float RotateSpeed = 180f;  // œ—Ã« /À«‰Ì…
    public float PitchSpeed = 180f;  // œ—Ã« /À«‰Ì…

    [Header("Vertical Look Limits")]
    public float MinPitch = -60f;
    public float MaxPitch = +60f;

    float _currentPitch = 0f;

    void Awake()
    {
        // 1) Ã·» PlayerInput Ê «·√ﬂ‘‰“
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions["Move"];
        _lookAction = _playerInput.actions["Look"];

        // 2) Ã·» «·—ÌÃÌœ»ÊœÌ
        _rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        _moveAction.Enable();
        _lookAction.Enable();
    }

    void OnDisable()
    {
        _moveAction.Disable();
        _lookAction.Disable();
    }

    void Update()
    {
        // 3) ﬁ—«¡… ﬁÌ„ «·Õ—ﬂ… Ê «·‰Ÿ—
        _moveAmt = _moveAction.ReadValue<Vector2>();
        _lookAmt = _lookAction.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        // 4) Õ—ﬂ… √„«„/Œ·›
        Vector3 forward = transform.forward * _moveAmt.y;
        Vector3 right = transform.right * _moveAmt.x;
        _rb.MovePosition(_rb.position + (forward + right) * WalkSpeed * Time.fixedDeltaTime);

        // 5) œÊ—«‰ √›ﬁÌ ÕÊ· «·„ÕÊ— Y
        float yawDelta = _lookAmt.x * RotateSpeed * Time.fixedDeltaTime;
        _rb.MoveRotation(_rb.rotation * Quaternion.Euler(0f, yawDelta, 0f));
    }

    void LateUpdate()
    {
        // 6) œÊ—«‰ —√”Ì (Pitch) ⁄·Ï „ÕÊ— X „Õ·Ì¯
        float pitchDelta = -_lookAmt.y * PitchSpeed * Time.deltaTime;
        _currentPitch = Mathf.Clamp(_currentPitch + pitchDelta, MinPitch, MaxPitch);
        // ‰› —÷ ≈‰ «·ﬂ«„Ì—« ÿ›· child ·‹ Â–« «·‹ GameObject »«”„ "CameraPivot"
        Transform camPivot = transform.Find("CameraPivot");
        if (camPivot != null)
            camPivot.localRotation = Quaternion.Euler(_currentPitch, 0f, 0f);
    }
}
