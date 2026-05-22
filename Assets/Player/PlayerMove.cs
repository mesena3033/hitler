using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [Header("移動")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotateSpeed = 360f;

    [Header("攻撃")]
    [SerializeField] private float attackDuration = .75f;

    private Animator animator;
    private Rigidbody rb;

    // 入力保持
    private Vector3 moveInput;
    private bool attackInput;

    // 状態
    private bool isAttacking = false;

    // コンボ攻撃
    // コンボ予約
    private bool IsComboQueued = false;
    private int comboCount = 0;
    [SerializeField] private int maxCombo = 3;

    // コンボ入力受付時間
    private float comboWindow = 2f;
    private float comboTimer = 0f;


    // 攻撃時間
    private float attackTimer = 0f;

    // 最後に押した横キー
    private Key lastHorizontalKey = Key.None;



    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // 入力処理
    void Update()
    {
        var kb = Keyboard.current;

        if (kb == null) return;

        // 攻撃入力
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            attackInput = true;
        }
        UpdateAttack();

        // 攻撃中は移動入力停止
        if (isAttacking)
        {
            moveInput = Vector3.zero;
            animator.SetBool(
            "IsMove",
            false
        );

            animator.SetBool(
                "IsIdle",
                false
            );
            return;
        }

        UpdateMoveInput(kb);

        
        UpdateAnimation();
    }

    // 物理処理
    void FixedUpdate()
    {
        if (isAttacking) return;

        MovePlayer();
    }

    // 移動入力
    void UpdateMoveInput(Keyboard kb)
    {
        moveInput = Vector3.zero;

        Transform camT = Camera.main.transform;

        // 向いている方に進む
        Vector3 camForward = camT.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = camT.right;
        camRight.y = 0;
        camRight.Normalize();

        // 後押し優先
        if (kb.aKey.wasPressedThisFrame)
        {
            lastHorizontalKey = Key.A;
        }

        else if (kb.dKey.wasPressedThisFrame)
        {
            lastHorizontalKey = Key.D;
        }

        // 前後
        if (kb.wKey.isPressed)
        {
            moveInput += camForward;
        }

        if (kb.sKey.isPressed)
        {
            moveInput -= camForward;
        }

        // 左右
        bool a = kb.aKey.isPressed;
        bool d = kb.dKey.isPressed;

        if (a && d)
        {
            if (lastHorizontalKey == Key.A)
            {
                moveInput -= camRight;
            }

            else if (lastHorizontalKey == Key.D)
            {
                moveInput += camRight;
            }
        }

        else if (a)
        {
            moveInput -= camRight;
        }

        else if (d)
        {
            moveInput += camRight;
        }

        moveInput.Normalize();
    }

    // プレイヤー移動
    void MovePlayer()
    {
        if (moveInput == Vector3.zero) return;

        float dt = Time.fixedDeltaTime;

        rb.MovePosition(
            rb.position +
            moveInput * speed * dt
        );

        Quaternion targetRot = Quaternion.LookRotation(moveInput);

        Quaternion rot =
            Quaternion.RotateTowards(rb.rotation, targetRot, rotateSpeed * dt);

        rb.MoveRotation(rot);
    }

    // 攻撃
    void UpdateAttack()
    {
        // 攻撃開始
        if (attackInput)
        {
            // 初回攻撃
            if (!isAttacking)
            {
                StartAttack();
            }

            // コンボ攻撃
            else if (comboCount < maxCombo && comboTimer > 0f)
            {
                IsComboQueued = true;
            }
            attackInput = false;
        }

        // 攻撃していない
        if (!isAttacking)
        {
            return;
        }

        // 攻撃時間
        attackTimer -= Time.deltaTime;
        // コンボ受付時間
        comboTimer -= Time.deltaTime;

        // 攻撃終了
        if (attackTimer <= 0f)
        {
            EndAttack();
        }
    }

    void StartAttack()
    {
        isAttacking = true;
        // 初回
        if (comboCount == 0)
        {
            comboCount = 1;
        }

        attackTimer = attackDuration;

        // 次入力受付
        comboTimer = comboWindow;

        // アニメーション
        animator.SetInteger("ComboCount",comboCount);
        animator.SetBool("IsAttack",true);
        animator.SetBool("IsMove", false);
        animator.SetBool("IsIdle", false);
       
    }

    void EndAttack()
    {
        // 次コンボあり
        if (IsComboQueued)
        {
            comboCount++;
            IsComboQueued = false;

            StartAttack();
            return;
        }

        // 終了
        isAttacking = false;
        comboCount = 0;
        IsComboQueued= false;
        animator.SetBool("IsAttack", false);
        animator.SetBool("IsIdle", true);
    }

    // アニメーション
    void UpdateAnimation()
    {
        if (isAttacking) return;

        bool moving = moveInput != Vector3.zero;

        animator.SetBool(
            "IsMove",
            moving
        );

        animator.SetBool(
            "IsIdle",
            !moving
        );
    }
}