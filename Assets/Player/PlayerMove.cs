using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [Header("移動")]
    [SerializeField] private float speed = 6f;
    [SerializeField] private float rotateSpeed = 15f;
    [SerializeField] private float dodgeSpeed = 10f;


    private Rigidbody rb;
    private PlayerAttack attack;

    // 入力保持
    private Vector3 moveInput;
    // 入力プロパティ
    public Vector3 MoveInput => moveInput;
    public bool IsDodge => isDodging;

    // 最後に押した横キー
    private Key lastHorizontalKey = Key.None;

    // 回避
    private Vector3 dodgeDirection;
    private float invicibleDuration = .4f;
    private float invicibleTimer;
    private bool isDodging;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        attack = GetComponent<PlayerAttack>();
        invicibleTimer = invicibleDuration;
    }

    // 入力処理
    void Update()
    {
        var kb = Keyboard.current;

        if (kb == null) return;

        // 攻撃中は移動停止
        if (attack.IsAttacking)
        {
            moveInput = Vector3.zero;
            return;
        }

        UpdateMoveInput(kb);
        Dodge(kb);
        if (isDodging)
        {
            invicibleTimer -= Time.deltaTime;

            if (invicibleTimer <= 0f)
            {
                isDodging = false;
            }
        }
    }

    // 物理処理
    void FixedUpdate()
    {
        if (isDodging)
        {
            rb.MovePosition(
                rb.position +
                dodgeDirection * dodgeSpeed * Time.fixedDeltaTime
            );

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

        Quaternion rot = Quaternion.Slerp(rb.rotation, targetRot, rotateSpeed * Time.fixedDeltaTime);

        rb.MoveRotation(rot);

    }


    // 回避
    void Dodge(Keyboard kb)
    {
        // 回避入力
        if (kb.spaceKey.wasPressedThisFrame && !isDodging)
        {
            isDodging = true;
            invicibleTimer = invicibleDuration;

            // 回避移動
            if (moveInput != Vector3.zero)
            {
                dodgeDirection = moveInput.normalized;
            }

            // 静止中
            else
            {
                dodgeDirection = -transform.forward;
            }
        }


       
    }
}

    