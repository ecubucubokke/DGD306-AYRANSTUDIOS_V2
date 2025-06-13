using UnityEngine;

public class Boss2_Missile : MonoBehaviour
{
    [Header("Missile Settings")]
    [SerializeField] private float speed = 5f;            // Travel speed
    [SerializeField] private float rotateSpeed = 200f;    // Homing rotation speed
    [SerializeField] private float explosionRadius = 2.5f;// Radius of explosion
    [SerializeField] private GameObject explosionEffect;  // VFX prefab (optional)

    private Transform target;
    private int damage;
    private Rigidbody2D rb;

    public void Initialize(Transform targetTransform, int damageAmount)
    {
        target = targetTransform;
        damage = damageAmount;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError($"{name}: Rigidbody2D component missing!");
        }
    }

    private void FixedUpdate()
    {
        if (target == null || rb == null)
        {
            Destroy(gameObject);
            return;
        }

        // Calculate direction towards target
        Vector2 direction = ((Vector2)target.position - rb.position).normalized;

        // Determine rotation amount; use transform.up as missile forward axis
        float rotateAmount = Vector3.Cross(direction, transform.up).z;
        rb.angularVelocity = -rotateAmount * rotateSpeed;

        // Propel missile forward constantly
        rb.linearVelocity = transform.up * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Explode();
    }

    private void Explode()
    {
        // Visual effect
        if (explosionEffect != null)
        {
            GameObject effect = Instantiate(explosionEffect, transform.position, Quaternion.identity);

            // Eğer ParticleSystem içeriyorsa, süresi kadar bekleyip yok et
            ParticleSystem ps = effect.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                Destroy(effect, ps.main.duration + ps.main.startLifetime.constantMax);
            }
            else
            {
                // Aksi halde varsayılan bir süre sonra yok et
                Destroy(effect, 0.5f);
            }
        }

        // Damage players in area
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (var col in colliders)
        {
            if (col.CompareTag("Player"))
            {
                Character_Controller_V1 player = col.GetComponent<Character_Controller_V1>();
                if (player != null)
                {
                    player.TakeDamage(damage);
                }
            }
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
} 