using UnityEngine;

public abstract class TowerAttackBehavior : MonoBehaviour
{
    public abstract void Init(Tower tower);
    
    public abstract void Attack(Enemy target, CurrentTowerStats currentTowerStats, Tower attacker);

    public abstract void SetAttackEffect(bool active);
}
