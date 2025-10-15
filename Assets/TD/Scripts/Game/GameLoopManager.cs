using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoopManager : MonoBehaviour
{
    public bool LoopShouldEnd;
    
    private static Queue<int> EnemyIDsToSummon;
    private static Queue<Enemy> EnemiesToRemove;

    // public static Vector3[] NodePositions;
    private void Start()
    {
        EnemyIDsToSummon = new Queue<int>();
        EnemiesToRemove = new Queue<Enemy>();
        EntitySummoner.Init();

        StartCoroutine(GameLoop());

        InvokeRepeating("SummonTest", 0f, 10f);
    }

    void SummonTest()
    {
        EnqueueEnemyIDToSummon(1);
    }
    IEnumerator GameLoop()
    {
        while (LoopShouldEnd == false)
        {
            if (EnemyIDsToSummon.Count > 0)
            {
                for (int i = 0; i < EnemyIDsToSummon.Count; i++)
                {
                    EntitySummoner.SummonEnemy(EnemyIDsToSummon.Dequeue());
                }
            }



            if (EnemiesToRemove.Count > 0)
            {
                for (int i = 0; i < EnemiesToRemove.Count; i++)
                {
                    EntitySummoner.RemoveEnemy(EnemiesToRemove.Dequeue());
                }
            }


            yield return null;
        }
    }

    public static void EnqueueEnemyIDToSummon(int ID)
    {
        EnemyIDsToSummon.Enqueue(ID);
    }
    
    public static void EnqueueEnemyToRemove(Enemy enemyToRemove)
    {
        EnemiesToRemove.Enqueue(enemyToRemove);
    }
}
