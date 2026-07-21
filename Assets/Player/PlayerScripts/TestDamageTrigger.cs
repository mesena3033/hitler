using UnityEngine;
using UnityEngine.InputSystem;

// テスト用: Kキーで被弾処理を行う（アニメ再生・HP減少・移動無効）
public class TestDamageTrigger : MonoBehaviour
{
    [SerializeField] private float enemyAttackPower = 50f;

    private PlayerStatus status;
    private PlayerAnimation anim;
    private PlayerAttack attack;

    void Start()
    {
        status = GetComponent<PlayerStatus>();
        anim = GetComponent<PlayerAnimation>();
        attack = GetComponent<PlayerAttack>();
    }

    void Update()
    {
        //var kb = Keyboard.current;
        //if (kb == null) return;

        //if (kb.kKey.wasPressedThisFrame && !status.IsInvincible)
    }
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Bullet"))
        {
            // ダメージ適用
            if (status != null)
            {
                status.ReceiveAttack(enemyAttackPower);
            }

            // アニメーション再生
            if (anim != null)
            {
                anim.PlayDamagedOnce();
            }

            /*// 移動無効
            if (move != null)
            {
                move.SetBeingHit(hitDisableDuration);
            }*/

            // コンボリセット
            if (attack != null)
            {
                attack.ResetCombo();
            }
        }
    }
        
    
}
