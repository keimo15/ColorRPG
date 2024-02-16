using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 絵を描くように見た目が変化するブロック
// 描画 -> 実体 -> 点滅 -> 透明 を繰り返す
public class DrawBlock : MonoBehaviour
{
    public int stage;                               // 配置されているステージ番号

    public float drawTime;                          // 描画時間
    public float existTime;                         // 実体時間
    public float blinkTime;                         // 点滅時間
    public float clearTime;                         // 透明時間

    private float timer;                            // 経過時間

    public Sprite[] spriteDrawBlocks;               // 見た目
    private int nowSprite;                          // 現在のスプライトのインデックス

    public DrawState firstState;                    // 初期状態
    private DrawState state;                        // 現在の状態
    private BoxCollider2D boxCollider2D;            // 当たり判定

    private bool haveReset;                         // 初期状態かの判定
    private bool haveStateReset;                    // 状態変化した直後かどうかの判定

    // 時間を分割する時に使う分数 
    private int numerator;                          // 分子
    private int denominator;                        // 分母

    void Start()
    {
        // スプライトの初期化
        GetComponent<SpriteRenderer>().sprite = spriteDrawBlocks[spriteDrawBlocks.Length-1];
        nowSprite = spriteDrawBlocks.Length;
        boxCollider2D = GetComponent<BoxCollider2D>();
        Reset();
    }

    void Update()
    {
        if (GameManager.instance.gameState != GameState.Action || stage != ButtleManager.nowStage)
        {
            if (!haveReset) Reset();
            return;
        }

        haveReset = false;      // 初期状態フラグを下げる

        switch (state)
        {
          // 「描画」
          case DrawState.Draw:
            Draw();
            break;

          // 「実体」
          case DrawState.Exist:
            Exist();
            break;

          // 「点滅」
          case DrawState.Blink:
            Blink();
            break;

          // 「透明」
          case DrawState.Clear:
            Clear();
            break;
          default:
            break;
        }

        timer += Time.deltaTime;
    }

    // 「描画」
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
            // 「実体」に遷移する
            GetComponent<SpriteRenderer>().sprite = spriteDrawBlocks[ spriteDrawBlocks.Length - 1 ];
            timer = 0;
            state = DrawState.Exist;
            haveStateReset = true;
        }
    }

    // 「実体」
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
            // 「点滅」に遷移する
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
            // 「透明」に遷移する
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
            // 描画に遷移する
            timer = 0;
            state = DrawState.Draw;
            haveStateReset = true;
        }
    }

    // 状態のリセット
    private void Reset()
    {
        //  初期状態に戻し、タイマーをリセットし、スプライトの表示する
        state = firstState;
        timer = 0;
        GetComponent<SpriteRenderer>().enabled = true;
        haveReset = true;
        haveStateReset = true;
    }
}
