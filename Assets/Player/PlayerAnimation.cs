using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private PlayerMove move;
    private PlayerAttack attack;

    void Start()
    {
        animator =GetComponent<Animator>();
        move = GetComponent<PlayerMove>();
        attack = GetComponent<PlayerAttack>();
    }

    void Update()
    {

        bool isMoving = move.MoveInput != Vector3.zero;
        animator.SetBool("IsMove", isMoving);
        animator.SetBool("IsIdle", !isMoving && !attack.IsAttacking);
        animator.SetBool("IsAttack", attack.IsAttacking);
        animator.SetInteger("ComboCount", attack.ComboCount);

        // 攻撃終了直後に強制Idle
        if (attack.JustFinishedAttack)
        {
            animator.Play("Idle");
        }
    }

}
