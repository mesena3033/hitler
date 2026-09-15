using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public void TitleToStart()
    {
        SceneManager.LoadScene("Stage1");
    }
}
