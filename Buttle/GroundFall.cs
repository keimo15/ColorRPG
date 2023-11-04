using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundFall : MonoBehaviour
{
    Rigidbody2D rbody;

    public int stageNum;                           // ステージ番号
    public float fallTime;                      // 何秒後に落下するか
    public float fallSpeed;                     // 落下速度

    public bool goFall;                        // 落下フラグ
    private Vector2 defaultPosition;
    private bool isReset;
    public float timer;

    [SerializeField] StageInfo stage;           // ステージ

    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        defaultPosition = new Vector2(rbody.position.x, rbody.position.y);
    }

    void Update()
    {
        if (GameManager.instance.gameState != GameState.Action || stageNum != ButtleManager.nowStage)
        {
            if (!isReset) Reset();
            return;
        }

        isReset = false;

        if (!goFall) return;

        timer += Time.deltaTime;
        if (timer > fallTime)
        {
            Fall();
        }
    }

    void Fall()
    {
        rbody.velocity = new Vector2(0, -fallSpeed);
        // 画面外に出たら停止して、元の場所に戻す
        if (rbody.position.y < stage.limitDown-1)
        {
            Reset();
        }
    }

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
            goFall = true;
        }
    }
}
