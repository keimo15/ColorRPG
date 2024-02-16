using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// タイルの色を変更する
public class TileColorChanger : MonoBehaviour
{
    public Tilemap tilemap;           // タイル
    public Color newColor;            // 変更先の色
    public AttributeColor tileColor;

    void Start()
    {
        ColorChange();
    }

    void ColorChange()
    {
        // 色が解放済みなら何もしない
        switch(tileColor)
        {
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

        // 全てのタイルマップを取得
        BoundsInt bounds = tilemap.cellBounds;

        foreach (Vector3Int tilePosition in bounds.allPositionsWithin)
        {
            TileBase tile = tilemap.GetTile(tilePosition);

            if (tile != null)
            {
                // ひとつずつ色を変えていく
                tilemap.SetTileFlags(tilePosition, TileFlags.None);
                tilemap.SetColor(tilePosition, newColor);
            }

            tile = null;
        }
    }
}
