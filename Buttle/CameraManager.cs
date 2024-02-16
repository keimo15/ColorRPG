using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // 位置情報
    GameObject player;
    Transform CameraPos;
    public float MoveX = 0;
    public float MoveY = 0;

    // アクションモード時のカメラ位置制御
    public void ActionCamera(int nowStage)
    {
        // ステージ番号に応じて、カメラ位置をずらす
        transform.position = new Vector3(0, nowStage * MoveY, -10);
    }

    // コマンドモード時のカメラの位置制御
    public void CommandCamera()
    {
        // コマンド選択中はカメラをバトル画面へずらす
        transform.position = new Vector3(MoveX, -MoveY, -10);
    }

    // アイテムコマンドモード時の位置制御
    public void ItemCommandCamera()
    {
        // コマンド選択中はカメラをステージから下へずらす
        transform.position = new Vector3(MoveX, -2 * MoveY, -10);
    }
}