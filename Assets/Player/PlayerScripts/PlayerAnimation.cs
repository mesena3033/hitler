using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator animator;
    private PlayerMove move;
    private PlayerAttack attack;
    private PlayerStatus status;

    public RuntimeAnimatorController mainController;
    bool isMoving;

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
        //move.CanNotMoving();

        isMoving = move.MoveInput != Vector3.zero;

        if (!move.canNotMove)
        {
            animator.SetBool("IsMoving", isMoving);
            animator.SetBool("IsIdling", !isMoving && !attack.IsAttacking);
        }
        bool isDead = status.IsPlayerDead;
        animator.SetBool("IsDied", status.IsPlayerDead);
    }

    public void SetDodge(bool value)
    {
        move.CanNotMoving();
        //Debug.Log("SetDodge : " + value);

        if (animator == null) return;

        animator.SetBool("IsDodging", value);
    }

    private float lastDamagedTime = -10f;
    private float damagedCooldown = 1.2f; // 同種の被弾で連続再生しない閾値
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
