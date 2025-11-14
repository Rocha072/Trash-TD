using UnityEngine;

public class Attack_Hitscan : TowerAttackBehavior
{
    [SerializeField] private Animator animator;
    private CurrentTowerStats currentStats;
    private Enemy currentTarget;
    public override void Init(Tower tower)
    {
        this.thisTower = tower;
        if (animator == null)
            animator = GetComponentInParent<Animator>(); 
    }

    public override void Attack(Enemy target, CurrentTowerStats currentTowerStats)
    {
        currentStats = currentTowerStats;
        currentTarget = target;

        if (animator != null)
        {
            animator.ResetTrigger("Attack");
            animator.SetTrigger("Attack");
        }
        else
        {
            currentTarget.TakeDamage(currentStats.Damage, this.thisTower);
            currentTarget.ApplySlow(currentStats.SlowFactor, currentStats.SlowDuration);
        }

    }
    
    public void AnimationEvent_ApplyDamage()
    {

        if (currentTarget != null)
        {
            currentTarget.TakeDamage(currentStats.Damage, this.thisTower);
            currentTarget.ApplySlow(currentStats.SlowFactor, currentStats.SlowDuration);
        }

    }

    public override void SetAttackEffect(bool active)
    {
        
    }
}
