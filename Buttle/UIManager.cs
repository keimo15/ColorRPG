using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField] EnemyController enemy;

    // 所持属性の数
    int haveRed = 0;
    int haveGreen = 0;
    int haveBlue = 0;

    // パンチインターバル
    public int punchInterval = 5;
    float timer = 0;

    // その他 UI
    public string retrySceneName = "";
    public GameObject bigAttackImage;
    public GameObject bigItemImage;
    public GameObject textR;
    public GameObject textG;
    public GameObject textB;
    public GameObject redNum;
    public GameObject greenNum;
    public GameObject blueNum;
    public GameObject enemyName;
    public GameObject helpMessage;
    public GameObject commandBoxBlack;
    public GameObject commandBoxRed;
    public GameObject commandBoxGreen;
    public GameObject commandBoxBlue;
    public GameObject commandBoxReturn;
    public GameObject commandBoxApple;
    public GameObject commandBoxHerb;
    public GameObject commandBoxFlower;
    public Sprite commandBoxNull;
    public GameObject crossImage;
    public GameObject stopEnemy;
    public GameObject punchIcon;
    public Sprite punchImage;
    public Sprite cantPunchImage;
    public GameObject punchCount;
    public GameObject speechBalloon;
    public bool punchTimerOK;

    // Start is called before the first frame update
    void Start()
    {
        InactiveImage(bigAttackImage);
        InactiveImage(bigItemImage);
        InactiveCommandImage();
        InactiveItemCommandImage();

        Color color = Color.white;
        enemyName.GetComponent<Text>().text = enemy.name;
        enemyName.GetComponent<Text>().color = color;

        // 各属性の開放状況に応じて非表示にする
        if (!GameManager.instance.canUseRed)
        {
            InactiveImage(textR);
            InactiveImage(redNum);
            commandBoxRed.GetComponent<Image>().sprite = commandBoxNull;
            commandBoxApple.GetComponent<Image>().sprite = commandBoxNull;
        }
        if (!GameManager.instance.canUseGreen)
        {
            InactiveImage(textG);
            InactiveImage(greenNum);
            commandBoxGreen.GetComponent<Image>().sprite = commandBoxNull;
            commandBoxHerb.GetComponent<Image>().sprite = commandBoxNull;
        }
        if (!GameManager.instance.canUseBlue)
        {
            InactiveImage(textB);
            InactiveImage(blueNum);
            commandBoxBlue.GetComponent<Image>().sprite = commandBoxNull;
            commandBoxFlower.GetComponent<Image>().sprite = commandBoxNull;
        }
        if (!GameManager.instance.canPunch)
        {
            InactiveImage(punchIcon);
            InactiveImage(punchCount);
            InactiveImage(speechBalloon);
        }
        else
        {
            timer = punchInterval;
            punchCount.GetComponent<Text>().text = "OK";
            punchIcon.GetComponent<Image>().sprite = punchImage;
            punchTimerOK = true;
        }
    }

    // 所持属性更新
    public void UpdateItemCount()
    {
        // 赤
        if (haveRed != GameManager.instance.haveRed)
        {
            redNum.GetComponent<Text>().text = GameManager.instance.haveRed.ToString();
            haveRed = GameManager.instance.haveRed;
        }
        // 緑
        if (haveGreen != GameManager.instance.haveGreen)
        {
            greenNum.GetComponent<Text>().text = GameManager.instance.haveGreen.ToString();
            haveGreen = GameManager.instance.haveGreen;
        }
        // 青
        if (haveBlue != GameManager.instance.haveBlue)
        {
            blueNum.GetComponent<Text>().text = GameManager.instance.haveBlue.ToString();
            haveBlue = GameManager.instance.haveBlue;
        }
    }

    // 画像を表示にする
    public void ActiveImage(GameObject image)
    {
        if (image == null) return;
        image.SetActive(true);
    }

    // 画像を非表示にする
    public void InactiveImage(GameObject image)
    {
        if (image == null) return;
        image.SetActive(false);
    }

    // 画像の色を薄くする
    public void LightenColor(GameObject image, float light)
    {
        if (image == null) return;
        image.GetComponent<Image>().color = new Color(255, 255, 255, light);
    }

    // 画像の薄さを戻す
    public void NoLightenColor(GameObject image)
    {
        if (image == null) return;
        image.GetComponent<Image>().color = new Color(255, 255, 255, 1);
    }

    // 攻撃コマンドを有効化する
    public void ActiveCommandImage()
    {
        ActiveImage(helpMessage);
        ActiveImage(crossImage);
        ActiveImage(commandBoxBlack);
        ActiveImage(commandBoxRed);
        ActiveImage(commandBoxGreen);
        ActiveImage(commandBoxBlue);
    }

    // 攻撃コマンドを無効化する
    public void InactiveCommandImage()
    {
        InactiveImage(helpMessage);
        InactiveImage(crossImage);
        InactiveImage(commandBoxBlack);
        InactiveImage(commandBoxRed);
        InactiveImage(commandBoxGreen);
        InactiveImage(commandBoxBlue);
    }

    // アイテムコマンドを有効化する
    public void ActiveItemCommandImage()
    {
        ActiveImage(helpMessage);
        ActiveImage(crossImage);
        ActiveImage(commandBoxReturn);
        ActiveImage(commandBoxApple);
        ActiveImage(commandBoxHerb);
        ActiveImage(commandBoxFlower);
    }

    // アイテムコマンドを無効化する
    public void InactiveItemCommandImage()
    {
        InactiveImage(helpMessage);
        InactiveImage(crossImage);
        InactiveImage(commandBoxReturn);
        InactiveImage(commandBoxApple);
        InactiveImage(commandBoxHerb);
        InactiveImage(commandBoxFlower);
    }

    // パンチにはインターバルが存在する
    public IEnumerator PunchTimer()
    {
        punchIcon.GetComponent<Image>().sprite = cantPunchImage;
        punchTimerOK = false;
        for (int i=punchInterval; i>0; i--)
        {
            punchCount.GetComponent<Text>().text = i.ToString();
            yield return new WaitForSeconds(1.0f);
        }
        punchCount.GetComponent<Text>().text = "OK";
        punchIcon.GetComponent<Image>().sprite = punchImage;
        punchTimerOK = true;
    }

    public IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }
}