using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleButton : MonoBehaviour
{
    public void ChangeScene()
    {
        SceneManager.LoadScene("PlayerScene");
    }
}
