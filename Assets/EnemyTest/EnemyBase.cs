using UnityEngine;
using UnityEngine.AI;
public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField] protected float attackRange = 5f;

    protected Transform player;
    protected NavMeshAgent agent;
    protected Animator animator;

    protected bool isAttacking = false;

    protected void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    protected void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected void Update()
    {
        if (player == null) return;
        if (!agent.isOnNavMesh)
        {
            return;
        }

        this.transform.LookAt(player.transform);

        // 距離を計算
        float distance = Vector3.Distance(transform.position, player.position);

        // 攻撃範囲で攻撃
        if (distance <= attackRange) 
        {
            agent.isStopped = true;
            isAttacking = true;
            Attack();
        }

        // 範囲外で詰めてくる
        else if (distance > attackRange)
        {
            animator.SetBool("isIdling", false);
            animator.SetBool("isMoving", true);
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }

        // プレイヤーがいない場合は停止
        else
        {
            animator.SetBool("isIdling", true);
            animator.SetBool("isMoving", false);
            agent.isStopped = false;
            
        }

        if (isAttacking)
        {
            agent.isStopped = true;
        }

        else
        {
            agent.isStopped = false;
        }


    }

    protected abstract void Attack();

    public void OnAttackEnd()
    {
        isAttacking = false;
    }
}
