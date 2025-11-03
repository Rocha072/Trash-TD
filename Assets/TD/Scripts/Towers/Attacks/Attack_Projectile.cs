using UnityEngine;

public class Attack_Projectile : TowerAttackBehavior
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Animator animator; 

    public override void Init(Tower tower)
    {
        if (animator == null)
            animator = GetComponentInParent<Animator>(); 
    }

    public override void Attack(Enemy target, float damage, float slowFactor, float slowDuration)
    {
        if (projectilePrefab == null) return;

        TowerProjectile newProjectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation).GetComponent<TowerProjectile>();
        newProjectile.target = target;
        newProjectile.damage = damage;
        newProjectile.slowDuration = slowDuration;
        newProjectile.slowFactor = slowFactor;

        if(animator != null)
        {
            animator.ResetTrigger("Attack");
            animator.SetTrigger("Attack");
        }
    }

    public override void SetAttackEffect(bool active)
    {
        
    }
}
