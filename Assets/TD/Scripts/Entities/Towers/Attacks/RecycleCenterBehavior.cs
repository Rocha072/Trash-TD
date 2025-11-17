using System.Collections.Generic;
using UnityEngine;

public class RecycleCenterBehavior: TowerAttackBehavior, IEnemyDeathListener
{
    private CurrentTowerStats currentStats;

    public override void Init(Tower tower)
    {
        this.thisTower = tower;
        EntitySummoner.Instance.RegisterDeathListener(this);
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
        {
            return; 
        }
        
        float distance = Vector3.Distance(this.transform.position, killedEnemy.transform.position);
        if (distance > this.currentStats.Range)
        {
            return; 
        }
        
       
        int bonusMoney = (int)this.currentStats.Damage; 
        PlayerEconomy.Instance.GainMoney(bonusMoney);
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
