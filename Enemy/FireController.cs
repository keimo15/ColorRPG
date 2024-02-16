using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// 壁や足場に触れると、一定時間後に消える敵の攻撃
public class FireController : MonoBehaviour
{
    public float deleteTime = 3.0f;         // 自動で削除する時間
    public float fireTime = 2.0f;           // 何か物に触れてから消えるまでの時間
    public float rotation;                  // オブジェクトを回転させる

    private bool touch;                     // 何かに触れたか
    private float timerDelete;              // 発射されてからのタイマー
    private float timerFire;                // 何かに触れてからのタイマー

    void Start()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, rotation);
        touch = false;
        timerDelete = deleteTime;
        timerFire = fireTime;
    }

    void Update()
    {
        if (touch)
        {
            // 何かに触れたなら timerFire を作動させる
            timerFire -= Time.deltaTime;
        }
        else
        {
            // 何にも触れていないなら timerDelete を作動させる
            timerDelete -= Time.deltaTime;
        }

        // どちらかのタイマーが一定時間経過したら削除する
        if (timerDelete <=0 || timerFire <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 何かに触れた
        touch = true;
    }
}
