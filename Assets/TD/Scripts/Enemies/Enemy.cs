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
    
    NavMeshAgent agent;

    List<Transform> path;

    public void Init(EnemyData data)
    {
        enemyData = data;
        agent = GetComponent<NavMeshAgent>();
        RestartState();
    }

    public void RestartState()
    {
        slowFactor = 1.0f;
        slowDurationTimer = 0f;
        Health = enemyData.MaxHealth;
        Speed = enemyData.MaxSpeed;
        StartCoroutine(MovementCoroutine());
    }

    void Update()
    {

        if (slowDurationTimer > 0)
        {
            slowDurationTimer -= Time.deltaTime;

            if (slowDurationTimer <= 0)
            {
                slowFactor = 1.0f;
                Speed = enemyData.MaxSpeed * slowFactor;
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

        foreach (Transform node in path)
        {
            agent.SetDestination(node.position);

            yield return new WaitUntil(() =>
            Vector3.Distance(transform.position, node.position) < enemyData.MinDistanceToPassNode);


        }

        PlayerLife.Instance.TakeDamage(this.enemyData.damage);
        EntitySummoner.Instance.RemoveEnemy(this);

    }

    public void TakeDamage(float damage, string type = "nothing")
    {
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

    private void Die()
    {
        PlayerEconomy.Instance.GainMoney(enemyData.dropAmount);
        EntitySummoner.Instance.RemoveEnemy(this);
    }

}
