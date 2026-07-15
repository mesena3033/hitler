using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPbar : MonoBehaviour
{
    [SerializeField] private Slider playerHpSlider;
    [SerializeField] private TextMeshProUGUI hpText;

    private PlayerStatus hp;

    float maxHP, currentHP;

    private void Awake()
    {
        playerHpSlider = GetComponent<Slider>();
        hp = FindFirstObjectByType<PlayerStatus>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerHpSlider.value = 100;

        maxHP = hp.MaxHP;
        currentHP = hp.CurrentHP;
    }

    // Update is called once per frame
    void Update()
    {
        currentHP = hp.CurrentHP;

        playerHpSlider.value = currentHP;
        hpText.text = currentHP + "/" + maxHP;
    }
}
