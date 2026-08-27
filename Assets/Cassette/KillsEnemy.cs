using UnityEngine;

public class KillsEnemyCount : MonoBehaviour
{
    public int KillCount = 0;

    public void AddKillCount()
    {
        KillCount++;

        Debug.Log("“G‚ğ“|‚µ‚½”F" + KillCount);
    }

    // KillsEnemyCount‚ğinspector‚Åİ’èŒãA
    // killsEnemyCount.AddKillCount()‚Åg‚¦‚é‚æ`B
}
