using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;

public class Tower : MonoBehaviour
{

    public TowerData towerData;
    public Enemy target;
    public Transform partToRotate;
    public VisualEffect attackEffect;
    public GameObject invalidMask;
    private float fireCountdown;
    public bool isBeingPlaced = true;
    private int coliding;
    public bool validPosition;
    [SerializeField] private float minHeight;
    [SerializeField] private float maxHeight;


    public void Init(TowerData data)
    {
        fireCountdown = 0f;
        this.towerData = data;
        attackEffect.Stop();

        InvokeRepeating(nameof(UpdateTarget), 0f, 0.1f);
    }

    void UpdateTarget()
    {
        List<Enemy> enemies = EntitySummoner.Instance.EnemiesInGame;


        foreach (Enemy enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy <= towerData.range)
            {
                target = enemy;
                return;
            }

        }
        target = null;


    }

    void Update()
    {

        VerifyValidPosition();
        if (isBeingPlaced) return;

        if (target == null)
        {
            attackEffect.Stop();
            return;
        }

        RotateTarget();
        attackEffect.Play();

        if (fireCountdown <= 0f)
        {
            Attack();
            fireCountdown = 1f / towerData.fireRate;
        }
        fireCountdown -= Time.deltaTime;

    }

    void RotateTarget()
    {
        Vector3 dir = target.transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * towerData.turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(rotation.x, rotation.y, 0f);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, towerData.range);
    }

    void Attack()
    {
        if (towerData.towerType == TowerData.TowerTypes.waterGun)
        {
            target.TakeDamage(towerData.Damage);
            target.ApplySlow(towerData.slowFactor, towerData.slowDuration);
        }

    }

    public void VerifyValidPosition()
    {
        validPosition = this.transform.position.y >= minHeight && this.transform.position.y <= maxHeight && coliding==0;

        if (validPosition)
        {
            this.invalidMask.SetActive(false);
            return;
        }

        this.invalidMask.SetActive(true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tower"))
            coliding++;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Tower"))
            coliding--;
    }

}
