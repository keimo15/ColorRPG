using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// マップ上でのカメラ
public class CameraMap : MonoBehaviour
{
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;
        // プレイヤーを追従する
        transform.position = new Vector3(player.transform.position.x,
                                         player.transform.position.y, -10);
    }
}
