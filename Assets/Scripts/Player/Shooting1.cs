using UnityEngine;

public class Shooting1 : MonoBehaviour
{
    [Header("Shooting Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private float bulletSpeed = 10f;

    [Header("Effects")]
    [SerializeField] private GameObject shootEffectPrefab;
    [SerializeField] private float shootEffectDuration = 0.2f;

    private float nextFireTime = 0f;
    private Character_Controller_V1 characterController;
    private Vector2 lastDirection = Vector2.right; // Default to right at start

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<Character_Controller_V1>();
    }

    // Update is called once per frame
    void Update()
    {
        // Update last direction based on input
        UpdateLastDirection();

        // Check for shooting input
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void UpdateLastDirection()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        if (horizontalInput != 0 || verticalInput != 0)
        {
            lastDirection = new Vector2(horizontalInput, verticalInput).normalized;
        }
    }

    void Shoot()
    {
        // Get the direction based on input
        Vector2 shootDirection = GetShootDirection();
        
        // Spawn shoot effect
        if (shootEffectPrefab != null)
        {
            GameObject shootEffect = Instantiate(shootEffectPrefab, firePoint.position, Quaternion.identity);
            Destroy(shootEffect, shootEffectDuration);
        }

        // Instantiate bullet
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet bulletScript = bullet.GetComponent<bullet>();
        
        if (bulletScript != null)
        {
            bulletScript.SetDirection(shootDirection);
        }
    }

    Vector2 GetShootDirection()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // If there's any input, use it to determine direction
        if (horizontalInput != 0 || verticalInput != 0)
        {
            return new Vector2(horizontalInput, verticalInput).normalized;
        }
        // If no input, use the last direction
        return lastDirection;
    }
}
