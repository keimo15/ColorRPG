using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanWalkOnWaterButtle : MonoBehaviour
{
    public string groundLayer;

    void Start()
    {
        // 水上歩きが解放されているなら歩けるようにする
        if (GameManager.instance.canWalk && groundLayer != null)
        {
            gameObject.layer = LayerMask.NameToLayer(groundLayer);
        }
    }
}
