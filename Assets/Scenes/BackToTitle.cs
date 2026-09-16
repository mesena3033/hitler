using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToTitle : MonoBehaviour
{
    public void BackToTitleScene()
    {
        SceneManager.LoadScene("Title");
    }
}
