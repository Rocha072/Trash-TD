using UnityEngine;

public class Attack_Hitscan : TowerAttackBehavior
{
    [SerializeField] private Animator animator;
    private CurrentTowerStats currentStats;
    private Enemy currentTarget;
    Tower Attacker;
    public override void Init(Tower tower)
    {
    
        if (animator == null)
            animator = GetComponentInParent<Animator>(); 
    }

    public override void Attack(Enemy target, CurrentTowerStats currentTowerStats, Tower attacker)
    {
        currentStats = currentTowerStats;
        currentTarget = target;
        Attacker = attacker;

        if (animator != null)
        {
            animator.ResetTrigger("Attack");
            animator.SetTrigger("Attack");
        }
        else
        {
            currentTarget.TakeDamage(currentStats.Damage, Attacker);
            currentTarget.ApplySlow(currentStats.SlowFactor, currentStats.SlowDuration);
        }

    }
    
    public void AnimationEvent_ApplyDamage()
    {

        if (currentTarget != null)
        {
            currentTarget.TakeDamage(currentStats.Damage, Attacker);
            currentTarget.ApplySlow(currentStats.SlowFactor, currentStats.SlowDuration);
        }

    }

    public override void SetAttackEffect(bool active)
    {
        
    }
}
