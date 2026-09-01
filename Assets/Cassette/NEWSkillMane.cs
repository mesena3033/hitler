using System.Collections.Generic;
using UnityEngine;
using System.Collections;

// スキルの管理と実行、スキルの仕訳を行うクラス
public class NEWSkillMane : MonoBehaviour
{
    [SerializeField]
    private List<SkillDataNo2> skillList = new();

    [SerializeField]
    private UpGradeSkill upGradeSkill;

    public Animator animator;
    public EffectManager effectManager;
    public PlayerAnimation playerAnimation;
    private PlayerMove playerMove;

    private Dictionary<int, SkillDataNo2> skillDict =
        new Dictionary<int, SkillDataNo2>();

    private Dictionary<int, float> coolTimes =
        new Dictionary<int, float>();

    private Dictionary<int, float> skillUpGrade =
    new Dictionary<int, float>();

    private GameObject currentEffect;

    public bool isUsingSkill = false;

    public int SkillCount => skillList.Count;

    public float AnimatorChangeTime = 0.1f;

    private void Awake()
    {
        Rebuild();
        playerMove = GetComponent<PlayerMove>();
        
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

        // スキルリストを辞書に変換
        for (int i = 0; i < skillList.Count; i++)
        {
            var s = skillList[i];
            if (s == null) continue;

            skillDict[i] = s;
        }
    }

    public SkillDataNo2 GetSkill(int id)
    {
        return skillDict[id];
    }

    // スキルを使用するメソッド
    public bool UseSkill(int id)
    {
        if (playerMove.IsDodging)
        {
            return false;

        }

        // スキルが存在するか確認
        if (!skillDict.TryGetValue(id, out var data))
            return false;

        // スキルのクールダウンがあるか確認
        if (coolTimes.TryGetValue(id, out float end))
        {
            if (Time.time < end)
                return false;
        }

        coolTimes[id] = Time.time + data.coolTime;

        isUsingSkill = true;

        // スキルの分類に応じて処理を分岐
        switch (data.type)
        {
            case SkillType.Attack:
                ExecuteAttack(id,data);
                break;

            case SkillType.Buff:
                ExecuteBuff(data);
                break;

            case SkillType.Support:
                ExecuteSupport(data);
                break;
        }

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
                StartCoroutine(ResetAnimator(AnimatorChangeTime));
            }
        }

        // Effect
        if (effectManager != null && data.effectPrefab != null)
        {
            effectManager.SpawnEffect(
                data.effectPrefab,
                data.effectSpawnPoint,
                data.effectDelay,
                effect =>
                {
                    currentEffect = effect;
                }
            );
        }
    }

    private IEnumerator ResetBool(string name, float time)
    {
        yield return new WaitForSeconds(time);
        animator.SetBool(name, false);
        animator.SetBool("IsIdle", true);
    }

    public IEnumerator ResetAnimator(float time)
    {
        yield return new WaitForSeconds(time);
       
       HitChangeAnimation();

        isUsingSkill = false;
    }

    private void ExecuteAttack(int skillID,SkillDataNo2 data)
    {
        Execute(data);
        PlayerAttack playerAttack = GetComponent<PlayerAttack>();
        if (playerAttack != null)
        {
            float multiplier =
        upGradeSkill.GetMultiplier(skillID);

            int damage =
                Mathf.RoundToInt(data.damage * multiplier);

            playerAttack.SkillDamage(damage);

            Debug.Log($"Skill {data.skillName} executed with damage: {playerAttack.SkillDamage(damage)}");
        }
       
    }
    private void ExecuteBuff(SkillDataNo2 data)
    {
        Execute(data);
        PlayerAttack playerAttack = GetComponent<PlayerAttack>();
        if (playerAttack != null)
        {
            playerAttack.ATKUp(data.buffValue);
        }
    }
    private void ExecuteSupport(SkillDataNo2 data)
    {
        Execute(data);
    }

    public void HitChangeAnimation()
    {
        effectManager.CancelSpawn();

        // 保存したPlayerAnimationを呼び出して、Animatorに格納
        if (animator.runtimeAnimatorController != null)
        {
            animator.runtimeAnimatorController = playerAnimation.mainController;
        }

        if (currentEffect != null)
        {
            Destroy(currentEffect);
            currentEffect = null;
        } // お試し
    }

    public float GetCoolTime(int skillID)
    {
        // CTが存在しない場合
        if (!coolTimes.TryGetValue(skillID, out float endTime))
        {
            return 0f;
        }

        // 現在時刻から終了時刻までの時間を計算
        float remainingTime = endTime - Time.time;

        // マイナスにならないようにする
        return Mathf.Max(0f, remainingTime);
    }

    public float GetMaxCoolTime(int skillID)
    {
        if (!skillDict.TryGetValue(skillID, out SkillDataNo2 data))
        {
            return 0f;
        }

        return data.coolTime;
    }
}
