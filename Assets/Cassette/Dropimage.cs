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
        if (draggedObj != null)
        {
            // ドロップされた Image の Sprite をこの Slot に移す
            Image draggedImage = draggedObj.GetComponent<Image>();
            Image slotImage = GetComponent<Image>();

            slotImage.sprite = draggedImage.sprite;
        }
    }
}
