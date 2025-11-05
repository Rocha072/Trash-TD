using UnityEngine;

public abstract class TowerAttackBehavior : MonoBehaviour
{
    public abstract void Init(Tower tower);
    
    public abstract void Attack(Enemy target, float damage, float range, float slowFactor, float slowDuration, float stunDuration);

    public abstract void SetAttackEffect(bool active);
}
