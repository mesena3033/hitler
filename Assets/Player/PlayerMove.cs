using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [Header("ˆÚ“®")]
    [SerializeField] private float speed = 6f;
    [SerializeField] private float rotateSpeed = 15f;

    [Header("‰ñ”ð")]
    [SerializeField] private float dodgeSpeed = 12f;
    [SerializeField] private float dodgeDuration = 0.4f;

    private Rigidbody rb;
    private PlayerAttack attack;

    private Vector3 moveInput;

    public Vector3 MoveInput => moveInput;
    public bool IsDodge => isDodging;

    private Vector3 dodgeDirection;
    private bool isDodging;
    private float dodgeTimer;

    private Key lastHorizontalKey = Key.None;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        attack = GetComponent<PlayerAttack>();
    }

    void Update()
    {
        var kb = Keyboard.current;

        if (kb == null) return;

        if (attack.IsAttacking)
        {
            moveInput = Vector3.zero;
            return;
        }

        UpdateMoveInput(kb);
        UpdateDodge(kb);

        if (isDodging)
        {
            dodgeTimer -= Time.deltaTime;

            if (dodgeTimer <= 0f)
            {
                isDodging = false;
            }
        }
    }

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

    void UpdateMoveInput(Keyboard kb)
    {
        moveInput = Vector3.zero;

        Transform cam = Camera.main.transform;

        Vector3 forward = cam.forward;
        forward.y = 0f;
        forward.Normalize();

        Vector3 right = cam.right;
        right.y = 0f;
        right.Normalize();

        if (kb.aKey.wasPressedThisFrame)
            lastHorizontalKey = Key.A;

        else if (kb.dKey.wasPressedThisFrame)
            lastHorizontalKey = Key.D;

        if (kb.wKey.isPressed)
            moveInput += forward;

        if (kb.sKey.isPressed)
            moveInput -= forward;

        bool a = kb.aKey.isPressed;
        bool d = kb.dKey.isPressed;

        if (a && d)
        {
            if (lastHorizontalKey == Key.A)
                moveInput -= right;
            else
                moveInput += right;
        }
        else if (a)
        {
            moveInput -= right;
        }
        else if (d)
        {
            moveInput += right;
        }

        moveInput.Normalize();
    }

    void MovePlayer()
    {
        if (moveInput == Vector3.zero)
            return;

        rb.MovePosition(
            rb.position +
            moveInput * speed * Time.fixedDeltaTime
        );

        Quaternion targetRot =
            Quaternion.LookRotation(moveInput);

        Quaternion rot =
            Quaternion.Slerp(
                rb.rotation,
                targetRot,
                rotateSpeed * Time.fixedDeltaTime
            );

        rb.MoveRotation(rot);
    }

    void UpdateDodge(Keyboard kb)
    {
        if (!kb.spaceKey.wasPressedThisFrame)
            return;

        if (isDodging)
            return;

        isDodging = true;
        dodgeTimer = dodgeDuration;

        if (moveInput != Vector3.zero)
        {
            dodgeDirection = moveInput.normalized;
        }
        else
        {
            dodgeDirection = -transform.forward;
        }
    }
}