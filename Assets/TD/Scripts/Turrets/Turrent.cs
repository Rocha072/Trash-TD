using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Turrent : MonoBehaviour
{
    public Enemy target;
    public float range = 15f;
    public float turnSpeed = 5f;
    public string enemyTag = "Enemy";
    public Transform partToRotate;
    void Start()
    {
        InvokeRepeating(nameof(UpdateTarget), 0f, 0.5f);
    }

    void UpdateTarget()
    {
        List<Enemy> enemies = EntitySummoner.EnemiesInGame;


        foreach (Enemy enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy <= range)
            {
                target = enemy;
                return;
            }

        }
        target = null;
        
        
    }

    void Update()
    {
        if (target == null)
        {
            return;
            //Desativa efeito
        }
            


        RotateTarget();
        //Ativa efeito
    }

    void RotateTarget()
    {   
        Vector3 dir = target.transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(rotation.x, rotation.y, 0f);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
