using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    [Header("canvasのみアタッチ")]
    [SerializeField] private Canvas canvas;

    //  スクリプトでアタッチ
    [Header("アタッチ確認")]
    [SerializeField]private GameObject gamePanel = null;
    [SerializeField] private Button gameMenuButton = null;

    [SerializeField]private GameObject menuPanel = null;
    [SerializeField]private Button menuButton = null;

    [SerializeField] private GameObject resultPanel = null;
    [SerializeField]private Button resultButton = null;

    private string sceneName = null;

    //  パネル状態の確認
    private bool menuActive = false;

    private void Awake()
    {
        //  game
        if (canvas.transform.Find("GamePanel") != null)
        {
            gamePanel = canvas.transform.Find("GamePanel").gameObject;

            if (gamePanel.transform.Find("GameMenuButton") != null)
            {
                gameMenuButton = gamePanel.transform.Find("GameMenuButton").GetComponent<Button>();
                gameMenuButton.onClick.AddListener(MenuButton);
            }
        }

        //  menu
        if(canvas.transform.Find("MenuPanel") != null)
        {
            menuPanel = canvas.transform.Find("MenuPanel").gameObject;

            if (menuPanel.transform.Find("MenuButton") != null)
            {
                menuButton = menuPanel.transform.Find("MenuButton").GetComponent<Button>();
                menuButton.onClick.AddListener(MenuButton);
            }
        }

        //  result
        if (canvas.transform.Find("ResultPanel") != null)
        {
            resultPanel = canvas.transform.Find("ResultPanel").gameObject;

            if (resultPanel.transform.Find("RestartButton") != null)
            {
                resultButton = resultPanel.transform.Find("RestartButton").GetComponent<Button>();
                resultButton.onClick.AddListener(RestartButton);
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
        if (Keyboard.current.uKey.wasPressedThisFrame)
        {
            MenuSwicth();
        }
    }

    public void RestartButton()
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
                gameMenuButton.gameObject.SetActive(false);
            }
            else
            {
                menuPanel.gameObject.SetActive(false);
                gameMenuButton.gameObject.SetActive(true);
            }
        }  
    }

    //  プレイヤーが死んだらresultの表示
    public void PlayerDespawn()
    {
        resultPanel.gameObject.SetActive(true);
    }

    //  任意のパネルがあるか確認
    public bool CheckPanelActive()
    {
        foreach (Transform child in canvas.transform)
        {
            if (child.gameObject == gamePanel)
                continue;

            if ((child.gameObject.tag != "Panel"))
                continue;

            if (child.gameObject.activeSelf)
            {
                return true;
            }
        }

        return false;
    }
}
