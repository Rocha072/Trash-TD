using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

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

    [Header("Valid Position Control")]
    public bool isBeingPlaced = true;
    public bool validPosition;
    private int coliding;
    [SerializeField] private float minHeight;
    [SerializeField] private float maxHeight;

    //[Header("Attack control")]
    private TowerAttackBehavior attackBehavior;
    private Enemy target;
    private float fireCountdown;

    [Header("Current Stats")]
    [SerializeField]private float currentDamage;
    [SerializeField]private float currentRange;
    [SerializeField]private float currentFireRate;
    [SerializeField]private float currentSlowFactor;
    [SerializeField]private float currentSlowDuration;
    [SerializeField]private float currentStunDuration;

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
        if(TryGetComponent(out Animator _animator))
        {
            animator = _animator;
            animator.enabled = false;
        }
    }
    public void Init(TowerData data)
    {
        this.towerData = data;
        fireCountdown = 0f;
        currentDamage = towerData.baseDamage;
        currentRange = towerData.baseRange;
        currentFireRate = towerData.baseFireRate;
        currentSlowDuration = towerData.baseSlowDuration;
        currentSlowFactor = towerData.baseSlowFactor;
        currentStunDuration = towerData.baseStunDuration;
        totalCostInvested = towerData.cost;


        attackBehavior = GetComponent<TowerAttackBehavior>();

        if (attackBehavior == null)
        {
            Debug.Log("Tower Attack is missing");
            return;
        }

        attackBehavior.Init(this);
        attackBehavior.SetAttackEffect(false);


        if (towerData.requiresTarget)
            InvokeRepeating(nameof(UpdateTarget), 0f, 0.1f);
    }


    void Update()
    {

        VerifyValidPosition();
        if (isBeingPlaced) return;

        if(animator!=null && !animator.enabled)
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
                    fireCountdown = 1f / currentFireRate;
                }
            }
        }
        else
        {
            attackBehavior.SetAttackEffect(true);

            if (fireCountdown <= 0f)
            {
                Attack();
                fireCountdown = 1f / currentFireRate;
            }
        }


        
    }
    void UpdateTarget()
    {
        if (WaveManager.Instance.currentState == WaveManager.WaveState.WaitingToStart) {
            target = null;
            return;
        };
        List<Enemy> enemies = EntitySummoner.Instance.EnemiesInGame;

        Enemy bestTarget = null;

        int bestTargetNodeIndex = -1;
        float bestTargetRemainingDistance = float.MaxValue;
        foreach (Enemy enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy > currentRange)
            {
                continue;
            }

            int enemyNodeIndex = enemy.CurrentNodeIndex;

            float enemyRemainingDistance = enemy.RemainingDistanceToNode;

            bool isBetterTarget = false;

            if (enemyNodeIndex > bestTargetNodeIndex)
            {
                isBetterTarget = true;
            }
            else if (enemyNodeIndex == bestTargetNodeIndex)
            {
                if (enemyRemainingDistance < bestTargetRemainingDistance)
                {
                    isBetterTarget = true;
                }
            }

            if (isBetterTarget)
            {
                bestTarget = enemy;
                bestTargetNodeIndex = enemyNodeIndex;
                bestTargetRemainingDistance = enemyRemainingDistance;
            }

        }
        
        target = bestTarget;


    }

    void RotateTarget()
    {
        Vector3 dir = target.transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotateY.rotation, lookRotation, Time.deltaTime * towerData.turnSpeed).eulerAngles;

        partToRotateY.rotation = Quaternion.Euler(0f, rotation.y, 0f);

        if(partToRotateX!=null)
            partToRotateX.localRotation = Quaternion.Euler(rotation.x, rotation.y, 0f);
    }

    void Attack()
    {
        attackBehavior.Attack(target, currentDamage, currentRange, currentSlowFactor, currentSlowDuration, currentStunDuration);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, currentRange);
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
