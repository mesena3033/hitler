using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackDuration = .75f;
    [SerializeField] private int maxCombo = 3;

    private bool attackInput;   // 攻撃入力状態か
    private bool isAttacking;   // 攻撃中か
    private bool comboQueued;   // コンボ受付時間か
    private int comboCount;
    private float attackTimer;

    // プロパティ
    public bool IsAttacking => isAttacking;
    public int ComboCount => comboCount;

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            attackInput = true;
        }

        UpdateAttack();
    }

    void UpdateAttack()
    {
        if (attackInput)
        {
            if (!isAttacking)
            {
                StartAttack();
            }

            else if (comboCount < maxCombo)
            {
                comboQueued = true;
            }

            attackInput = false;
        }

        if (!isAttacking) return;

        attackTimer -= Time.deltaTime;

        if(attackTimer <= 0)
        {
            EndAttack();
        }
        
    }

    void StartAttack()
    {
        isAttacking = true;

        if(comboCount == 0)
        {
            comboCount = 1;
        }

        attackTimer = attackDuration;
    }

    void EndAttack()
    {
        if(comboQueued && comboCount < maxCombo)
        {
            comboCount++;
            comboQueued = false;

            StartAttack();

            return;
        }

        isAttacking = false;
        comboQueued = false;
        comboCount = 0;
    }
}


