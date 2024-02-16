using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// マップでのプレイヤー操作
public class PlayerMap : MonoBehaviour
{
    public float speed = 3.0f;              // 移動スピード
    int direction = 0;                      // 移動方向
    float axisH;                            // 横軸の入力
    float axisV;                            // 縦軸の入力
    float angleZ = -90.0f;                  // 回転角度
    Rigidbody2D rbody;                      // 当たり判定
    Animator animator;                      // Animator

    [SerializeField] MapUIManager ui;
    [SerializeField] MapManager mapManager;

    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (GameManager.instance.gameState != GameState.Map) return;

        axisH = Input.GetAxisRaw("Horizontal");
        axisV = Input.GetAxisRaw("Vertical");

        ui.UpdateItemCount();

        // パンチ
        if (mapManager.redBlock != null && Input.GetKeyDown(KeyCode.Return) && GameManager.instance.canPunch)
        {
            // パンチされた RedBlock を探す
            SoundManager.soundManager.PlaySE(SEType.Punch);
            Destroy(mapManager.redBlock);
        }

        // 移動していなければアニメーションを更新しない
        if (axisH == 0 && axisV == 0)
        {
            animator.speed = 0f;
            return;
        }

        animator.speed = 1.0f;

        // キー入力から向いている方向とアニメーション更新
        int dir = GetDirection();
        if (dir != direction)
        {
            direction = dir;
            animator.SetInteger("Direction", direction);
        }

        // 移動中でエンカウント可能ならランダムでエンカウント
        if (mapManager.canEncount)
        {
            mapManager.Encount();
        }
    }

    void FixedUpdate()
    {
        // 移動速度の更新
        rbody.velocity = new Vector2(axisH, axisV). normalized * speed;
    }

    int GetDirection()
    {
        int dir;

        // キー入力から移動角度を求める
        Vector2 fromPt = transform.position;
        Vector2 toPt = new Vector2(fromPt.x + axisH, fromPt.y + axisV);
        angleZ = GetAngle(fromPt, toPt);

        if (angleZ >= -45 && angleZ < 45)
        {
            dir = 3;
        }
        else if (angleZ >= 45 && angleZ <= 135)
        {
            dir = 2;
        }
        else if (angleZ >= -135 && angleZ < -45)
        {
            dir = 0;
        }
        else 
        {
            dir = 1;
        }

        return dir;
    }

    // p1 から p2 の角度を返す
    float GetAngle(Vector2 p1, Vector2 p2)
    {
        float angle;
        if (axisH != 0 || axisV != 0)
        {
            // 移動中であれば角度を更新する
            // p1 から p2 への差分（原点を 0 にするため）
            float dx = p2.x - p1.x;
            float dy = p2.y - p1.y;
            // アークタンジェント 2 関数で角度（ラジアン）を求める
            float rad = Mathf.Atan2(dy,dx);
            // ラジアンを度に変換して返す
            angle = rad * Mathf.Rad2Deg;
        }
        else
        {
            // 停止中であれば以前の角度を維持
            angle = angleZ;
        }
        return angle;
    }

    // 会話中などに動かないようにする
    public void Stop()
    {
        axisH = 0;
        axisV = 0;
        rbody.velocity = new Vector2(0, 0);
    }
}
