using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class Enemy : MonoBehaviour
{
    public float MaxHealth;
    public float Health;
    public float Speed;
    public int ID;

    NavMeshAgent agent;

    List<Transform> path;

    [SerializeField] float dist = 2;


    public void Init()
    {
        Health = MaxHealth;
        
        agent = GetComponent<NavMeshAgent>();

        StartCoroutine(MovementCoroutine());
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

        EntitySummoner.RemoveEnemy(this);

    }
}
