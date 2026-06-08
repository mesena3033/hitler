using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public void ChangeScene()
    {
        SceneManager.LoadScene("PlayerScene");
    }
}
