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

    private float currentDamage;
    private float currentRange;
    private float currentFireRate;
    private float currentSlowFactor;
    private float currentSlowDuration;


    private int totalCostInvested;
    public int SellValue
    {
        get { return Mathf.RoundToInt(totalCostInvested * 0.7f); }
    }

    //Estado do upgrade (indice)
    private int currentTierA = 0;
    private int currentTierB = 0;


    public void Init(TowerData data)
    {
        this.towerData = data;
        fireCountdown = 0f;
        SetAttackEfect(false);

        currentDamage = towerData.baseDamage;
        currentRange = towerData.baseRange;
        currentFireRate = towerData.baseFireRate;
        currentSlowDuration = towerData.baseSlowDuration;
        currentSlowFactor = towerData.baseSlowFactor;
        totalCostInvested = towerData.cost;
        
        InvokeRepeating(nameof(UpdateTarget), 0f, 0.1f);
    }

    void UpdateTarget()
    {
        List<Enemy> enemies = EntitySummoner.Instance.EnemiesInGame;


        foreach (Enemy enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy <= currentRange)
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
            SetAttackEfect(false);
            return;
        }

        RotateTarget();
        SetAttackEfect(true);

        if (fireCountdown <= 0f)
        {
            Attack();
            fireCountdown = 1f / currentFireRate;
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

    void Attack()
    {
        //Cada torre ataca de uma forma
        if (towerData.towerType == TowerData.TowerTypes.waterGun)
        {
            target.TakeDamage(currentDamage);
            target.ApplySlow(currentSlowFactor, currentSlowDuration);
        }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, currentRange);
    }

    void SetAttackEfect(bool active)
    {
        if (attackEffect == null) return;

        if (active)
            attackEffect.Play();

        else
            attackEffect.Stop();
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

    public void TryApplyUpgrade(int pathIndex) // 0 => A, 1 => B
    {
        //Retorna se tentou comprar o caminho errado

        if (pathIndex == 0 && currentTierB > 0)
        {
            return;
        }
        if (pathIndex == 1 && currentTierA > 0)
        {
            return;
        }

        // Pega o upgrade que deve ser incrementado
        UpgradeDefinition upgradeToApply = GetNextUpgrade(pathIndex);

        if (upgradeToApply == null)
        {
            return;
        }

        if (PlayerEconomy.Instance.CanBuy(upgradeToApply.upgradeCost))
        {
            PlayerEconomy.Instance.Buy(upgradeToApply.upgradeCost);

            totalCostInvested += upgradeToApply.upgradeCost;

            ApplyStats(upgradeToApply.statModifiers);

            //Se for trocar prefab, cor, qualquer coisa é aqui

            if (pathIndex == 0) currentTierA++;
            else currentTierB++;

            SelectedTowerCardManager.Instance.RefreshUI(this);
        }
    }

    private void ApplyStats(UpgradeStats stats)
    {
        currentDamage += stats.damage_add;
        currentRange += stats.range_add;
        currentSlowDuration += stats.slowDuration_add;
        currentFireRate *= stats.fireRate_multiplier;
        currentSlowFactor *= stats.slowFactor_multiplier;

        // Atualizar o anel de range
    }

    public UpgradeDefinition GetNextUpgrade(int pathIndex)
    {
        if (pathIndex == 0)
        {
            if (currentTierA >= 3)
                return null;

            return towerData.pathA_Upgrades[currentTierA];
        }
        else //Adicionar mais condicionais se for adicionar mais paths
        {
            if (currentTierB >= 3)
                return null;

            return towerData.pathB_Upgrades[currentTierB];
        }

    }
    
    public bool IsPathLocked(int pathIndex)
    {
        if (pathIndex == 0 && currentTierB > 0) return true;
        if (pathIndex == 1 && currentTierA > 0) return true;
        return false;
    }

}
