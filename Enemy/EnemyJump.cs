using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJump : MonoBehaviour
{
    Rigidbody2D rbody;

    public float jumpHeight   = 9.0f;
    public float jumpInterval = 2.0f;
    public LayerMask groundLayer;

    float timer = 0.0f;
    bool onGround = false;

    // Start is called before the first frame update
    void Start()
    {
        rbody = this.GetComponent<Rigidbody2D>();
        timer = jumpInterval;
    }

    void Update()
    {
        if (GameManager.instance.gameState != GameState.Action || gameObject == null)
        {
            return;
        }

        timer -= Time.deltaTime;
        if (timer > 0)
        {
            return;
        }

        // 地上判定
        onGround = Physics2D.CircleCast(transform.position,    // 発射位置
                                        0.2f,                  // 円の半径
                                        Vector2.down,          // 発射方向
                                        0.0f,                  // 発射距離
                                        groundLayer);          // 検出するレイヤー
        if (onGround)
        {
            // ジャンプさせる
            Vector2 jumpPw = new Vector2(0, jumpHeight);       // ジャンプさせるベクトルを作る
            rbody.AddForce(jumpPw, ForceMode2D.Impulse);       // 瞬間的な力を加える
            timer = jumpInterval;
        }
    }
}
