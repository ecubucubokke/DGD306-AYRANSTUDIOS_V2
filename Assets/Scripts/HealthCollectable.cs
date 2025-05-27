using UnityEngine;

public class HealthCollectable : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int healAmount = 10; // İyileştirme miktarı
    [SerializeField] private float moveSpeed = 3f; // Toplanabilir objenin hareket hızı
    [SerializeField] private float magnetRange = 3f; // Oyuncuya çekilme mesafesi

    private Transform player; // En yakın oyuncu
    private bool isMovingToPlayer = false;

    void Start()
    {
        // En yakın oyuncuyu bul
        FindNearestPlayer();
    }

    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            // Oyuncu yakındaysa objeyi oyuncuya doğru hareket ettir
            if (distanceToPlayer <= magnetRange)
            {
                isMovingToPlayer = true;
            }

            if (isMovingToPlayer)
            {
                Vector2 direction = (player.position - transform.position).normalized;
                transform.Translate(direction * moveSpeed * Time.deltaTime);
            }
        }
    }

    void FindNearestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        float closestDistance = float.MaxValue;

        foreach (GameObject playerObj in players)
        {
            float distance = Vector2.Distance(transform.position, playerObj.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                player = playerObj.transform;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Character_Controller_V1 playerController = other.GetComponent<Character_Controller_V1>();
            if (playerController != null)
            {
                // Oyuncunun canını iyileştir
                playerController.Heal(healAmount);
                // Objeyi yok et
                Destroy(gameObject);
            }
        }
    }
} 