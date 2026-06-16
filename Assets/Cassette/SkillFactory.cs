using System.Collections.Generic;
using UnityEngine;

public class SkillFactory : MonoBehaviour
{
    // ƒXƒLƒ‹ˆê——
    static readonly AbstractSkill[] skills = {
        new LightningSkill(),
        new HealSkill()
    };
    public SkillDataNo2 Create(SkillDataNo2 skillData)
    {
        switch (skillData.skilltype)
        {
            case SkillType.Attack:
                return new AttackSkill(skillData);
        }
    }
}
