using UnityEngine;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject titlePanel = null;
    [SerializeField] private GameObject gamePanel = null;

    private string sceneName = null;

    private void Start()
    {
    }

    private void Update()
    {
        //  現在のシーン取得
        sceneName = SceneManager.GetActiveScene().name;

        //  タイトルかゲームか区別
        if (sceneName == "TitleScene")
        {
            titlePanel.gameObject.SetActive(true);
            gamePanel.gameObject.SetActive(false);
        }
        else
        {
            titlePanel.gameObject.SetActive(false);
            gamePanel.gameObject.SetActive(true);
        }
    }

    public void TitleButton()
    {
        //  スタートクリックで移動
        SceneManager.LoadScene("PlayerScene");
    }

    public void Restart()
    {
        //  現在のシーンのリロード
        SceneManager.LoadScene(sceneName);
    }
}
