using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillItem : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text skillName;

    public void Setup(SkillDataNo2 data)
    {
        icon.sprite = data.Icon;
        skillName.text = data.skillName;
    }
}