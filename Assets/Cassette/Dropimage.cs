/*
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
        if (draggedObj == null) return;

        SkillItem skillItem = draggedObj.GetComponent<SkillItem>();
        SlotItem slot = GetComponent<SlotItem>();

        // ドラッグしてきた Image をスロットの子にする
        draggedObj.transform.SetParent(slot.transform, false);

        // スロットの skillID を更新
        slot.skillID = skillItem.SkillID;

        draggedObj.GetComponent<DragImage>().MarkDropped();
    }
}
*/
