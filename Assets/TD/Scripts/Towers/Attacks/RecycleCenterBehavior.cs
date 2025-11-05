using System.Collections.Generic;
using UnityEngine;

public class RecycleCenterBehavior: TowerAttackBehavior
{
    private Tower thisTower;

    public override void Init(Tower tower)
    {
        this.thisTower = tower;
    }

    public override void Attack(Enemy target, float damage, float range, float slowFactor, float slowDuration, float stunDuration)
    {
        

        if (IsCollectorInRange(range))
        {
            int moneyToGenerate = (int)damage;
            PlayerEconomy.Instance.GainMoney(moneyToGenerate);
            StatsManager.Instance.AddMoneyGeneratedByCollections(moneyToGenerate);
        }
    }

    public override void SetAttackEffect(bool active)
    {

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
