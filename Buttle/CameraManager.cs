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
        transform.position = new Vector3(0, nowStage * MoveY, -10);
    }

    // コマンドモード時のカメラの位置制御
    public void CommandCamera()
    {
        transform.position = new Vector3(MoveX, -MoveY, -10);
    }

    // アイテムコマンドモード時の位置制御
    public void ItemCommandCamera()
    {
        transform.position = new Vector3(MoveX, -2 * MoveY, -10);
    }
}