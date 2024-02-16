using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 透明な壁を進行状況によって無効化する
public class ClearWall : MonoBehaviour
{
    public AttributeColor keyColor;          // 透明な壁を無効化するトリガーとなる色

    void Start()
    {
        gameObject.GetComponent<Collider2D>().enabled = true;

        switch (keyColor)
        {
            case AttributeColor.Green:
              if (!GameManager.instance.canUseGreen) return;
              break;
            case AttributeColor.Blue:
              if (!GameManager.instance.canUseBlue) return;
              break;
            case AttributeColor.Red:
              if (!GameManager.instance.canUseRed) return;
              break;
            default:
              return;
        }

        gameObject.GetComponent<Collider2D>().enabled = false;
    }
}
