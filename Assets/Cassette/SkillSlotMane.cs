using UnityEngine;

public class SkillSlotMane : MonoBehaviour
{
    [SerializeField]
    private SkillSlot[] slots = new SkillSlot[3];

    [SerializeField]
    private NEWSkillMane skillManager;

    private int[] equippedSkills = { -1, -1, -1 };

    private void Awake()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].Initialize(
                this,
                skillManager,
                i);
        }
    }

    // スキルを装備する
    public void EquipSkill(int slotIndex, int skillID)
    {
        for (int i = 0; i < equippedSkills.Length; i++)
        {
            if (i == slotIndex)
                continue;

            if (equippedSkills[i] == skillID)
            {
                Debug.Log("このスキルは既に装備されています。");
                return;
            }
        }

        equippedSkills[slotIndex] = skillID;

        slots[slotIndex].Refresh();
    }

    // スキルを外す
    public void UnequipSkill(int slotIndex)
    {
        equippedSkills[slotIndex] = -1;

        slots[slotIndex].Refresh();
    }

    // スロットのスキルIDを取得
    public int GetSkillID(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slots.Length)
            return -1;

        return equippedSkills[slotIndex];
    }
}