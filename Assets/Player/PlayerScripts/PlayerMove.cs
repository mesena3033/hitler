using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [Header("移動")]
    private float speed = 12f;
    private float rotateSpeed = 800f;

    private Rigidbody rb;
    private PlayerAttack attack;
    private PlayerAnimation playerAnimation;
    private Animator animator;
    private PlayerStatus status;

    // 入力保持
    private Vector3 moveInput;
    // 入力プロパティ
    public Vector3 MoveInput => moveInput;

    // 最後に押した横キー
    private Key lastHorizontalKey = Key.None;

    // 被弾
    private bool isBeingHit = false;
    public bool IsBeingHit => isBeingHit;

    // 無敵タイマー
    private float hitDisableTimer = 1f;

    public bool IsDodging => isDodging;

    // 回避
    [Header("回避")]
    private float dodgeDistance = 15f;
    private float dodgeDuration = 1.2f;
    private float dodgeCooldown = 1f; // 回避のクールタイム（秒）
   

    private bool isDodging = false;
    private bool isDodgePending = false;
    private Vector3 dodgeDirection = Vector3.zero;
    private float dodgeTimer = 0f;
    private float dodgeSpeed = 10f;
    private float dodgePendingTimer = 0f;
    private float dodgeCooldownTimer = 0f;

    //  時間管理用
    private float dt = 0.0f;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        attack = GetComponent<PlayerAttack>();
        playerAnimation = GetComponent<PlayerAnimation>();
        status = GetComponent<PlayerStatus>();
        // Rigidbody の補間を有効にして、物理移動とアニメーションのズレを軽減
        if (rb != null) rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    // 入力処理
    void Update()
    {
        var kb = Keyboard.current;

        if (kb == null) return;

        // 攻撃中または被弾中は入力を無効化
        if (attack.IsAttacking || hitDisableTimer > 0f || status.IsPlayerDead)
        {
            moveInput = Vector3.zero;
            // 被弾無効時間のカウントダウン
            if (hitDisableTimer > 0f)
            {
                hitDisableTimer -= Time.deltaTime;
                if (hitDisableTimer <= 0f)
                {
                    Debug.Log("Hit End");
                    isBeingHit = false;
                    hitDisableTimer = 0f;
                }
            }

            return;
        }

        // 回避中または回避開始待ち中は通常入力を処理しない
        // クールタイム中でも移動は可能にして、次回回避のみ制限する
        if (isDodging /*|| isDodgePending*/)
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
        Debug.Log("Hit Start : " + duration);
        isBeingHit = true;
        hitDisableTimer = Mathf.Max(0f, duration);

    }


    // 物理処理
    void FixedUpdate()
    {
        if (attack.IsAttacking || isBeingHit) return;

        dt = Time.fixedDeltaTime;

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

        rb.MovePosition(rb.position + moveInput * speed * dt);

        Quaternion targetRot = Quaternion.LookRotation(moveInput);

        Quaternion rot =
            Quaternion.RotateTowards(rb.rotation, targetRot, rotateSpeed * dt);

        rb.MoveRotation(rot);

    }

    // 回避開始
    void StartDodge()
    {
        if(moveInput != Vector3.zero)
        {
            dodgeDirection = moveInput.normalized;
        }
        else
        {
            dodgeDirection = transform.forward;
        }
        //animator.SetBool("IsAttacking",false);
        isDodging = true;
        dodgeTimer = dodgeDuration;
        dodgeSpeed = dodgeDistance / dodgeDuration;

        playerAnimation.SetDodge(true);
        
    }

}