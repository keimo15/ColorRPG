using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//  タイトル画面の管理
public class TitleMenu : MonoBehaviour
{
    int pointMenu;              // 0:つづきからあそぶ, 1:はじめからあそぶ, 2:あそびかた
    float axisV;                // 縦入力
    float light = 0.5f;         // 非選択オブジェクトの色の薄さ

    float pushTime = 0.3f;      // 長押しで次に移動するまでの時間
    float upTimer;              // 上入力の長押し時間
    float downTimer;            // 下入力の長押し時間

    public GameObject[] menus;  // メニュー項目のテキスト
    public SaveData saveData;

    private bool isReset;

    void Start()
    {
        GameManager.instance.gameState = GameState.Title;
        Reset();
    }

    void Update()
    {
        if (GameManager.instance.gameState != GameState.Title)
        {
            if (!isReset) Reset();
            return;
        }

        isReset = false;

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
            if (pointMenu < menus.Length - 1 && downTimer > pushTime)
            {
                pointMenu++;
                downTimer = 0.001f;                    // 0 にすると入力リセットの扱いになってしまう
            }
            // 新たに上入力が入ったとき
            else if (pointMenu < menus.Length - 1 && downTimer == 0)
            {
                pointMenu++;
            }
            downTimer += Time.deltaTime;
            upTimer = 0;
        } 
        else
        {
            // 長押し状態を解除する
            upTimer = 0;
            downTimer = 0;
        }

        EmphasizeText();

        // スペースキーでモードの選択をする
        if (Input.GetButtonDown("Jump"))
        {
            SoundManager.soundManager.PlaySE(SEType.Click);
            switch (pointMenu)
            {
              case 0:
                if (saveData.FindJsonfile())
                {
                    saveData.LoadPlayerData();
                    SceneManager.LoadScene(GameManager.instance.lastMapScene);
                }
                else
                {
                    StartNewGame();
                }
                break;
              case 1:
                StartNewGame();
                break;
              case 2:
                SceneManager.LoadScene("HowToPlay");
                break;
              default:
                break;
            }   
        }
    }

    void StartNewGame()
    {
        saveData.Initialize();
        SceneManager.LoadScene("Opening");
    }

    // 選択中のテキストを強調する
    void EmphasizeText()
    {
        for (int i=0; i<menus.Length; i++)
        {
            if (i == pointMenu) NoLightenColor(menus[i]);
            else LightenColor(menus[i], light);
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

    // 状態のリセット
    private void Reset()
    {
        pointMenu = 0;   
        axisV     = 0.0f;
        upTimer   = 0.0f;
        downTimer = 0.0f;
        EmphasizeText();
        SoundManager.soundManager.PlayBgm(BGMType.Menu);

        isReset = true;
    }
}
