using UnityEngine;

public class Bullet_V2 : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float lifeTime = 2f;
    public int damage = 1;
    public LayerMask targetLayers;
    public GameObject hitEffect;

    private void OnEnable()
    {
        Invoke("DeactivateBullet", lifeTime);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & targetLayers) != 0)
        {
            // Check if the hit object has a health component
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }

            // Spawn hit effect if assigned
            if (hitEffect != null)
            {
                Instantiate(hitEffect, transform.position, Quaternion.identity);
            }

            DeactivateBullet();
        }
    }

    private void DeactivateBullet()
    {
        gameObject.SetActive(false);
    }
}

// Interface for objects that can take damage
public interface IDamageable
{
    void TakeDamage(int damage);
} 