using UnityEngine;
using System.Collections.Generic;

public class EnemySpawnSystem_Boss2 : MonoBehaviour
{
    [System.Serializable]
    public class SpawnPoint
    {
        public Transform point;              // Location to spawn enemy
        public GameObject enemyPrefab;       // Which enemy to spawn
        [HideInInspector] public GameObject spawnedEnemy; // Runtime reference
        [HideInInspector] public bool isRespawning = false; // Added for per-spawnPoint respawn state
    }

    [Header("Spawn Points")]
    [SerializeField] private List<SpawnPoint> spawnPoints = new List<SpawnPoint>();

    [Header("Respawn Settings")]
    [SerializeField] private float respawnDelay = 5f; // Delay after enemy death before respawning

    private bool isSpawningEnabled = true;

    private void Update()
    {
        if (!isSpawningEnabled) return;

        foreach (var sp in spawnPoints)
        {
            if (sp.spawnedEnemy == null && !sp.isRespawning)
            {
                sp.isRespawning = true; // Respawn süreci başladı
                StartCoroutine(SpawnAfterDelay(sp));
            }
        }
    }

    private System.Collections.IEnumerator SpawnAfterDelay(SpawnPoint sp)
    {
        yield return new WaitForSeconds(respawnDelay);

        if (!isSpawningEnabled) {
            sp.isRespawning = false;
            yield break;
        }

        if (sp.spawnedEnemy == null)
        {
            sp.spawnedEnemy = Instantiate(sp.enemyPrefab, sp.point.position, Quaternion.identity);
        }

        sp.isRespawning = false;
    }

    public void StopSpawning()
    {
        isSpawningEnabled = false;
    }

    private void LateUpdate()
    {
        // Clear references to destroyed enemies (Unity sets them to null after Destroy())
        foreach (var sp in spawnPoints)
        {
            if (sp.spawnedEnemy == null) continue;
            if (sp.spawnedEnemy.Equals(null))
            {
                sp.spawnedEnemy = null;
            }
        }
    }
} 