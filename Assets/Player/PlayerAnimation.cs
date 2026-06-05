using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;

    private PlayerMove move;
    private PlayerAttack attack;

    void Start()
    {
        animator = GetComponent<Animator>();

        move = GetComponent<PlayerMove>();
        attack = GetComponent<PlayerAttack>();
    }

    void Update()
    {
        bool moving =
            move.MoveInput != Vector3.zero;

        bool dodging =
            move.IsDodge;

        animator.SetBool(
            "IsDodge",
            dodging
        );

        animator.SetBool(
            "IsMove",
            moving && !dodging
        );

        animator.SetBool(
            "IsIdle",
            !moving &&
            !dodging &&
            !attack.IsAttacking
        );
    }
}