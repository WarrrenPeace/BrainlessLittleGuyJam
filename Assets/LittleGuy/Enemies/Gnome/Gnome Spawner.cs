using Unity.Mathematics;
using UnityEngine;

public class GoblinSpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] private float spawnDelay = 0.5f;
    [SerializeField] private float spawnFrequency = 0.5f;
    [SerializeField] private float spawnRadius;
    [SerializeField] private int amountForWave = 10;
    void Start()
    {
        StartSpawnSequence();
    }
    void StartSpawnSequence()
    {
        InvokeRepeating("SpawnWave",spawnDelay,spawnFrequency);
        //SpawnWave();
    }

    void SpawnWave()
    {
        if(amountForWave >= 1)
        {
            amountForWave --;
            SpawnEnemyToAttackPlayer(enemyPrefab);
        }
    }
    void SpawnEnemyToAttackPlayer(GameObject enemy)
    {
        if(enemy != null)
        {
            Instantiate(enemy,GetRandomPointAroundScreen(spawnRadius),quaternion.identity);
        }
        else
        {
            return;
        }
        
    }
    Vector2 GetRandomPointAroundScreen(float radius)
    {
        float angle = UnityEngine.Random.Range(0, Mathf.PI * 2);
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
    }
}
