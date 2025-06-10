using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Boss2 : MonoBehaviour
{
    #region Serialized Fields

    [Header("Attack Settings")]
    [SerializeField] private GameObject missilePrefab;          // Projectile to launch
    [SerializeField] private Transform missileSpawnPoint;         // Where missile originates
    [SerializeField] private float attackCooldown = 4f;           // Time between attacks
    [SerializeField] private int missileDamage = 30;             // Damage that explosion deals

    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 400;
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI healthText;

    [Header("References")]
    [SerializeField] private Animator animator;                  // Handles attack/death animations
    [SerializeField] private AudioClip missileLaunchSfx;
    [SerializeField] private AudioClip deathSfx;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private EnemySpawnSystem_Boss2 spawnSystem; // Reference to spawner controlling support enemies

    [Header("UI & Portal")]
    [SerializeField] private GameObject victoryPanel;            // UI shown when boss dies (already disabled in scene)
    [SerializeField] private GameObject portalPrefab;            // Prefab that transfers player to next scene
    [SerializeField] private Transform portalSpawnPoint;         // Where the portal should appear

    #endregion

    private bool isDead = false;
    private bool canAttack = true;
    private int currentHealth;

    private void Awake()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;
        UpdateHealthUI();

        if (spawnSystem == null)
        {
            spawnSystem = FindObjectOfType<EnemySpawnSystem_Boss2>();
        }
    }

    private void OnEnable()
    {
        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        while (!isDead)
        {
            if (canAttack)
            {
                PerformMissileAttack();
                yield return new WaitForSeconds(attackCooldown);
            }
            yield return null;
        }
    }

    private void PerformMissileAttack()
    {
        animator.SetTrigger("Attack");
        audioSource?.PlayOneShot(missileLaunchSfx);

        // Find target player (random among active players)
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length == 0) return;
        GameObject targetPlayer = players[Random.Range(0, players.Length)];

        // Spawn missile
        GameObject missileObj = Instantiate(missilePrefab, missileSpawnPoint.position, Quaternion.identity);
        Boss2_Missile missile = missileObj.GetComponent<Boss2_Missile>();
        if (missile != null)
        {
            missile.Initialize(targetPlayer.transform, missileDamage);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;
        currentHealth -= damage;
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        animator.SetTrigger("Die");
        audioSource?.PlayOneShot(deathSfx);

        // Stop spawner
        if (spawnSystem != null)
        {
            spawnSystem.StopSpawning();
        }

        // Activate victory panel
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }

        // Spawn portal
        if (portalPrefab != null && portalSpawnPoint != null)
        {
            Instantiate(portalPrefab, portalSpawnPoint.position, Quaternion.identity);
        }

        // Disable collider/other components
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        // Optionally destroy after delay
        Destroy(gameObject, 5f);
    }

    private void UpdateHealthUI()
    {
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
        if (healthText != null)
        {
            healthText.text = $"Boss: {currentHealth}/{maxHealth}";
        }
    }
} 