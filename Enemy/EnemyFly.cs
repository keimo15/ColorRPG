using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFly : MonoBehaviour
{
    Rigidbody2D rbody;

    public float speed = 5.0f;
    public bool isToRight = true;
    public bool isToUp = false;

    private EnemyController enemy;
    [SerializeField] StageInfo[] stages;

    Vector2 enemyStartPos;

    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        enemy = GetComponent<EnemyController>();
    }

    void Update()
    {
        if (gameObject == null) return;

        if (GameManager.instance.gameState != GameState.Action)
        {
            isToRight = true;
            isToUp = false;
            rbody.velocity = new Vector2(0, 0);
            return;
        }

        speed = 5.0f + (enemy.maxHp - enemy.hp) / 2;

        Vector2 enemyStartPos = stages[ButtleManager.nowStage].enemyStartPos;

        if (isToRight)
        {
            if (enemy.enemyPos.position.x < enemyStartPos.x + 7.0f)
            {
                rbody.velocity = new Vector2(speed, speed * (-1) / 2);
            }
            else if (enemy.enemyPos.position.x >= enemyStartPos.x + 7.0f && !isToUp)
            {
                isToUp = true;
            }
            else if (enemy.enemyPos.position.y < enemyStartPos.y + 3.5f && isToUp)
            {
                rbody.velocity = new Vector2(0, speed);
            }
            else if (enemy.enemyPos.position.y >= enemyStartPos.y + 3.5f)
            {
                isToRight = false;
                isToUp = false;
            }
        }
        else
        {
            if (enemy.enemyPos.position.x > enemyStartPos.x - 7.0f)
            {
                rbody.velocity = new Vector2(-speed, speed * (-1) / 2);
            }
            else if (enemy.enemyPos.position.x <= enemyStartPos.x - 7.0f && !isToUp)
            {
                isToUp = true;
            }
            else if (enemy.enemyPos.position.y < enemyStartPos.y + 3.5f && isToUp)
            {
                rbody.velocity = new Vector2(0, speed);
            }
            else if (enemy.enemyPos.position.y >= enemyStartPos.y + 3.5f)
            {
                isToRight = true;
                isToUp = false;
            }
        }
    }
}
