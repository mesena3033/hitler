using UnityEngine;
using UnityEngine.EventSystems;

// imageをドラッグするためのスクリプト
public class DragImage : MonoBehaviour,
    IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private Vector2 originalPosition;

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
        originalPosition = rectTransform.anchoredPosition;
        canvasGroup.alpha = 0.6f; // 半透明にしてドラッグ中を分かりやすく
        canvasGroup.blocksRaycasts = false; // ドロップ先が Raycast を受け取れるように
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

        // ドロップ成功していなければ元の位置に戻す
        rectTransform.anchoredPosition = originalPosition;
    }
}
