using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Character_Controller_V1 : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 12f;
    
    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private PlayerInputManager.PlayerNumber playerNumber;

    private Rigidbody2D rb;
    private bool isGrounded;
    private Vector2 moveInput;
    private PlayerInputManager inputManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        inputManager = GetComponent<PlayerInputManager>();
        playerNumber = inputManager.playerNumber;
        
        // Initialize health
        currentHealth = maxHealth;
        UpdateHealthUI();
        
        // Hide Player 2's health UI in single player mode
        if (GameManager.Instance != null && 
            GameManager.Instance.currentGameMode == GameManager.GameMode.SinglePlayer && 
            playerNumber == PlayerInputManager.PlayerNumber.Player2)
        {
            if (healthSlider != null) healthSlider.gameObject.SetActive(false);
            if (healthText != null) healthText.gameObject.SetActive(false);
        }
        
        // Subscribe to events
        inputManager.OnMove += HandleMove;
        inputManager.OnJump += HandleJump;
    }

    void OnDestroy()
    {
        // Unsubscribe from events
        if (inputManager != null)
        {
            inputManager.OnMove -= HandleMove;
            inputManager.OnJump -= HandleJump;
        }
    }

    void HandleMove(Vector2 input)
    {
        moveInput = input;
    }

    void HandleJump()
    {
        if (isGrounded)
        {
            Jump();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    void FixedUpdate()
    {
        // Move the character
        Move();
    }

    void Move()
    {
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    public void TakeDamage(int damage)
    {
        Debug.Log($"Player {playerNumber} taking {damage} damage. Current health: {currentHealth}");
        currentHealth = Mathf.Max(0, currentHealth - damage);
        Debug.Log($"Player {playerNumber} health after damage: {currentHealth}");
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"Player {playerNumber} died!");
        if (GameManager.Instance != null)
        {
            GameManager.Instance.PlayerDied(playerNumber);
        }
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        if (healthText != null)
        {
            healthText.text = $"P{(int)playerNumber + 1}: {currentHealth}/{maxHealth}";
        }
    }

    // Optional: Visualize ground check in editor
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
