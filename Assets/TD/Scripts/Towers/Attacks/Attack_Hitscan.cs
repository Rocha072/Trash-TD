using UnityEngine;

public class Attack_Hitscan : TowerAttackBehavior
{
    [SerializeField] private Animator animator; 

    public override void Init(Tower tower)
    {
    
        if (animator == null)
            animator = GetComponentInParent<Animator>(); 
    }

    public override void Attack(Enemy target, float damage, float range, float slowFactor, float slowDuration, float stunDuration)
    {
        target.TakeDamage(damage);
        target.ApplySlow(slowFactor, slowDuration);
        
        if (animator != null)
        {
            animator.ResetTrigger("Attack");
            animator.SetTrigger("Attack");
        }
    }

    public override void SetAttackEffect(bool active)
    {
        
    }
}
