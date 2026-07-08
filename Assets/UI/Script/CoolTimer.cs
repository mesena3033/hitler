using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//UIを使うので追記
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    //画像コンポーネント用変数を宣言
    Image clockImage;

    private void Start()
    {
        //自身の画像コンポーネントを取得
        clockImage = GetComponent<Image>();
    }

    //fillAmountの値を変更する関数（ClockTimerから呼ばれる）
    public void UpdateClock(float second)
    {
        //受け取ったfloat型の値を代入する
        clockImage.fillAmount = second;
    }
}