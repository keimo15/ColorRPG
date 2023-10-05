using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ダイアログボックス周りは TalkController と同じ

public class Chest : MonoBehaviour
{
    int row;
    public string name;
    public string[] contents;
    public GameObject nameBox;
    public GameObject textBox;
    public GameObject talkBox;
    bool canOpen = false;           // 開封圏内にいるか
    static bool isOpened = false;   // 開封済みか
    public Sprite openChest;        // 開いている宝箱
    public string newAbility;       // 獲得する能力

    void Start()
    {
        row = 0;
        InactiveTalkBox();
        // 既に開いているとき
        if (isOpened)
        {
            row = contents.Length;
            GetComponent<SpriteRenderer>().sprite = openChest;
        }
    }

    void Update()
    {
        // 開封可能圏外なら何もしない
        if (isOpened || !canOpen) return;

        if (Input.GetButtonDown("Jump"))
        {
            GameManager.gameState = GameState.Talking;
            NextMessage();
        }
    }

    void NextMessage()
    {
        ActiveTalkBox();
        nameBox.GetComponent<Text>().text = name;
        // メッセージがまだ続く場合
        if (row < contents.Length)
        {
            textBox.GetComponent<Text>().text = contents[row];
            row++;
        }
        // メッセージ終了
        else
        {
            InactiveTalkBox();
            GetNewAbility(newAbility);
            GetComponent<SpriteRenderer>().sprite = openChest;
            GameManager.gameState = GameState.Map;
            isOpened = true;
        }
    }

    void GetNewAbility(string ability)
    {
        switch(ability)
        {
          case "Jump":
            PlayerController.canJump = true;
            break;
          default:
            break;
        }
    }

    // 開封可能圏内に入ったらフラグをオンにする
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            canOpen = true;
        }
    }

    // 開封可能圏内から外れたらフラグをオフにする
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            canOpen = false;
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
