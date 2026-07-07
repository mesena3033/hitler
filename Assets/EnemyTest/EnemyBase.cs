using UnityEngine;
using UnityEngine.AI;
public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField] protected float attackRange = 5f;

    protected Transform player;
    private NavMeshAgent agent;
    protected Animator animator;


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

        // 距離を計算
        float distance = Vector3.Distance(transform.position, player.position);

        // 攻撃範囲で攻撃
        if (distance <= attackRange) 
        {
            agent.isStopped = true;
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

        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    protected abstract void Attack();
}
