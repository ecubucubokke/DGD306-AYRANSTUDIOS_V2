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
    private Vector2 lastDirection = Vector2.right;
    private PlayerInputManager inputManager;
    private Vector2 moveInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<Character_Controller_V1>();
        inputManager = GetComponent<PlayerInputManager>();
        
        // Subscribe to events
        inputManager.OnMove += HandleMove;
        inputManager.OnFire += HandleFire;
    }

    void OnDestroy()
    {
        // Unsubscribe from events
        if (inputManager != null)
        {
            inputManager.OnMove -= HandleMove;
            inputManager.OnFire -= HandleFire;
        }
    }

    void HandleMove(Vector2 input)
    {
        moveInput = input;
        if (input != Vector2.zero)
        {
            lastDirection = input.normalized;
        }
    }

    void HandleFire()
    {
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Update()
    {
        // Sürekli ateş etme kontrolü
        if (inputManager.IsFirePressed() && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        Vector2 shootDirection = GetShootDirection();
        
        if (shootEffectPrefab != null)
        {
            GameObject shootEffect = Instantiate(shootEffectPrefab, firePoint.position, Quaternion.identity);
            Destroy(shootEffect, shootEffectDuration);
        }

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet bulletScript = bullet.GetComponent<bullet>();
        
        if (bulletScript != null)
        {
            bulletScript.SetDirection(shootDirection);
        }
    }

    Vector2 GetShootDirection()
    {
        if (moveInput != Vector2.zero)
        {
            return moveInput.normalized;
        }
        return lastDirection;
    }
}
