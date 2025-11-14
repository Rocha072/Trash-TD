using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class Enemy : MonoBehaviour
{
    public SoundEmitter soundEmitterPrefab;
    public EnemyData enemyData;
    public float Health;
    public float Speed;
    private float slowFactor; // 1.0f = sem lentidão
    private float slowDurationTimer;
    private float stunDurationTimer;

    private bool isDead;
    private Tower lastAttacker;
    
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
        EntitySummoner.Instance.HandleEnemyDeath(this, null);

    }

    public void TakeDamage(float damage, Tower attacker)
    {
        if (isDead) return;

        this.Health -= damage;
        this.lastAttacker = attacker;

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

        SoundHandler.Instance.PlaySoundAtPosition(enemyData.dieSoundEffect, transform.position, enemyData.dieVolume, singleExecution: true);

        EntitySummoner.Instance.HandleEnemyDeath(this, lastAttacker);
    }

}
