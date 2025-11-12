using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;


[System.Serializable]
public struct CurrentTowerStats
{
    public float Damage;
    public float Range;
    public float FireRate;
    public float SlowFactor;
    public float SlowDuration;
    public float StunDuration;
}

public enum TargetingPriority
{
    First, Last, Strongest   
}

public class Tower : MonoBehaviour
{
    [Header("Tower Properties")]
    public TowerData towerData;
    public Transform partToRotateY;
    public Transform partToRotateX;

    [Header("Masks")]
    public GameObject invalidMask;
    public GameObject hoverMask;
    public GameObject selectMask;
    public GameObject RangeObject;

    [Header("Valid Position Control")]
    public bool isBeingPlaced = true;
    public bool validPosition;
    private int coliding;
    [SerializeField] private float minHeight;
    [SerializeField] private float maxHeight;

    [Header("Attack Control")]
    public TargetingPriority currentPriority = TargetingPriority.First;
    [SerializeField] private float targetSearchFrequency = 0.1f;
    private TowerAttackBehavior attackBehavior;
    private Enemy target;
    private float fireCountdown;


    [Header("Current Stats")]

    [SerializeField] private CurrentTowerStats currentStats;
    private int totalCostInvested;
    public int SellValue
    {
        get { return Mathf.RoundToInt(totalCostInvested * 0.7f); }
    }

    //Estado do upgrade (indice)
    private int currentTierA = 0;
    private int currentTierB = 0;


    private Animator animator;


    void Start()
    {
        if (TryGetComponent(out Animator _animator))
        {
            animator = _animator;
            animator.enabled = false;
        }
    }
    public void Init(TowerData data)
    {
        this.towerData = data;
        fireCountdown = 0f;
        currentStats.Damage = towerData.baseDamage;
        currentStats.Range = towerData.baseRange;
        currentStats.FireRate = towerData.baseFireRate;
        currentStats.SlowDuration = towerData.baseSlowDuration;
        currentStats.SlowFactor = towerData.baseSlowFactor;
        currentStats.StunDuration = towerData.baseStunDuration;
        totalCostInvested = towerData.cost;


        attackBehavior = GetComponent<TowerAttackBehavior>();

        if (attackBehavior == null)
        {
            Debug.Log("Tower Attack is missing");
            return;
        }

        attackBehavior.Init(this);
        attackBehavior.SetAttackEffect(false);

        this.RangeObject.SetActive(true);
        UpdateRange();

        if (towerData.requiresTarget)
            InvokeRepeating(nameof(UpdateTarget), 0f, targetSearchFrequency);
    }


    void Update()
    {

        VerifyValidPosition();
        if (isBeingPlaced) return;

        if (animator != null && !animator.enabled)
        {
            animator.enabled = true;
        }

        if (fireCountdown > 0f)
            fireCountdown -= Time.deltaTime;

        if (WaveManager.Instance.currentState == WaveManager.WaveState.WaitingToStart)
        {
            attackBehavior.SetAttackEffect(false);
            return;
        }

        if (towerData.requiresTarget)
        {
            if (target == null)
            {
                attackBehavior.SetAttackEffect(false);
            }
            else
            {
                RotateTarget();

                attackBehavior.SetAttackEffect(true);

                if (fireCountdown <= 0f)
                {
                    Attack();
                    fireCountdown = 1f / currentStats.FireRate;
                }
            }
        }
        else
        {
            attackBehavior.SetAttackEffect(true);

            if (fireCountdown <= 0f)
            {
                Attack();
                fireCountdown = 1f / currentStats.FireRate;
            }
        }



    }

