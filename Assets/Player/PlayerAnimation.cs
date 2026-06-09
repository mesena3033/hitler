using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private PlayerMove move;
    private PlayerAttack attack;
    [SerializeField] private string dodgeBool = "IsDodging";
    [SerializeField] private string dodgeClipName = "Dodge"; // 回避アニメーションのクリップ名


    private void Start()
    {
        animator = GetComponent<Animator>();
        move = GetComponent<PlayerMove>();
        attack = GetComponent<PlayerAttack>();
    }

    void Update()
    {
        bool moving = move.MoveInput != Vector3.zero;

        animator.SetBool("IsMoving", moving);

        animator.SetBool("IsIdling", !moving && !attack.IsAttacking);

        if (Keyboard.current.kKey.isPressed) 
        {
            // 直接ダメージアニメーションを再生して優先させる
            if (animator != null)
            {
                animator.Play("Damaged", 0, 0f);
                // 0.5秒間移動できないようにする
                // 移動無効を PlayerMove 側で扱う
                move.SetBeingHit(1.5f);
                Invoke(nameof(ResetHit), 1.5f);

            }
            animator.SetBool("IsDamaged", true);
            // 移動無効はすでに PlayerMove.SetBeingHit で設定済み
            attack.ResetCombo();
        }

        else
        {
            animator.SetBool("IsDamaged", false);
            move.IsBeingHit = false;
        }
    }

    public void SetDodge(bool value)
    {
        if (animator == null) return;
        animator.SetBool(dodgeBool, value);
    }

    // 追加: Invoke から呼ばれるメソッドを定義
    private void ResetHit()
    {
        if (move != null) move.IsBeingHit = false;
        if (animator != null) animator.SetBool("IsDamaged", false);

    }
}
