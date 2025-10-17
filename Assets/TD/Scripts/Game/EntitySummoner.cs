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

    [Header("Turret Configuration")]
    public List<TurretBlueprint> turretBlueprints;
    

    [Header("In Game Objects")]
    public GameObject Spawnner;
    public List<Transform> enemyPath;
    public List<Enemy> EnemiesInGame;
    public List<Turret> TurretsInGame;


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

        TurretsInGame = new List<Turret>();



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

    public void SummonTurret(int TurretID, Vector3 positionToSpawn)
    {

        if (TurretID < 0 || TurretID >= turretBlueprints.Count)
        {
            Debug.Log($"Turret with ID {TurretID} not found");
            return;
        }

        TurretBlueprint turretToSummon = turretBlueprints[TurretID];

        GameObject turretInstance = Instantiate(turretToSummon.turretPrefab, positionToSpawn, Quaternion.identity);

        Turret turret = turretInstance.GetComponent<Turret>();
        TurretsInGame.Add(turret);

        if (turret != null)
            turret.Init(turretToSummon.turretData);
        else
            Debug.LogError("O prefab da torre não contém o script TowerController!");

    }

    
}

[System.Serializable]
public class TurretBlueprint
{
    public TurretData turretData;
    public GameObject turretPrefab;

}