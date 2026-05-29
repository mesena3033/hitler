using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;

    private PlayerMove move;
    private PlayerAttack attack;

    private void Start()
    {
        animator = GetComponent<Animator>();
        move = GetComponent<PlayerMove>();
        attack = GetComponent<PlayerAttack>();
    }

    void Update()
    {
        bool moving = move.MoveInput != Vector3.zero;

        

        animator.SetBool("IsMove", moving);

        animator.SetBool("IsIdle", !moving && !attack.IsAttacking);

        animator.SetBool("IsAttack", attack.IsAttacking);

        animator.SetInteger("ComboCount", attack.ComboCount);
    }

}
