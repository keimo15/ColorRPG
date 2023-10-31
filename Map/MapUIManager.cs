using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MapUIManager : MonoBehaviour
{
    // マップ名の表示
    public string mapName = "";
    public GameObject mapNameObject;
    public GameObject mapNameBox;

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
        // マップ名を表示し、3秒後に非表示にする
        ActiveImage(mapNameObject);
        ActiveImage(mapNameBox);
        mapNameObject.GetComponent<Text>().text = mapName;
        StartCoroutine(DelayMethod(3.0f, () =>
        {
            InactiveImage(mapNameObject);
            InactiveImage(mapNameBox);
        }));

        // 各属性の開放状況に応じて非表示にする
        if (!GameManager.instance.canUseRed)
        {
            InactiveImage(textR);
            InactiveImage(redNum);
            InactiveImage(iconApple);
            InactiveImage(appleNum);
        }
        if (!GameManager.instance.canUseGreen)
        {
            InactiveImage(textG);
            InactiveImage(greenNum);
            InactiveImage(iconHerb);
            InactiveImage(herbNum);
        }
        if (!GameManager.instance.canUseBlue)
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
        // ゴールド
        if (haveGold != GameManager.instance.haveGold)
        {
            goldNum.GetComponent<Text>().text = GameManager.instance.haveGold.ToString();
            haveGold = GameManager.instance.haveGold;
        }
        // りんご
        if (haveApple != GameManager.instance.haveApple)
        {
            appleNum.GetComponent<Text>().text = GameManager.instance.haveApple.ToString();
            haveApple = GameManager.instance.haveApple;
        }
        // やくそう
        if (haveHerb != GameManager.instance.haveHerb)
        {
            herbNum.GetComponent<Text>().text = GameManager.instance.haveHerb.ToString();
            haveHerb = GameManager.instance.haveHerb;
        }
        // はな
        if (haveFlower != GameManager.instance.haveFlower)
        {
            flowerNum.GetComponent<Text>().text = GameManager.instance.haveFlower.ToString();
            haveFlower = GameManager.instance.haveFlower;
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

    public IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }
}
