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

public class GroundMover1 : MonoBehaviour
{
    Rigidbody2D rbody;

    public Direction direction;                 // 移動方向
    public int stageNum;                        // 設置されているステージ番号
    public float speed;                         // 移動速度
    Vector2 startPos;                           // 移動開始位置
    public bool isStartPos = true;              // 開始位置にいるかどうか
    [SerializeField] StageInfo stage;           // ステージ
    [SerializeField] GameManager gameManager;

    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        startPos = new Vector2(rbody.position.x, rbody.position.y);
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
                isStartPos = true;
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
        switch (direction)
        {
          case Direction.right:
            rbody.velocity = new Vector2(speed, 0);
            if (rbody.position.x > stage.limitRight)
            {
                Vector2 v = new Vector2(stage.limitLeft, rbody.position.y);
                rbody.MovePosition(v);
            }
            break;
          case Direction.left:
            rbody.velocity = new Vector2(-speed, 0);
            if (rbody.position.x < stage.limitLeft)
            {
                Vector2 v = new Vector2(stage.limitRight, rbody.position.y);
                rbody.MovePosition(v);
            }
            break;
          case Direction.up:
            rbody.velocity = new Vector2(0, speed);
            if (rbody.position.y > stage.limitUp)
            {
                Vector2 v = new Vector2(rbody.position.x, stage.limitDown);
                rbody.MovePosition(v);
            }
            break;
          case Direction.down:
            rbody.velocity = new Vector2(0, -speed);
            if (rbody.position.y < stage.limitDown)
            {
                Vector2 v = new Vector2(rbody.position.x, stage.limitUp);
                rbody.MovePosition(v);
            }
            break;
        }
    }
}