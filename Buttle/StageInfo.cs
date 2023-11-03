using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInfo : MonoBehaviour
{
    // 画面端
    public float limitLeft;
    public float limitRight;
    public float limitUp;
    public float limitDown;

    // リスポーン地点
    public Vector2 playerStartPos;
    public Vector2 enemyStartPos;

    [SerializeField] PlayerButtle player;

    // プレイヤーが画面外に出たなら初期位置に戻し、ダメージを与える
    public void OutRangePlayerMoveToStartPos(Transform playerPos)
    {
        if (player == null)  return;

        if (playerPos.position.x < limitLeft || playerPos.position.x > limitRight
         || playerPos.position.y > limitUp   || playerPos.position.y < limitDown)
        {
            PlayerMoveToStartPos(playerPos);
            player.GetDamage();
        }
    }

    // プレイヤーを初期位置に戻す
    public void PlayerMoveToStartPos(Transform playerPos)
    {
        if (playerPos == null) return;
        playerPos.position = playerStartPos;
    }

    // 敵を初期位置に戻す
    public void EnemyMoveToStartPos(Transform enemyPos)
    {
        if (enemyPos == null) return;
        enemyPos.position = enemyStartPos;
    }
}
