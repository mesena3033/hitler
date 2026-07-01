using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// imageをドロップするためのスクリプト
public class Dropimage : MonoBehaviour
{
    // ドロップした時に別の Image の Sprite をこの Slot に移す処理
    public void OnDrop(PointerEventData eventData)
    {
        var draggedObj = eventData.pointerDrag;
        if (draggedObj != null) return;

        SkillItem skillItem = draggedObj.GetComponent<SkillItem>();
        if (skillItem == null) return;

        SlotItem slot = GetComponent<SlotItem>();
        if (slot == null) return;

        Image draggedImage = draggedObj.GetComponent<Image>();
        slot.iconImage.sprite = draggedImage.sprite;

        slot.skillID = skillItem.skillID;

        draggedObj.GetComponent<DragImage>().MarkDropped();
    }
}
