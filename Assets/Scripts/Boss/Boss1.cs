using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Boss1 : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float moveRange = 4f;
    [SerializeField] private float moveDelay = 2f;
    [SerializeField] private float rightEdgeOffset = 2f;

    [Header("Attack Settings")]
    [SerializeField] private float attackCooldown = 5f;
    [SerializeField] private float laserDuration = 3f;
    [SerializeField] private float laserRotationSpeed = 30f;
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private Transform laserSpawnPoint;
    [SerializeField] private int laserDamage = 20;
    [SerializeField] private float laserWindup = 0.4f;

    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 500;
    [SerializeField] private int currentHealth;
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI healthText;

    [Header("UI Settings")]
    [SerializeField] private GameObject victoryTextPrefab;
    [SerializeField] private float victoryTextDuration = 3f;

    [Header("Portal Settings")]
    [SerializeField] private GameObject portalPrefab;
    [SerializeField] private Transform portalSpawnPoint;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip laserSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioSource audioSource;

    [Header("Spawner Reference")]
    [SerializeField] private EnemySpawner enemySpawner;

    private Animator animator;
    private bool isMovingUp = true;
    private bool canAttack = true;
    private bool isDead = false;
    private GameObject currentLaser;
    private Vector3 startPosition;
    private float screenTop;
    private float screenBottom;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;
        UpdateHealthUI();

        if (enemySpawner == null)
        {
            enemySpawner = FindObjectOfType<EnemySpawner>();
        }

        // Calculate screen boundaries
        Camera mainCamera = Camera.main;
        screenTop = mainCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;
        screenBottom = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;

        // Set initial position
        startPosition = new Vector3(
            mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - rightEdgeOffset,
            transform.position.y,
            transform.position.z
        );
        transform.position = startPosition;

        StartCoroutine(MovementRoutine());
        StartCoroutine(AttackRoutine());
    }

    private IEnumerator MovementRoutine()
    {
        while (!isDead)
        {
            // Calculate target position using moveRange
            float targetY = isMovingUp ? 
                startPosition.y + moveRange : 
                startPosition.y - moveRange;
            
            Vector3 targetPosition = new Vector3(transform.position.x, targetY, transform.position.z);

            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    targetPosition,
                    moveSpeed * Time.deltaTime
                );
                yield return null;
            }

            isMovingUp = !isMovingUp;
            yield return new WaitForSeconds(moveDelay);
        }
    }

    private IEnumerator AttackRoutine()
    {
        while (!isDead)
        {
            if (canAttack)
            {
                StartCoroutine(PerformLaserAttack());
                yield return new WaitForSeconds(attackCooldown);
            }
            yield return null;
        }
    }

    private IEnumerator PerformLaserAttack()
    {
        canAttack = false;
        animator.SetTrigger("Attack");

        // Wind-up delay allows animation to play before firing
        yield return new WaitForSeconds(laserWindup);

        audioSource.PlayOneShot(laserSound);

        // Spawn laser
        currentLaser = Instantiate(laserPrefab, laserSpawnPoint.position, Quaternion.identity);
        currentLaser.transform.parent = transform;

        // Set laser damage
        LaserDamage laserDamageComponent = currentLaser.GetComponent<LaserDamage>();
        if (laserDamageComponent != null)
        {
            laserDamageComponent.SetDamage(laserDamage);
        }

        float elapsedTime = 0f;
        while (elapsedTime < laserDuration)
        {
            // Rotate laser to follow player
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                Vector3 direction = player.transform.position - currentLaser.transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                currentLaser.transform.rotation = Quaternion.RotateTowards(
                    currentLaser.transform.rotation,
                    Quaternion.Euler(0, 0, angle),
                    laserRotationSpeed * Time.deltaTime
                );
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(currentLaser);
        canAttack = true;
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
        audioSource.PlayOneShot(deathSound);
        
        // Notify enemy spawner to stop spawning
        if (enemySpawner != null)
        {
            enemySpawner.StopSpawning();
        }

        // Show victory text
        if (victoryTextPrefab != null)
        {
            GameObject victoryText = Instantiate(victoryTextPrefab, Vector3.zero, Quaternion.identity);
            victoryText.transform.SetParent(GameObject.Find("Canvas").transform, false);
            Destroy(victoryText, victoryTextDuration);
        }

        // Spawn portal
        if (portalPrefab != null && portalSpawnPoint != null)
        {
            Instantiate(portalPrefab, portalSpawnPoint.position, Quaternion.identity);
        }

        // Disable colliders and other components
        GetComponent<Collider2D>().enabled = false;
        if (currentLaser != null)
        {
            Destroy(currentLaser);
        }
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