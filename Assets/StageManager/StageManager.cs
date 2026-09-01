using UnityEngine;
using UnityEngine.Rendering;

public class StageManager : MonoBehaviour
{
    [SerializeField] private GameObject[] stages;

    // ステージごとのスポナー
    [SerializeField] private Transform[] playerSpawnPoints;

    [SerializeField] private EnemySpawner[] enemySpawners;

    private int currentStage = 0;

    public int CurrentStage => currentStage;

    private PlayerMove player;

    void Start()
    {
        for (int i = 0; i < stages.Length; i++) 
        {
            stages[i].SetActive(i == 0);
        }

        player = FindAnyObjectByType<PlayerMove>();

        MovePlayerToSpawnPoint();
    }

    public EnemySpawner GetCurrentSpawner()
    {
        return enemySpawners[currentStage];
    }

    // スポーン
    private void MovePlayerToSpawnPoint()
    {
        if (player == null) return;

        if (currentStage >= playerSpawnPoints.Length)
        {
            Debug.Log("PlayerSpawnPointの数がない");
        }

        Transform spawnPoint = playerSpawnPoints[currentStage];

        if (spawnPoint == null)
        {
            Debug.LogError($"Stage {currentStage} のPlayerSpawnPointが設定されていません");
            return;
        }

        // プレイヤーをスポーン地点に移動
        player.transform.position = spawnPoint.position;
        player.transform.rotation = spawnPoint.rotation;
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
