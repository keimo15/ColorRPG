using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    public int stage;                   // ステージ番号

    public GameObject objPrefab;        // 発生させる Prefab データ
    public GameObject gate;             // 発射位置
    public float delayTime = 3.0f;      // 遅延時間
    public float fireSpeed = 4.0f;      // 発射速度
    public float direction = 180f;      // 発射方向
    Vector2 v;
    Vector2 gatePos;                    // 発射位置の座標

    public float passedTimes = 0;       // 経過時間

    [SerializeField] GameManager gameManager;

    void Start()
    {
        float x = Mathf.Cos(direction * Mathf.Deg2Rad);
        float y = Mathf.Sin(direction * Mathf.Deg2Rad);
        v = new Vector2(x, y) * fireSpeed;
        gatePos = new Vector2(gate.transform.position.x, gate.transform.position.y);
    }
    
    void Update()
    {
        if (GameManager.gameState != GameState.Action || stage != gameManager.nowStage)
        {
            return;
        }

        passedTimes += Time.deltaTime;
        if (passedTimes > delayTime)
        {
            passedTimes = 0;
            GameObject obj = Instantiate(objPrefab, gatePos, Quaternion.identity);
            Rigidbody2D rbody = obj.GetComponent<Rigidbody2D>();
            rbody.AddForce(v, ForceMode2D.Impulse);
        }
    }
}
