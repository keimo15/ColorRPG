using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// マップの切り替え地点
public class Exit : MonoBehaviour
{
    public MapSceneName sceneName;                          // 移動先のシーン
    public int doorNumber = 0;                              // ドア番号
    public Direction direction = Direction.Down;            // 出口の方向

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // プレイヤーが触れたら、マップ遷移する
            MapChanger.ChangeScene(sceneName.ToString(), doorNumber);
        }
    }
}
