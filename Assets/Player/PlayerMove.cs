using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float speed = 5f;  // 移動速度

    [SerializeField] private float invicibleTime = 1.5f;  // 無敵時間

    [SerializeField] private float rotateSpeed = 540f;   // 向く速度

    private Key lastHorizontalKey = Key.None;  // 最後に押された水平移動キー
    private Rigidbody rb;

    private void Start()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
    }

    void FixedUpdate()
    {
        
        var kb = Keyboard.current;
        if (kb == null) return; // Input System が初期化されていない場合は何もしない

        Vector3 move = Vector3.zero;

        // カメラの向きを基準に前後左右を決定（カメラの水平成分のみを使用）
        Transform camT = Camera.main != null ? Camera.main.transform : null;
        Vector3 camForward = camT != null ? camT.forward : transform.forward;
        camForward.y = 0f;
        camForward.Normalize();
        Vector3 camRight = camT != null ? camT.right : transform.right;
        camRight.y = 0f;
        camRight.Normalize();

        // 押した瞬間を記録
        if (kb.aKey.wasPressedThisFrame)
        {
            lastHorizontalKey = Key.A;
        }

        else if (kb.dKey.wasPressedThisFrame)
        {
            lastHorizontalKey = Key.D;
        }

        // 前後移動（カメラ基準）
        if (kb.wKey.isPressed)
        {
            move += camForward;
        }

        if (kb.sKey.isPressed)
        {
            move -= camForward;
        }

        // 左右移動（カメラ基準）
        bool a = kb.aKey.isPressed;
        bool d = kb.dKey.isPressed;
        if (a && d)
        {
            //最後に押されたキーの方に移動
            if (lastHorizontalKey == Key.A)
            {
                move -= camRight;
            }
            else if (lastHorizontalKey == Key.D)
            {
                move += camRight;
            }
        }
        else if (a)
        {
            move -= camRight;
        }
        else if (d)
        {
            move += camRight;
        }

        // 移動量がある場合のみ移動・回転
        if (move != Vector3.zero)
        {
            // 斜め移動でも速度一定（ワールド座標で移動）
            Vector3 dir = move.normalized;
            float dt = Time.fixedDeltaTime;
            // Rigidbody があれば物理挙動で移動・回転する
            if (rb != null)
            {
                rb.MovePosition(rb.position + dir * speed * dt);

                // 移動方向に滑らかに向く
                Vector3 lookDir = new Vector3(dir.x, 0f, dir.z);
                if (lookDir.sqrMagnitude > 0f)
                {
                    Quaternion targetRot = Quaternion.LookRotation(lookDir);
                    float step = rotateSpeed * dt; 
                    Quaternion newRot = Quaternion.RotateTowards(rb.rotation, targetRot, step);
                    rb.MoveRotation(newRot);
                }
            }
            else
            {
                transform.Translate(dir * dt * speed, Space.World);

                // 移動方向に滑らかに向く
                Vector3 lookDir = new Vector3(dir.x, 0f, dir.z);
                if (lookDir.sqrMagnitude > 0f)
                {
                    Quaternion targetRot = Quaternion.LookRotation(lookDir);
                    float step = rotateSpeed * dt; 
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, step);
                }
            }
        }
    }
}
