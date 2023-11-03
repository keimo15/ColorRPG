using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FireController : MonoBehaviour
{
    public float deleteTime = 3.0f;         // 自動で削除する時間
    public float fireTime = 2.0f;           // 何か物に触れてから消えるまでの時間
    public float rotation;                  // オブジェクトを回転させる

    private bool touch;                     // 何かに触れたか
    private float timerDelete;
    private float timerFire;

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
            timerFire -= Time.deltaTime;
        }
        else
        {
            timerDelete -= Time.deltaTime;
        }

        if (timerDelete <=0 || timerFire <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        touch = true;
    }
}
