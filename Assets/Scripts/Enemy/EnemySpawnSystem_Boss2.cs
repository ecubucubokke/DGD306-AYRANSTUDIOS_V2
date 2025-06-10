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
    }

    [Header("Spawn Points")]
    [SerializeField] private List<SpawnPoint> spawnPoints = new List<SpawnPoint>();

    [Header("Respawn Settings")]
    [SerializeField] private float respawnDelay = 2f; // Delay after enemy death before respawning

    private bool isSpawningEnabled = true;

    private void Update()
    {
        if (!isSpawningEnabled) return;

        foreach (var sp in spawnPoints)
        {
            if (sp.spawnedEnemy == null)
            {
                // Try respawn if delay passed
                StartCoroutine(SpawnAfterDelay(sp));
            }
        }
    }

    private System.Collections.IEnumerator SpawnAfterDelay(SpawnPoint sp)
    {
        // Prevent multiple coroutines for same point
        if (sp.spawnedEnemy != null) yield break;
        yield return new WaitForSeconds(respawnDelay);

        if (!isSpawningEnabled || sp.spawnedEnemy != null) yield break;

        sp.spawnedEnemy = Instantiate(sp.enemyPrefab, sp.point.position, Quaternion.identity);
    }

    public void StopSpawning()
    {
        isSpawningEnabled = false;
    }
} 