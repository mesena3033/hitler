using UnityEngine;

public class EnemyStatus : MonoBehaviour, IDamageable
{
    [SerializeField] int maxHP = 50;
    [SerializeField] float defensePower = 3f; // 攻撃値から引く値（小数可）
    private WaveSystem waveSystem;
    private KillsEnemyCount killsEnemyCount;

    int currentHP;

    void Awake() => currentHP = maxHP;
    
    void Start()
    {
        waveSystem = FindFirstObjectByType<WaveSystem>();
        killsEnemyCount = FindFirstObjectByType<KillsEnemyCount>();
    }
    // 外部参照用
    public float DefensePower => defensePower;

    // IDamageable 実装（PlayerAttack から整数ダメージが渡される想定）
    public void ApplyDamage(int damage)
    {
        int final = Mathf.Max(0, Mathf.FloorToInt(damage - defensePower));
        if (final <= 0) return; 
        currentHP -= final;
        // 被弾アニメーションなど
        if (currentHP <= 0) Die();
    }

    void Die()
    {
        // 死亡処理（アニメーション、削除など）s
        if (waveSystem.isWaveRunning)
        {
            killsEnemyCount.AddKillCount();
        }
        Destroy(gameObject);
        
    }
}