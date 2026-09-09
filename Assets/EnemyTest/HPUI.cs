using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HPUI : MonoBehaviour
{
    private Slider hpSlider;
    [SerializeField] private GameObject hpUI;
    [SerializeField] private GameObject hpComponent;
    private int hp;
    private int maxHp;
    private void Start()
    {
        
    }

    void LateUpdate()
    {
        transform.rotation = Camera.main.transform.rotation;
    }

    public int GetHp()
    {
        return hp;
    }

    public int GetMaxHp()
    {
        return maxHp;
    }

    public void UpdateHPVal()
    {
        hpSlider.value = (float)GetHp() / (float)GetMaxHp();
    }

}
