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
    bool canOpen;                   // 開封圏内にいるか
    bool isOpened;                  // 開封済みか
    public Sprite openChest;        // 開いている宝箱
    public Ability newAbility;      // 獲得する能力

    void Start()
    {
        isOpened = false;
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
            SoundManager.soundManager.PlaySE(SEType.Click);
            GameManager.instance.gameState = GameState.Talking;
            NextMessage();
        }
    }

    void NextMessage()
    {
        ActiveTalkBox();
        nameBox.GetComponent<Text>().text = name;

        // メッセージ終了
        if (row == contents.Length)
        {
            InactiveTalkBox();
            GetNewAbility();
            GetComponent<SpriteRenderer>().sprite = openChest;
            GameManager.instance.gameState = GameState.Map;
            isOpened = true;
        }
        
        // 獲得済みの場合
        else if ((newAbility == Ability.Jump && GameManager.instance.canJump) ||
            (newAbility == Ability.Walk && GameManager.instance.canWalk) ||
            (newAbility == Ability.Punch && GameManager.instance.canPunch))
        {
            textBox.GetComponent<Text>().text = "宝箱は空っぽだった...。";
            row = contents.Length;
        }

        // メッセージがまだ続く場合
        else if (row < contents.Length)
        {
            textBox.GetComponent<Text>().text = contents[row];
            row++;
        }
    }

    // 能力解放
    void GetNewAbility()
    {
        switch(newAbility)
        {
          case Ability.Jump:
            GameManager.instance.canJump = true;
            break;
          case Ability.Walk:
            GameManager.instance.canWalk = true;
            break;
          case Ability.Punch:
            GameManager.instance.canPunch = true;
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
