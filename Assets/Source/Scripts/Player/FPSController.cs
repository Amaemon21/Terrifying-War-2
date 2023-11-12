using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour 
{
    [SerializeField] private Transform cameraTransform = null;

    [Space]
    [SerializeField] private float speed = 10.0f;
    [SerializeField] private float gravity = -20.0f;
    [SerializeField] private float jumpHeight = 2.0f;
    [SerializeField] private float runMultiplier = 2.0f;

    [Space]
    [SerializeField, Range(0f, 90f)] private float jumpSlopeLimit = 0.0f;

    [Space]
    [SerializeField] public float _mouseSensitivity;
    [SerializeField] public float _aimSensitivity;

    private CharacterController _characterController = null;
    private float _jumpMultiplier = 0.0f;
    private float _yVelocity = 0.0f;
    private float _originalSlopeLimit = 0.0f;
    private float _xRotation = 0.0f;

    private void Awake() 
    {
        _mouseSensitivity = PlayerPrefs.GetFloat("Mouse", _mouseSensitivity);
        _aimSensitivity = PlayerPrefs.GetFloat("Aim", _aimSensitivity);

        _characterController = GetComponent<CharacterController>();

        _originalSlopeLimit = _characterController.slopeLimit;
        _jumpMultiplier = Mathf.Sqrt(jumpHeight * -2f * gravity);


        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update() 
    {
        CheckGround();
        Move();
    }
    
    void LateUpdate() 
    {
        Look();
    }

    public void SetSensitivityMouse(float value)
    {
        _mouseSensitivity = value;

        PlayerPrefs.SetFloat("Mouse", _mouseSensitivity);
    }

    public void SetSensitivityAim(float value)
    {
        _aimSensitivity = value;

        PlayerPrefs.SetFloat("Aim", _aimSensitivity);
    }

    private void Look()
    {
        float mouseX = 0;
        float mouseY = 0;

        if (Input.GetKey(ButtonsManager.Instance.ScopeButton))
        {
            mouseX = Input.GetAxis("Mouse X") * _aimSensitivity;
            mouseY = Input.GetAxis("Mouse Y") * _aimSensitivity;
        }
        else
        {
            mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;
            mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity;
        }

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void Move() 
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 move = (transform.right * x + transform.forward * z).normalized;
        move = move * speed * Time.deltaTime;

        if (Input.GetKey(ButtonsManager.Instance.RunningButton))
        {
            move *= runMultiplier;
        }

        if (Input.GetKeyDown(ButtonsManager.Instance.JumpButton) && _characterController.isGrounded) 
        {
            _yVelocity += _jumpMultiplier;
        }

        _yVelocity += gravity * Time.deltaTime;

        move.y = _yVelocity * Time.deltaTime;

        _characterController.Move(move);
    }

    private void CheckGround()
    {
        if (_characterController.isGrounded || _characterController.collisionFlags == CollisionFlags.Above) _yVelocity = -0.1f;

        if (_characterController.isGrounded)
        {
            _characterController.slopeLimit = _originalSlopeLimit;
        }
        else
        {
            _characterController.slopeLimit = jumpSlopeLimit;
        }
    }
}
