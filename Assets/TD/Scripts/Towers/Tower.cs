using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;

public class Tower : MonoBehaviour
{
    [Header("Tower Properties")]
    public TowerData towerData;
    public Transform partToRotate;
    public VisualEffect attackEffect;

    [Header("Masks")]
    public GameObject invalidMask;
    public GameObject hoverMask;
    public GameObject selectMask;

    public bool isBeingPlaced = true;
    public bool validPosition;

    [SerializeField] private float minHeight;
    [SerializeField] private float maxHeight;

    private Enemy target;
    private float fireCountdown;
    private int coliding;

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
    //void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawWireSphere(transform.position, towerData.range);
    //}

    void Attack()
    {   
        //Cada torre ataca de uma forma
        if (towerData.towerType == TowerData.TowerTypes.waterGun)
        {
            target.TakeDamage(towerData.Damage);
            target.ApplySlow(towerData.slowFactor, towerData.slowDuration);
        }

    }

    public void VerifyValidPosition()
    {
        validPosition = this.transform.position.y >= minHeight && this.transform.position.y <= maxHeight && coliding==0;

        this.invalidMask.SetActive(!validPosition);
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

    public void HoverTower()
    {
        if (this.selectMask.activeSelf || isBeingPlaced) return;

        this.hoverMask.SetActive(true);
    }

    public void UnhoverTower()
    {
        this.hoverMask.SetActive(false);
    }

    public void SelectTower()
    {
        hoverMask.SetActive(false);
        this.selectMask.SetActive(true);
    }

    public void UnselectTower()
    {
        this.selectMask.SetActive(false);
    }

}
