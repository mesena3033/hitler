using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class WaveSystem : MonoBehaviour
{
    // ウェーブ数
    int waveCount = 3;
    int currentWave = 1;

    // ウェーブプロパティ
    public int CurrentWave { get { return currentWave; } set { currentWave = value; } }


    public bool isGameStop = false;

    // ウェーブ時間
    float waveTimeLimit = 15f;
    float initWaveTime = 15f;

    // 時間制限
    float stageTimeLimit = 60f;
    float initStageTime = 60f;

    // ウェーブ中か
    public bool isWaveRunning = false;

    // ウェーブ開始した瞬間
    bool isWaveStarted= false;

    // ステージクリアしたか
    public bool isStageCleared = false;

    // クリア条件キル数
    private int normaKillCount = 6;

    // ステージクリアCanvas
    [SerializeField] private GameObject stageEndCanvas;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private TextMeshProUGUI stageTimerText;
    [SerializeField] private TextMeshProUGUI waveTimerText;
    [SerializeField] private TextMeshProUGUI killCountText;
    [SerializeField] private StageManager stageManager;

    private PlayerMove move;
    private PlayerStatus status;
    private WavePanel panel;
    private EnemySpawner[] spawners;
    private KillsEnemyCount killsEnemyCount;

    private void Start()
    {
        status = FindFirstObjectByType<PlayerStatus>();
        panel = FindFirstObjectByType<WavePanel>();
        move = FindFirstObjectByType<PlayerMove>();
        spawners = FindObjectsByType<EnemySpawner>(FindObjectsSortMode.None);
        killsEnemyCount = FindFirstObjectByType<KillsEnemyCount>();
        currentWave = 1;
        isWaveRunning = false;
        isWaveStarted = false;
        waveTimeLimit = initWaveTime;
        stageTimeLimit = initStageTime;
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

        // タイマー表示
        stageTimerText.text = "StageTimeLimit \n  " + Mathf.CeilToInt(stageTimeLimit).ToString();
        waveTimerText.text = "WaveTimeLimit \n  " + Mathf.CeilToInt(waveTimeLimit).ToString();

        // キル数表示
        killCountText.text = killsEnemyCount.KillCount.ToString() + " / " + normaKillCount + " Kills";

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

        waveTimeLimit -= Time.deltaTime; // ウェーブタイマー
        stageTimeLimit -= Time.deltaTime; // ステージタイマー


        // wave終了   
        if (waveTimeLimit <= 0f) 
        {
            isWaveRunning = false;

            WaveEnd();
        }

        // ステージクリア処理
        if (stageTimeLimit <= 0f) 
        {
            if (killsEnemyCount.KillCount >= normaKillCount)    // 6kills
            {
                Debug.Log("ステージクリア");
                StageClear();
            }

            else
            {
                status.CurrentHP = 0;
                status.IsPlayerDead = true;
            }
        }

    }

    // ウェーブ終了処理（元の挙動に戻す）
    private void WaveEnd()
    {
        currentWave++;
        waveTimeLimit = initWaveTime;
        isWaveStarted = false;
        
    }

    // ステージクリア処理
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
        waveTimeLimit = initWaveTime;
        stageTimeLimit = initStageTime;

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
        waveTimeLimit = initWaveTime;
        stageTimeLimit = initStageTime;
        isWaveStarted = false;
        isWaveRunning = false;

        move.CanNotMove = false;
        Debug.Log($"次のステージ開始。Wave = {currentWave}");


    }
}