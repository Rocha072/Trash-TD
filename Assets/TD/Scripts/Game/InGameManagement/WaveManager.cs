using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.InputSystem;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    [Header("All Waves List")]
    public List<WaveDefinition> allWaves;

    public int currentWaveIndex;
    private int enemiesAlive;

    [Header("Musics")]
    public AudioClip faseMusic;

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
        SoundHandler.Instance.PlayMusic(faseMusic, 0.01f);
        currentWaveIndex = 0;
        enemiesAlive = 0;
        UIManager.Instance.UpdateCurrentWaveText();
        UIManager.Instance.UpdateMoneyText();
        UIManager.Instance.UpdateLifeText();
        EntitySummoner.Instance.Init();
        currentState = WaveState.WaitingToStart;
    }


    public void StartNextWave()
    {
        if (currentState != WaveState.WaitingToStart)
            return;

        if (currentWaveIndex >= allWaves.Count)
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
                     
            Debug.Log("Wave completed");

            if (currentWaveIndex >= allWaves.Count)
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

