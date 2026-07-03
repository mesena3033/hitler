using UnityEngine;
using UnityEngine.UI;

public class LoopScroll : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform viewPortContent;
    [SerializeField] private RectTransform contentPanelTrans;
    [SerializeField] private HorizontalLayoutGroup HLG;

    [Header("Prefab")]
    [SerializeField] private SkillItem skillItemPrefab;

    [Header("Skill")]
    [SerializeField] private NEWSkillMane skillManager;

    Vector2 OldVelocity;
    bool isUpdated;

    private float ItemWidth =>
        skillItemPrefab.GetComponent<RectTransform>().rect.width + HLG.spacing;


    private int SkillCount => skillManager.SkillCount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isUpdated = false;
        OldVelocity = Vector2.zero;
        int ItemToAdd =
            Mathf.CeilToInt(viewPortContent.rect.width / ItemWidth);

        for (int i = 0; i < SkillCount; i++)
        {
            CreateItem(i, false);
        }

        for (int i = 0; i < ItemToAdd; i++)
        {
            int id = i % SkillCount;
            CreateItem(id, false);
        }
        for (int i = 0; i < ItemToAdd; i++)
        {
            int id = SkillCount - i - 1;

            while (id < 0)
                id += SkillCount;

            CreateItem(id, true);
        }
        contentPanelTrans.localPosition =
            new Vector3(
                -ItemWidth * ItemToAdd,
                contentPanelTrans.localPosition.y,
                contentPanelTrans.localPosition.z);
    }

    void Update()
    {
        if(isUpdated)
        {
            scrollRect.velocity = OldVelocity;
            isUpdated = false;
        }

        float loopWidth = SkillCount * ItemWidth;

        if (contentPanelTrans.localPosition.x > 0)
        {
            Canvas.ForceUpdateCanvases();

            OldVelocity = scrollRect.velocity;

            contentPanelTrans.localPosition -=
                new Vector3(loopWidth, 0, 0);

            isUpdated = true;
        }
        else if (contentPanelTrans.localPosition.x < -loopWidth)
        {
            Canvas.ForceUpdateCanvases();

            OldVelocity = scrollRect.velocity;

            contentPanelTrans.localPosition +=
                new Vector3(loopWidth, 0, 0);

            isUpdated = true;
        }
    }

    private void CreateItem(int skillID, bool firstSibling)
    {
        SkillItem item =
            Instantiate(skillItemPrefab, contentPanelTrans);

        item.Setup(
            skillID,
            skillManager.GetSkill(skillID));

        if (firstSibling)
            item.transform.SetAsFirstSibling();
        else
            item.transform.SetAsLastSibling();
    }
}
