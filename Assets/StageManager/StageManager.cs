using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] private GameObject[] stages;


    // ステージごとのスポナー
    [SerializeField] private Transform[] playerSpawnPoints;
    [SerializeField] private EnemySpawner[] enemySpawners;

    private int currentStage = 0;

    public int CurrentStage => currentStage;

    void Start()
    {
        for (int i = 0; i < stages.Length; i++) 
        {
            stages[i].SetActive(i == 0);
        }
    }

    public EnemySpawner GetCurrentSpawner()
    {
        return enemySpawners[currentStage];
    }

    public void NextStage()
    {
        // 現在のステージを消す
        stages[currentStage].SetActive(false);

        currentStage++;

        // ゲームクリア
        if (currentStage >= stages.Length) 
        { 
            Debug.Log("Game Clear!");
            return;
        }

        // 次のステージを表示
        stages[currentStage].SetActive(true);
    }

    public Transform GetCurrentPlayerSpawnPoint()
    {
        return playerSpawnPoints[CurrentStage];
    }
}
