using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    [Header("canvasのみアタッチ")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject gamePanel = null;
    [SerializeField] private GameObject menuPanel = null;
    [SerializeField] private GameObject menuButton = null;
    [SerializeField] private GameObject resultPanel = null;

    private string sceneName = null;
    private bool menuActive = false;

    private void Awake()
    {
        gamePanel = canvas.transform.Find("GamePanel").gameObject;
        menuPanel = canvas.transform.Find("MenuPanel").gameObject;
        menuButton = gamePanel.transform.Find("MenuButton").gameObject;
        resultPanel = canvas.transform.Find("ResultPanel").gameObject;
    }

    private void Start()
    {
        //  現在のシーン名取得
        sceneName = SceneManager.GetActiveScene().name;

        if (sceneName != "TitleScene")
        {
            if (gamePanel != null)
            {
                gamePanel.gameObject.SetActive(true);
            }
        }
    }

    private void Update()
    {
        sceneName = SceneManager.GetActiveScene().name;

        //  メニューの表示判定
        menuActive = menuPanel.activeSelf;

        //  メニューの表示/非表示
        if (false)
        {
            //MenuSwicth();
        }
    }

    public void Restart()
    {
        //  現在のシーンのリロード
        SceneManager.LoadScene(sceneName);
    }

    //  メニューの表示/非表示(UIボタンから)
    public void MenuButton()
    {
        MenuSwicth();
    }

    public void MenuSwicth()
    {
        if (menuPanel != null)
        {
            if (menuActive != true)
            {
                menuPanel.gameObject.SetActive(true);
                menuButton.gameObject.SetActive(false);
            }
            else
            {
                menuPanel.gameObject.SetActive(false);
                menuButton.gameObject.SetActive(true);
            }
        }  
    }
}
