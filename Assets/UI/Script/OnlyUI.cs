using UnityEngine;

public class OnlyUI : MonoBehaviour
{
    void Awake()
    {
        //  タグで重複確認
        string gameObjectTagName = this.gameObject.tag;
        var objs = GameObject.FindGameObjectsWithTag(gameObjectTagName);
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
            return;
        }

        //  自分を保持
        DontDestroyOnLoad(this.gameObject);
    }
}
