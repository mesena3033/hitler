using UnityEngine;
public enum SkillType
{
    Attack,
    Buff,
    Support
}

[System.Serializable]
public class SkillDataNo2
{
    [Header("属性")]
    public SkillType type;

    [Header("Animator Controller")]
    public RuntimeAnimatorController animatorController;

    [Header("Animator Bool")]
    public string animatorBool;
    public float resetTime = 0.1f;

    [Header("エフェクト")]
    public GameObject effectPrefab;

    [Header("発生位置")]
    public Transform effectSpawnPoint;

    [Header("発生遅延")]
    public float effectDelay;

    [Header("クールタイム")]
    public float cooldown = 1f;

    [Header("スキルの名前&アイコン")]
    public string skillName;
    public Sprite Icon;

    [Header("スキルの効果")]
    public int damage;        // Attack 用
    public float buffValue;   // Buff 用
    public float healValue;   // Support 用
}
