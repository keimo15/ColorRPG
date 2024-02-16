using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanWalkOnWaterButtle : MonoBehaviour
{
    public string groundLayer;  // 着地判定が行われるレイヤー

    void Start()
    {
        // 水上歩きが解放されているなら歩けるようにする（着地判定が行われるレイヤーに変更する）
        if (GameManager.instance.canWalk && groundLayer != null)
        {
            gameObject.layer = LayerMask.NameToLayer(groundLayer);
        }
    }
}
