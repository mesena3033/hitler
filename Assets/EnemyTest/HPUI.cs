using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Unity.Cinemachine;

public class HPUI : MonoBehaviour
{
    [SerializeField] private Slider hpSlider;
    private int maxHp;

    private void Start()
    {
        SetHP(1f);
    }

    void LateUpdate()
    {
        // HPUI‚ğƒJƒƒ‰‚Ì•ûŒü‚ÉŒü‚¯‚é
        transform.rotation = Camera.main.transform.rotation;
    }

    // HP‚ÌŠ„‡‚ğó‚¯æ‚Á‚Ä•\¦
    public void SetHP(float hpRate)
    {
        hpSlider.value = hpRate;
    }

}
