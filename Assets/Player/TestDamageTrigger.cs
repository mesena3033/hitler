using UnityEngine;
using UnityEngine.InputSystem;

// テスト用: Kキーで被弾処理を行う（アニメ再生・HP減少・移動無効）
public class TestDamageTrigger : MonoBehaviour
{
    [SerializeField] private float enemyAttackPower = 50f;
    [SerializeField] private float hitDisableDuration = 0.5f;

    private PlayerStatus status;
    private PlayerAnimation anim;
    private PlayerMove move;
    private PlayerAttack attack;

    void Start()
    {
        status = GetComponent<PlayerStatus>();
        anim = GetComponent<PlayerAnimation>();
        move = GetComponent<PlayerMove>();
        attack = GetComponent<PlayerAttack>();
    }

    void Update()
    {
        //var kb = Keyboard.current;
        //if (kb == null) return;

        // if (kb.kKey.wasPressedThisFrame)
    }


    private void OnCollisionEnter(Collision enemyAttack)
    {
        // ダメージ適用
        if (enemyAttack.gameObject.CompareTag("Bullet"))
        {
            if (status != null)
            {
                status.ReceiveAttack(enemyAttackPower);
            }

            // アニメーション再生
            if (anim != null)
            {
                anim.PlayDamagedOnce();
            }

            // 移動無効
            if (move != null)
            {
                move.SetBeingHit(hitDisableDuration);
            }

            // コンボリセット
            if (attack != null)
            {
                attack.ResetCombo();
            }

            Debug.Log($"TestDamageTrigger: applied attack {enemyAttackPower}");
        }
    }

}

