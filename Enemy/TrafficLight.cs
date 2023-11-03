using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrafficLight : MonoBehaviour
{
    public int stage;                       // 配置されているステージ番号

    // 信号機としての機能
    private int color;                      // 0: 青, 1: 黄, 2: 赤
    public float[] trafficTime;             // 各色の時間
    public Sprite[] spriteTrafficLights;    // 見た目

    private float timer;                    // 経過時間

    private bool haveReset;
    private bool haveStateReset;
    private bool haveFired;

    // プレハブの発射
    public GameObject objPrefab;            // 発生させる Prefab データ
    public GameObject gate;                 // 発射位置
    public float fireSpeed = 4.0f;          // 発射速度
    public float direction = 180f;          // 発射方向
    Vector2 v;
    Vector2 gatePos;                        // 発射位置の座標
    private float axisH = 0.0f;                     // 左右入力

    void Start()
    {
        Reset();
        float x = Mathf.Cos(direction * Mathf.Deg2Rad);
        float y = Mathf.Sin(direction * Mathf.Deg2Rad);
        v = new Vector2(x, y) * fireSpeed;
        gatePos = new Vector2(gate.transform.position.x, gate.transform.position.y);
    }

    void Update()
    {
        if (GameManager.instance.gameState != GameState.Action || stage != ButtleManager.nowStage)
        {
            if (!haveReset) Reset();
            return;
        }

        haveReset = false;

        TrafficControl();

        timer += Time.deltaTime;

        // 水平方向の入力チェック
        axisH = Input.GetAxisRaw("Horizontal");

        // 赤信号の時に入力が攻撃される
        if (!haveFired && color == 2 && axisH != 0) Fire();
    }

    private void Fire()
    {
        // プレハブを発射
        GameObject obj = Instantiate(objPrefab, gatePos, Quaternion.identity);
        Rigidbody2D rbody = obj.GetComponent<Rigidbody2D>();
        rbody.AddForce(v, ForceMode2D.Impulse);
        haveFired = true;
    }

    private void TrafficControl()
    {
        if (haveStateReset)
        {
            // 見た目の変更
            GetComponent<SpriteRenderer>().sprite = spriteTrafficLights[color];
            haveStateReset = false;
            if (color == 2)
            {
                haveFired = false;
            }
        }
        if (timer > trafficTime[color])
        {
            // 次の色への変更
            timer = 0;
            color++;
            color %= 3;
            haveStateReset = true;
        }
    }

    private void Reset()
    {
        color = 0;
        timer = 0;
        haveReset = true;
        haveStateReset = true;
        haveFired = false;
        GetComponent<SpriteRenderer>().sprite = spriteTrafficLights[color];
    }
}
