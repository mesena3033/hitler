using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    // HP
    [SerializeField] private int maxHP = 100;
    [SerializeField] private int currentHP;

    private PlayerMove move;
    public int CurrentHP => currentHP;

    private bool isPlayerDead = false;
    public bool IsPlayerDead => isPlayerDead;

    // 攻撃力
    [SerializeField] private float baseAttackPower = 10f;
    [SerializeField] private float buffMultiplier = 1f; // バフ倍率（デフォルト1）
    public float BaseAttackPower => baseAttackPower;
    public float BuffMultiplier => buffMultiplier;
    public float AttackPower => baseAttackPower * Mathf.Max(0.0001f, buffMultiplier);
    
    // 防御力
    [SerializeField] private float defensePower = 5f;
    public float DefensePower => defensePower;

    void Start()
    {
        currentHP = maxHP;
        move = GetComponent<PlayerMove>();
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
        if (!move.IsDodging)
        {
            currentHP -= damage;
            if (currentHP <= 0)
            {
                currentHP = 0;
                isPlayerDead = true;
                return;
            }
            // TODO: 死亡処理など
            // 被弾時に移動無効を通知（プレイヤー自身の場合）
            var mover = GetComponent<PlayerMove>();
            if (mover != null)
            {
                mover.SetBeingHit(0.5f);
            }
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
