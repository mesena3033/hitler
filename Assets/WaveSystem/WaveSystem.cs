using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

public class WaveSystem : MonoBehaviour
{
    // ウェーブ数
    int waveCount = 3;
    int currentWave = 0;

    public int CurrentWave => currentWave;

    // ウェーブ時間
    float waveTime = 0f;
    // 時間制限
    float timeLimit = 5f;

    // ウェーブ中か
    public bool isWaveRunning = false;

    // ウェーブ開始した瞬間
    bool isWaveStarted= false;

    bool isClear = false;

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
    }

    private void Update()
    {
        // 上限値越え処理
        if (currentWave > waveCount) return;
        //Debug.Log("現ウェーブ: " +currentWave);

        // ウェーブタイム進行
        if (waveTime > 0f) 
        {
            if (!status.IsPlayerDead)
            {
                if ((!isWaveStarted))
                {
                    isWaveStarted = true;
                    isWaveRunning = true;

                    WaveStart();

                    //Debug.Log(waveTime);
                }
                waveTime -= Time.deltaTime;

            }

            else
            {
                // 死亡処理
                waveTime = 0f;
                isWaveStarted = false;
                isWaveRunning = false;
                WaveEnd();
            }
        }

        else
        {
            isWaveRunning = false;
            WaveEnd();


        }


    }

    // ウェーブ終了処理
    private void WaveEnd()
    {
        //Debug.Log("クリア");
        panel.waveStartText.text = "クリア";
        move.CanNotMove = true;
        if (currentWave < waveCount)
        {
            currentWave++;
            waveTime = timeLimit;
        }

    }

    private void WaveStart()
    {
        panel.WaveStartPanel();
        move.CanNotMove = false;
        // 敵スポーン
        foreach (EnemySpawner spawner in spawners)
        {
            spawner.EnemySpawn();
        }
    }

}
