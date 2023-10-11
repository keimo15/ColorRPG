using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CanWalkOnWaterMap : MonoBehaviour
{
    public Tilemap tilemap;

    void Update()
    {
        if (tilemap.GetComponent<Collider2D>().enabled)
        {
            if (PlayerController.canWalk && PlayerController.canUseBlue && tilemap != null)
            {
                tilemap.GetComponent<Collider2D>().enabled = false;
            }
        }
    }
}
