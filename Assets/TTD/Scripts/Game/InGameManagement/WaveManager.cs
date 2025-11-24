using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    [Header("All Waves List")]
    public List<WaveDefinition> allWaves;

    public int currentWaveIndex;
    public int maxWavesForThisDifficulty;
    private int enemiesAlive;

    [Header("Musics")]
    public AudioClip faseMusic;
    public float musicVolume;

    public enum WaveState
    {
        WaitingToStart, Spawning, WaitingForClear, AllWavesComplete
    }

    public WaveState currentState;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        SoundHandler.Instance.PlayMusic(faseMusic, musicVolume);
        currentWaveIndex = 0;
        enemiesAlive = 0;
        SetupDifficulty();
        UIManager.Instance.UpdateCurrentWaveText();
        UIManager.Instance.UpdateMoneyText();
        UIManager.Instance.UpdateLifeText();
        EntitySummoner.Instance.Init();
        currentState = WaveState.WaitingToStart;
    }

    void SetupDifficulty()
    {
        switch (LevelSettings.DifficultyChosed)
        {
            case Difficulty.Easy:
                maxWavesForThisDifficulty = Mathf.Min(5, allWaves.Count); 
                break;
            case Difficulty.Medium:
                maxWavesForThisDifficulty = Mathf.Min(10, allWaves.Count);
                break;
            case Difficulty.Hard:
            default:
                maxWavesForThisDifficulty = allWaves.Count; 
                break;
        }
    }


    public void StartNextWave()
    {
        if (currentState != WaveState.WaitingToStart)
            return;

        if (currentWaveIndex >= maxWavesForThisDifficulty)
        {
            UIManager.Instance.showVictoryScreen();
            currentState = WaveState.AllWavesComplete;
            return;
        }

        
        StartCoroutine(RunWave(allWaves[currentWaveIndex]));
        currentWaveIndex++;
    }

    private IEnumerator RunWave(WaveDefinition wave)
    {
        currentState = WaveState.Spawning;
        
        int groupsRunning = wave.enemyGroups.Count;

        System.Action onGroupFinished = () => { groupsRunning--; };

        foreach (EnemyGroup group in wave.enemyGroups)
        {
            StartCoroutine(RunEnemyGroup(group, onGroupFinished));
        }

        yield return new WaitUntil(() => groupsRunning == 0);

        currentState = WaveState.WaitingForClear;
        CheckWaveCompletion();

    }

    private IEnumerator RunEnemyGroup(EnemyGroup group, System.Action onFinishedCall)
    {
        yield return new WaitForSeconds(group.delayBeforeStartGroup);

        for (int i = 0; i < group.count; i++)
        {
            EntitySummoner.Instance.SummonEnemy(group.enemyID);
            OnEnemySpawned();

            if (i < group.count - 1)
            {
                yield return new WaitForSeconds(group.spawnIntervalBetweenMembers);
            }
        }

        onFinishedCall?.Invoke();
    }


    public void OnEnemySpawned()
    {
        enemiesAlive++;
    }

    //Chamar essa funcao quando qualquer inimigo morre
    public void OnEnemyDied()
    {
        enemiesAlive--;  
        CheckWaveCompletion();
    }

    private void CheckWaveCompletion()
    {
        if(currentState == WaveState.WaitingForClear && enemiesAlive == 0)
        {
            WaveDefinition completedWave = allWaves[currentWaveIndex - 1];
            PlayerEconomy.Instance.GainMoney(completedWave.waveReward);
            StatsManager.Instance.AddMoneyGeneratedByWaves(completedWave.waveReward);

            if (currentWaveIndex >= maxWavesForThisDifficulty)
            {
                UIManager.Instance.showVictoryScreen();
                currentState = WaveState.AllWavesComplete;
            }

            else
            {
                UIManager.Instance.UpdateCurrentWaveText(); 
                currentState = WaveState.WaitingToStart;
            }
        }
    }
}

