using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject[] enemyPrefabs; // Inspector'da düşman prefablarını atayabileceğiniz dizi
    [SerializeField] private float spawnInterval = 2f; // Düşmanlar arası spawn süresi
    [SerializeField] private float initialDelay = 1f; // Başlangıç gecikmesi
    [SerializeField] private Transform[] spawnPoints; // Spawn noktaları

    [Header("Boss Reference")]
    [SerializeField] private Boss1 boss; // Boss1 referansı

    private bool isSpawning = true;

    private void Start()
    {
        if (boss == null)
        {
            boss = FindObjectOfType<Boss1>();
        }

        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(initialDelay);

        while (isSpawning && boss != null)
        {
            if (enemyPrefabs.Length > 0 && spawnPoints.Length > 0)
            {
                // Rastgele bir düşman ve spawn noktası seç
                GameObject randomEnemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
                Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

                // Düşmanı spawn et
                Instantiate(randomEnemy, randomSpawnPoint.position, Quaternion.identity);
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // Boss öldüğünde çağrılacak metod
    public void StopSpawning()
    {
        isSpawning = false;
    }
} 