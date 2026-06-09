using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private PlayerMove move;
    private PlayerAttack attack;
    private PlayerStatus status;
    [SerializeField] private string dodgeBool = "IsDodging";

    public RuntimeAnimatorController mainController;
    bool moving;

    private void Start()
    {
        animator = GetComponent<Animator>();
        move = GetComponent<PlayerMove>();
        attack = GetComponent<PlayerAttack>();
        status = GetComponent<PlayerStatus>();
    }

    void Update()
    {
        moving = move.MoveInput != Vector3.zero;

        animator.SetBool("IsMoving", moving);
        animator.SetBool("IsIdling", !moving && !attack.IsAttacking);

        bool isDead = status.IsPlayerDead;
        animator.SetBool("IsDead",status.IsPlayerDead);
      
    }

    public void SetDodge(bool value)
    {
        if (animator == null) return;
        animator.SetBool(dodgeBool, value);
    }

    private float lastDamagedTime = -10f;
    [SerializeField] private float damagedCooldown = 0.6f; // 同種の被弾で連続再生しない閾値
    private bool damagedPlaying = false;

    public void PlayDamagedOnce()
    {
        if (animator == null) return;
        if (damagedPlaying) return;
        if (Time.time - lastDamagedTime < damagedCooldown) return;

        damagedPlaying = true;
        lastDamagedTime = Time.time;

        animator.Play("Damaged", 0, 0f);
        animator.SetBool("IsDamaged", true);
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
