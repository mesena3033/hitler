using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class NEWSkillMane : MonoBehaviour
{
    [SerializeField]
    private List<SkillDataNo2> skillList = new();

    public Animator animator;
    public EffectManager effectManager;
    public PlayerAnimation playerAnimation;

    private Dictionary<int, SkillDataNo2> skillDict =
        new Dictionary<int, SkillDataNo2>();

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

    private void Start()
    {
        int skillID = skillList.Count;
    }

    private void Rebuild()
    {
        skillDict.Clear();

        for (int i = 0; i < skillList.Count; i++)
        {
            var s = skillList[i];
            if (s == null) continue;

            skillDict[i] = s;
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

    private void Execute(SkillDataNo2 data)
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
