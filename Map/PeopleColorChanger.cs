using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleColorChanger : MonoBehaviour
{
    public enum NpcColor
    {
        Red,
        Green,
        Blue,
    }

    public Color newColor;      // 変更後の色
    public NpcColor npcColor;   // NPCの色
    public bool canUseColor;    // NPCの色が解放されているか

    void Start()
    {
        canUseColor = true;
        ColorChange();
    }

    void ColorChange()
    {
        switch(npcColor)
        {
            // NPCの色が解放されているなら何もしない
          case NpcColor.Red:
            if (PlayerController.canUseRed) return;
            break;
          case NpcColor.Green:
            if (PlayerController.canUseGreen) return;
            break;
          case NpcColor.Blue:
            if (PlayerController.canUseBlue) return;
            break;
          default:
            break;
        }
        canUseColor = false;
        GetComponent<SpriteRenderer>().color = newColor;
    }
}
