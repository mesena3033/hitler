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
    private Image coolTimeImage;

    [SerializeField]
    private TMP_Text text;

    [Header("Skill Manager")]
    [SerializeField]
    private NEWSkillMane skillManager;

    private int currentSkillID = -1;

    private void Start()
    {
        Refresh();
    }

    private void Update()
    {
        RefreshCooldown();
    }

    // SkillSlotの内容をUIに反映
    public void Refresh()
    {
        if (skillSlot == null)
        {
            Debug.LogWarning(
                $"{name} : SkillSlotが設定されていません。");

            return;
        }

        if (skillManager == null)
        {
            Debug.LogWarning(
                $"{name} : NEWSkillManeが設定されていません。");

            return;
        }

        if (icon == null)
        {
            Debug.LogWarning(
                $"{name} : Iconが設定されていません。");

            return;
        }

        if (coolTimeImage == null)
        {
            Debug.LogWarning(
                $"{name} : CooldownImageが設定されていません。");

            return;
        }

        if (text == null)
        {
            Debug.LogWarning(
                $"{name} : Textが設定されていません。");

            return;
        }

        // SkillSlotから現在のSkillIDを取得
        int skillID = skillSlot.GetSkillID();

        // スキルが変更された場合
        if (currentSkillID != skillID)
        {
            currentSkillID = skillID;

            UpdateSkillDisplay(skillID);
        }
    }

    // スキルそのものをUIへ反映
    private void UpdateSkillDisplay(int skillID)
    {
        // スロットが空の場合
        if (skillID < 0)
        {
            icon.sprite = null;
            icon.enabled = false;

            coolTimeImage.sprite = null;
            coolTimeImage.enabled = false;

            text.text = "";

            return;
        }

        // SkillIDからSkillDataを取得
        SkillDataNo2 data = skillManager.GetSkill(skillID);

        if (data == null)
        {
            icon.sprite = null;
            icon.enabled = false;

            coolTimeImage.sprite = null;
            coolTimeImage.enabled = false;

            text.text = "";

            Debug.LogWarning(
                $"SkillUI : SkillDataが取得できませんでした。ID = {skillID}");

            return;
        }

        // 通常アイコンを設定
        icon.sprite = data.Icon;
        icon.enabled = true;

        // CT用アイコンにも同じSpriteを設定
        coolTimeImage.sprite = data.Icon;
        coolTimeImage.enabled = true;

        // 最初はスキル名を表示
        text.text = data.skillName;

        // CT表示を更新
        RefreshCooldown();
    }

    // クールタイム表示を更新
    private void RefreshCooldown()
    {
        // スキルが設定されていない場合
        if (currentSkillID < 0 || skillManager == null || coolTimeImage == null || text == null)
        {
            return;
        }

        // 現在のCTを取得
        float remainingTime =
            skillManager.GetCoolTime(currentSkillID);

        // 最大CTを取得
        float maxCoolTime =
            skillManager.GetMaxCoolTime(currentSkillID);

        // CTが存在しない、または終了した場合
        if (remainingTime <= 0f || maxCoolTime <= 0f)
        {
            coolTimeImage.fillAmount = 0f;

            coolTimeImage.enabled = false;

            SkillDataNo2 data =
                skillManager.GetSkill(currentSkillID);

            text.text = data.skillName;

            return;
        }

        // CT中
        coolTimeImage.enabled = true;

        // CTの残り割合を計算
        float coolTimeRate =
            remainingTime / maxCoolTime;

        // 0～1の範囲に収める
        coolTimeRate =
            Mathf.Clamp01(coolTimeRate);

        // 上から下へ黒い部分を減らす
        coolTimeImage.fillAmount = coolTimeRate;

        // 残りCTを表示
        text.text =
            remainingTime.ToString("F1");
    }
}