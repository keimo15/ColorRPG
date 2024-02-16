using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 画面内を反射するリング
public class AquqController : MonoBehaviour
{
    public float deleteTime = 3.0f;     // 削除する時間指定
    public float rotation;              // オブジェクトを回転させる
    float limitUp    =  5f;
    float limitDown  = -5f;
    float limitRight =  9f;
    float limitLeft  = -9f;

    void Start()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, rotation);
        Destroy(gameObject, deleteTime);    // 削除設定
    }

    void FixedUpdate()
    {
        if (transform.position.x > limitRight || transform.position.x < limitLeft)
        {
            // x軸方向の速度を反転させる
            Vector3 currentVelocity = GetComponent<Rigidbody2D>().velocity;
            currentVelocity.x = -currentVelocity.x;
            GetComponent<Rigidbody2D>().velocity = currentVelocity;
        }
        if (transform.position.y > limitUp || transform.position.y < limitDown)
        {
            // y軸方向の速度を反転させる
            Vector3 currentVelocity = GetComponent<Rigidbody2D>().velocity;
            currentVelocity.y = -currentVelocity.y;
            GetComponent<Rigidbody2D>().velocity = currentVelocity;
        }
    }
}
