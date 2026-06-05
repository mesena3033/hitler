using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackDuration = 0.8f;
    [SerializeField] private int maxCombo = 3;

    [SerializeField] private float attackCooldown = 0.5f;

    private Animator animator;

    private bool attackInput;
    private bool isAttacking;
    private bool isComboQueued;

    private int comboCount;
    private float attackTimer;

    private float cooldownTimer;

    public bool IsAttacking => isAttacking;
    public int ComboCount => comboCount;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;

        if (Mouse.current.leftButton.wasPressedThisFrame)
            attackInput = true;

        UpdateAttack();
    }

    void UpdateAttack()
    {
        if (attackInput)
        {
            if (!isAttacking && cooldownTimer <= 0f)
            {
                StartAttack();
            }
            else if (isAttacking &&
                     comboCount < maxCombo &&
                     attackTimer <= 0.4f)
            {
                isComboQueued = true;
            }

            attackInput = false;
        }

        if (!isAttacking)
            return;

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

        if (comboCount == 0)
            comboCount = 1;

        attackTimer = attackDuration;

        animator.Play("Combo" + comboCount);
    }

    void EndAttack()
    {
        if (isComboQueued &&
            comboCount < maxCombo)
        {
            comboCount++;

            StartAttack();
            return;
        }

        isAttacking = false;
        isComboQueued = false;
        comboCount = 0;

        cooldownTimer = attackCooldown;
    }
}