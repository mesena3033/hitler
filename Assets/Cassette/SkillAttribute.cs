using UnityEngine;

public enum SkillType
{
    Attack,
    Buff,
    Support
}

[CreateAssetMenu(fileName = "SkillAttribute", menuName = "Game/SkillAttribute")]
public class SkillAttribute
{
    public int skillID;
    public SkillType skillType;

    // Attack —p
    public float power;

    // Buff —p
    public float buffAmount;
    public float duration;

    // Support —p
    public float area;
    public int targetCount;
}
