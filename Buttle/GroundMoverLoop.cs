using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 任意の距離を往復する足場
public class GroundMoverLoop : MonoBehaviour
{
    Rigidbody2D rbody;

    public Direction direction;                 // 移動方向
    public int stageNum;                        // 設置されているステージ番号
    public float speed;                         // 移動速度
    Vector2 startPos;                           // 移動開始位置
    public float moveDistance;                  // 移動する距離
    float distance;                             // 移動距離 
    public bool isStartPos = true;              // 開始位置にいるかどうか
    Direction startDirection;                   // 最初の移動方向

    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        startPos = new Vector2(rbody.position.x, rbody.position.y);
        startDirection = direction;
    }

    void FixedUpdate()
    {
        if (gameObject == null) return;

        // アクション中ではない、もしくは別のステージにいるときは、動かさない
        if (GameManager.instance.gameState != GameState.Action || stageNum != ButtleManager.nowStage)
        {
            // 動かさないときに初期位置にいなければ、停止させて初期位置に戻す
            if (!isStartPos) Reset();
            return;
        }

        // 動き出したら初期位置フラグを下げる
        if (isStartPos)
        {
            isStartPos = false;
        }

        Move();
    }

    void Move()
    {
        distance += speed * Time.deltaTime;
        // 一定距離移動したら、移動距離をリセットし逆向きに進むようにする
        switch (direction)
        {
          case Direction.Right:
            rbody.velocity = new Vector2(speed, 0);
            if (distance >= moveDistance)
            {
                direction = Direction.Left;
                distance = 0;
            }
            break;
          case Direction.Left:
            rbody.velocity = new Vector2(-speed, 0);
            if (distance >= moveDistance)
            {
                direction = Direction.Right;
                distance = 0;
            }
            break;
          case Direction.Up:
            rbody.velocity = new Vector2(0, speed);
            if (distance >= moveDistance)
            {
                direction = Direction.Down;
                distance = 0;
            }
            break;
          case Direction.Down:
            rbody.velocity = new Vector2(0, -speed);
            if (distance >= moveDistance)
            {
                direction = Direction.Up;
                distance = 0;
            }
            break;
        }
    }

    // 状態のリセット
    private void Reset()
    {
        rbody.velocity = new Vector2(0, 0);
        rbody.MovePosition(startPos);
        distance = 0;
        isStartPos = true;
        direction = startDirection;
    }
}