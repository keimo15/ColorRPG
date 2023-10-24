using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleMenu : MonoBehaviour
{
    int pointMenu;              // 0:つづきからあそぶ, 1:はじめからあそぶ, 2:あそびかた
    public int howMenu;         // メニュー項目の数
    float axisV;                // 縦入力
    float light = 0.5f;         // 非選択オブジェクトの色の薄さ

    float pushTime = 0.3f;
    float upTimer;              // 上入力の長押し時間
    float downTimer;            // 下入力の長押し時間

    public GameObject continueText;
    public GameObject newGameText;
    public GameObject howToPlayText;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.gameState = GameState.Title;
        pointMenu = 0;   
        axisV     = 0.0f;
        upTimer   = 0.0f;
        downTimer = 0.0f;
        NoLightenColor(continueText);
        LightenColor(newGameText, light);
        LightenColor(howToPlayText, light);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameState != GameState.Title)
        {
            return;
        }

        axisV = Input.GetAxisRaw("Vertical");       // 上下キー入力
        if (axisV > 0)
        {
            // 長押しされているとき
            if (pointMenu > 0 && upTimer > pushTime)
            {
                pointMenu--;
                upTimer = 0.001f;                    // 0 にすると入力リセットの扱いになってしまう
            }
            // 新たに上入力が入ったとき
            else if (pointMenu > 0 && upTimer == 0)
            {
                pointMenu--;
            }
            upTimer += Time.deltaTime;
            downTimer = 0;
        } 
        else if (axisV < 0)
        {
            // 長押しされているとき
            if (pointMenu < howMenu - 1 && downTimer > pushTime)
            {
                pointMenu++;
                downTimer = 0.001f;                    // 0 にすると入力リセットの扱いになってしまう
            }
            // 新たに上入力が入ったとき
            else if (pointMenu < howMenu - 1 && downTimer == 0)
            {
                pointMenu++;
            }
            downTimer += Time.deltaTime;
            upTimer = 0;
        } 
        else
        {
            upTimer = 0;
            downTimer = 0;
        }

        // Debug.Log(pointMenu);

        switch (pointMenu)
        {
          case 0:
            NoLightenColor(continueText);
            LightenColor(newGameText, light);
            LightenColor(howToPlayText, light);
            break;
          case 1:
            NoLightenColor(newGameText);
            LightenColor(continueText, light);
            LightenColor(howToPlayText, light);
            break;
          case 2:
            NoLightenColor(howToPlayText);
            LightenColor(newGameText, light);
            LightenColor(continueText, light);
            break;
          default:
            break;
        }
    }

    // 画像の色を薄くする
    void LightenColor(GameObject image, float light)
    {
        if (image == null) return;
        image.GetComponent<Text>().color = new Color(0, 0, 0, light);
    }

    // 画像の薄さを戻す
    void NoLightenColor(GameObject image)
    {
        if (image == null) return;
        image.GetComponent<Text>().color = new Color(0, 0, 0, 1);
    }
}
