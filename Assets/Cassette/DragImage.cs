using UnityEngine;
using UnityEngine.UI;

public class DragImage : MonoBehaviour
{
    public static DragImage Instance { get; private set; }

    [Header("Drag Icon")]
    [SerializeField] private Image dragIcon;

    // 現在ドラッグ中のSkillID
    public int CurrentSkillID { get; private set; } = -1;

    public NEWSkillMane skillManager;

    private Canvas canvas;
    private RectTransform iconRect;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        canvas = GetComponentInParent<Canvas>();
        iconRect = dragIcon.rectTransform;

        dragIcon.gameObject.SetActive(false);
    }

    // ドラッグ開始
    public void BeginDrag(int skillID)
    {
        CurrentSkillID = skillID;

        Debug.Log(name);
        Debug.Log(skillID);

        SkillDataNo2 data =
         skillManager.GetSkill(skillID);

        dragIcon.sprite = data.Icon;

        dragIcon.gameObject.SetActive(true);
    }

    // ドラッグ中
    public void Drag(Vector2 mousePosition)
    {
        iconRect.position = mousePosition;
    }

    // ドラッグ終了
    public void EndDrag()
    {
        CurrentSkillID = -1;

        dragIcon.gameObject.SetActive(false);
    }
}