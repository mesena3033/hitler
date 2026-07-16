using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WavePanel : MonoBehaviour
{
    private WaveSystem waveSystem;
    private TextMeshProUGUI waveStartText;
    void Start()
    {
        waveSystem = FindFirstObjectByType<WaveSystem>();
    }

    public void WaveStartPanel()
    {
        if (waveSystem == null) return;
        this.waveStartText = GetComponent<TextMeshProUGUI>();
        this.waveStartText.text = "Wave" + waveSystem.CurrentWave + "Start";
    }

}
