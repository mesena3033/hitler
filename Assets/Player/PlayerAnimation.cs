using UnityEngine;

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

        animator.SetBool("IsMove", moving);

        animator.SetBool("IsIdle", !moving && !attack.IsAttacking);

    }

    public void SetDodge(bool value)
    {
        if (animator == null) return;
        animator.SetBool(dodgeBool, value);
    }

    public float GetDodgeClipLength()
    {
        if (animator == null || animator.runtimeAnimatorController == null) return 0f;
        var clips = animator.runtimeAnimatorController.animationClips;
        for (int i = 0; i < clips.Length; i++)
        {
            if (clips[i].name == dodgeClipName)
                return clips[i].length;
        }
        return 0f;
    }
}
