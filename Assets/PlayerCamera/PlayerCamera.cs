using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public GameObject player;   // プレイヤーオブジェクトをアタッチ

    Vector3 vel = Vector3.zero; // カメラの速度の初期化

    [SerializeField] private Vector3 offset = new Vector3(0, 3, -5); // カメラのオフセット
    [SerializeField] private Quaternion rotation = Quaternion.identity;

    private void LateUpdate()
    {
        // transform.position = player.transform.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, player.transform.position + offset, ref vel, 0.2f);
        transform .rotation = rotation;
    }

}
