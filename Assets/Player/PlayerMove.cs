using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [Header("移動")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotateSpeed = 360f;

    private Rigidbody rb;
    private PlayerAttack attack;
    private PlayerAnimation playerAnimation;

    // 入力保持
    private Vector3 moveInput;
    // 入力プロパティ
    public Vector3 MoveInput => moveInput;


    // 最後に押した横キー
    private Key lastHorizontalKey = Key.None;

    // 被弾
    private bool isBeingHit = false;
    public bool IsBeingHit {
        get { return isBeingHit; }
        set { isBeingHit = value; }
    }
    private float hitDisableTimer = 0f;

    // 回避
    [Header("回避")]
    [SerializeField] private float dodgeDistance = 5f;
    [SerializeField] private float dodgeDuration = 0.3f;
    [SerializeField] private float dodgeStartDelay = 0.08f; // アニメーション開始後に移動を始める遅延（秒）
    [SerializeField] private float dodgeCooldown = 1.5f; // 回避のクールタイム（秒）
    [SerializeField] private float dodgeSink = 0.15f; // 回避時に下げるYオフセット
   

    private bool isDodging = false;
    private bool isDodgePending = false;
    private Vector3 dodgeDirection = Vector3.zero;
    private float dodgeTimer = 0f;
    private float dodgeSpeed = 0f;
    private float dodgePendingTimer = 0f;
    private float dodgeCooldownTimer = 0f;
    private float dodgeBaseY = 0f;
    private float dodgeSinkTimer = 0f;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        attack = GetComponent<PlayerAttack>();
        playerAnimation = GetComponent<PlayerAnimation>();
    }

    // 入力処理
    void Update()
    {
        var kb = Keyboard.current;

        if (kb == null) return;

        // 攻撃中または被弾中は入力を無効化
        if (attack.IsAttacking || hitDisableTimer > 0f)
        {
            moveInput = Vector3.zero;
            // 被弾無効時間のカウントダウン
            if (hitDisableTimer > 0f)
            {
                hitDisableTimer -= Time.deltaTime;
                if (hitDisableTimer <= 0f)
                {
                    isBeingHit = false;
                    hitDisableTimer = 0f;
                }
            }

            return;
        }

        // 回避中または回避開始待ち中は通常入力を処理しない
        // クールタイム中でも移動は可能にして、次回回避のみ制限する
        if (isDodging || isDodgePending)
        {
            moveInput = Vector3.zero;
            return;
        }

        UpdateMoveInput(kb);

        // スペースで回避開始
        if (kb.spaceKey.wasPressedThisFrame && !attack.IsAttacking && !isDodging && !isDodgePending && dodgeCooldownTimer <= 0f)
        {
            StartDodge();
            attack.ResetCombo();
        }
    }

    // 外部から被弾状態を設定する（duration 秒間移動を無効化）
    public void SetBeingHit(float duration)
    {
        isBeingHit = true;
        hitDisableTimer = Mathf.Max(0f, duration);
    }

    // 物理処理
    void FixedUpdate()
    {
        if (attack.IsAttacking || isBeingHit) return;

        float dt = Time.fixedDeltaTime;

        // 回避開始待ちの処理: アニメーションが始まってから移動を開始するための遅延
        if (isDodgePending)
        {
            dodgePendingTimer -= dt;
            if (dodgePendingTimer <= 0f)
            {
                isDodgePending = false;
                isDodging = true;
                dodgeTimer = dodgeDuration;
                dodgeSpeed = dodgeDistance / Mathf.Max(0.0001f, dodgeDuration);
                // Animator の bool は既に true にしているはず
            }
        }

        // クールタイムのカウントダウン
        if (dodgeCooldownTimer > 0f)
        {
            dodgeCooldownTimer -= dt;
            if (dodgeCooldownTimer < 0f) dodgeCooldownTimer = 0f;
        }

        if (isDodging)
        {
            // 回避移動（横移動＋少し沈めて空中で回らないようにする）
            Vector3 next = rb.position + dodgeDirection * dodgeSpeed * dt;
            // sink が残っている間だけ下げる
            if (dodgeSinkTimer > 0f)
            {
                next.y = dodgeBaseY - dodgeSink;
                dodgeSinkTimer -= dt;
                if (dodgeSinkTimer < 0f) dodgeSinkTimer = 0f;
            }
            else
            {
                next.y = dodgeBaseY;
            }
            rb.MovePosition(next);

            // 回避中は常に移動方向を向くように回転を固定する
            Quaternion targetRot = Quaternion.LookRotation(dodgeDirection);
            Quaternion rot = Quaternion.RotateTowards(rb.rotation, targetRot, rotateSpeed * dt);
            rb.MoveRotation(rot);

            dodgeTimer -= dt;
            if (dodgeTimer <= 0f)
            {
                isDodging = false;
                if (playerAnimation != null)
                {
                    playerAnimation.SetDodge(false);
                }
                // 回避終了でクールタイム開始
                dodgeCooldownTimer = dodgeCooldown;
                // 高さを元に戻す
                rb.MovePosition(new Vector3(rb.position.x, dodgeBaseY, rb.position.z));
            }

            return;
        }

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

        rb.MovePosition(rb.position + moveInput * speed * dt);

        Quaternion targetRot = Quaternion.LookRotation(moveInput);

        Quaternion rot =
            Quaternion.RotateTowards(rb.rotation, targetRot, rotateSpeed * dt);

        rb.MoveRotation(rot);

    }

    // 回避開始
    void StartDodge()
    {
        // 現在向いている方向へ回避する
        Vector3 dir = transform.forward;
        dir.y = 0f;
        dir.Normalize();

        dodgeDirection = dir;
        // アニメーションが始まってから移動を始めるために待機状態にする
        isDodgePending = true;
        isDodging = false;
        dodgePendingTimer = dodgeStartDelay;

        if (playerAnimation != null)
        {
            playerAnimation.SetDodge(true);
        }
    }

}