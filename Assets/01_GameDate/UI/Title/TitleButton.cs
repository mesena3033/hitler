using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class TitleButton : MonoBehaviour
{
    public void ChangeScene()
    {
        SceneManager.LoadScene("PlayerScene");
    }
}
