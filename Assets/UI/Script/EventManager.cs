using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    [Header("canvasのみアタッチ")]
    [SerializeField] private Canvas canvas;
    [Space]

    [Header("シーン名")]
    [SerializeField] private string titleSceneName;
    [SerializeField] private string gameSceneName1;
    [SerializeField] private string gameSceneName2;
    [SerializeField] private string gameSceneName3; //  予備

    //  スクリプトでアタッチするUI
    //  タイトルパネル
    private GameObject titlePanel;
    private Button titleButton;
    private Button titleMenuButton;

    //  ゲームパネル
    private GameObject gamePanel;
    private Button gameMenuButton;

    //  メニューパネル
    private GameObject menuPanel;
    private Button menuButton;

    //  リザルトパネル
    private GameObject resultPanel;
    private TextMeshProUGUI resultText;
    private Button nextButton;
    private Button restartButton;
    private Button titleBackButton;

    //  クリアパネル(未実装)

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
                titleButton.onClick.AddListener(NextButton);
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

    private void NextButton()   //  Stageが増えたら追記
    {
        //  TitleScene => PlayerScene
        if(sceneName == titleSceneName && gameSceneName1 != null)
        {
            SceneManager.LoadScene(gameSceneName1);
        }
        //  PlayerScene => Stage1
        if (sceneName == gameSceneName1 && gameSceneName2 != null)
        {
            SceneManager.LoadScene(gameSceneName2);
        }
        //  Stage1 => ?
        else if (sceneName == gameSceneName2 && gameSceneName3 != null)
        {
            SceneManager.LoadScene(gameSceneName3);
        }
    }

    private void RestartButton()
    {
        //  現在のシーンのリロード
        SceneManager.LoadScene(sceneName);
    }

    private void TitleBackButton()
    {
        //  タイトルバック
        SceneManager.LoadScene(titleSceneName);
    }

    public bool MenuActiveLock()    //  メニューを反応させていいタイミングか判定
    {
        foreach (Transform child in canvas.transform)
        {
            if (child.gameObject == titlePanel)
                continue;

            if (child.gameObject == gamePanel)
                continue;

            if (child.gameObject == menuPanel)
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

    private void MenuButton()    //  メニューのON/OFF切り替え
    {
        if (MenuActiveLock() != true)
        {
            if (menuPanel != null && menuActive != true)
            {
                menuPanel.gameObject.SetActive(true);

                //  タイトルシーンの時はタイトルにあるボタンを消す
                if (sceneName == titleSceneName)
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

                if (sceneName == titleSceneName)
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

    public bool GetPanelActive()    //  外部から任意パネルのon/offの検出
    {
        foreach (Transform child in canvas.transform)
        {
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

    //  クリア2つはエネミー関係が進んでから
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
}
