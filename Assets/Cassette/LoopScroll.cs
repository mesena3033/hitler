using UnityEngine;
using UnityEngine.UI;

public class LoopScroll : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform viewPortcontent;
    public RectTransform contentPanelTrans;
    public HorizontalLayoutGroup HLG;

    public RectTransform[] contentItems;

    public NEWSkillMane skillManager;

    Vector2 OldVelocity;
    bool isUpdated;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isUpdated = false;
        OldVelocity = Vector2.zero;
        int ItemToAdd = Mathf.CeilToInt(viewPortcontent.rect.width / (contentItems[0].rect.width + HLG.spacing));

        for (int i = 0;i < ItemToAdd; i++)
        {
            int skillID = i % contentItems.Length;

            RectTransform RT = Instantiate(contentItems[i % contentItems.Length], contentPanelTrans);
            RT.SetAsLastSibling();

            // skillID ‚ð•t—^
            SkillItem si = RT.gameObject.AddComponent<SkillItem>();
            si.skillID = skillID;
        }

        for (int i = 0; i < ItemToAdd; i++)
        {
            int skillID = contentItems.Length - i - 1;
            while (skillID < 0)
                skillID += contentItems.Length;

            RectTransform RT = Instantiate(contentItems[skillID], contentPanelTrans);
            RT.SetAsFirstSibling();

            SkillItem si = RT.gameObject.AddComponent<SkillItem>();
            si.skillID = skillID;
        }
            contentPanelTrans.localPosition = new Vector3((0 - (contentItems[0].rect.width + HLG.spacing)* ItemToAdd),
            contentPanelTrans.localPosition.y,
            contentPanelTrans.localPosition.z
            );
    }

    void Update()
    {
        if(isUpdated)
        {
            scrollRect.velocity = OldVelocity;
            isUpdated = false;
        }

        if (contentPanelTrans.localPosition.x > 0)
        {
            Canvas.ForceUpdateCanvases();
            OldVelocity = scrollRect.velocity;
            contentPanelTrans.localPosition -= new Vector3(contentItems.Length * (contentItems[0].rect.width + HLG.spacing), 0, 0);
            isUpdated = true;
        }
        else if (contentPanelTrans.localPosition.x < 0 - (contentItems.Length * (contentItems[0].rect.width + HLG.spacing)))
        {
            Canvas.ForceUpdateCanvases();
            OldVelocity = scrollRect.velocity;
            contentPanelTrans.localPosition += new Vector3(contentItems.Length * (contentItems[0].rect.width + HLG.spacing), 0, 0);
            isUpdated = true;
        }
    }
}
