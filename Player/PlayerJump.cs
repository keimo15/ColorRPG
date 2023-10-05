using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    Rigidbody2D rbody;

    public float JumpHeight = 9.0f;     // ジャンプ力

    // Start is called before the first frame update
    void Start()
    {
        rbody = this.GetComponent<Rigidbody2D>();
    }

    public void Jump()
    {
        Vector2 jumpPw = new Vector2(0, JumpHeight);    // ジャンプさせるベクトルを作る
        rbody.AddForce(jumpPw, ForceMode2D.Impulse);    // 瞬間的な力を加える
    }
}
