using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpStand : MonoBehaviour
{
    public int stageNum;
    public float jumpHeight;
    private Vector2 jumpPw;
    private Rigidbody2D rbody;
    private bool goJump;

    [SerializeField] PlayerButtle player;

    void Start()
    {
        jumpPw = new Vector2(0, jumpHeight);
        rbody = player.GetComponent<Rigidbody2D>();
        goJump = false;
    }

    void Update()
    {
        if (goJump)
        {
            rbody.AddForce(jumpPw, ForceMode2D.Impulse);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            goJump = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            goJump = false;
        }
    }
}
