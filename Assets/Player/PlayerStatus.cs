using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    // HP
    [SerializeField] private int maxHP = 100;
    [SerializeField] private int currentHP;

    public int CurrentHP => currentHP;

    // 攻撃力
    [SerializeField] private float baseAttackPower = 10f;
    [SerializeField] private float buffMultiplier = 1f; // バフ倍率（デフォルト1）
    public float BaseAttackPower => baseAttackPower;
    public float BuffMultiplier => buffMultiplier;
    public float AttackPower => baseAttackPower * Mathf.Max(0.0001f, buffMultiplier);
    
    // 防御力
    [SerializeField] private float defensePower = 5f;
    public float DefensePower => defensePower;

    void Awake()
    {
        // 保証: currentHP 初期化
        if (currentHP <= 0) currentHP = maxHP;
    }

    void Start()
    {
        currentHP = maxHP;

    }

    void Update()
    {
        // プレイヤーが死んだら
        if (currentHP <= 0)
        {
            // 死亡処理を呼び出す
            var respawn = GetComponent<PlayerRespawn>();
            if (respawn != null)
            {
                respawn.PlayerDied();
            }
        }
    }



    // ダメージ処理
    public void ApplyDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP < 0) currentHP = 0;
        // TODO: 死亡処理など
        // 被弾時に移動無効を通知（プレイヤー自身の場合）
        var mover = GetComponent<PlayerMove>();
        if (mover != null)
        {
            mover.SetBeingHit(0.5f);
        }
    }

    // 敵の攻撃力でダメージを受ける（攻撃力 - 防御力）
    public void ReceiveAttack(float enemyAttackPower)
    {
        float raw = enemyAttackPower;
        int dmg = Mathf.Max(0, Mathf.FloorToInt(raw - defensePower));
        ApplyDamage(dmg);
    }

}
