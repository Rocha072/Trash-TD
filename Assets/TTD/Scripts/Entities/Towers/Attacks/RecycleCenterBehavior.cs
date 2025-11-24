using System.Collections.Generic;
using UnityEngine;

public class RecycleCenterBehavior: TowerAttackBehavior, IEnemyDeathListener
{
    private CurrentTowerStats currentStats;
     [SerializeField] private Animator animator;
    public override void Init(Tower tower)
    {
        this.thisTower = tower;
        attackSoundEmitter = SoundHandler.Instance.PlaySoundAtPosition(thisTower.towerData.attackSound, transform.position, thisTower.towerData.attackSoundVolume, parent: transform);
        EntitySummoner.Instance.RegisterDeathListener(this);
        if (animator == null)
            animator = GetComponentInParent<Animator>(); 
    }

    private void OnDestroy()
    {
        if (EntitySummoner.Instance != null)
        {
            EntitySummoner.Instance.UnregisterDeathListener(this);
        }
    }

    public override void Attack(Enemy target, CurrentTowerStats towerStats)
    {
        this.currentStats = towerStats;
    }

    public override void SetAttackEffect(bool active)
    {
        
    }

    public void OnEnemyDeath(Tower killer, Enemy killedEnemy)
    {
        if (killer == null || !killer.towerData.isCollector)
            return; 
        
        
        if(Random.value > currentStats.GenerateRate)
            return;

        float distance = Vector3.Distance(this.transform.position, killedEnemy.transform.position);

        if (distance > this.currentStats.Range)
            return;
        
        if (animator != null)
        {
            animator.ResetTrigger("Attack");
            animator.SetTrigger("Attack");
        }

        if(attackSoundEmitter != null && !thisTower.towerData.attackSoundLoop)
            attackSoundEmitter.ReplaySound();

        int bonusMoney = (int)this.currentStats.Damage; 
        StatsManager.Instance.AddMoneyGeneratedByCollections(bonusMoney);
        PlayerEconomy.Instance.GainMoney(bonusMoney);
    }

    public override void SetLoopAttackSound(bool active)
    {
        if(attackSoundEmitter == null) return;
        if(active)
            attackSoundEmitter.ResumeSound();
        else
            attackSoundEmitter.PauseSound();
    }

    private bool IsCollectorInRange(float range)
    {
        List<Tower> towersInGame = EntitySummoner.Instance.TowersInGame;

        foreach (Tower tower in towersInGame)
        {
            if (tower == thisTower) continue;

            if (tower.towerData.isCollector)
            {
                float distance = Vector3.Distance(transform.position, tower.transform.position);
                if (distance <= range)
                    return true;
            }
        }

        return false;
    }
}
