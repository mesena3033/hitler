using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("攻撃")]
    [SerializeField] private float attackDuration = .65f;
    [SerializeField] private int maxCombo = 3;

    private bool isAttackInput;  // 攻撃入力済みか
    private bool isAttacking;   // 攻撃中か
    private bool isComboQueued; // コンボ入力受付
    private float attackTimer;
    private int comboCount = 0;
    // プロパティ
    public bool IsAttacking => isAttacking;
    public int ComboCount => comboCount;

    // 強制的にIdle状態に遷移
    public bool JustFinishedAttack { get; private set; }
    void Update()
    {
        JustFinishedAttack = false;
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            isAttackInput = true;
        }

        UpdateAttack();
    }

    void UpdateAttack()
    {
        // 入力
        if (isAttackInput)
        {
            // 初回攻撃
            if (!isAttacking)
            {
                StartAttack();
            }

            // コンボ
            else if(ComboCount < maxCombo)
            {
                isComboQueued = true;
            }

            isAttackInput = false;
        }

        if (!isAttacking) return;

        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f)
        {
            EndAttack();
        }
    }

    void StartAttack()
    {
        isAttacking = true;
        isComboQueued = false;

        // 初回
        if(comboCount == 0)
        {
            comboCount = 1;
        }

        Debug.Log("combo" + comboCount);
        attackTimer = attackDuration;
    }

    void EndAttack()
    {
        // 次コンボ
        if (isComboQueued && comboCount < maxCombo)
        {
            comboCount++;
            StartAttack();
            return;
        }

        // 終了
        isAttacking = false;
        comboCount = 0;
        isComboQueued = false;
        attackTimer = 0;
        JustFinishedAttack = true;
    }
}
