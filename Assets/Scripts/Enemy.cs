using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private int health = 3;

    [Header("Player Tracking")]
    [SerializeField] private bool followPlayer = true;
    [SerializeField] private float followRange = 5f; // Oyuncuyu takip etme mesafesi
    private Transform player;

    [Header("Shooting Settings")]
    [SerializeField] private bool canShoot = true;
    [SerializeField] private float shootRange = 7f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private GameObject enemyBulletPrefab;
    [SerializeField] private Transform firePoint;
    private float nextFireTime = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (followPlayer || canShoot)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Follow player if in range
        if (followPlayer && distanceToPlayer <= followRange)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            transform.Translate(direction * moveSpeed * Time.deltaTime);
        }

        // Shoot at player if in range
        if (canShoot && distanceToPlayer <= shootRange && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        if (enemyBulletPrefab != null && firePoint != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            GameObject bullet = Instantiate(enemyBulletPrefab, firePoint.position, Quaternion.identity);
            Enemy_Bullet bulletScript = bullet.GetComponent<Enemy_Bullet>();
            
            if (bulletScript != null)
            {
                bulletScript.SetDirection(direction);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            health--;
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
