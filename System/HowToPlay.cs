using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// 遊び方画面の管理
public class HowToPlay : MonoBehaviour
{
    int pointPage;                      // 何ページ目を指しているか          
    float axisH;                        // 横入力

    float pushTime = 0.3f;              // 長押しで次に移動するまでの時間
    float rightTimer;                   // 右入力の長押し時間
    float leftTimer;                    // 左入力の長押し時間

    public GameObject howToPlay;        // 遊び方のゲームオブジェクト
    public Sprite[] howToPlayImages;    // 遊び方のスプライト

    private bool isReset;

    void Start()
    {
        GameManager.instance.gameState = GameState.HowToPlay;
        Reset();
    }

    void Update()
    {
        if (GameManager.instance.gameState != GameState.HowToPlay)
        {
            if (!isReset) Reset();
            return;
        }

        isReset = false;

        axisH = Input.GetAxisRaw("Horizontal");     // 左右キー入力
        if (axisH < 0)
        {
            // 長押しされているとき
            if (pointPage > 0 && leftTimer > pushTime)
            {
                pointPage--;
                leftTimer = 0.001f;                    // 0 にすると入力リセットの扱いになってしまう
            }
            // 新たに左入力が入ったとき
            else if (pointPage > 0 && leftTimer == 0)
            {
                pointPage--;
            }
            leftTimer += Time.deltaTime;
            rightTimer = 0;
        } 
        else if (axisH > 0)
        {
            // 長押しされているとき
            if (pointPage < howToPlayImages.Length - 1 && rightTimer > pushTime)
            {
                pointPage++;
                rightTimer = 0.001f;                    // 0 にすると入力リセットの扱いになってしまう
            }
            // 新たに右入力が入ったとき
            else if (pointPage < howToPlayImages.Length - 1 && rightTimer == 0)
            {
                pointPage++;
            }
            rightTimer += Time.deltaTime;
            leftTimer = 0;
        } 
        else
        {
            // 長押し状態を解除する
            leftTimer = 0;
            rightTimer = 0;
        }

        howToPlay.GetComponent<Image>().sprite = howToPlayImages[pointPage];

        // エンターキーが入力されるとタイトルに戻る
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SoundManager.soundManager.PlaySE(SEType.Click);
            SceneManager.LoadScene("TitleMain");
        }
    }

    // 状態のリセット
    private void Reset()
    {
        howToPlay.GetComponent<Image>().sprite = howToPlayImages[0];
        pointPage  = 0;
        axisH      = 0;
        rightTimer = 0;
        leftTimer  = 0;
        SoundManager.soundManager.PlayBgm(BGMType.Menu);

        isReset = true;
    }
}
