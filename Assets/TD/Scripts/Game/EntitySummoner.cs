using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class EntitySummoner : MonoBehaviour
{
    public static List<Enemy> EnemiesInGame;
    public static Dictionary<int, GameObject> EnemyPrefabs;
    public static Dictionary<int, Queue<Enemy>> EnemyObjectPools;


    static GameObject Spawnner; 
    public static List<Transform> enemyPath;
    private static bool IsInitialized;

    public static void Init()
    {
        if (!IsInitialized)
        {
            
            EnemyPrefabs = new Dictionary<int, GameObject>();
            EnemyObjectPools = new Dictionary<int, Queue<Enemy>>();
            EnemiesInGame = new List<Enemy>();

            Spawnner = GameObject.Find("Enemy Spawner");

            enemyPath = new List<Transform>();
            
            if (Spawnner != null)
            {
                foreach (Transform childNode in Spawnner.transform)
                {
                    enemyPath.Add(childNode);
                }
            }
            else
            {
                Debug.LogError("Objeto 'Enemy Spawner' não encontrado na cena!");
            }


            EnemySummonData[] EnemiesScrObj = Resources.LoadAll<EnemySummonData>("Enemies");

            foreach (EnemySummonData enemy in EnemiesScrObj)
            {
                EnemyPrefabs.Add(enemy.EnemyID, enemy.EnemyPrefab);
                EnemyObjectPools.Add(enemy.EnemyID, new Queue<Enemy>());
            }

            

            IsInitialized = true;
        }
        else
        {
            Debug.Log("Class EntitySummoner already initilized");
        }

    }


    public static Enemy SummonEnemy(int EnemyID)
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

    
    public static void RemoveEnemy(Enemy EnemyToRemove)
    {
        EnemyObjectPools[EnemyToRemove.ID].Enqueue(EnemyToRemove);
        EnemyToRemove.gameObject.SetActive(false);
        EnemiesInGame.Remove(EnemyToRemove);
    }



    
}
