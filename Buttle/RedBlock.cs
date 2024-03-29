using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// プレイヤーに破壊されたブロックを元に戻す
public class RedBlock : MonoBehaviour
{
    Sprite originalSprite;      // 元の見た目
    bool isReset;               // リセットされているかどうか

    void Start()
    {
        // デフォルトの見た目を取得しておく
        originalSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        isReset = true;
    }

    void Update()
    {
        if (GameManager.instance.gameState == GameState.Action)
        {
            isReset = false;
            return;
        }

        if (!isReset)
        {
            Reset();
        }
    }

    // 状態のリセット
    void Reset()
    {
        // スプライトをデフォルトに戻し、表示する
        gameObject.SetActive(true);
        gameObject.GetComponent<SpriteRenderer>().sprite = originalSprite;
        gameObject.GetComponent<Collider2D>().enabled = true;
        isReset = true;
    }
}
