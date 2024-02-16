using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 伸び縮みする足場
public class GroundStretcher : MonoBehaviour
{
    public int stageNum;                // ステージ番号
    public Transform targetObject;
    public Direction stretchDirection;  // 伸びる方向
    public float stretchSpeed = 1.0f;   // 伸縮スピード
    public float defaultTimes = 0f;     // 初期時間
    float passedTimes = 0f;             // 経過時間
    public float startTime = 0f;        // 伸び始める時間
    public float backTime = 0f;         // 縮み始める時間
    public bool doStretch = true;       // 伸びるか縮むか
    bool isDefault;
    private Vector3 originalScale;      // 元の大きさ
    private Vector2 newScale;           // 変更後の大きさ

    void Start()
    {
        // デフォルトの長さを取得しておく
        originalScale = targetObject.localScale;
        Reset();
    }

    void Update()
    {
        // アクション中ではない、もしくは別のステージにいるときは、動かさない
        if (GameManager.instance.gameState != GameState.Action || stageNum != ButtleManager.nowStage)
        {
            if (!isDefault) Reset();
            return;
        }

        isDefault = false;

        // 伸び始めて一定時間経過したら、縮むようにフラグを下げる
        if (passedTimes >= backTime)
        {
            doStretch = false;
        }

        // 縮み始めて一定時間経過したら、伸びるようにフラグを立てる
        else if (passedTimes <= startTime)
        {
            doStretch = true;
        }

        if (doStretch)
        {
            passedTimes += Time.deltaTime;
        }
        else
        {
            passedTimes -= Time.deltaTime;
        }

        Stretch();
    }

    private void Stretch()
    {
        // 伸びる方向によって分岐
        switch (stretchDirection)
        {
          case Direction.Right:
            newScale.x = originalScale.x + stretchSpeed * passedTimes;
            break;
          case Direction.Left:
            newScale.x = originalScale.x - stretchSpeed * passedTimes;
            break;
          case Direction.Up:
            newScale.y = originalScale.y + stretchSpeed * passedTimes;
            break;
          case Direction.Down:
            newScale.y = originalScale.y - stretchSpeed * passedTimes;
            break;
        }
        targetObject.localScale = new Vector3(newScale.x, newScale.y, originalScale.z);
    }

    // 状態のリセット
    private void Reset()
    {
        targetObject.localScale = originalScale;
        newScale.x = originalScale.x;
        newScale.y = originalScale.y;
        isDefault = true;
        passedTimes = defaultTimes;
    }
}
