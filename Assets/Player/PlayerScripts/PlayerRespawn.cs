using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    // 死亡処理
    public void PlayerDied()
    {
        // プレイヤーを非表示にする
        gameObject.SetActive(false);
        // 3秒後にリスポーンする
        Invoke(nameof(Respawn), 3f);
    }

    // リスポーン処理（存在しなかったため追加）
    private void Respawn()
    {
        // プレイヤーを再表示する
        gameObject.SetActive(true);
    }
}
