using UnityEngine;
using UnityEngine.VFX;
public class Attack_HitscanVFXEffect : TowerAttackBehavior
{
    [SerializeField] private VisualEffect attackEffect;
    [SerializeField] private Animator animator;


    public override void Init(Tower tower)
    {
        this.thisTower = tower;
        attackSoundEmitter = SoundHandler.Instance.PlaySoundAtPosition(thisTower.towerData.attackSound, transform.position, thisTower.towerData.attackSoundVolume, parent: transform);
        if (animator == null)
            animator = GetComponentInParent<Animator>(); 
    }

    public override void Attack(Enemy target, CurrentTowerStats currentTowerStats)
    {
        target.TakeDamage(currentTowerStats.Damage, this.thisTower);
        target.ApplySlow(currentTowerStats.SlowFactor, currentTowerStats.SlowDuration);
        
        if(attackSoundEmitter != null && !thisTower.towerData.attackSoundLoop)
            attackSoundEmitter.ReplaySound();

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

    public override void SetLoopAttackSound(bool active)
    {
        if(attackSoundEmitter == null) return;
        if(active)
            attackSoundEmitter.ResumeSound();
        else
            attackSoundEmitter.PauseSound();
    }
}
