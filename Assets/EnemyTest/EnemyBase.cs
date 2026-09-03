using UnityEngine;
using UnityEngine.AI;
public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField] protected float attackRange = 5f;

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

        // プレイヤーが動けない場合は停止
        if (playerMove.CanNotMove)
        {
            Debug.Log("敵停止：PlayerMove.CanNotMove = true");
            agent.isStopped = true;
            return;
        }

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

        // agent.isStopped = false;

        this.transform.LookAt(player.transform);


        // 攻撃範囲で攻撃
        if (distance <= attackRange) 
        {
            agent.isStopped = true;
            animator.SetBool("isIdling", false);
            animator.SetBool("isMoving", false);

            // 攻撃中
            if(!isAttacking)
            {
                isAttacking = true;
                Attack();
            }

            return; 
        }
        
        agent.isStopped = false;


        animator.SetBool("isIdling", false);
        animator.SetBool("isMoving", true);
        agent.SetDestination(player.position);
       
        Vector3 lookPosition = player.position;
        transform.LookAt(lookPosition);

    }


    protected abstract void Attack();

    public void OnAttackEnd()
    {
        isAttacking = false;

        if (player == null) return;
        if (!agent.isOnNavMesh) return;

        // 攻撃終了後、移動可能状態に戻す
        agent.isStopped = false;
    }
}
