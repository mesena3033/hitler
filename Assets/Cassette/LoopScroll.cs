using UnityEngine;
using UnityEngine.UI;

public class LoopScroll : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform viewPortcontent;
    public RectTransform contentPanelTrans;
    public HorizontalLayoutGroup HLG;

    public RectTransform[] contentItems;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int ItemToAdd = Mathf.CeilToInt(viewPortcontent.rect.width / (contentItems[0].rect.width + HLG.spacing));

        for (int i = 0;i < ItemToAdd; i++)
        { 
            RectTransform RT = Instantiate(contentItems[i % contentItems.Length], contentPanelTrans);
            RT.SetAsLastSibling();
        }

        for (int i = 0; i < ItemToAdd; i++)
        {
            int num = contentItems.Length - i - 1;
            while (num < 0)
            {
                num += contentItems.Length;
            }
            RectTransform RT = Instantiate(contentItems[num],contentPanelTrans);
            RT.SetAsFirstSibling();
        }
    }
}
