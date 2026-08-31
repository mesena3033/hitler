using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    [Header("参照するSkillSlot")]
    [SerializeField]
    private SkillSlot skillSlot;

    [Header("UI")]
    [SerializeField]
    private Image icon;

    [SerializeField]
    private TMP_Text text;

    [SerializeField]
    private NEWSkillMane skillManager;

    private void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        // SkillSlotから現在のSkillIDを取得
        int skillID =
            skillSlot.GetSkillID();

        // スロットが空の場合
        if (skillID < 0)
        {
            icon.enabled = false;
            text.text = "";
            return;
        }

        // SkillIDからSkillDataを取得
        SkillDataNo2 data =
            skillManager.GetSkill(skillID);

        if (data == null)
        {
            icon.sprite = null;
            icon.enabled = false;

            text.text = "";

        }

        // アイコンを同期
        icon.sprite = data.Icon;
        icon.enabled = true;

        // 現在は確認用にスキル名を表示
        text.text = data.skillName;
    }
}
