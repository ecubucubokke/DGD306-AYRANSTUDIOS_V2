using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerInputManager : MonoBehaviour
{
    public enum PlayerNumber
    {
        Player1,
        Player2
    }

    public PlayerNumber playerNumber;
    private PlayerInput playerInput;
    private Vector2 moveInput;
    private bool jumpPressed;
    private bool firePressed;

    // Events
    public event Action<Vector2> OnMove;
    public event Action OnJump;
    public event Action OnFire;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput component is missing!");
            return;
        }
    }

    // Input System Events
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        OnMove?.Invoke(moveInput);
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpPressed = true;
            OnJump?.Invoke();
        }
        else if (context.canceled)
        {
            jumpPressed = false;
        }
    }

    public void OnFireInput(InputAction.CallbackContext context)
    {
        if (context.started || context.performed)
        {
            firePressed = true;
            OnFire?.Invoke();
        }
        else if (context.canceled)
        {
            firePressed = false;
        }
    }

    // Getter methods for other scripts
    public Vector2 GetMoveInput()
    {
        return moveInput;
    }

    public bool IsJumpPressed()
    {
        return jumpPressed;
    }

    public bool IsFirePressed()
    {
        return firePressed;
    }

    private void Start()
    {
        // Tek oyunculu modda Player2'yi devre dışı bırak
        if (playerNumber == PlayerNumber.Player2 && 
            GameManager.Instance.currentGameMode == GameManager.GameMode.SinglePlayer)
        {
            gameObject.SetActive(false);
        }
    }
} 