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
    int currentWave = 0;
    public bool isGameStop = false;

    public int CurrentWave => currentWave;

    // ウェーブ時間
    float waveTime = 0f;
    // 時間制限
    float timeLimit = 5f;

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
        currentWave = 0;
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
        if (currentWave > waveCount) return;
        //Debug.Log("現ウェーブ: " +currentWave);

        // ウェーブタイム進行
        if (waveTime > 0f)
        {
            if (!status.IsPlayerDead)
            {
                if (!isWaveStarted)
                {
                    isWaveStarted = true;
                    isWaveRunning = true;
                    WaveEnd();

                    //Debug.Log(waveTime);
                }
                waveTime -= Time.deltaTime;

            }

            else
            {
                currentWave++;
                waveTime -= Time.deltaTime;
            }
        }

        else
        {
            isWaveRunning = false;
            // ウェーブ時間が尽きたときの終了処理
            WaveEnd();

        }

        // ステージクリア処理

    }

    // ウェーブ終了処理（元の挙動に戻す）
    private void WaveEnd()
    {
        if (currentWave < waveCount)
        {
            // 次のウェーブへ
            currentWave++;
            waveTime = timeLimit;
            WaveStart();
        }

        else
        {
            // 最終ウェーブクリア：ステージクリア表示
            StageClear();
            if (stageEndCanvas != null)
            {
                stageEndCanvas.SetActive(true);
            }
        }

        
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

        //Debug.Log("Spawner取得前");
        EnemySpawner spawner = stageManager.GetCurrentSpawner();

        //Debug.Log("Spawner = " + spawner);

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

        // ウェーブ数をリセット
        currentWave = 0;
        waveTime = timeLimit;
        isWaveStarted = false;
        isWaveRunning = false;

        move.CanNotMove = false;

        
    }
}