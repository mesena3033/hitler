using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine.SceneManagement;

public class WaveSystem : MonoBehaviour
{
    // ウェーブ数
    int waveCount = 3;
    int currentWave = 1;

    // ウェーブプロパティ
    public int CurrentWave { get { return waveCount; } set { waveCount = value; } }


    public bool isGameStop = false;

    // ウェーブ時間
    float waveTime = 0f;
    // 時間制限
    float timeLimit = 20f;

    // ステージクリア時のスキル選択フェーズ
    float preparationPhase = 20f;

    // ウェーブ待機時間
    float waitTime = 10f;
    // ウェーブ中か
    public bool isWaveRunning = false;

    // ウェーブ開始した瞬間
    bool isWaveStarted= false;

    public bool isStageCleared = false;
    // ステージクリアCanvas
    [SerializeField] private GameObject stageEndCanvas;
    [SerializeField] private GameObject nextButton;

    [SerializeField] private StageManager stageManager;

    private PlayerMove move;
    private PlayerStatus status;
    private WavePanel panel;
    private EnemySpawner[] spawners;

    private void Start()
    {
        status = FindFirstObjectByType<PlayerStatus>();
        panel = FindFirstObjectByType<WavePanel>();
        move = FindFirstObjectByType<PlayerMove>();
        spawners = FindObjectsByType<EnemySpawner>(FindObjectsSortMode.None);
        currentWave = 1;
        waveTime = timeLimit;
        isWaveRunning = false;
        isWaveStarted = false;
        preparationPhase = 20f;
        nextButton.SetActive(false);
        isGameStop = false;

        if(stageEndCanvas != null)
        {
            stageEndCanvas.SetActive(false);
        }
    }

    private void Update()
    {
        if(isStageCleared) return;

        // 上限値越え処理
        //if (currentWave >= waveCount) return;
        //Debug.Log("現ウェーブ: " +currentWave);

        // プレイヤーが死亡していたら
        if (status.IsPlayerDead) return;

        if (!isWaveStarted) 
        {
            isWaveStarted = true;
            isWaveRunning = true;

            WaveStart();
        }

        waveTime -= Time.deltaTime;

        // wave終了
        if (waveTime <= 0f) 
        {
            isWaveRunning = false;

            WaveEnd();
        }

        // ステージクリア処理

    }

    // ウェーブ終了処理（元の挙動に戻す）
    private void WaveEnd()
    {
        if (currentWave >= waveCount)
        {
            StageClear();
            return;
        }

        currentWave++;
        waveTime = timeLimit;
        isWaveStarted = false;
        
    }

    private void StageClear()
    {
        // 敵を消す
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }


        isStageCleared = true;
        isGameStop = true;
        isWaveRunning = false;
        // ステージクリアUIを表示
        stageEndCanvas.SetActive(true);
        nextButton.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        move.CanNotMove = true;

        // ゲームを止める。スキル選択時間を考慮
        if (preparationPhase > 0.0f)
        {
            preparationPhase -= Time.deltaTime;
            
        }

        TimeStop(ref waitTime);

    }

    private void WaveStart()
    {
        panel.WaveStartPanel();
        move.CanNotMove = false;

        EnemySpawner spawner = stageManager.GetCurrentSpawner();

        if (spawner == null)
        {
            Debug.LogError("現在のステージのEnemySpawnerが取得できません");
            return;
        }

        spawner.EnemySpawn(currentWave);
    }

    public bool TimeStop(ref float time)
    {
        if (time > 0f)
        {
            time -= Time.deltaTime;
            return false;
        }

        return true;
    }

    // 次のステージへ
    public void GoNextStage()
    {
        Time.timeScale = 1f;
        stageManager.NextStage();

        // ステージクリア状態を解除
        isStageCleared = false;
        isGameStop = false;

        // クリアキャンバスをけす
        stageEndCanvas.SetActive(false);
        nextButton.SetActive(false);

        // ウェーブ数をリセット
        currentWave = 1;
        waveTime = timeLimit;
        isWaveStarted = false;
        isWaveRunning = false;

        move.CanNotMove = false;
        Debug.Log($"次のステージ開始。Wave = {currentWave}");


    }
}