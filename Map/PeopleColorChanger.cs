using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NPC の色を変更する
public class PeopleColorChanger : MonoBehaviour
{
    public enum NpcColor
    {
        Red,
        Green,
        Blue,
    }

    public Color newColor;             // 変更後の色
    public AttributeColor npcColor;    // NPCの色
    public bool canUseColor;           // NPCの色が解放されているか

    void Start()
    {
        canUseColor = true;
        ColorChange();
    }

    // 色の変更
    void ColorChange()
    {
        switch(npcColor)
        {
            // NPCの色が解放されているなら何もしない
            case AttributeColor.Red:
                if (GameManager.instance.canUseRed) return;
                break;
            case AttributeColor.Green:
                if (GameManager.instance.canUseGreen) return;
                break;
            case AttributeColor.Blue:
                if (GameManager.instance.canUseBlue) return;
                break;
            default:
                break;
        }
        canUseColor = false;
        GetComponent<SpriteRenderer>().color = newColor;
    }
}
