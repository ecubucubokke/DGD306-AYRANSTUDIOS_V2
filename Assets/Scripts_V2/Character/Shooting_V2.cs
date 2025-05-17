using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class Shooting_V2 : MonoBehaviour
{
    [Header("Bullet Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 10f;
    public float fireRate = 0.2f;
    public int poolSize = 20;

    [Header("References")]
    private Character_Controller_V3 characterController;

    private float nextFireTime;
    private Queue<GameObject> bulletPool;
    private bool isFacingRight = true;

    void Awake()
    {
        characterController = GetComponent<Character_Controller_V3>();
        InitializeBulletPool();
    }

    void InitializeBulletPool()
    {
        bulletPool = new Queue<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            bulletPool.Enqueue(bullet);
        }
    }

    private GameObject GetBullet()
    {
        if (bulletPool.Count > 0)
        {
            GameObject bullet = bulletPool.Dequeue();
            bullet.SetActive(true);
            return bullet;
        }
        return Instantiate(bulletPrefab);
    }

    private void ReturnBulletToPool(GameObject bullet)
    {
        bullet.SetActive(false);
        bulletPool.Enqueue(bullet);
    }

    private void FireBullet(Vector2 direction)
    {
        if (Time.time >= nextFireTime)
        {
            GameObject bullet = GetBullet();
            bullet.transform.position = firePoint.position;
            bullet.GetComponent<Rigidbody2D>().linearVelocity = direction * bulletSpeed;
            nextFireTime = Time.time + fireRate;
        }
    }

    public void OnFireRight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            FireBullet(Vector2.right);
        }
    }

    public void OnFireUpRight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            FireBullet((Vector2.right + Vector2.up).normalized);
        }
    }

    public void OnFireUp(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            FireBullet(Vector2.up);
        }
    }

    public void OnFireUpLeft(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            FireBullet((Vector2.left + Vector2.up).normalized);
        }
    }

    public void OnFireLeft(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            FireBullet(Vector2.left);
        }
    }

    public void OnFireDownLeft(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            FireBullet((Vector2.left + Vector2.down).normalized);
        }
    }

    public void OnFireDown(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            FireBullet(Vector2.down);
        }
    }

    public void OnFireDownRight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            FireBullet((Vector2.right + Vector2.down).normalized);
        }
    }
}