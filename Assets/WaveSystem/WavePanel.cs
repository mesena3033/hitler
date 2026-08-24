using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WavePanel : MonoBehaviour
{
    private WaveSystem waveSystem;
    public TextMeshProUGUI waveStartText;

    void Awake()
    {
        waveSystem = FindAnyObjectByType<WaveSystem>();
    }

    public void WaveStartPanel()
    {
        waveStartText.text = "Wave" + waveSystem.CurrentWave + "Start"; 
    }


}
