using UnityEngine;
using System.Collections.Generic;

public class UsedSkills : MonoBehaviour
{
    public HashSet<int> usedSkills = new HashSet<int>();

    public void MarkUsed(int skillID)
    {
        usedSkills.Add(skillID);
    }

    public bool IsUsed(int skillID)
    {
        return usedSkills.Contains(skillID);
    }
}
