using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpStand : MonoBehaviour
{
    public float jumpHeight;
    [SerializeField] PlayerButtle player;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.goJumpByJumpStand = true;
            player.plusJumpHeight = jumpHeight;
        }
    }
}
