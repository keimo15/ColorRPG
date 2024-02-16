using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 一定の方向に進み続ける足場
public class GroundMoverStraight : MonoBehaviour
{
    Rigidbody2D rbody;

    public Direction direction;                 // 移動方向
    public int stageNum;                        // 設置されているステージ番号
    public float speed;                         // 移動速度
    Vector2 startPos;                           // 移動開始位置
    public bool isStartPos = true;              // 開始位置にいるかどうか
    [SerializeField] StageInfo stage;           // ステージ

    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        startPos = new Vector2(rbody.position.x, rbody.position.y);
    }

    void FixedUpdate()
    {
        // アクション中ではない、もしくは別のステージにいるときは、動かさない
        if (GameManager.instance.gameState != GameState.Action || stageNum != ButtleManager.nowStage)
        {
            // 動かさないときに初期位置にいなければ、停止させて初期位置に戻す
            if (!isStartPos) Reset();
            return;
        }

        // 動き出したら初期位置フラグを下げる
        isStartPos = false;

        Move();
    }

    void Move()
    {
        // 任意の方向に進み続け、画面外に出たなら逆から出現する（右端に消えたら、左端から出現する）
        switch (direction)
        {
          case Direction.Right:
            rbody.velocity = new Vector2(speed, 0);
            if (rbody.position.x > stage.limitRight)
            {
                Vector2 v = new Vector2(stage.limitLeft, rbody.position.y);
                rbody.MovePosition(v);
            }
            break;
          case Direction.Left:
            rbody.velocity = new Vector2(-speed, 0);
            if (rbody.position.x < stage.limitLeft)
            {
                Vector2 v = new Vector2(stage.limitRight, rbody.position.y);
                rbody.MovePosition(v);
            }
            break;
          case Direction.Up:
            rbody.velocity = new Vector2(0, speed);
            if (rbody.position.y > stage.limitUp)
            {
                Vector2 v = new Vector2(rbody.position.x, stage.limitDown);
                rbody.MovePosition(v);
            }
            break;
          case Direction.Down:
            rbody.velocity = new Vector2(0, -speed);
            if (rbody.position.y < stage.limitDown)
            {
                Vector2 v = new Vector2(rbody.position.x, stage.limitUp);
                rbody.MovePosition(v);
            }
            break;
        }
    }

    // 状態のリセット
    private void Reset()
    {
        rbody.velocity = new Vector2(0, 0);
        rbody.MovePosition(startPos);
        isStartPos = true;
    }
}