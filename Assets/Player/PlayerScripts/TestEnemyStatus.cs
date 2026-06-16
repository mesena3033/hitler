using UnityEngine;

public class EnemyStatus : MonoBehaviour, IDamageable
{
    [SerializeField] int maxHP = 50;
    [SerializeField] float defensePower = 3f; // 攻撃値から引く値（小数可）

    int currentHP;

    void Awake() => currentHP = maxHP;

    // 外部参照用
    public float DefensePower => defensePower;

    // IDamageable 実装（PlayerAttack から整数ダメージが渡される想定）
    public void ApplyDamage(int damage)
    {
        int final = Mathf.Max(0, Mathf.FloorToInt(damage - defensePower));
        if (final <= 0) return; // ダメージ無効なら何もしない／エフェクトだけ出すことも可

        currentHP -= final;
        // ここで被弾エフェクト・アニメーションなどを呼ぶ
        if (currentHP <= 0) Die();
    }

    void Die()
    {
        // 死亡処理（アニメーション、削除など）
        Destroy(gameObject);
    }
}