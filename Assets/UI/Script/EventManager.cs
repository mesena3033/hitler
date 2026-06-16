using UnityEngine;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject titlePanel = null;
    [SerializeField] private GameObject gamePanel = null;

    private string sceneName = null;

    //  DontDestroyOnLoadを1回だけ実行する
    private bool first;

    private void Awake()
    {
        sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "TitleScene")
        {
            first = true;
        }

        if (first == true)
        {
            DontDestroyOnLoad(this.gameObject);
            DontDestroyOnLoad(canvas);

            first = false;
        }
        else if (first == false)
        {
        }
    }

    private void Start()
    {
        Debug.Log(first);
    }

    private void Update()
    {
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

        //  現在のシーン取得
        sceneName = SceneManager.GetActiveScene().name;
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
