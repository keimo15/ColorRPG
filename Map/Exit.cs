using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ExitDirection
{
    right,
    left,
    down,
    up,
}

public class Exit : MonoBehaviour
{
    public MapSceneName sceneName;                          // 移動先のシーン
    public int doorNumber = 0;                              // ドア番号
    public ExitDirection direction = ExitDirection.down;    // ドアの位置

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            MapChanger.ChangeScene(sceneName.ToString(), doorNumber);
        }
    }
}
