using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class Enemy : MonoBehaviour
{
    public float MaxHealth = 100f;
    public float Health;
    public float Speed;
    public float MaxSpeed = 4f;
    private float slowFactor = 1.0f; // 1.0f = sem lentidão
    private float slowDurationTimer = 0f;
    public int ID;

    NavMeshAgent agent;

    List<Transform> path;

    [SerializeField] float dist = 2;


    public void Init()
    {
        Health = MaxHealth;

        Speed = MaxSpeed;

        agent = GetComponent<NavMeshAgent>();

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
                Speed = MaxSpeed * slowFactor;
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

            yield return new WaitUntil(() => Vector3.Distance(transform.position, node.position) < dist);


        }

        EntitySummoner.Instance.RemoveEnemy(this);

    }

    public void TakeDamage(float damage, string type = "nothing")
    {
        Health -= damage;

        if (Health <= 0f)
        {
            die();
        }
    }

    public void ApplySlow(float factor, float duration)
    {
        
        if (factor < this.slowFactor)
        {
            this.slowFactor = factor;
        }

    
        this.slowDurationTimer = duration;

        Speed = MaxSpeed * slowFactor;
    }

    private void die()
    {
        EntitySummoner.Instance.RemoveEnemy(this);
    }

}
