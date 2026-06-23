using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public void TitleButton()
    {
        //  スタートクリックで移動
        SceneManager.LoadScene("PlayerScene");
    }
}