    void UpdateTarget()
    {
        if (WaveManager.Instance.currentState == WaveManager.WaveState.WaitingToStart)
        {
            target = null;
            return;
        }

        List<Enemy> enemiesInRange = new List<Enemy>();

        foreach (Enemy enemy in EntitySummoner.Instance.EnemiesInGame)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy <= currentStats.Range)
            {
                enemiesInRange.Add(enemy);
            }
        }

        if (enemiesInRange.Count == 0)
        {
            target = null;
            return;
        }

        Enemy bestTarget = null;

        switch (currentPriority)
        {
            case TargetingPriority.First:
                bestTarget = SearchFirstEnemy(enemiesInRange);
                break;

            case TargetingPriority.Last:
                bestTarget = SearchLastEnemy(enemiesInRange);
                break;

            case TargetingPriority.Strongest:
                bestTarget = SearchStrongerEnemy(enemiesInRange);
                break;

        }

        target = bestTarget;
    }

    Enemy SearchFirstEnemy(List<Enemy> enemiesInRange)
    {
        Enemy bestTarget = null;
        int bestNodeIndex = -1;
        float minRemainingDist = float.MaxValue;
        bestTarget = enemiesInRange[0];

        foreach (Enemy enemy in enemiesInRange)
        {
            int enemyNodeIndex = enemy.CurrentNodeIndex;
            float enemyDist = enemy.RemainingDistanceToNode;

            if (enemyNodeIndex > bestNodeIndex)
            {
                bestNodeIndex = enemyNodeIndex;
                minRemainingDist = enemyDist;
                bestTarget = enemy;
            }
            else if (enemyNodeIndex == bestNodeIndex && enemyDist < minRemainingDist)
            {
                minRemainingDist = enemyDist;
                bestTarget = enemy;
            }
        }

        return bestTarget;
    }
    Enemy SearchLastEnemy(List<Enemy> enemiesInRange)
    {
        Enemy bestTarget = null;
        int worstNodeIndex = int.MaxValue;
        float maxRemainingDist = 0f;
        bestTarget = enemiesInRange[0];

        foreach (Enemy enemy in enemiesInRange)
        {
            int enemyNodeIndex = enemy.CurrentNodeIndex;
            float enemyDist = enemy.RemainingDistanceToNode;

            if (enemyNodeIndex < worstNodeIndex)
            {
                worstNodeIndex = enemyNodeIndex;
                maxRemainingDist = enemyDist;
                bestTarget = enemy;
            }
            else if (enemyNodeIndex == worstNodeIndex && enemyDist > maxRemainingDist)
            {
                maxRemainingDist = enemyDist;
                bestTarget = enemy;
            }
        }
        return bestTarget;
    }

    Enemy SearchStrongerEnemy(List<Enemy> enemiesInRange)
    {
        Enemy bestTarget = null;
        float maxHealth = 0f;
                
        bestTarget = enemiesInRange[0]; 

        foreach (Enemy enemy in enemiesInRange)
        {
            if (enemy.enemyData.MaxHealth > maxHealth)
            {
                maxHealth = enemy.enemyData.MaxHealth;
                bestTarget = enemy;
            }
        }
        return bestTarget;
    }

    void RotateTarget()
    {
        Vector3 dir = target.transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotateY.rotation, lookRotation, Time.deltaTime * towerData.turnSpeed).eulerAngles;

        partToRotateY.rotation = Quaternion.Euler(0f, rotation.y, 0f);

        if (partToRotateX != null)
            partToRotateX.localRotation = Quaternion.Euler(rotation.x, rotation.y, 0f);
    }

    void Attack()
    {
        attackBehavior.Attack(target, currentStats);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, currentStats.Range);
    }


    public void VerifyValidPosition()
    {
        validPosition = this.transform.position.y >= minHeight && this.transform.position.y <= maxHeight && coliding == 0;

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
        this.RangeObject.SetActive(true);
    }

    public void UnselectTower()
    {
        this.selectMask.SetActive(false);
        this.RangeObject.SetActive(false);
    }

    public void UpdateRange()
    {
        this.RangeObject.transform.localScale = new Vector3(currentStats.Range / transform.lossyScale.x * 2, 0.1f, currentStats.Range / transform.lossyScale.z * 2);
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
        currentStats.Damage += stats.damage_add;
        currentStats.Range += stats.range_add;
        currentStats.Damage *= stats.damage_multiplier;
        currentStats.SlowDuration += stats.slowDuration_add;
        currentStats.FireRate *= stats.fireRate_multiplier;
        currentStats.SlowFactor *= stats.slowFactor_multiplier;
        currentStats.StunDuration += stats.StunDuration_add;
        UpdateRange();
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

