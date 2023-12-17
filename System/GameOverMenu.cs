using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    int pointMenu;                                              // 0:つづきからあそぶ, 1:はじめからあそぶ, 2:あそびかた
    float axisV;                                                // 縦入力
    float light = 0.5f;                                         // 非選択オブジェクトの色の薄さ

    float pushTime = 0.3f;
    float upTimer;                                              // 上入力の長押し時間
    float downTimer;                                            // 下入力の長押し時間

    public GameObject[] menus;                                  // メニュー項目のテキスト
    string TitleScene = "TitleMain";                            // タイトルシーンのシーン名

    private bool isReset = false;

    public SaveData saveData;

    // Start is called before the first frame update
    void Start()
    {
        UpdateUseData();
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.gameState != GameState.GameOver)
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
            upTimer = 0;
            downTimer = 0;
        }

        EmphasizeText();

        if (Input.GetButtonDown("Jump"))
        {
            switch (pointMenu)
            {
              case 0:
                saveData.Continue();
                SceneManager.LoadScene(GameManager.instance.lastMapScene);
                break;
              case 1:
                SceneManager.LoadScene(TitleScene);
                break;
              default:
                break;
            }   
        }
    }

    // 死んでもアイテムの使用回数などは引き継ぐ
    void UpdateUseData()
    {
        // 現在の状態を取得
        int sumDamage = GameManager.instance.sumGetDamage;
        int sumItem   = GameManager.instance.sumUseItem;

        // 以前のセーブデータを読み込み
        saveData.LoadPlayerData();

        // 使用回数などのみ最新に更新する
        GameManager.instance.sumGetDamage = sumDamage;
        GameManager.instance.sumUseItem   = sumItem;
        saveData.SavePlayerData(GameManager.instance);
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

    private void Reset()
    {
        GameManager.instance.gameState = GameState.GameOver;
        pointMenu = 0;   
        axisV     = 0.0f;
        upTimer   = 0.0f;
        downTimer = 0.0f;
        EmphasizeText();

        isReset = true;
    }
}
