using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class EntitySummoner : MonoBehaviour
{
    public static EntitySummoner Instance { get; private set; }

    [Header("Enemy Configuration")]
    public Dictionary<int, GameObject> EnemyPrefabs; //tem que mudar
    public Dictionary<int, Queue<Enemy>> EnemyObjectPools;  //tem que mudar

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
       
        EnemyPrefabs = new Dictionary<int, GameObject>(); //tem que mudar
        EnemyObjectPools = new Dictionary<int, Queue<Enemy>>(); //tem que mudar
        
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



        EnemySummonData[] EnemiesScrObj = Resources.LoadAll<EnemySummonData>("Enemies");

        foreach (EnemySummonData enemy in EnemiesScrObj)
        {
            EnemyPrefabs.Add(enemy.EnemyID, enemy.EnemyPrefab);
            EnemyObjectPools.Add(enemy.EnemyID, new Queue<Enemy>());
            }

    }


    public Enemy SummonEnemy(int EnemyID)
    {
        Enemy SummonedEnemy = null;

        if (EnemyPrefabs.ContainsKey(EnemyID))
        {
            Queue<Enemy> ReferencedQueue = EnemyObjectPools[EnemyID];

            if (ReferencedQueue.Count > 0)
            {
                SummonedEnemy = ReferencedQueue.Dequeue();
                SummonedEnemy.gameObject.SetActive(true);
                SummonedEnemy.transform.position = Spawnner.transform.position;
                SummonedEnemy.Init();
            }
            else
            {
                GameObject NewEnemy = Instantiate(EnemyPrefabs[EnemyID], Spawnner.transform);
                SummonedEnemy = NewEnemy.GetComponent<Enemy>();
                SummonedEnemy.Path = enemyPath;
                SummonedEnemy.Init();
            }
        }
        else
        {
            Debug.Log($"Enemy with ID {EnemyID} not found");
        }

       
        EnemiesInGame.Add(SummonedEnemy);
        SummonedEnemy.ID = EnemyID;
        return SummonedEnemy;
    }

    
    public void RemoveEnemy(Enemy EnemyToRemove)
    {
        EnemyObjectPools[EnemyToRemove.ID].Enqueue(EnemyToRemove);
        EnemyToRemove.gameObject.SetActive(false);
        EnemiesInGame.Remove(EnemyToRemove);
    }

    public void SummonTower(int TowerID, Vector3 positionToSpawn)
    {

        if (TowerID < 0 || TowerID >= towerBlueprints.Count)
        {
            Debug.Log($"Tower with ID {TowerID} not found");
            return;
        }

        TowerBlueprint towerToSummon = towerBlueprints[TowerID];

        GameObject towerInstance = Instantiate(towerToSummon.towerPrefab, positionToSpawn, Quaternion.identity);

        Tower tower = towerInstance.GetComponent<Tower>();
        TowersInGame.Add(tower);

        if (tower != null)
            tower.Init(towerToSummon.towerData);
        else
            Debug.LogError("O prefab da torre não contém o script TowerController!");

    }

    
}

[System.Serializable]
public class TowerBlueprint
{
    public TowerData towerData;
    public GameObject towerPrefab;

}