using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    public Transform playerObject;   // プレイヤーオブジェクトをアタッチ

    Vector3 vel = Vector3.zero; // カメラの速度の初期化

    [SerializeField] private Vector3 offset = new Vector3(0, 9, -5); // カメラのオフセット 
    [SerializeField] private float mouseSensitivity = 0.1f; // マウス感度（調整用）
    [SerializeField] private bool invertY = false;

    private Vector3 baseOffset;
    private float yaw = 0f;
    private float pitch = 10f;
    private float radius = 0f;

    private void Awake()
    {
        baseOffset = offset;
        if (playerObject != null)
        {
            // 初期の球面座標（radius, yaw, pitch）を baseOffset から計算
            radius = baseOffset.magnitude;
            // yaw は X/Z 平面の角度
            yaw = Mathf.Atan2(baseOffset.x, baseOffset.z) * Mathf.Rad2Deg + playerObject.eulerAngles.y;
            // pitch は垂直角（上を正）
            float horiz = new Vector2(baseOffset.x, baseOffset.z).magnitude;
            pitch = Mathf.Atan2(baseOffset.y, horiz) * Mathf.Rad2Deg;
        }
    }

    private void LateUpdate()
    {
        rotateCamera();
    }
　　　

    // カメラ回転（Input System を使用）
    private void rotateCamera()
    {
        var mouse = Mouse.current;
        if (mouse == null) return; // マウスデバイスがない場合は何もしない

        Vector2 delta = mouse.delta.ReadValue();
        // 感度と方向を反映
        float mx = delta.x * mouseSensitivity;
        float my = delta.y * mouseSensitivity * (invertY ? 1f : -1f);

        // ヨー（Y軸回転）とピッチ（右軸回転）を適用してプレイヤー中心を軸にオフセットを回転
        yaw += mx;
        pitch += my;
        // ピッチの上下の限界を設定して不自然な反転を防止
        pitch = Mathf.Clamp(pitch, -90f, 20f);

        // 球面座標から位置を計算（yaw は Y 軸周り、pitch は右軸周りの角）
        float clampedPitch = Mathf.Clamp(pitch, -89f, 89f);
        // Quaternion.Euler は (x=pitch、y=yaw) だがここでは上向きが正となるように使用
        Quaternion rot = Quaternion.Euler(clampedPitch, yaw, 0f);
        Vector3 localPos = rot * new Vector3(0f, 0f, -radius);
        Vector3 desiredPos = playerObject.position + localPos;

        // 位置をスムーズに移動
        transform.position = Vector3.SmoothDamp(transform.position, desiredPos, ref vel, 0.12f);

        // プレイヤー方向を滑らかに向く
        Vector3 lookDir = playerObject.position - transform.position;
        if (lookDir.sqrMagnitude > 0f)
        {
            Quaternion targetRot = Quaternion.LookRotation(lookDir.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 10f * Time.deltaTime);
        }
    }

}
