using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour, 
    IDropHandler, IPointerClickHandler
{
    private int slotIndex;

    private SkillSlotMane skillSlotMane;
    private NEWSkillMane skillManager;

    [Header("UI")]
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text skillName;

    public void Initialize(
        SkillSlotMane skillSlotMane,
        NEWSkillMane skillManager,
        int slotIndex)
    {
        this.skillSlotMane = skillSlotMane;
        this.skillManager = skillManager;
        this.slotIndex = slotIndex;

        Refresh();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (DragImage.Instance.CurrentSkillID < 0)
            return;

        skillSlotMane.EquipSkill(
            slotIndex,
            DragImage.Instance.CurrentSkillID);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            skillSlotMane.UnequipSkill(slotIndex);
        }
    }

    public void Refresh()
    {
        int skillID =
            skillSlotMane.GetSkillID(slotIndex);

        if (skillID == -1)
        {
            icon.sprite = null;
            icon.enabled = false;
            skillName.text = "";
            Debug.Log("ID" + skillID);
            Debug.Log(icon.sprite);
            return;
        }

            SkillDataNo2 data =
                skillManager.GetSkill(skillID);

        icon.enabled = true;
        icon.sprite = data.Icon;
        skillName.text = data.skillName;
    }
}