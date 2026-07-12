using Unity.Mathematics;
using UnityEngine;

public class LineSpawner : MonoBehaviour
{
    LineRenderer LR;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] private float spawnDelay = 0.5f;
    [SerializeField] private float spawnFrequency = 0.5f;
    [SerializeField] private float lineHeight;
    [SerializeField] private int amountForWave = 10;
    
    void Start()
    {
        LR = GetComponent<LineRenderer>();
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
            Instantiate(enemy,GetRandomPointAlongLine(LR.GetPosition(0),LR.GetPosition(1)),quaternion.identity);
        }
        else
        {
            return;
        }
        
    }
    Vector2 GetRandomPointAlongLine(Vector2 start, Vector2 end)
    {
        Vector3 direction = end - start;
        return start + UnityEngine.Random.value * (Vector2)direction;
    }
}
