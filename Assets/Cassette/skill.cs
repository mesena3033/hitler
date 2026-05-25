using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class SkillData
{
    public int skillID;
    public AnimatorController controller;
}

public class skill : MonoBehaviour
{
    [Header("スキル一覧")]
    public List<SkillData> skillList = new();

    [Header("対象Animator")]
    public Animator targetAnimator;

    private Dictionary<int, AnimatorController> _skillDict;

    // ここでスキルIDとAnimatorControllerの対応を辞書にしておく
    void Start()
    {
        _skillDict = new Dictionary<int, AnimatorController>();

        foreach (var skill in skillList)
        {
            if (!_skillDict.ContainsKey(skill.skillID))
            {
                _skillDict.Add(skill.skillID, skill.controller);
            }
        }
    }

    // スキルIDを指定してAnimatorControllerを切り替えるメソッド
    public void UseSkill(int skillID)
    {
        if (_skillDict.TryGetValue(skillID, out var controller))
        {
            targetAnimator.runtimeAnimatorController = controller;
        }
    }

    private void Update()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        if (kb.digit1Key.wasPressedThisFrame)
        {
            UseSkill(1);
        }

        if (kb.digit2Key.wasPressedThisFrame)
        {
            UseSkill(2);
        }

        if (kb.digit3Key.wasPressedThisFrame)
        {
            UseSkill(3);
        }
    }
}
