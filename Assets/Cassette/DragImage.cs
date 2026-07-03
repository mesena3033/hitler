using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// imageをドラッグするためのスクリプト
public class DragImage : MonoBehaviour,
    IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private Transform originalParent;
    private Vector2 originalPosition;

    private SlotItem originalSlot;

    private bool dropped = false;

    public UsedSkills usedSkills;
    public Image usedSprite; // 黒色の使用済み画像

    void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        rectTransform = GetComponent<RectTransform>();

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    // ドラッグ開始時の処理
    public void OnPointerDown(PointerEventData eventData)
    {
        originalParent = transform.parent;
        originalPosition = rectTransform.anchoredPosition;

        originalSlot = originalParent.GetComponent<SlotItem>();

        transform.SetParent(canvas.transform, true);

        canvasGroup.alpha = 0.6f; // 半透明にしてドラッグ中を分かりやすく
        canvasGroup.blocksRaycasts = false; // ドロップ先が Raycast を受け取れるように

        dropped = false;
    }
    // ドラッグ中のimageの位置を更新する処理
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
    // ドラッグ終了時に元の位置に戻す処理
    public void OnPointerUp(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        transform.SetParent(originalParent, true);
        // ドロップ成功していなければ元の位置に戻す
        rectTransform.anchoredPosition = originalPosition;

        if (!dropped)
        {
            if (originalSlot != null)
            {
                originalSlot.SetUsed();

                int skillID = GetComponent<SkillItem>().SkillID;
                usedSkills.MarkUsed(skillID);

                MarkAllSameSkillUsed(skillID);
            }
        }
    }

    private void MarkAllSameSkillUsed(int skillID)
    {
        SlotItem[] slots = FindObjectsOfType<SlotItem>();

        foreach (var slot in slots)
        {
            if (slot.skillID == skillID)
            {
                slot.SetUsed();
            }
        }
    }

    public void MarkDropped()
    {
        dropped = true;
    }
}
