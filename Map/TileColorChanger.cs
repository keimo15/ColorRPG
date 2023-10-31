using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileColorChanger : MonoBehaviour
{
    public enum TileColor
    {
        Red,
        Green,
        Blue
    }

    public Tilemap tilemap;
    public Color newColor;
    public TileColor tileColor;

    // Start is called before the first frame update
    void Start()
    {
        ColorChange();
    }

    void ColorChange()
    {
        // 色が解放済みなら何もしない
        switch(tileColor)
        {
          case TileColor.Red:
            if (GameManager.instance.canUseRed) return;
            break;
          case TileColor.Green:
            if (GameManager.instance.canUseGreen) return;
            break;
          case TileColor.Blue:
            if (GameManager.instance.canUseBlue) return;
            break;
          default:
            break;
        }

        BoundsInt bounds = tilemap.cellBounds;

        foreach (Vector3Int tilePosition in bounds.allPositionsWithin)
        {
            TileBase tile = tilemap.GetTile(tilePosition);

            if (tile != null)
            {
                tilemap.SetTileFlags(tilePosition, TileFlags.None);
                tilemap.SetColor(tilePosition, newColor);
            }

            tile = null;
        }
    }
}
