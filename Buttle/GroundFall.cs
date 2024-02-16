using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 乗ると落下する足場
public class GroundFall : MonoBehaviour
{
    Rigidbody2D rbody;

    public int stageNum;                        // 接地されているステージ番号
    public float fallTime;                      // 何秒後に落下するか
    public float fallSpeed;                     // 落下速度

    public bool goFall;                         // 落下フラグ
    private Vector2 defaultPosition;            // 初期位置
    private bool isReset;                       // 初期化されているか
    public float timer;                         // 落下タイマー

    [SerializeField] StageInfo stage;           // ステージ

    void Start()
    {
        // 初期位置の取得
        defaultPosition = new Vector2(rbody.position.x, rbody.position.y);
        rbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (GameManager.instance.gameState != GameState.Action || stageNum != ButtleManager.nowStage)
        {
            if (!isReset) Reset();
            return;
        }

        isReset = false;

        // 落下フラグが立っていなければ何もしない
        if (!goFall) return;

        timer += Time.deltaTime;
        // プレイヤーが触れてから一定時間経過したら落下させる
        if (timer > fallTime)
        {
            Fall();
        }
    }

    void Fall()
    {
        rbody.velocity = new Vector2(0, -fallSpeed);
        // 画面外まで落下したら、元の場所に戻す
        if (rbody.position.y < stage.limitDown-1)
        {
            Reset();
        }
    }

    // 状態のリセット
    private void Reset()
    {
        timer = 0;
        goFall = false;
        isReset = true;
        rbody.velocity = new Vector2(0, 0);
        rbody.MovePosition(defaultPosition);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            // プレイヤーが触れたら、落下フラグを立てる
            goFall = true;
        }
    }
}
