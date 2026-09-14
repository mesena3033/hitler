using UnityEngine;

public class KillsEnemyCount : MonoBehaviour
{
    public int KillCount = 0;

    public void AddKillCount()
    {
        KillCount++;

        Debug.Log("敵を倒した数：" + KillCount);
    }

    // KillsEnemyCountをinspectorで設定後、
    // killsEnemyCount.AddKillCount()で使えるよ～。
    // りょうかーい
}
