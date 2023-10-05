using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMap : MonoBehaviour
{
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;
        // プレイヤーの位置と連動させる
        transform.position = new Vector3(player.transform.position.x,
                                         player.transform.position.y, -10);
    }
}
