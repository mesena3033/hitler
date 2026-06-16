using UnityEngine;
public enum SkillType
{
    Attack,
    Buff,
    Support
}

[CreateAssetMenu]
public class SkillDataNo2 : MonoBehaviour
{
    public int id;

    public SkillType skilltype;

    public string animatorBool;
    public float resetTime;

    public GameObject effectPrefab;

    public Transform effectSpawnPoint;

    public float effectDelay;

    public float cooldown;
}
