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
        player = FindFirstObjectByType<PlayerMove>();

        for (int i = 0; i < stages.Length; i++) 
        {
            stages[i].SetActive(i == 0);
        }

        // 開始地点へ
        MovePlayerToSpawnPoint();
    }

    public EnemySpawner GetCurrentSpawner()
    {
        if (enemySpawners == null || enemySpawners.Length == 0)
        {
            Debug.LogError("EnemySpawnerが設定されていません");
            return null;
        }

        if (currentStage < 0 || currentStage >= enemySpawners.Length)
        {
            Debug.LogError(
                $"EnemySpawnerの範囲外です。currentStage = {currentStage}, " +
                $"Spawner数 = {enemySpawners.Length}"
            );
            return null;
        }

        return enemySpawners[currentStage];
    }

    // スポーン
    private void MovePlayerToSpawnPoint()
    {
        if (player == null)
            return;

        if (playerSpawnPoints == null ||
            currentStage < 0 ||
            currentStage >= playerSpawnPoints.Length)
            return;

        Transform spawnPoint = playerSpawnPoints[currentStage];

        if (spawnPoint == null)
            return;

        Rigidbody rb = player.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.position = spawnPoint.position;
            rb.rotation = spawnPoint.rotation;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        else
        {
            player.transform.position = spawnPoint.position;
            player.transform.rotation = spawnPoint.rotation;
        }
    }

    public void NextStage()
    {
        // 現在のステージを消す
        stages[currentStage].SetActive(false);
        // 次のステージへ
        currentStage++;

        // ゲームクリア
        if (currentStage >= stages.Length) 
        { 
            Debug.Log("Game Clear!");
            return;
        }

        // 次のステージを表示
        stages[currentStage].SetActive(true);

        // プレイヤーを次のステージの開始地点へ
        MovePlayerToSpawnPoint();

    }

    public Transform GetCurrentPlayerSpawnPoint()
    {
        return playerSpawnPoints[CurrentStage];
    }

}
