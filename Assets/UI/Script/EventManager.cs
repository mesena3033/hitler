using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    [Header("canvasのみアタッチ")]
    [SerializeField] private Canvas canvas;

    //  スクリプトでアタッチするUI
    private GameObject gamePanel = null;
    private Button gameMenuButton = null;

    private GameObject menuPanel = null;
    private Button menuButton = null;

    private GameObject resultPanel = null;
    private Button nextButton = null;
    private Button resultButton = null;
    private Button titleBackButton = null;

    //  判定用関数
    private string sceneName = null;
    private bool menuActive = false;

    private void Start()
    {
        //  各canvas直下のUIを走査 + アタッチ
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
        if (canvas.transform.Find("MenuPanel") != null)
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

            if (resultPanel.transform.Find("NextButton") != null)
            {
                nextButton = resultPanel.transform.Find("NextButton").GetComponent<Button>();
                nextButton.onClick.AddListener(NextButton);
            }
            if (resultPanel.transform.Find("RestartButton") != null)
            {
                resultButton = resultPanel.transform.Find("RestartButton").GetComponent<Button>();
                resultButton.onClick.AddListener(RestartButton);
            }
            if (resultPanel.transform.Find("TitleBackButton") != null)
            {
                titleBackButton = resultPanel.transform.Find("TitleBackButton").GetComponent<Button>();
                titleBackButton.onClick.AddListener(TitleBackButton);
            }
        }

        //  現在のシーン名取得
        sceneName = SceneManager.GetActiveScene().name;

        gamePanel.gameObject.SetActive(true);
    }

    private void Update()
    {
        sceneName = SceneManager.GetActiveScene().name;

        //  メニューの表示判定
        menuActive = menuPanel.activeSelf;

        //  メニューの表示/非表示 //  ビルド時はEscapeで反応
#if UNITY_EDITOR
        if (Keyboard.current.uKey.wasPressedThisFrame)
#else
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
#endif
        {
            MenuSwicth();
        }
    }

    private void RestartButton()
    {
        //  現在のシーンのリロード
        SceneManager.LoadScene(sceneName);
    }

    private void NextButton()
    {
        //  PlayerScene => Stage1
        if (sceneName == "PlayerScene")
        {
            SceneManager.LoadScene("Stage1");
        }
        //  Stage1 => ?
        else if (sceneName == "Stage1")
        {
        }
    }

    private void TitleBackButton()
    {
        //  タイトルバック
        SceneManager.LoadScene("TitleScene");
    }

    private void MenuButton()    //  メニューの表示/非表示(UIボタン、Escape(ビルド時))
    {
        MenuSwicth();
    }

    private void MenuSwicth()    //  メニューのON/OFF切り替え
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

    public bool GetPanelActive()    //  外部からメニューon/offの検出
    {
        foreach (Transform child in canvas.transform)
        {
            // GamePanel は除外
            if (child.gameObject == gamePanel)
                continue;

            //  Panelタグに限定
            if (child.gameObject.tag != "Panel")
                continue;

            // アクティブなら true
            if (child.gameObject.activeSelf)
                return true;
        }
        return false;
    }

    public void PlayerDespawn()
    {
        //  プレイヤーが死んだらresultの表示
        resultPanel.gameObject.SetActive(true);
    }
}
