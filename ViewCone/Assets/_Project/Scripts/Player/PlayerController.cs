using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float mouseSensitivity = 2f;

    private CharacterController _cc;
    private Animator _animator;
    private InputAction _moveAction;
    private float _rotationY = 0f;

    void Awake()
    {
        _cc = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _moveAction = InputSystem.actions.FindAction("Player/Move");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnEnable() => _moveAction?.Enable();
    void OnDisable() => _moveAction?.Disable();

    void Update()
    {
        float mouseX = Mouse.current.delta.x.ReadValue();
        _rotationY += mouseX * mouseSensitivity;
        transform.rotation = Quaternion.Euler(0, _rotationY, 0);

        bool isRunning = Keyboard.current.leftShiftKey.isPressed;
        float currentSpeed = isRunning ? moveSpeed * 2f : moveSpeed;

        Vector2 input = _moveAction != null ? _moveAction.ReadValue<Vector2>() : Vector2.zero;
        Vector3 move = transform.forward * input.y + transform.right * input.x;
        move.y -= 9.8f;
        _cc.Move(move * currentSpeed * Time.deltaTime);

        if (_animator != null)
            _animator.SetFloat("Speed", new Vector2(input.x, input.y).magnitude * (isRunning ? 2f : 1f));
    }
}