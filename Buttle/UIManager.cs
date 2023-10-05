using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] EnemyController enemy;

    // 所持属性の数
    int haveRed = 0;
    int haveGreen = 0;
    int haveBlue = 0;

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
        if (!PlayerController.canUseRed)
        {
            InactiveImage(textR);
            InactiveImage(redNum);
            commandBoxRed.GetComponent<Image>().sprite = commandBoxNull;
            commandBoxApple.GetComponent<Image>().sprite = commandBoxNull;
        }
        if (!PlayerController.canUseGreen)
        {
            InactiveImage(textG);
            InactiveImage(greenNum);
            commandBoxGreen.GetComponent<Image>().sprite = commandBoxNull;
            commandBoxHerb.GetComponent<Image>().sprite = commandBoxNull;
        }
        if (!PlayerController.canUseBlue)
        {
            InactiveImage(textB);
            InactiveImage(blueNum);
            commandBoxBlue.GetComponent<Image>().sprite = commandBoxNull;
            commandBoxFlower.GetComponent<Image>().sprite = commandBoxNull;
        }
    }

    // 所持属性更新
    public void UpdateItemCount()
    {
        // 赤
        if (haveRed != PlayerController.haveRed)
        {
            redNum.GetComponent<Text>().text = PlayerController.haveRed.ToString();
            haveRed = PlayerController.haveRed;
        }
        // 緑
        if (haveGreen != PlayerController.haveGreen)
        {
            greenNum.GetComponent<Text>().text = PlayerController.haveGreen.ToString();
            haveGreen = PlayerController.haveGreen;
        }
        // 青
        if (haveBlue != PlayerController.haveBlue)
        {
            blueNum.GetComponent<Text>().text = PlayerController.haveBlue.ToString();
            haveBlue = PlayerController.haveBlue;
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
        ActiveImage(crossImage);
        ActiveImage(commandBoxBlack);
        ActiveImage(commandBoxRed);
        ActiveImage(commandBoxGreen);
        ActiveImage(commandBoxBlue);
    }

    // 攻撃コマンドを無効化する
    public void InactiveCommandImage()
    {
        InactiveImage(crossImage);
        InactiveImage(commandBoxBlack);
        InactiveImage(commandBoxRed);
        InactiveImage(commandBoxGreen);
        InactiveImage(commandBoxBlue);
    }

    // アイテムコマンドを有効化する
    public void ActiveItemCommandImage()
    {
        ActiveImage(crossImage);
        ActiveImage(commandBoxReturn);
        ActiveImage(commandBoxApple);
        ActiveImage(commandBoxHerb);
        ActiveImage(commandBoxFlower);
    }

    // アイテムコマンドを無効化する
    public void InactiveItemCommandImage()
    {
        InactiveImage(crossImage);
        InactiveImage(commandBoxReturn);
        InactiveImage(commandBoxApple);
        InactiveImage(commandBoxHerb);
        InactiveImage(commandBoxFlower);
    }

    // リトライ
    public void Retry()
    {
        // HP を戻す
        PlayerController.hp = 5;

        // ゲーム中に戻す
        SceneManager.LoadScene(retrySceneName);     // シーン移動
        GameManager.gameState = GameState.Action;
    }

    public IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }
}