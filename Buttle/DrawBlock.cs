using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DrawState
{
    Draw,
    Exist,
    Blink,
    Clear,
}


public class DrawBlock : MonoBehaviour
{
    public int stage;           // 配置されているステージ番号

    public float drawTime;      // 描画時間
    public float existTime;     // 実体時間
    public float blinkTime;     // 点滅時間
    public float clearTime;     // 透明時間

    private float timer;        // 経過時間

    public Sprite[] spriteDrawBlocks;   // 見た目
    private int nowSprite;

    public DrawState firstState;
    private DrawState state;
    private BoxCollider2D boxCollider2D;

    private bool haveReset;
    private bool haveStateReset;

    // 時間を分割する時に使う分数 
    private int numerator;                          // 分子
    private int denominator;                        // 分母

    // Start is called before the first frame update
    void Start()
    {
        haveReset = true;
        haveStateReset = true;
        state = firstState;
        timer = 0;
        GetComponent<SpriteRenderer>().sprite = spriteDrawBlocks[spriteDrawBlocks.Length-1];
        nowSprite = spriteDrawBlocks.Length;
        GetComponent<SpriteRenderer>().enabled = true;
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.gameState != GameState.Action || stage != ButtleManager.nowStage)
        {
            if (!haveReset) Reset();
            return;
        }

        haveReset = false;

        switch (state)
        {
          // 描画
          case DrawState.Draw:
            Draw();
            break;

          // 実体
          case DrawState.Exist:
            Exist();
            break;

          // 点滅
          case DrawState.Blink:
            Blink();
            break;

          // 透明
          case DrawState.Clear:
            Clear();
            break;
          default:
            break;
        }

        timer += Time.deltaTime;
    }

    // 描画
    private void Draw()
    {
        if (haveStateReset)
        {
            // 当たり判定をなくす
            boxCollider2D.enabled = false;

            // スプライトを表示
            GetComponent<SpriteRenderer>().enabled = true;

            // 時間分割用の分数の初期化
            numerator = 1;
            denominator = spriteDrawBlocks.Length - 1;

            // スプライトの設定
            nowSprite = -1;
            GetComponent<SpriteRenderer>().sprite = spriteDrawBlocks[0];

            haveStateReset = false;
        }
        else if (timer < drawTime * numerator / denominator)
        {
            // 時間を分割して、ゲームオブジェクトを描画していく
            if (numerator < spriteDrawBlocks.Length)
            {
                if (nowSprite != numerator - 1)
                {
                    nowSprite = numerator - 1;
                    GetComponent<SpriteRenderer>().sprite = spriteDrawBlocks[nowSprite];
                }
            }
        }
        else if (timer >= drawTime * numerator / denominator && numerator < denominator)
        {
            numerator++;
        }
        else if (timer > drawTime)
        {
            GetComponent<SpriteRenderer>().sprite = spriteDrawBlocks[ spriteDrawBlocks.Length - 1 ];
            timer = 0;
            state = DrawState.Exist;
            haveStateReset = true;
        }
    }

    // 実体
    private void Exist()
    {
        if (haveStateReset)
        {
            // 当たり判定をつける
            boxCollider2D.enabled = true;

            // スプライトを表示
            GetComponent<SpriteRenderer>().enabled = true;

            // スプライトの設定
            nowSprite = spriteDrawBlocks.Length - 1;
            GetComponent<SpriteRenderer>().sprite = spriteDrawBlocks[nowSprite];

            haveStateReset = false;
        }
        if (timer > existTime)
        {
            timer = 0;
            state = DrawState.Blink;
            haveStateReset = true;
        }
    }

    // 点滅
    private void Blink()
    {
        if (haveStateReset)
        {
            // 当たり判定をつける
            boxCollider2D.enabled = true;

            // スプライトを表示
            GetComponent<SpriteRenderer>().enabled = true;

            // スプライトの設定
            nowSprite = spriteDrawBlocks.Length - 1;
            GetComponent<SpriteRenderer>().sprite = spriteDrawBlocks[nowSprite];

            haveStateReset = false;
        }

        // 表示と非表示を繰り返して点滅を表現する
        float val = Mathf.Sin(Time.time * 10);
        if (val > 0)
        {
            // スプライトを表示
            GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            // スプライトを非表示
            GetComponent<SpriteRenderer>().enabled = false;
        }

        if (timer > blinkTime)
        {
            GetComponent<SpriteRenderer>().enabled = true;
            timer = 0;
            state = DrawState.Clear;
            haveStateReset = true;
        }
    }

    // 透明
    private void Clear()
    {
        if (haveStateReset)
        {
            // 当たり判定をなくす
            boxCollider2D.enabled = false;

            // スプライトの設定
            nowSprite = spriteDrawBlocks.Length - 1;
            GetComponent<SpriteRenderer>().sprite = spriteDrawBlocks[nowSprite];

            // スプライトを非表示にする
            GetComponent<SpriteRenderer>().enabled = false;

            haveStateReset = false;
        }

        if (timer > clearTime)
        {
            timer = 0;
            state = DrawState.Draw;
            haveStateReset = true;
        }
    }

    private void Reset()
    {
        state = firstState;
        timer = 0;
        GetComponent<SpriteRenderer>().enabled = true;
        haveReset = true;
        haveStateReset = true;
    }
}
