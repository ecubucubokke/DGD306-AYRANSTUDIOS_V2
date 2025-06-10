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

    public void Initialize(Transform targetTransform, int damageAmount)
    {
        target = targetTransform;
        damage = damageAmount;
    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // Homing movement
        Vector2 direction = (Vector2)target.position - (Vector2)transform.position;
        direction.Normalize();
        float rotateAmount = Vector3.Cross(direction, transform.right).z;
        GetComponent<Rigidbody2D>().angularVelocity = -rotateAmount * rotateSpeed;
        GetComponent<Rigidbody2D>().linearVelocity = transform.right * speed;
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
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
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