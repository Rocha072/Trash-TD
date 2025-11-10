using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class Enemy : MonoBehaviour
{
    public EnemyData enemyData;
    public float Health;
    public float Speed;
    private float slowFactor; // 1.0f = sem lentidão
    private float slowDurationTimer;
    private float stunDurationTimer;
    private bool isDead;
    
    NavMeshAgent agent;

    List<Transform> path;

    private int currentNodeIndex;
    public int CurrentNodeIndex
    {
        get { return currentNodeIndex; }
    }
    public float RemainingDistanceToNode
    {
        get
        {
            if (agent.hasPath && !isDead)
            {
                return agent.remainingDistance;
            }
            
            return float.MaxValue;
        }
    }

    public void Init(EnemyData data, int startNodeIndex)
    {
        enemyData = data;
        agent = GetComponent<NavMeshAgent>();
        RestartState(startNodeIndex);
    }

    public void RestartState(int startNodeIndex)
    {
        slowFactor = 1.0f;
        slowDurationTimer = 0f;
        currentNodeIndex = startNodeIndex;
        stunDurationTimer = 0f;
        isDead = false;
        Health = enemyData.MaxHealth;
        Speed = enemyData.MaxSpeed;
        agent.acceleration = enemyData.Acceleration;
        StartCoroutine(MovementCoroutine());
    }

    void Update()
    {
        if (stunDurationTimer > 0)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            Speed = 0;
            stunDurationTimer -= Time.deltaTime;
        }

        else
        {
            agent.isStopped = false;
            if (slowDurationTimer > 0)
            {
                slowDurationTimer -= Time.deltaTime;
            }
            else
            {
                slowFactor = 1.0f;
                Speed = enemyData.MaxSpeed;
            }

        }

        agent.speed = Speed;

    }

    public List<Transform> Path
    {
        get => path;
        set => path = value;
    }

    IEnumerator MovementCoroutine()
    {

        for (int i = currentNodeIndex; i<path.Count; i++)
        {
            currentNodeIndex = i;

            Transform node = path[i];

            agent.SetDestination(node.position);

            yield return new WaitUntil(() =>
            Vector3.Distance(transform.position, node.position) < enemyData.MinDistanceToPassNode);


        }

        PlayerLife.Instance.TakeDamage(this.enemyData.damage);
        WaveManager.Instance.OnEnemyDied();
        EntitySummoner.Instance.RemoveEnemy(this);

    }

    public void TakeDamage(float damage, string type = "nothing")
    {
        if (isDead) return;

        this.Health -= damage;

        if (Health <= 0f)
        {
            Die();
        }
    }

    public void ApplySlow(float factor, float duration)
    {

        if (factor < this.slowFactor)
        {
            this.slowFactor = factor;
        }


        this.slowDurationTimer = duration;

        Speed = enemyData.MaxSpeed * slowFactor;
    }
    
    public void ApplyStun(float duration)
    {
        this.stunDurationTimer = duration;
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;

        if (enemyData.spawnOnDeathList != null && enemyData.spawnOnDeathList.Count > 0)
        {
            int totalChildrenToSpawn = 0;
            foreach (var group in enemyData.spawnOnDeathList)
            {
                totalChildrenToSpawn += group.amount;
            }

            for (int i = 0; i < totalChildrenToSpawn; i++)
            {
                WaveManager.Instance.OnEnemySpawned();
            }
            EntitySummoner.Instance.SpawnEnemiesWithDelay(enemyData.spawnOnDeathList, transform.position, currentNodeIndex, 0.1f);
        }
        
        WaveManager.Instance.OnEnemyDied();
        PlayerEconomy.Instance.GainMoney(enemyData.dropAmount);
        StatsManager.Instance.AddTrashCollected();
        StatsManager.Instance.AddMoneyGeneratedByCollections(enemyData.dropAmount);
        EntitySummoner.Instance.RemoveEnemy(this);
    }

}
