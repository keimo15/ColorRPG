using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerReward : MonoBehaviour
{
    int row;
    string name = "";
    string[] contents;
    public GameObject nameBox;
    public GameObject textBox;
    public GameObject talkBox;

    bool talking;

    [SerializeField] GameManager gameManager;
    [SerializeField] EnemyController enemy;
    [SerializeField] UIManager ui;

    void Start()
    {
        talking = false;
        row = 0;
        InactiveTalkBox();
        contents = new string[3];
        contents[0] = enemy.name + "を倒した";
        contents[1] = enemy.dropGold.ToString() + "ゴールド";
        if (enemy.dropRed != 0)
        {
            contents[1] += "と赤エネルギーx" + enemy.dropRed.ToString();
        }
        if (enemy.dropGreen != 0)
        {
            contents[1] += "と緑エネルギーx" + enemy.dropGreen.ToString();
        }
        if (enemy.dropBlue != 0)
        {
            contents[1] += "と青エネルギーx" + enemy.dropBlue.ToString();
        }
        contents[1] += "を手に入れた";
    }

    void Update()
    {
        if (GameManager.gameState != GameState.Reward) return;
        
        if (!talking)
        {
            NextTalk();
            talking = true;
        }
        if (Input.GetButtonDown("Jump"))
        {
            NextTalk();
        }
    }

    void NextTalk()
    {
        ActiveTalkBox();
        nameBox.GetComponent<Text>().text = name;
        // 会話がまだ続く場合
        if (row < contents.Length - 1)
        {
            textBox.GetComponent<Text>().text = contents[row];
            row++;
        }
        else if (enemy.isSymbol && row < contents.Length)
        {
            contents[2] = "「";
            if (enemy.name == "ForestBoss")
            {
                contents[2] += "緑";
            }
            contents[2] += "」を世界に取り戻した！";
            gameManager.unlockColor(enemy.name);
            textBox.GetComponent<Text>().text = contents[row];
            row++;
        }
        // 会話終了
        else
        {
            GetItem();
            InactiveTalkBox();
            SceneManager.LoadScene(PlayerMap.lastScene);
        }
    }

    // ドロップ品を得る
    void GetItem()
    {
        PlayerController.haveGold  += enemy.dropGold;
        PlayerController.haveRed   += enemy.dropRed;
        PlayerController.haveGreen += enemy.dropGreen;
        PlayerController.haveBlue  += enemy.dropBlue;
    }

    // talkBox を表示する
    public void ActiveTalkBox()
    {
        ActiveImage(talkBox);
        ActiveImage(nameBox);
        ActiveImage(textBox);
    }

    // talkBox を非表示にする
    public void InactiveTalkBox()
    {
        InactiveImage(talkBox);
        InactiveImage(nameBox);
        InactiveImage(textBox);
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
