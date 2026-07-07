using UnityEngine;
public class ShortEnemy : EnemyBase
{
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private int attackDamage = 10;
    protected override void Attack()
    {
        Debug.Log("近距離攻撃！");
    }
}