using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class EntitySummoner : MonoBehaviour
{
    public static EntitySummoner Instance { get; private set; }

    [Header("Enemy Configuration")]
    public List<EnemyBlueprint> enemyBlueprints;
    private Dictionary<int, Queue<Enemy>> enemiesDisabled;  //tem que mudar

    [Header("Tower Configuration")]
    public List<TowerBlueprint> towerBlueprints;


    [Header("In Game Objects")]
    public GameObject Spawnner;
    public List<Transform> enemyPath;
    public List<Enemy> EnemiesInGame;
    public List<Tower> TowersInGame;


    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void Init()
    {

        enemiesDisabled = new Dictionary<int, Queue<Enemy>>();

        EnemiesInGame = new List<Enemy>();

        enemyPath = new List<Transform>();

        TowersInGame = new List<Tower>();



        if (Spawnner != null)
        {
            foreach (Transform childNode in Spawnner.transform)
            {
                enemyPath.Add(childNode);
            }
        }
        else
            Debug.LogError("Objeto 'Enemy Spawner' não encontrado!");



        for (int i = 0; i < enemyBlueprints.Count; i++)
        {
            enemiesDisabled.Add(i, new Queue<Enemy>());
        }

    }


    public Enemy SummonEnemy(int EnemyID, Vector3 spawnPosition = default, int startNodeIndex = 0)
    {
        if (EnemyID < 0 || EnemyID >= towerBlueprints.Count)
        {
            Debug.Log($"Enemy with ID {EnemyID} not found");
            return null;
        }

        if (spawnPosition == Vector3.zero) 
            spawnPosition = Spawnner.transform.position;
        
        Enemy SummonedEnemy;

        EnemyBlueprint enemyToSummon = enemyBlueprints[EnemyID];

        Queue<Enemy> ReferencedQueue = enemiesDisabled[EnemyID];

        if (ReferencedQueue.Count > 0)
        {
            SummonedEnemy = ReferencedQueue.Dequeue();
            SummonedEnemy.gameObject.SetActive(true);
            SummonedEnemy.transform.position = spawnPosition;
            SummonedEnemy.RestartState(startNodeIndex);
        }
        else
        {
            GameObject NewEnemy = Instantiate(enemyToSummon.enemyPrefab, spawnPosition, Quaternion.identity);
            SummonedEnemy = NewEnemy.GetComponent<Enemy>();
            SummonedEnemy.Path = enemyPath;
            SummonedEnemy.Init(enemyToSummon.enemyData, startNodeIndex);
        }

        
        EnemiesInGame.Add(SummonedEnemy);
        return SummonedEnemy;
    }


    public void RemoveEnemy(Enemy EnemyToRemove)
    {
        enemiesDisabled[EnemyToRemove.enemyData.ID].Enqueue(EnemyToRemove);
        EnemyToRemove.gameObject.SetActive(false);
        EnemiesInGame.Remove(EnemyToRemove);
    }

    public void SpawnEnemiesWithDelay(List<EnemyDeathSpawnInfo> spawns, Vector3 position, int nodeIndex, float delayBetweenSpawns = 0.1f)
    {
        StartCoroutine(SpawnRoutine(spawns, position, nodeIndex, delayBetweenSpawns));
    }


    private IEnumerator SpawnRoutine(List<EnemyDeathSpawnInfo> spawns, Vector3 position, int nodeIndex, float delay)
    {
        foreach (EnemyDeathSpawnInfo group in spawns)
        {
            for (int i = 0; i < group.amount; i++)
            {
                SummonEnemy(group.enemyID, position, nodeIndex);
                yield return new WaitForSeconds(delay);
            }
        }
    }
    
    public Tower SummonTower(int TowerID, Vector3 positionToSpawn)
    {

        if (TowerID < 0 || TowerID >= towerBlueprints.Count)
        {
            Debug.Log($"Tower with ID {TowerID} not found");
            return null;
        }

        TowerBlueprint towerToSummon = towerBlueprints[TowerID];

        GameObject towerInstance = Instantiate(towerToSummon.towerPrefab, positionToSpawn, Quaternion.identity);

        Tower tower = towerInstance.GetComponent<Tower>();
        TowersInGame.Add(tower);

        if (tower != null)
            tower.Init(towerToSummon.towerData);
        else
            Debug.LogError("O prefab da torre não contém o script TowerController!");

        return tower;

    }

    public void RemoveTower(Tower TowerToRemove)
    {
        TowersInGame.Remove(TowerToRemove);
        StartCoroutine(SafeDestroyRoutine(TowerToRemove));
    }

    private IEnumerator SafeDestroyRoutine(Tower TowerToRemove)
    {
        TowerToRemove.transform.position = new Vector3(1000, 1000, 1000);

        yield return new WaitForFixedUpdate();
        
        Destroy(TowerToRemove.gameObject);
    }

}

[System.Serializable]
public class TowerBlueprint
{
    public TowerData towerData;
    public GameObject towerPrefab;

}

[System.Serializable]
public class EnemyBlueprint
{
    public EnemyData enemyData;
    public GameObject enemyPrefab;

}



