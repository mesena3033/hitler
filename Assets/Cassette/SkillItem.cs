using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillItem : MonoBehaviour,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text skillName;

    public int SkillID { get; private set; }

    public void Setup(int skillID, SkillDataNo2 data)
    {
        SkillID = skillID;

        skillName.text = data.skillName;

        if (data.Icon != null)
        {
            icon.sprite = data.Icon;
            icon.enabled = true;
        }
        else
        {
            icon.enabled = false;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        DragImage.Instance.BeginDrag(SkillID);
    }

    public void OnDrag(PointerEventData eventData)
    {
        DragImage.Instance.Drag(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DragImage.Instance.EndDrag();
    }
}