using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;

public class Turrent : MonoBehaviour
{

    public Turrets turretSrc;
    public Enemy target;
    public string enemyTag = "Enemy";
    public Transform partToRotate;
    public VisualEffect attackEffect;
    private float fireCountdown = 0f;   


    void Start()
    {
        InvokeRepeating(nameof(UpdateTarget), 0f, 0.1f);
    }

    void UpdateTarget()
    {
        List<Enemy> enemies = EntitySummoner.EnemiesInGame;


        foreach (Enemy enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy <=  turretSrc.range)
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
            attackEffect.Stop();
            return;
        }

        RotateTarget();
        attackEffect.Play();

        if (fireCountdown <= 0f) {
            Attack();
            fireCountdown = 1f / turretSrc.fireRate;
        }
        fireCountdown -= Time.deltaTime;

    }

    void RotateTarget()
    {   
        Vector3 dir = target.transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turretSrc.turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(rotation.x, rotation.y, 0f);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, turretSrc.range);
    }

    void Attack()
    {
        if (turretSrc.turretType == Turrets.TurretTypes.waterGun)
        {
            Debug.Log("Atack");
            target.TakeDamage(turretSrc.Damage);
            target.ApplySlow(turretSrc.slowFactor, turretSrc.slowDuration);
        }
        
    }
}
