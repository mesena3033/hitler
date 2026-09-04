using UnityEngine;
using UnityEngine.AI;
public class ShortEnemy : EnemyBase
{
    //[SerializeField] private float attackCooldown = 1.3f;
    [SerializeField] private int attackDamage = 10;

    protected override void Attack()
    {
        animator.SetTrigger("Attack");
    }

}