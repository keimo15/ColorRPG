using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Item
{
    Apple,
    Herb,
    Flower,
}

public class ShopController : MonoBehaviour
{
    // 会話に必要な変数
    public string name;
    public string content;
    public GameObject nameBox;
    public GameObject textBox;
    public GameObject talkBox;
    bool canTalk = false;

    // 商品と値段
    public Item item;
    public int price;

    // true: はい, false: いいえ
    public bool questionPointer;
    public GameObject yesText;
    public GameObject noText;
    public GameObject yesPointer;
    public GameObject noPointer;

    float axisH = 0.0f;

    [SerializeField] PeopleColorChanger peopleColor;
    
    void Start()
    {
        questionPointer = true;
        InactiveTalkBox();
    }

    // Update is called once per frame
    void Update()
    {
        if (!canTalk) return;

        // 会話していない状態でスペースキーを押すと会話が始まる
        if (GameManager.gameState != GameState.Talking && Input.GetButtonDown("Jump"))
        {
            GameManager.gameState = GameState.Talking;
            ActiveTalkBox();
            nameBox.GetComponent<Text>().text = name;
            if (peopleColor.canUseColor)
            {
                textBox.GetComponent<Text>().text = content;
            }
            else
            {
                textBox.GetComponent<Text>().text = ".........。";
            }
            questionPointer = true;
            InactiveImage(noPointer);
            return;
        }

        if (GameManager.gameState != GameState.Talking) return;

        // 左右キー入力
        axisH = Input.GetAxisRaw("Horizontal");
        if (axisH < 0 && !questionPointer)
        {
            questionPointer = true;
            ActiveImage(yesPointer);
            InactiveImage(noPointer);
        }
        else if (axisH > 0 && questionPointer)
        {
            questionPointer = false;
            InactiveImage(yesPointer);
            ActiveImage(noPointer);
        }

        // 会話してるときにスペースキーを押すと質問への回答ができる
        if (Input.GetButtonDown("Jump"))
        {
            // 「はい」が選択されているならアイテムを購入する
            if (peopleColor.canUseColor && questionPointer && PlayerController.haveGold >= price)
            {
                switch (item)
                {
                  case Item.Apple:
                    PlayerController.haveApple++;
                    break;
                  case Item.Herb:
                    PlayerController.haveHerb++;
                    break;
                  case Item.Flower:
                    PlayerController.haveFlower++;
                    break;
                  default:
                    break;
                }
                PlayerController.haveGold -= price;
            }
            InactiveTalkBox();
            GameManager.gameState = GameState.Map;
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
        ActiveImage(yesText);
        ActiveImage(noText);
        ActiveImage(yesPointer);
        ActiveImage(noPointer);
    }

    // talkBox を非表示にする
    public void InactiveTalkBox()
    {
        InactiveImage(talkBox);
        InactiveImage(nameBox);
        InactiveImage(textBox);
        InactiveImage(yesText);
        InactiveImage(noText);
        InactiveImage(yesPointer);
        InactiveImage(noPointer);
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
