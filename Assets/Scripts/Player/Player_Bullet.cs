using UnityEngine;

public class bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 10f;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, bulletLifetime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * bulletSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Bullet hit: {other.gameObject.name} with tag: {other.tag}"); // Debug log

        // Spawn hit effect
        if (hitEffectPrefab != null)
        {
            GameObject hitEffect = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            Destroy(hitEffect, hitEffectDuration);
        }

        if (other.CompareTag("Enemy"))
        {
            // Handle enemy damage
            Destroy(gameObject);
        }
        else if (other.CompareTag("Ground") || other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Debug.Log("Bullet hit ground!"); // Debug log
            Destroy(gameObject);
        }
    }
}
