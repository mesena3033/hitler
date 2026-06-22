using UnityEngine;

[System.Serializable]
public class SkillDataNo2
{
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
}
