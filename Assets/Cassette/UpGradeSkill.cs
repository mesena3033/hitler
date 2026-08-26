using UnityEngine;

public class UpGradeSkill : MonoBehaviour
{
    [SerializeField]
    private NEWSkillMane skillManager;

    float Normalrate = 1.0f;

    // 指定したスキルIDから、現在のスキルレベルに応じた倍率を取得する
    public float GetMultiplier(int skillID)
    {
        // SkillManagerが設定されていない場合
        if (skillManager == null)
        {
            Debug.LogWarning(
                "UpGradeSkill : skillManagerが設定されていません。");

            return Normalrate;
        }

        // IDが存在する範囲か確認
        if (skillID < 0 || skillID >= skillManager.SkillCount)
        {
            Debug.LogWarning(
                $"UpGradeSkill : 存在しないSkillIDです。ID = {skillID}");

            return Normalrate;
        }

        // SkillIDからSkillDataを取得
        SkillDataNo2 skillData =
            skillManager.GetSkill(skillID);

        // SkillDataが存在しない場合
        if (skillData == null)
        {
            Debug.LogWarning(
                $"UpGradeSkill : SkillDataが存在しません。ID = {skillID}");

            return Normalrate;
        }

        // 現在のスキルレベルを取得
        int skillLevel =
            skillData.skillLevel;

        // スキル固有の成長率を取得
        float skillUpGrade =
            skillData.skillUpGrade;

        // レベル1では基礎倍率の1.0倍
        if (skillLevel <= 1)
        {
            return Normalrate;
        }

        // レベルに応じた倍率を計算
        
        float multiplier =
            Normalrate +
            (skillLevel - 1) * skillUpGrade;

        return multiplier;
    }
}
