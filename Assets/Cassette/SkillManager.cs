using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class SkillManager : MonoBehaviour
{
    [SerializeField]
    private List<SkillData> skillList = new();

    public Animator animator;
    public EffectManager effectManager;
    public PlayerAnimation playerAnimation;

    private Dictionary<int, SkillData> skillDict =
        new Dictionary<int, SkillData>();

    private Dictionary<int, float> cooldowns =
        new Dictionary<int, float>();

    public float AnimatorChangeTime = 0.1f;

    // スキル終了処理
    bool skillActive = false;

    private void Awake()
    {
        Rebuild();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        Rebuild();
    }
#endif

    private void Rebuild()
    {
        skillDict.Clear();

        foreach (var s in skillList)
        {
            if (s == null) continue;

            if (!skillDict.ContainsKey(s.skillID))
                skillDict.Add(s.skillID, s);
            else
                Debug.LogWarning($"Duplicate SkillID: {s.skillID}");
        }
    }

    public bool UseSkill(int id)
    {
        if (!skillDict.TryGetValue(id, out var data))
            return false;

        if (cooldowns.TryGetValue(id, out float end))
        {
            if (Time.time < end)
                return false;
        }

        cooldowns[id] = Time.time + data.cooldown;

        Execute(data);

        return true;
    }

    private void Execute(SkillData data)
    {
        // Animator
        if (animator != null)
        {
            if (data.animatorController != null)
                animator.runtimeAnimatorController = data.animatorController;

            if (!string.IsNullOrEmpty(data.animatorBool))
            {
                animator.SetBool(data.animatorBool, true);
                StartCoroutine(ResetBool(data.animatorBool, data.resetTime));
                StartCoroutine(ResetAnimator(data.animatorController.name, AnimatorChangeTime));
            }
        }

        // Effect
        if (effectManager != null && data.effectPrefab != null)
        {
            effectManager.SpawnEffect(
                data.effectPrefab,
                data.effectSpawnPoint,
                data.effectDelay
            );
        }
    }

    private IEnumerator ResetBool(string name, float time)
    {
        yield return new WaitForSeconds(time);
        animator.SetBool(name, false);
        animator.SetBool("IsIdle", true);
    }

    private IEnumerator ResetAnimator(string name, float time)
    {
        yield return new WaitForSeconds(time);
        // 保存したPlayerAnimationを呼び出して、Animatorに格納
        if (animator.runtimeAnimatorController != null)
        {
            animator.runtimeAnimatorController = playerAnimation.mainController;
        }
    }
}