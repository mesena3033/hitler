using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RandomDrop : MonoBehaviour
{
    [SerializeField]
    private NEWSkillMane skillManager;

    [Header("1ウェーブで抽選する回数")]
    [SerializeField]
    private int dropCount = 3;

    [Header("スキルレベル上限")]
    [SerializeField]
    private int maxSkillLevel = 10;


    private void Update()
    {
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            RollDrop();
        }
    }

    public void RollDrop()
    {
        if (skillManager == null)
            return;

        Debug.Log("Press F to roll drop");

        // 抽選候補(ID)を作成
        List<int> candidates = new List<int>();

        for (int i = 0; i < skillManager.SkillCount; i++)
        {
            candidates.Add(i);
        }

        // 候補が無いなら終了
        if (candidates.Count == 0)
            return;

        // 抽選
        int count = Mathf.Min(dropCount, candidates.Count);

        for (int i = 0; i < count; i++)
        {
            // 候補からランダムに選ぶ
            int randomIndex = Random.Range(0, candidates.Count);
            int skillID = candidates[randomIndex];

            SkillDataNo2 skill =
                skillManager.GetSkill(skillID);

            // レベルアップ
            skill.skillLevel =
                Mathf.Min(skill.skillLevel + 1, maxSkillLevel);

            Debug.Log(
                $"{skill.skillName} が Lv.{skill.skillLevel} になりました。");

            // 同じウェーブでは重複しない
            candidates.RemoveAt(randomIndex);
        }
    }
}
