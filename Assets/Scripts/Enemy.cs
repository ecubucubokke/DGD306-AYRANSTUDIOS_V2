using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private int health = 3;

    [Header("Player Tracking")]
    [SerializeField] private bool followPlayer = true;
    [SerializeField] private float followRange = 5f; // Oyuncuyu takip etme mesafesi
    private Transform[] players = new Transform[2];
    private Transform currentTarget;

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
            // Find both players
            GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
            for (int i = 0; i < playerObjects.Length && i < 2; i++)
            {
                players[i] = playerObjects[i].transform;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (players[0] == null && players[1] == null) return;

        // Find closest player
        currentTarget = FindClosestPlayer();

        if (currentTarget != null)
        {
            float distanceToTarget = Vector2.Distance(transform.position, currentTarget.position);

            // Follow player if in range
            if (followPlayer && distanceToTarget <= followRange)
            {
                Vector2 direction = (currentTarget.position - transform.position).normalized;
                transform.Translate(direction * moveSpeed * Time.deltaTime);
            }

            // Shoot at player if in range
            if (canShoot && distanceToTarget <= shootRange && Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    private Transform FindClosestPlayer()
    {
        Transform closest = null;
        float closestDistance = float.MaxValue;

        foreach (Transform player in players)
        {
            if (player != null)
            {
                float distance = Vector2.Distance(transform.position, player.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closest = player;
                }
            }
        }

        return closest;
    }

    void Shoot()
    {
        if (enemyBulletPrefab != null && firePoint != null && currentTarget != null)
        {
            Vector2 direction = (currentTarget.position - transform.position).normalized;
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
