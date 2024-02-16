using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 一定時間経過したら敵の飛び道具を削除する
public class EnemyAttackController : MonoBehaviour
{
    public float deleteTime = 3.0f;         // 削除する時間指定
    public float rotation;                  // オブジェクトを回転させる

    void Start()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, rotation);
        Destroy(gameObject, deleteTime);    // 削除設定
    }
}
