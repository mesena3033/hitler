using UnityEngine;
using UnityEngine.AI;
public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField] protected float attackRange = 7f;
    float attackCooldownTimer = 0f;
    [SerializeField] private float attackCooldown= 1.3f;

    protected Transform player;
    protected NavMeshAgent agent;
    protected Animator animator;
    private PlayerMove playerMove;

    protected bool isAttacking = false;

    protected void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

    }

    protected void Start()
    {
        playerMove = FindFirstObjectByType<PlayerMove>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected void Update()
    {
        if (player == null) return;
        if (!agent.isOnNavMesh)return;

        // 攻撃CT
        if (attackCooldownTimer > 0f) 
        {
            attackCooldownTimer -= Time.deltaTime;
        }

        // プレイヤーが動けない場合は停止
        if (playerMove.CanNotMove)
        {
            //Debug.Log("敵停止：PlayerMove.CanNotMove = true");
            agent.isStopped = true;
            return;
        }

        agent.SetDestination(player.position);

        // 距離を計算
        float distance = Vector3.Distance(transform.position, player.position);

        // 攻撃中に移動を停止
        if (isAttacking)
        {
            agent.isStopped = true;
            animator.SetBool("isIdling", false);
            animator.SetBool("isMoving", false);
            return;
        }


        // 攻撃範囲で攻撃
        if (distance <= attackRange)
        {
            Vector3 lookPosition = player.position;
            lookPosition.y = transform.position.y;

            transform.LookAt(lookPosition);


            agent.isStopped = true;
            animator.SetBool("isIdling", false);
            animator.SetBool("isMoving", false);

            // 攻撃できるなら攻撃
            if (!isAttacking && attackCooldownTimer <= 0f)
            {
                isAttacking = true;
                attackCooldownTimer = attackCooldown;

                Attack();
            }

            return;
        }

        agent.isStopped = false;
        animator.SetBool("isIdling", false);
        animator.SetBool("isMoving", true);

    }


    protected abstract void Attack();

    public void OnAttackEnd()
    {
        Debug.Log("OnAttackEnd 呼ばれた");
        isAttacking = false;

        if (player == null) return;

        if (!agent.isOnNavMesh) return; 

        // 攻撃終了後、移動可能状態に戻す
        agent.isStopped = false;
        // プレイヤーを再び追跡
        agent.SetDestination(player.position);
        // 攻撃アニメーションから強制的に抜ける
        animator.CrossFade("Idle", 0.05f);
       
    }

}