using UnityEngine;

public class Enemy_Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 8f;
    [SerializeField] private float bulletLifetime = 2f;
    [SerializeField] private int damage = 1;

    [Header("Effects")]
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private float hitEffectDuration = 0.5f;

    private Vector2 direction;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void Start()
    {
        Destroy(gameObject, bulletLifetime);
    }

    void Update()
    {
        transform.Translate(direction * bulletSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hitEffectPrefab != null)
        {
            GameObject hitEffect = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            Destroy(hitEffect, hitEffectDuration);
        }

        if (other.CompareTag("Player"))
        {
            Character_Controller_V1 player = other.GetComponent<Character_Controller_V1>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        else if (other.CompareTag("Ground") || other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }
    }
} 