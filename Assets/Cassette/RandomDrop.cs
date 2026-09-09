using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


// リザルト画面に渡すための抽選結果
public class DropResult
{
    public int skillID;
    public string skillName;

    public int beforeLevel;
    public int afterLevel;

    public int levelUpCount;
}


public class RandomDrop : MonoBehaviour
{
    [SerializeField]
    private NEWSkillMane skillManager;

    [SerializeField]
    private WaveSystem waveSystem;

    [Header("1ウェーブで抽選する回数")]
    [SerializeField]
    private int dropCount = 2;

    [Header("スキルレベル上限")]
    [SerializeField]
    private int maxSkillLevel = 10;


    // 抽選結果を保存
    // Key   = スキルID
    // Value = 当選回数
    private Dictionary<int, int> dropResults = new();


    // ステージ終了時にリザルト画面へ渡す結果
    private List<DropResult> resultList = new();

    private void Update()
    {
        // お試し用
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            RollDrop();
        }

        // ステージ終了時に呼ぶ
        // ApplyDropResults();
    }

    // 1Wave分の抽選を行う
    public void RollDrop()
    {
        if (skillManager == null)
            return;

        int currentWave = waveSystem.CurrentWave;

        // 現在Lvが上限に達していないSkillだけを候補にする
        List<int> candidates = CreateCandidates();

        // 抽選可能なSkillがない場合
        if (candidates.Count == 0)
        {
            Debug.Log("抽選可能なSkillがありません。");
            return;
        }


        // 1Wave分の抽選

        for (int i = 0; i < dropCount; i++)
        {
            if (candidates.Count == 0)
                break;

            int randomIndex = Random.Range(0, candidates.Count);

            int skillID = candidates[randomIndex];

            SkillDataNo2 skill = skillManager.GetSkill(skillID);

            // 当選回数を記録
            if (dropResults.ContainsKey(skillID))
            {
                dropResults[skillID]++;
            }
            else
            {
                dropResults.Add(skillID, 1);
            }

            Debug.Log(
                $"抽選結果：{skill.skillName} (ID:{skillID})");
        }

        Debug.Log($"{dropCount}回の抽選が完了しました。");
    }

    /// 抽選候補を作成する
    private List<int> CreateCandidates()
    {
        List<int> candidates = new();

        for (int i = 0; i < skillManager.SkillCount; i++)
        {
            SkillDataNo2 skill = skillManager.GetSkill(i);

            // Lv上限に達しているSkillは候補から除外
            if (skill.skillLevel >= maxSkillLevel)
                continue;

            candidates.Add(i);
        }

        return candidates;
    }

    // ステージ終了時に抽選結果をまとめて反映する
    public void ApplyDropResults()
    {
        if (skillManager == null)
            return;

        // 前回のリザルトを削除
        resultList.Clear();

        foreach (KeyValuePair<int, int> result in dropResults)
        {
            int skillID = result.Key;
            int levelUpCount = result.Value;

            SkillDataNo2 skill = skillManager.GetSkill(skillID);

            // 現在のレベルを保存
            int beforeLevel = skill.skillLevel;

            // レベルアップ
            skill.skillLevel = Mathf.Min(
                skill.skillLevel + levelUpCount,
                maxSkillLevel
            );

            // 実際に上がったレベル数
            int actualLevelUpCount =
                skill.skillLevel - beforeLevel;

            // リザルト用データを作成
            DropResult dropResult = new DropResult
            {
                skillID = skillID,
                skillName = skill.skillName,

                beforeLevel = beforeLevel,
                afterLevel = skill.skillLevel,

                levelUpCount = actualLevelUpCount
            };

            resultList.Add(dropResult);

            Debug.Log(
                $"{skill.skillName} : " +
                $"Lv.{beforeLevel} → Lv.{skill.skillLevel} " +
                $"(+{actualLevelUpCount})");
        }


        // 抽選結果をリセット
        dropResults.Clear();
    }


    /// <summary>
    /// ステージリザルト用の結果を取得する
    /// </summary>
    public List<DropResult> GetDropResults()
    {
        return resultList;
    }
}
