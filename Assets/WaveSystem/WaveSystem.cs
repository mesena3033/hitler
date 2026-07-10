using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WaveSystem : MonoBehaviour
{
    // ウェーブ数
    int waveCount = 3;
    int currentWave = 0; 
    // ウェーブ時間
    float waveTime = 0f;
    // 時間制限
    float timeLimit = 5f;

    // ウェーブ中か
    bool isWaveRunning = false;

    bool isClear = false;

    private PlayerStatus status;

    private void Start()
    {
        status = FindFirstObjectByType<PlayerStatus>();
        currentWave = 0;
        waveTime = timeLimit;
        isWaveRunning = false;
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
                isWaveRunning = true;

                waveTime -= Time.deltaTime;

                //Debug.Log(waveTime);
            }

            else
            {
                // 死亡処理
                waveTime = 0f;
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

        if(currentWave < waveCount)
        {
            currentWave++;
            waveTime = timeLimit;
        }
    }

}
