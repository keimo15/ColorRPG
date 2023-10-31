using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum MapSceneName
{
    MapBlueTown,
    MapBlueIsland,
    MapFirstRoad,
    MapGreenTown,
    MapGreenForest,
    MapRedTown,
    MapRedCave1,
    MapRedCave2,
    MapRedCave3,
}

public class MapChanger : MonoBehaviour
{
    public static int doorNumber = 0;                                  // 最後に通過したドア番号

    // Start is called before the first frame update
    void Start()
    {
        // プレイヤーキャラクター位置
        // 出入り口を配列で得る
        GameObject[] enters = GameObject.FindGameObjectsWithTag("Exit");
        for (int i = 0; i < enters.Length; i++)
        {
            GameObject doorObj = enters[i];             // 配列から取り出す
            Exit exit = doorObj.GetComponent<Exit>();
            if (doorNumber == exit.doorNumber && exit != null)
            {
                // ドア番号同じ
                // プレイヤーキャラクター出入り口に移動
                float x = doorObj.transform.position.x;
                float y = doorObj.transform.position.y;
                if (exit.direction == ExitDirection.up)
                {
                    y += 2;
                }
                else if (exit.direction == ExitDirection.right)
                {
                    x += 2;
                }
                else if (exit.direction == ExitDirection.down)
                {
                    y -= 2;
                }
                else if (exit.direction == ExitDirection.left)
                {
                    x -= 2;
                }
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    player.transform.position = new Vector3(x, y);
                }
                break;  // ループを抜ける
            }
        }
        enters = null;
    }

    // シーン移動
    public static void ChangeScene(string scenename, int doornum)
    {
        doorNumber = doornum;               // ドア番号を static 変数に保存
        SceneManager.LoadScene(scenename);  // シーン移動
    }
}
