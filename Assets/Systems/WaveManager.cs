using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;
    public event Action WaveStarted;
    public event Action WaveEnded;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] private float spawnDelay = 0.5f;
    [SerializeField] private float spawnFrequency = 0.5f;
    [SerializeField] private float spawnRadius;
    [SerializeField] private int amountForWave;

    [Header("Wave")]
    private int wave = 1;
    private bool isWaveStarted = false;
    private bool isWaveTimerStarted = false;
    private float setTimebetweenWaves = 4;
    private float timebetweenWaves;

    public List<GameObject> enemiesInCurrentWave;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        StartGame();
    }
    void StartGame()
    {
        timebetweenWaves = setTimebetweenWaves;
        isWaveTimerStarted = true;
    }
    void Update()
    {
        if(isWaveTimerStarted)
        {
            NextWaveTimer();
        }
    }
    void NextWaveTimer()
    {
        if(timebetweenWaves > 0)
        {
            timebetweenWaves -= 1 * Time.deltaTime;
        }
        else
        {
            isWaveTimerStarted = false;
            StartWave();
        }
    }
    void UpdateWaveProgress()
    {
        enemiesInCurrentWave.RemoveAll(item => item == null);
        if(EnemiesLeftInWave() <= 0)
        {
            //End the wave
            EndWave();
        }
    }
    
    void StartWave()
    {
        //Tell player wave is starting
        //WaveStarted.Invoke();

        //Spawn enemies over timer or spawn all at once
        DetermineAmountForWave();
        InvokeRepeating("SpawnEnemiesOverTime",spawnDelay,spawnFrequency);
        isWaveStarted = true;
        InvokeRepeating("UpdateWaveProgress",1,1);
    }
    void DetermineAmountForWave()
    {
        if(wave <= 5)
        {
            amountForWave = UnityEngine.Random.Range(2,5);
        }
        else
        {
            amountForWave = UnityEngine.Random.Range(2,5) + (UnityEngine.Random.Range(1,2) * wave-5);
        }
        
    }
    void EndWave()
    {
        wave ++;
        CancelInvoke("UpdateWaveProgress");
        CancelInvoke("SpawnEnemiesOverTime");
        timebetweenWaves = setTimebetweenWaves;
        isWaveTimerStarted = true;
    }

    void SpawnEnemiesOverTime()
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
            enemiesInCurrentWave.Add(Instantiate(enemy,GetRandomPointAroundScreen(spawnRadius),quaternion.identity));
        }
        
    }
    Vector2 GetRandomPointAroundScreen(float radius)
    {
        float angle = UnityEngine.Random.Range(0, Mathf.PI * 2);
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
    }
    public int EnemiesLeftInWave()
    {
        if(isWaveStarted)
        {
            return enemiesInCurrentWave.Count;
        }
        else return 0;
    }
}
