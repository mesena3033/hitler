using UnityEngine;
public class ShortEnemy : EnemyBase
{
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private int attackDamage = 10;
    protected override void Attack()
    {
        animator.SetTrigger("isAttacking");
    }

}