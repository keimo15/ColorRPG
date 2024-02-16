using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ジャンプ台（上から乗ると自動的に大ジャンプする）
public class JumpStand : MonoBehaviour
{
    public float jumpHeight;
    [SerializeField] PlayerButtle player;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // プレイヤーが触れたらジャンプフラグを立てて、ジャンプさせる
            player.goJumpByJumpStand = true;
            player.plusJumpHeight = jumpHeight;
        }
    }
}
