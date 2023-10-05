using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkController : MonoBehaviour
{
    int row;
    public string name;
    public string[] contents;
    public GameObject nameBox;
    public GameObject textBox;
    public GameObject talkBox;
    bool canTalk = false;

    [SerializeField] PeopleColorChanger peopleColor;

    void Start()
    {
        row = 0;
        InactiveTalkBox();
    }

    void Update()
    {
        // 会話可能圏外なら何もしない
        if (!canTalk) return;

        if (Input.GetButtonDown("Jump"))
        {
            GameManager.gameState = GameState.Talking;
            NextTalk();
        }
    }

    void NextTalk()
    {
        ActiveTalkBox();
        nameBox.GetComponent<Text>().text = name;
        // 会話がまだ続く場合
        if (row < contents.Length)
        {
            if (peopleColor == null || peopleColor.canUseColor)
            {
                textBox.GetComponent<Text>().text = contents[row];
                row++;
            }
            else
            {
                textBox.GetComponent<Text>().text = ".........。";
                row = contents.Length;
            }
        }
        // 会話終了
        else
        {
            InactiveTalkBox();
            GameManager.gameState = GameState.Map;
            row = 0;
        }
    }

    // 会話可能圏内に入ったらフラグをオンにする
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            canTalk = true;
        }
    }

    // 会話可能圏内から外れたらフラグをオフにする
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            canTalk = false;
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