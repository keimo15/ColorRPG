using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    right,
    left,
    up,
    down,
}

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
    [SerializeField] GameManager gameManager;
    Direction startDirection;

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
        if (GameManager.gameState != GameState.Action || stageNum != gameManager.nowStage)
        {
            // 動かさないときに初期位置にいなければ、停止させて初期位置に戻す
            if (!isStartPos)
            {
                rbody.velocity = new Vector2(0, 0);
                rbody.MovePosition(startPos);
                distance = 0;
                isStartPos = true;
                direction = startDirection;
            }
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
        switch (direction)
        {
          case Direction.right:
            rbody.velocity = new Vector2(speed, 0);
            if (distance >= moveDistance)
            {
                direction = Direction.left;
                distance = 0;
            }
            break;
          case Direction.left:
            rbody.velocity = new Vector2(-speed, 0);
            if (distance >= moveDistance)
            {
                direction = Direction.right;
                distance = 0;
            }
            break;
          case Direction.up:
            rbody.velocity = new Vector2(0, speed);
            if (distance >= moveDistance)
            {
                direction = Direction.down;
                distance = 0;
            }
            break;
          case Direction.down:
            rbody.velocity = new Vector2(0, -speed);
            if (distance >= moveDistance)
            {
                direction = Direction.up;
                distance = 0;
            }
            break;
        }
    }
}