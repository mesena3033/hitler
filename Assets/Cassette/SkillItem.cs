using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI skillNameText;
    [SerializeField] private Image iconImage;

    public int SkillID { get; private set; }

    private SkillDataNo2 skillData;

    public SkillDataNo2 Data => skillData;

    public void Setup(int id, SkillDataNo2 data)
    {
        SkillID = id;
        skillData = data;

        // 名前
        if (skillNameText != null)
            skillNameText.text = data.skillName;

        // アイコン
        if (iconImage != null)
        {
            if (data.Icon != null)
            {
                iconImage.sprite = data.Icon;
                iconImage.enabled = true;
            }
            else
            {
                iconImage.enabled = false;
            }
        }
    }
}