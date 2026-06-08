using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class SkillData1 : MonoBehaviour
{
    public int skillID1;

    [Header("Animator パラメータ")]
    public string animatorName = ""; // SetBool で呼びたいパラメータ名（優先）
    public float animatorResetTime = 0f; // >0 なら自動で false に戻すまでの秒数

    [Header("Animator Controller")]
    public RuntimeAnimatorController animatorController; // スキルごとにコントローラーを差し替えたい場合

    [Header("エフェクト同期")]
    public string watchStateName = ""; // Motion1 が監視するステート名（空なら即時再生）
    [Range(0f, 1f)]
    public float effectSpawnNormalizedTime = 0.1f; // 何%時点でエフェクトを再生するか

    [Header("クールタイム")]
    public float cooldown = 1f; // クールタイム（秒）
}

public class skill : MonoBehaviour
{
    [Header("スキル一覧")]
    public List<SkillData1> skillList = new();

    [Header("対象Animator系")]
    public Animator targetAnimator;

    [Header("エフェクトハンドラ")]
    public Motion1 motionHandler; // Inspectorで割り当て。未設定なら自動で探す

    private Dictionary<int, SkillData1> _skillDict;

    private Dictionary<int, float> _skillCooldownEnd = new();

    void Start()
    {
        if (targetAnimator == null)
        {
            targetAnimator = GetComponent<Animator>();
        }

        if (motionHandler == null)
        {
            motionHandler = GetComponent<Motion1>();
            if (motionHandler == null)
            {
                motionHandler = Object.FindFirstObjectByType<Motion1>();
            }
        }

        _skillDict = new Dictionary<int, SkillData1>();

        foreach (var s in skillList)
        {
            if (!_skillDict.ContainsKey(s.skillID1))
            {
                _skillDict.Add(s.skillID1, s);
            }
        }
    }

    // スキルIDを指定して Animator のコントローラー差し替えやパラメータを設定する
    // エフェクトの再生は Motion1.RequestSpawnForSkill に委譲し、アニメーションと同期する
    public void UseSkill(int skillID1)
    {
        if (_skillDict == null) return;

        if (_skillCooldownEnd.TryGetValue(skillID1, out float endTime))
        {
            if (Time.time < endTime)
            {
                return;
            }
        }

        // UseSkillの現在のIDを調べる
        Debug.Log($"UseSkill called with skillID: {skillID1}");

        if (_skillDict.TryGetValue(skillID1, out var data))
        {

            // CT開始
            _skillCooldownEnd[skillID1] = Time.time + data.cooldown;

            // スキルごとに Animator Controller を差し替える（指定があれば）
            if (targetAnimator != null && data.animatorController != null)
            {
                targetAnimator.runtimeAnimatorController = data.animatorController;
            }

            if (targetAnimator != null && !string.IsNullOrEmpty(data.animatorName))
            {
                targetAnimator.SetBool(data.animatorName, true);
                StartCoroutine(ResetAnimatorBoolAfter(data.animatorName, data.animatorResetTime > 0f ? data.animatorResetTime : 0.1f));
            }

            // Motion1 にエフェクトの再生をリクエスト（同期）
            if (motionHandler != null)
            {
                motionHandler.RequestSpawnForSkill(skillID1, data.watchStateName, data.effectSpawnNormalizedTime);

                if (motionHandler.animator == null && targetAnimator != null)
                {
                    motionHandler.animator = targetAnimator;
                }
            }
        }
    }

    private IEnumerator ResetAnimatorBoolAfter(string paramName, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (targetAnimator != null)
        {
            targetAnimator.SetBool(paramName, false);
        }
    }

    void Update()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        if (kb.digit1Key.wasPressedThisFrame)
        {
            UseSkill(1);
        }
        else if (kb.digit2Key.wasPressedThisFrame)
        {
            UseSkill(2);
        }
        else if (kb.digit3Key.wasPressedThisFrame)
        {
            UseSkill(3);
        }
    }
}
