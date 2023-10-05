using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafController : MonoBehaviour
{
    public float deleteTime = 3.0f;         // 削除する時間指定
    public float rotation;                  // オブジェクトを回転させる

    void Start()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, rotation);
        Destroy(gameObject, deleteTime);    // 削除設定
    }
}
