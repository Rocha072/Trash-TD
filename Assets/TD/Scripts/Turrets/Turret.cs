using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;

public class Turret : MonoBehaviour
{

    public TurretData turretData;
    public Enemy target;
    public string enemyTag = "Enemy";
    public Transform partToRotate;
    public VisualEffect attackEffect;
    private float fireCountdown = 0f;   


    public void Init(TurretData data)
    {
        this.turretData = data;

        InvokeRepeating(nameof(UpdateTarget), 0f, 0.1f);
    }

    void UpdateTarget()
    {
        List<Enemy> enemies = EntitySummoner.Instance.EnemiesInGame;


        foreach (Enemy enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy <=  turretData.range)
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
            fireCountdown = 1f / turretData.fireRate;
        }
        fireCountdown -= Time.deltaTime;

    }

    void RotateTarget()
    {   
        Vector3 dir = target.transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turretData.turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(rotation.x, rotation.y, 0f);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, turretData.range);
    }

    void Attack()
    {
        if (turretData.turretType == TurretData.TurretTypes.waterGun)
        {
            Debug.Log("Atack");
            target.TakeDamage(turretData.Damage);
            target.ApplySlow(turretData.slowFactor, turretData.slowDuration);
        }
        
    }
}
