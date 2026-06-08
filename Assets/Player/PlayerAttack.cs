using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackDuration = .8f;
    [SerializeField] private int maxCombo = 3;

    private Animator animator;
    [SerializeField] private Collider swordCollider;
    private SwordHit swordHitComponent;
    private HashSet<int> hitTargets = new HashSet<int>();

    private bool attackInput;   // 攻撃入力状態か
    private bool isAttacking;   // 攻撃中か
    private bool isComboQueued;   // コンボ受付時間か
    private int comboCount = 0;
    private float attackTimer;

    // 攻撃CT
    private float attackCT = 0.5f;
    private float currentAttack;

    // プロパティ
    public bool IsAttacking => isAttacking;
    public int ComboCount => comboCount;

    void Start()
    {
        animator = GetComponent<Animator>();
        currentAttack = 0f;
        // try to find SwordHit component in children and init
        swordHitComponent = GetComponentInChildren<SwordHit>();
        if (swordHitComponent != null)
        {
            swordHitComponent.Init(this);
            var col = swordHitComponent.GetComponent<Collider>();
            if (col != null && swordCollider == null) swordCollider = col;
        }

        if (swordCollider != null) swordCollider.enabled = false;
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            attackInput = true;
        }

        if(currentAttack > 0f)
        {
            currentAttack -= Time.deltaTime;
        }

        //Debug.Log("aaa= " + currentAttack);
        UpdateAttack();
    }

    void UpdateAttack()
    {
        if (attackInput) 
        {
            // 初回攻撃
            if (!isAttacking && currentAttack <= 0f)
            {
                Debug.Log("StartAttack");
                StartAttack();
            }

            // コンボ攻撃
            else if (isAttacking && comboCount < maxCombo && attackTimer <= 0.4f) 
            {
                isComboQueued = true;
            }

            attackInput = false;

        }

        
        if (!isAttacking) return;

        // コンボ受付時間
        attackTimer -= Time.deltaTime;

        // 攻撃終了
        if(attackTimer < 0f)
        {
            EndAttack();
        }
        
    }

    void StartAttack()
    {
        isAttacking = true;
        isComboQueued = false;

        if (comboCount == 0)
        {
            comboCount = 1;
        }

        attackTimer = attackDuration;

        // 攻撃開始時にヒット対象リストをクリア
        hitTargets.Clear();

        animator.Play("Combo" + comboCount);

        // 攻撃時に剣の当たり判定を有効化
        if (swordCollider != null) swordCollider.enabled = true;

        //Debug.Log("Combo = " + comboCount);
    }

    void EndAttack()
    {
        if (isComboQueued && comboCount < maxCombo )
        {
            comboCount++;

            StartAttack();

            return;
        }

        // 終了
        isAttacking = false;
        comboCount = 0;
        isComboQueued = false;
        attackTimer = 0f;
        currentAttack = attackCT;

        // 攻撃終了で当たり判定を無効化
        if (swordCollider != null) swordCollider.enabled = false;
    }

    // Called by SwordHit when collider hits something
    public void OnSwordHit(Collider other)
    {
        if (other == null) return;

        int id = other.gameObject.GetInstanceID();
        if (hitTargets.Contains(id)) return; // 既にヒット済み
        hitTargets.Add(id);

        // ターゲットがダメージを受けられるか確認
        var damageable = other.GetComponent<IDamageable>();
        if (damageable == null) return;

        // 相手の防御力があれば使う
        float targetDefense = 0f;
        var targetStatus = other.GetComponent<PlayerStatus>();
        if (targetStatus != null) targetDefense = targetStatus.DefensePower;

        int dmg = CalculateDamage(targetDefense);
        damageable.ApplyDamage(dmg);

        // デバッグ出力: プレイヤーHP, 敵HP, プレイヤー攻撃力, 敵防御力
        var playerStatus = GetComponent<PlayerStatus>();
        int playerHP = playerStatus != null ? playerStatus.CurrentHP : -1;
        float playerAtk = playerStatus != null ? playerStatus.AttackPower : -1f;
        int enemyHP = -1;
        if (targetStatus != null) enemyHP = targetStatus.CurrentHP;

        Debug.Log($"PlayerHP={playerHP} EnemyHP={enemyHP} PlayerATK={playerAtk} EnemyDEF={targetDefense} Damage={dmg}");
    }
    private int CalculateDamage(float targetDefense)
    {
        var status = GetComponent<PlayerStatus>();
        if (status == null) return 0;

        float attackPower = status.AttackPower; // 基礎攻撃力 * バフ
        float defense = targetDefense;

        int dmg = Mathf.Max(0, Mathf.FloorToInt(attackPower - defense));
        return dmg;
    }
}

// ダメージ計算
// ダメージ = 攻撃力 - 防御力
/*
 *  基礎攻撃力、攻撃力　、バフ、実際の攻撃力が、　１０・１．５＝１５
 *  １０　　　　　　　　　１．５
/*
 */

