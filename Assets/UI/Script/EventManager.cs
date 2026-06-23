using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    [Header("canvasのみアタッチ")]
    [SerializeField] private Canvas canvas;

    //  スクリプトでアタッチ
    [Header("アタッチ不要")]
    //  各パネル、UIアタッチ用
    [SerializeField]private GameObject gamePanel = null;
    [SerializeField] private Button menuButton = null;

    [SerializeField]private GameObject menuPanel = null;
    [SerializeField]private Button closeButton = null;

    [SerializeField] private GameObject resultPanel = null;
    [SerializeField]private Button resultButton = null;

    private string sceneName = null;
    private bool menuActive = false;

    private void Awake()
    {
        //  game
        if (canvas.transform.Find("GamePanel") != null)
        {
            gamePanel = canvas.transform.Find("GamePanel").gameObject;

            if (gamePanel.transform.Find("MenuButton") == null)
            {
                menuButton = gamePanel.transform.Find("MenuButton").GetComponent<Button>();
            }
        }

        //  menu
        if(canvas.transform.Find("MenuPanel") != null)
        {
            menuPanel = canvas.transform.Find("MenuPanel").gameObject;

            if (menuPanel.transform.Find("CloseButton") != null)
            {
                closeButton = menuPanel.transform.Find("CloseButton").GetComponent<Button>();
            }
        }

        //  result
        if (canvas.transform.Find("ResultPanel") != null)
        {
            resultPanel = canvas.transform.Find("ResultPanel").gameObject;

            if (resultPanel.transform.Find("RestartButton") != null)
            {
                resultButton = resultPanel.transform.Find("RestartButton").GetComponent<Button>();
            }
        }
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

    //  メニューのON/OFF切り替え
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
