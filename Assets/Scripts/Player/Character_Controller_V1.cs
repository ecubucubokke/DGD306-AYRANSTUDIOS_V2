using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

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
    [SerializeField] private float invincibilityDuration = 1f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool isGrounded;
    private Vector2 moveInput;
    private PlayerInputManager inputManager;
    private bool isInvincible = false;

    // Animation parameter names
    private readonly string xVelocityParam = "xVelocity";
    private readonly string yVelocityParam = "yVelocity";
    private readonly string isJumpingParam = "isJumping";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
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
        
        // Update animation parameters
        UpdateAnimationParameters();
    }

    void UpdateAnimationParameters()
    {
        if (animator != null)
        {
            // Set x velocity for horizontal movement
            animator.SetFloat(xVelocityParam, Mathf.Abs(rb.linearVelocity.x));
            
            // Set y velocity for vertical movement
            animator.SetFloat(yVelocityParam, rb.linearVelocity.y);
            
            // Set jumping state
            animator.SetBool(isJumpingParam, !isGrounded);
        }
    }

    void FixedUpdate()
    {
        // Move the character
        Move();
    }

    void Move()
    {
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
        
        // Flip sprite based on movement direction
        if (moveInput.x != 0)
        {
            spriteRenderer.flipX = moveInput.x < 0;
        }
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        if (animator != null)
        {
            animator.SetBool(isJumpingParam, true);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        Debug.Log($"Player {playerNumber} taking {damage} damage. Current health: {currentHealth}");
        currentHealth = Mathf.Max(0, currentHealth - damage);
        Debug.Log($"Player {playerNumber} health after damage: {currentHealth}");
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvincibilityRoutine());
        }
    }

    private IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;
        float flashDuration = 0.1f; // Her yanıp sönme süresi
        float elapsedTime = 0f;
        bool isVisible = true;

        while (elapsedTime < invincibilityDuration)
        {
            // Sprite'ı yanıp söndür
            spriteRenderer.color = new Color(1f, 1f, 1f, isVisible ? 1f : 0.3f);
            isVisible = !isVisible;
            
            yield return new WaitForSeconds(flashDuration);
            elapsedTime += flashDuration;
        }
        
        isInvincible = false;
        spriteRenderer.color = Color.white; // Normal rengine dön
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
