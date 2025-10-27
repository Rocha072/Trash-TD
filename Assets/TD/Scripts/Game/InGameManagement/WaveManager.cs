using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    [Header("All Waves List")]
    public List<WaveDefinition> allWaves;

    private int currentWaveIndex = 0;
    private int enemiesAlive = 0;

    public enum WaveState
    {
        WaitingToStart, Spawning, WaitingForClear, AllWavesComplete
    }

    private WaveState currentState;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        UIManager.Instance.UpdateMoneyText();
        UIManager.Instance.UpdateLifeText();
        EntitySummoner.Instance.Init();
        currentState = WaveState.WaitingToStart;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Muda o estado do botao e
            StartNextWave();
        }
    }

    public void StartNextWave()
    {
        if (currentState != WaveState.WaitingToStart)
            return;

        if (currentWaveIndex >= allWaves.Count)
        {
            //Chama a tela de fase completa (ou funcao)
            Debug.Log("Ganhou");
            currentState = WaveState.AllWavesComplete;
            return;
        }

        //Muda o botao de Start Wave
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
            UIManager.Instance.UpdateMoneyText();            
            Debug.Log("Wave completed");

            if (currentWaveIndex >= allWaves.Count)
            {
                //Aparece tela de fase completa (ou funcao)
                Debug.Log("Fase completa");
                currentState = WaveState.AllWavesComplete;
            }

            else
            {
                //Atualiza botao de passar wave
                currentState = WaveState.WaitingToStart;
            }
        }
    }
}

