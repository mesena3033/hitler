using UnityEngine;
using UnityEngine.UI;

public class WavePanel : MonoBehaviour
{
    private WaveSystem waveSystem;
    private Text waveStartText;
    void Start()
    {
        waveSystem = GetComponent<WaveSystem>();
    }

    public void WaveStartPanel()
    {
        this.waveStartText = GetComponent<Text>();
        this.waveStartText.text = "Wave" + waveSystem.CurrentWave + "Start";
    }

}
