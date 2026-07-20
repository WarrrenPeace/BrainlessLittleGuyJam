using System;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;
    public static event Action waveStarted;
    public static event Action waveEnded;
    GoblinSpawner GS;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] private float spawnFrequency = 0.5f;
    [SerializeField] private float spawnRadius;
    private int amountForWave = 4;
    private int enemiesSpawnedSoFar;
    private int totalEnemiesForWave;
    

    [Header("Wave related")]
    [SerializeField] private int wave = 1;
    
    private bool isWaveStarted = false;
    private bool isWaveTimerStarted = false;
    [SerializeField] private float setTimebetweenWaves = 4;
    private float timebetweenWaves;
    

    

    [Header("UI")]
    [SerializeField] TextMeshProUGUI waveCOUNTER;
    [SerializeField] GameObject timerOBJECT;
    [SerializeField] TextMeshProUGUI timerTEXT;
    [SerializeField] GameObject waveProgressOBJECT;
    [SerializeField] Slider waveProgressSLIDER;
    
    
    float minuites;
    float seconds;

    public List<GameObject> enemiesInCurrentWave;
    void Awake()
    {
        instance = this;
        GS = GetComponent<GoblinSpawner>();
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
            UpdateTimer();
            if(!timerOBJECT.activeInHierarchy)
            {
                timerOBJECT.SetActive(true);
            }
        }
        else
        {
            if(timerOBJECT.activeInHierarchy)
            {
                timerOBJECT.SetActive(false);
            }
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
    void UpdateTimer()
    {
        DisplayTime(timebetweenWaves);
    }
    void DisplayTime(float timeRemaning)
    {
        minuites = Mathf.FloorToInt(timeRemaning / 60);
        seconds = Mathf.FloorToInt(timeRemaning % 60);

        timerTEXT.text = string.Format("{0:00}:{1:00}", minuites, seconds);
    }
    void UpdateWaveProgress()
    {
        enemiesInCurrentWave.RemoveAll(item => item == null);
        UpdateWaveProgressSlider();
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
        totalEnemiesForWave = amountForWave;
        SetUpWaveProgressSlider(true);
        waveCOUNTER.text = wave.ToString();
        isWaveStarted = true;
        InvokeRepeating("SpawnEnemiesOverTime",0,spawnFrequency);
        InvokeRepeating("UpdateWaveProgress",1,0.1f);
        
        
    }
    void DetermineAmountForWave()
    {
        if(wave <= 3)
        {
            amountForWave = UnityEngine.Random.Range(1,5);
            spawnFrequency = 1; 
            return;
        }
        else if (wave <= 35)
        {
            amountForWave = UnityEngine.Random.Range(3,6) + (UnityEngine.Random.Range(1,3) * wave-3);
            spawnFrequency = Mathf.Lerp(1f, 0f, (wave - 1f) / 40f);

            float normalized = Mathf.InverseLerp(1f, 40f, wave);
            spawnFrequency = 1f - normalized;
            return;
        }
        else
        {
            amountForWave = UnityEngine.Random.Range(7,10) + (UnityEngine.Random.Range(2,7) * wave-3);
            spawnFrequency = 0.2f;
        }
    }
    void SetUpWaveProgressSlider(bool active)
    {
        if(active)
        {
            waveProgressOBJECT.SetActive(true);
            waveProgressSLIDER.maxValue = amountForWave;
            waveProgressSLIDER.value = amountForWave;
        }
        else
        {
            waveProgressOBJECT.SetActive(false);
            enemiesSpawnedSoFar = 0;
            totalEnemiesForWave = 0;
        }
        
    }
    void UpdateWaveProgressSlider()
    {
        waveProgressSLIDER.value = totalEnemiesForWave - (enemiesSpawnedSoFar - EnemiesLeftInWave());
    }
    void EndWave()
    {
        waveEnded.Invoke();

        wave ++;
        SetUpWaveProgressSlider(false);
        CancelInvoke("UpdateWaveProgress");
        CancelInvoke("SpawnEnemiesOverTime");
        timebetweenWaves = setTimebetweenWaves;
        isWaveTimerStarted = true;

        //Spawn a new friendly gnome
        GS.SpawnGnomeAfterWave();
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
            enemiesSpawnedSoFar ++;
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
