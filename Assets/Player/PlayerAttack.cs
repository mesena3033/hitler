using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackDuration = .8f;
    [SerializeField] private int maxCombo = 3;

    private Animator animator;

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

        Debug.Log("aaa= " + currentAttack);
        UpdateAttack();
    }

    void UpdateAttack()
    {
        Debug.Log(
    "Input=" + attackInput +
    " IsAttacking=" + isAttacking +
    " CurrentAttack=" + currentAttack
);
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

        animator.Play("Combo" + comboCount);

        Debug.Log("Combo = " + comboCount);
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
    }
}


