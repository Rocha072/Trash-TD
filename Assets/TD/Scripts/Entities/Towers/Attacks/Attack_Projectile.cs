using UnityEngine;

public class Attack_Projectile : TowerAttackBehavior
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform shootPoint;
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
        if (projectilePrefab == null) return;

        TowerProjectile newProjectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation).GetComponent<TowerProjectile>();
        newProjectile.SetInfoProjectile(target, currentTowerStats, this.thisTower);
        

        if(attackSoundEmitter != null && !thisTower.towerData.attackSoundLoop)
            attackSoundEmitter.ReplaySound();

        if(animator != null)
        {
            animator.ResetTrigger("Attack");
            animator.SetTrigger("Attack");
        }
    }

    public override void SetAttackEffect(bool active)
    {
        
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
