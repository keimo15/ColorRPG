using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInfo : MonoBehaviour
{
    public GameObject playerObject;

    // 画面端
    public float limitLeft;
    public float limitRight;
    public float limitUp;
    public float limitDown;

    // リスポーン地点
    public Vector2 playerStartPos;
    public Vector2 enemyStartPos;

    [SerializeField] PlayerButtle player;

    public void OutRangePlayerMoveToStartPos(Transform playerPos)
    {
        if (playerObject == null || player == null)  return;

        if (playerPos.position.x < limitLeft || playerPos.position.x > limitRight
         || playerPos.position.y > limitUp   || playerPos.position.y < limitDown)
        {
            playerPos.position = playerStartPos;
            player.GetDamage(playerObject);
        }
    }

    public void PlayerMoveToStartPos(Transform playerPos)
    {
        if (playerPos == null) return;
        playerPos.position = playerStartPos;
    }

    public void EnemyMoveToStartPos(Transform enemyPos)
    {
        if (enemyPos == null) return;
        enemyPos.position = enemyStartPos;
    }
}
