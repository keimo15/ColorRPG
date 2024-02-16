using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// NPC との会話
public class TalkController : MonoBehaviour
{
    int row;                    // 会話の行数
    public string name;         // NPC の名前
    public string[] contents;   // 会話内容
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

        // 会話可能圏内でスペースキーを押すと会話を開始する
        if (Input.GetButtonDown("Jump"))
        {
            SoundManager.soundManager.PlaySE(SEType.Click);

            // 会話中はプレイヤーを停止させる
            GameManager.instance.gameState = GameState.Talking;
            PlayerMap player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMap>();
            player.Stop();
            NextTalk();
        }
    }

    // 次の会話内容へ
    void NextTalk()
    {
        ActiveTalkBox();
        nameBox.GetComponent<Text>().text = name;
        // 会話がまだ続く場合
        if (row < contents.Length)
        {
            // 色が解放されているなら会話できる
            if (peopleColor == null || peopleColor.canUseColor)
            {
                textBox.GetComponent<Text>().text = contents[row];
                row++;
            }
            // 色が解放されていなければ、会話内容が「.......。」になる
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
            GameManager.instance.gameState = GameState.Map;
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