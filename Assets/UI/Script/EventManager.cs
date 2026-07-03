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
    [Header("アタッチ確認用")]
    private GameObject titlePanel;
    private Button titleButton;
    private Button titleMenuButton;

    private GameObject gamePanel;
    private Button gameMenuButton;

    private GameObject menuPanel;
    private Button menuButton;

    private GameObject resultPanel;
    private TextMeshProUGUI resultText;
    private Button nextButton;
    private Button restartButton;
    private Button titleBackButton;

    //  判定用関数
    private string sceneName;
    private bool menuActive = false;

    private void Start()
    {
        //  各canvas直下のUIを走査 + アタッチ
        //  title
        if (canvas.transform.Find("TitlePanel") != null)
        {
            titlePanel = canvas.transform.Find("TitlePanel").gameObject;

            if(titlePanel.transform.Find("TitleButton") != null)
            {
                titleButton=titlePanel.transform.Find("TitleButton").GetComponent<Button>();
                titleButton.onClick.AddListener(TitleButton);
            }

            if (titlePanel.transform.Find("TitleMenuButton") != null)
            {
                titleMenuButton = titlePanel.transform.Find("TitleMenuButton").GetComponent<Button>();
                titleMenuButton.onClick.AddListener(MenuButton);
            }
        }
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

            if(resultPanel.transform.Find("ResultText") != null)
            {
                resultText = resultPanel.transform.Find("ResultText").GetComponent<TextMeshProUGUI>();
            }
            if (resultPanel.transform.Find("NextButton") != null)
            {
                nextButton = resultPanel.transform.Find("NextButton").GetComponent<Button>();
                nextButton.onClick.AddListener(NextButton);
            }
            if (resultPanel.transform.Find("RestartButton") != null)
            {
                restartButton = resultPanel.transform.Find("RestartButton").GetComponent<Button>();
                restartButton.onClick.AddListener(RestartButton);
            }
            if (resultPanel.transform.Find("TitleBackButton") != null)
            {
                titleBackButton = resultPanel.transform.Find("TitleBackButton").GetComponent<Button>();
                titleBackButton.onClick.AddListener(TitleBackButton);
            }
        }

        //  現在のシーン名取得
        sceneName = SceneManager.GetActiveScene().name;

        //  タイトルシーンの時タイトル表示
        if(titlePanel != null && sceneName == "TitleScene")
        {
            titlePanel.gameObject.SetActive(true);
        }
        else if(gamePanel != null)
        {
            gamePanel.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        sceneName = SceneManager.GetActiveScene().name;

        //  メニューの表示判定
        if (menuPanel != null)
        {
            menuActive = menuPanel.activeSelf;

            //  メニューの表示/非表示 //  ビルド時はEscapeで反応
#if UNITY_EDITOR
            if (Keyboard.current.uKey.wasPressedThisFrame)
#else
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
#endif
            {
                MenuButton();
            }
        }
    }

    private void TitleButton()
    {
        //  ボタンクリックで移動
        SceneManager.LoadScene("PlayerScene");
    }

    private void RestartButton()
    {
        //  現在のシーンのリロード
        SceneManager.LoadScene(sceneName);
    }

    private void NextButton()   //  Stageが増えたら追記
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

    private void MenuButton()    //  メニューのON/OFF切り替え
    {
        if (menuPanel != null)
        {
            if (menuActive != true)
            {
                menuPanel.gameObject.SetActive(true);

                //  タイトルシーンの時はタイトルにあるボタンを消す
                if (sceneName == "TitleScene")
                {
                    titleMenuButton.gameObject.SetActive(false);
                }
                else
                {
                    gameMenuButton.gameObject.SetActive(false);
                }
                    
            }
            else
            {
                menuPanel.gameObject.SetActive(false);

                if (sceneName == "TitleScene")
                {
                    titleMenuButton.gameObject.SetActive(true);
                }
                else
                {
                    gameMenuButton.gameObject.SetActive(true);
                }
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
        if (resultPanel != null)
        {
            //  プレイヤーが死んだらresultの表示
            resultPanel.gameObject.SetActive(true);

            //  デス時はリスタート
            resultText.text = "Never Give Up";

            nextButton.gameObject.SetActive(false);
            restartButton.gameObject.SetActive(true);
        }
    }

    public void StageClear()
    {
        if (resultPanel != null)
        {
            resultPanel.gameObject.SetActive(true);

            //  クリア時はリザルトと、Next
            resultText.text = "Stage Clear!";

            nextButton.gameObject.SetActive(true);
            restartButton.gameObject.SetActive(false);
        }
    }

    public void GameClear()
    {
        if (resultPanel != null)
        {
        }
    }
}
