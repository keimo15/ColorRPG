using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// バトル勝利時の報酬管理
public class PlayerReward : MonoBehaviour
{
    // テキストボックス周り
    int row;
    string name = "";
    string[] contents;
    public GameObject nameBox;
    public GameObject textBox;
    public GameObject talkBox;
    bool talking;

    [SerializeField] EnemyController enemy;
    [SerializeField] UIManager ui;

    void Start()
    {
        // デフォルトでは「○○を倒した」を表示
        talking = false;
        row = 0;
        InactiveTalkBox();
        contents = new string[3];
        // 敵の名前や情報などに応じて、表示するメッセージが変化する
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
        if (GameManager.instance.gameState != GameState.Reward) return;
        
        if (!talking)
        {
            NextTalk();
            talking = true;
            SoundManager.soundManager.PlayBgm(BGMType.Clear);
        }
        if (Input.GetButtonDown("Jump"))
        {
            NextTalk();
            SoundManager.soundManager.PlaySE(SEType.Click);
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
            else if (enemy.name == "IslandBoss")
            {
                contents[2] += "青";
            }
            else if (enemy.name == "CaveBoss")
            {
                contents[2] += "赤";
            }
            contents[2] += "」を世界に取り戻した！";
            KillSymbolEnemy(enemy.name);
            textBox.GetComponent<Text>().text = contents[row];
            row++;
        }
        // 会話終了
        else
        {
            GetItem();
            InactiveTalkBox();
            SceneManager.LoadScene(GameManager.instance.lastMapScene);
        }
    }

    // ドロップ品を得る
    void GetItem()
    {
        GameManager.instance.haveGold  += enemy.dropGold;
        GameManager.instance.haveRed   += enemy.dropRed;
        GameManager.instance.haveGreen += enemy.dropGreen;
        GameManager.instance.haveBlue  += enemy.dropBlue;
        HaveLimit(GameManager.instance.haveGold, 99);
        HaveLimit(GameManager.instance.haveRed, 99);
        HaveLimit(GameManager.instance.haveGreen, 99);
        HaveLimit(GameManager.instance.haveBlue, 99);
    }

    int HaveLimit(int num, int limit)
    {
        if (num > limit) return limit;
        return num;
    }

    // ボスの討伐の記録と色の解放
    void KillSymbolEnemy(string enemyName)
    {
        switch(enemyName)
        {
          case "ForestBoss":
            GameManager.instance.canUseGreen = true;
            GameManager.instance.symbolEnemiesIsDead[0] = true;
            break;
          case "IslandBoss":
            GameManager.instance.canUseBlue = true;
            GameManager.instance.symbolEnemiesIsDead[1] = true;
            break;
          case "CaveBoss":
            GameManager.instance.canUseRed = true;
            GameManager.instance.symbolEnemiesIsDead[2] = true;
            break;
          default:
            break;
        }
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
