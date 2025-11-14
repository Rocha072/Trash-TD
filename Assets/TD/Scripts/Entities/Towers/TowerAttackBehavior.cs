using UnityEngine;

public abstract class TowerAttackBehavior : MonoBehaviour
{
    protected Tower thisTower;
    public abstract void Init(Tower tower);
    
    public abstract void Attack(Enemy target, CurrentTowerStats currentTowerStats);

    public abstract void SetAttackEffect(bool active);
}
