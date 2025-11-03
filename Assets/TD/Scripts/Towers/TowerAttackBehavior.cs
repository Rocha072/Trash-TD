using UnityEngine;

public abstract class TowerAttackBehavior : MonoBehaviour
{
    public abstract void Init(Tower tower);
    
    public abstract void Attack(Enemy target, float damage, float slowFactor, float slowDuration);

    public abstract void SetAttackEffect(bool active);
}
