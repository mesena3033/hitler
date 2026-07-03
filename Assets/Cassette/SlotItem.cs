using UnityEngine;
using UnityEngine.UI;

public class SlotItem : MonoBehaviour
{
    public int skillID = -1;    // セットされているスキルID
    public GameObject usedImagePrefab;  // 黒色 Image のプレハブ

    private GameObject usedImageInstance;

    public void SetUsed()
    {
        // すでに黒色 Image があるなら何もしない
        if (usedImageInstance != null) return;

        // 黒色 Image を生成してスロットに置く
        usedImageInstance = Instantiate(usedImagePrefab, transform);
        usedImageInstance.transform.SetAsLastSibling();

        // スロットの skillID を保持
        // （使用済みとして扱う）
    }

    public void ClearUsed()
    {
        if (usedImageInstance != null)
        {
            Destroy(usedImageInstance);
            usedImageInstance = null;
        }
    }
}
