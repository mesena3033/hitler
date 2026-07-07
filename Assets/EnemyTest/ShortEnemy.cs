using UnityEngine;
public class ShortEnemy : EnemyBase
{
    [SerializeField] private float attackCooldown = 1.3f;
    [SerializeField] private int attackDamage = 10;

    // 攻撃のオーバーライド
    protected override void Attack()
    {
        animator.SetTrigger("isAttacking");
    }

}