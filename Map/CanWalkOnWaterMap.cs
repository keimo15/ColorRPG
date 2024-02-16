using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// マップ上の水色のタイルの上を歩けるようにする
public class CanWalkOnWaterMap : MonoBehaviour
{
    public Tilemap tilemap;

    void Update()
    {
        if (tilemap.GetComponent<Collider2D>().enabled)
        {
            if (GameManager.instance.canWalk && GameManager.instance.canUseBlue && tilemap != null)
            {
                // 「青」を開放していれば、当たり判定を消すことで、上を歩けるようにする
                tilemap.GetComponent<Collider2D>().enabled = false;
            }
        }
    }
}
