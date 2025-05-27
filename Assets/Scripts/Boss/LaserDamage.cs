using UnityEngine;

public class LaserDamage : MonoBehaviour
{
    private int damage;

    public void SetDamage(int damageAmount)
    {
        damage = damageAmount;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Character_Controller_V1 player = other.GetComponent<Character_Controller_V1>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
    }
} 