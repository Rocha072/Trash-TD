using UnityEngine;
using UnityEngine.VFX;
public class Attack_HitscanVFXEffect : TowerAttackBehavior
{
    [SerializeField] private VisualEffect attackEffect;
    [SerializeField] private Animator animator; 

    public override void Init(Tower tower)
    {
    
        if (animator == null)
            animator = GetComponentInParent<Animator>(); 
    }

    public override void Attack(Enemy target, float damage, float slowFactor, float slowDuration)
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
        if (attackEffect == null) return;
        
        if (active)
            attackEffect.Play();
        else
            attackEffect.Stop();
    }
}
