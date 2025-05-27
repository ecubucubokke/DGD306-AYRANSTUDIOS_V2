using UnityEngine;

public class BossLaser : MonoBehaviour
{
    [SerializeField] private float damage = 20f;
    [SerializeField] private float maxLength = 20f;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private LayerMask playerLayer;

    private void Start()
    {
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }
    }

    private void Update()
    {
        // Cast ray to detect player
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, maxLength, playerLayer);
        
        // Update line renderer
        lineRenderer.SetPosition(0, transform.position);
        if (hit.collider != null)
        {
            lineRenderer.SetPosition(1, hit.point);
            
            // Deal damage to player
            Character_Controller_V1 player = hit.collider.GetComponent<Character_Controller_V1>();
            if (player != null)
            {
                player.TakeDamage((int)damage);
            }
        }
        else
        {
            lineRenderer.SetPosition(1, transform.position + transform.right * maxLength);
        }
    }
} 