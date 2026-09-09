using UnityEngine;

public class EnemyStatus : MonoBehaviour, IDamageable
{
    [SerializeField] int maxHP = 50;
    [SerializeField] float defensePower = 3f; // 攻撃値から引く値（小数可）
    private WaveSystem waveSystem;
    private KillsEnemyCount killsEnemyCount;
    private EnemyBase enemyBase;
    private HPUI hpUI;

    private int currentHP;

    public int CurrentHP => currentHP;
    public int MaxHP => maxHP;

    void Awake()
    {
        currentHP = maxHP;
    }
    
    void Start()
    {
        waveSystem = FindFirstObjectByType<WaveSystem>();
        killsEnemyCount = FindFirstObjectByType<KillsEnemyCount>();
        enemyBase = GetComponent<EnemyBase>();
        hpUI = GetComponentInChildren<HPUI>();
        UpdateHPUI();
    }
    // 外部参照用
    public float DefensePower => defensePower;

    // IDamageable 実装（PlayerAttack から整数ダメージが渡される想定）
    public void ApplyDamage(int damage)
    {
        int final = Mathf.Max(0, Mathf.FloorToInt(damage - defensePower));
        if (final <= 0) return; 
        currentHP -= final;
        // 被弾アニメーション
        enemyBase.OnDamagedAnim();
        // HPUI 更新
        UpdateHPUI();

        if (currentHP <= 0) Die();
    }

    private void UpdateHPUI()
    {
        if (hpUI == null) return;
        float hpRate = (float)currentHP / maxHP;
        hpUI.SetHP(hpRate);
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