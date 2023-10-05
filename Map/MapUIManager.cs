using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapUIManager : MonoBehaviour
{
    // [SerializeField] GameManager gameManager;
    // [SerializeField] PlayerMap player;

    // 所持属性の数
    int haveRed = 0;
    int haveGreen = 0;
    int haveBlue = 0;

    // 属性エネルギーの GameObject
    public GameObject textR;
    public GameObject textG;
    public GameObject textB;
    public GameObject redNum;
    public GameObject greenNum;
    public GameObject blueNum;

    // アイテムの数
    int haveGold = 0;
    int haveApple = 0;
    int haveHerb = 0;
    int haveFlower = 0;

    // アイテムの GameObject
    public GameObject iconGold;
    public GameObject iconApple;
    public GameObject iconHerb;
    public GameObject iconFlower;
    public GameObject goldNum;
    public GameObject appleNum;
    public GameObject herbNum;
    public GameObject flowerNum;

    void Start()
    {
        // 各属性の開放状況に応じて非表示にする
        if (!PlayerController.canUseRed)
        {
            InactiveImage(textR);
            InactiveImage(redNum);
            InactiveImage(iconApple);
            InactiveImage(appleNum);
        }
        if (!PlayerController.canUseGreen)
        {
            InactiveImage(textG);
            InactiveImage(greenNum);
            InactiveImage(iconHerb);
            InactiveImage(herbNum);
        }
        if (!PlayerController.canUseBlue)
        {
            InactiveImage(textB);
            InactiveImage(blueNum);
            InactiveImage(iconFlower);
            InactiveImage(flowerNum);
        }
    }

    // 所持数更新
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
        // ゴールド
        if (haveGold != PlayerController.haveGold)
        {
            goldNum.GetComponent<Text>().text = PlayerController.haveGold.ToString();
            haveGold = PlayerController.haveGold;
        }
        // りんご
        if (haveApple != PlayerController.haveApple)
        {
            appleNum.GetComponent<Text>().text = PlayerController.haveApple.ToString();
            haveApple = PlayerController.haveApple;
        }
        // やくそう
        if (haveHerb != PlayerController.haveHerb)
        {
            herbNum.GetComponent<Text>().text = PlayerController.haveHerb.ToString();
            haveHerb = PlayerController.haveHerb;
        }
        // はな
        if (haveFlower != PlayerController.haveFlower)
        {
            flowerNum.GetComponent<Text>().text = PlayerController.haveFlower.ToString();
            haveFlower = PlayerController.haveFlower;
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
}
