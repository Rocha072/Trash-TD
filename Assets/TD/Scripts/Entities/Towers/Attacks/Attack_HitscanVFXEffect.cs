using UnityEngine;
using UnityEngine.VFX;
public class Attack_HitscanVFXEffect : TowerAttackBehavior
{
    [SerializeField] private VisualEffect attackEffect;
    [SerializeField] private Animator animator;


    public override void Init(Tower tower)
    {
        this.thisTower = tower;
        if (animator == null)
            animator = GetComponentInParent<Animator>(); 
    }

    public override void Attack(Enemy target, CurrentTowerStats currentTowerStats)
    {
        target.TakeDamage(currentTowerStats.Damage, this.thisTower);
        target.ApplySlow(currentTowerStats.SlowFactor, currentTowerStats.SlowDuration);
        
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
