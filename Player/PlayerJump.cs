using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    Rigidbody2D rbody;

    public float jumpHeight = 9.0f;     // ジャンプ力

    void Start()
    {
        rbody = this.GetComponent<Rigidbody2D>();
    }

    public void Jump(float plusJumpHeight)
    {
        Debug.Log("Jump: PlayerJump");
        Vector2 jumpPw = new Vector2(0, jumpHeight + plusJumpHeight);    // ジャンプさせるベクトルを作る
        rbody.AddForce(jumpPw, ForceMode2D.Impulse);                     // 瞬間的な力を加える
    }
}
