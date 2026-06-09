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

    // デバッグ用: 現在のステータスを文字列で返す
    public override string ToString()
    {
        return $"HP={currentHP} ATK={AttackPower} DEF={DefensePower}";
    }

    void Start()
    {
        currentHP = maxHP;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
