using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator animator;
    private PlayerMove move;
    private PlayerAttack attack;
    private PlayerStatus status;

    public RuntimeAnimatorController mainController;
    bool moving;

    private float dodgingTimer = 1f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        move = GetComponent<PlayerMove>();
        attack = GetComponent<PlayerAttack>();
        // アニメーターを物理と同期
        if (animator != null) animator.updateMode = AnimatorUpdateMode.Fixed;
        status = GetComponent<PlayerStatus>();
    }

    void Update()
    {
        moving = move.MoveInput != Vector3.zero;

        animator.SetBool("IsMoving", moving);
        animator.SetBool("IsIdling", !moving && !attack.IsAttacking);

        bool isDead = status.IsPlayerDead;
        animator.SetBool("IsDied", status.IsPlayerDead);
    }

    public void SetDodge(bool value)
    {
        /*if (animator == null) return;
        Debug.Log("SetDodge : " + value);
        animator.SetBool("IsDodging", value);

        if (value)
        {
            animator.SetFloat("DodgeCT",1f);
        }
        else
        {
            animator.SetFloat("DodgeCT", 0f);
        }*/
        Debug.Log("SetDodge : " + value);

        if (animator == null) return;

        animator.SetBool("IsDodging", value);
    }

    private float lastDamagedTime = -10f;
    [SerializeField] private float damagedCooldown = 1f; // 同種の被弾で連続再生しない閾値
    private bool damagedPlaying = false;

    public void PlayDamagedOnce()
    {
        if (animator == null) return;
        if (damagedPlaying) return;
        if (Time.time - lastDamagedTime < damagedCooldown) return;

        damagedPlaying = true;
        lastDamagedTime = Time.time;

        if (!status.IsPlayerDead && !move.IsDodging)
        {
            animator.Play("Damaged", 0, 0f);
            animator.SetBool("IsDamaged", true);
            // 被弾後無敵
            float timer = 1f;
            if (timer > 0f) 
            {
                timer-=Time.deltaTime;

            }
        }

        // 自動で IsDamaged/playing を解除
        CancelInvoke(nameof(ResetDamaged));
        Invoke(nameof(ResetDamaged), damagedCooldown);
    }

    private void ResetDamaged()
    {
        if (animator != null) animator.SetBool("IsDamaged", false);
        damagedPlaying = false;
    }


}
