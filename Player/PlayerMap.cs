using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class PlayerMap : MonoBehaviour
{
    public float speed = 3.0f;              // 移動スピード
    int direction = 0;                      // 移動方向
    float axisH;                            // 横軸
    float axisV;                            // 縦軸
    public float angleZ = -90.0f;           // 回転角度
    Rigidbody2D rbody;                      // 当たり判定
    Animator animator;                      // Animator
    public bool canEncount = true;          // エンカウントするか否か
    public int rate = 300;                  // エンカウント率
    public string enemyScene;               // エンカウント先のシーン
    public static string lastScene;         // エンカウント前のシーン
    public static Vector2 lastPlayerPos;    // エンカウント前の座標

    public string[] enemies;                                                // ランダムエンカウント一覧
    public SymbolEncount[] symbolEncount;                                   // シンボルエンカウント一覧（現在のマップの）
    public static string[] symbolEnemies = {"ForestBoss", "IslandBoss"};    // シンボルエンカウント一覧
    public static bool[] symbolEnemiesIsDead = new bool[2];                 // シンボルエンカウント討伐状況
    System.Random r = new System.Random();

    [SerializeField] MapUIManager ui;

    public static bool doButtle;    // バトル後にこのシーンに遷移してきたかどうか

    // Start is called before the first frame update
    void Start()
    {
        GameStart();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameState != GameState.Map) return;

        axisH = Input.GetAxisRaw("Horizontal");
        axisV = Input.GetAxisRaw("Vertical");

        ui.UpdateItemCount();

        // 移動していなければ更新しない
        if (axisH == 0 && axisV == 0)
        {
            animator.speed = 0f;
            return;
        }

        animator.speed = 1.0f;

        // キー入力から向いている方向とアニメーション更新
        int dir = GetDirection();
        if (dir != direction)
        {
            direction = dir;
            animator.SetInteger("Direction", direction);
        }

        // 移動中でエンカウント可能ならランダムでエンカウント
        if (canEncount)
        {
            Encount();
        }
    }

    void FixedUpdate()
    {
        // 移動速度の更新
        rbody.velocity = new Vector2(axisH, axisV). normalized * speed;
    }

    // 接触
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "NoEncountTrigger")
        {
            canEncount = false;
        }
    }

    void GameStart()
    {
        GameManager.gameState = GameState.Map;
        rbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        // マップを開くたびに討伐済みのシンボルエンカウントを削除する
        for (int i=0; i<symbolEnemiesIsDead.Length; i++)
        {
            if (symbolEnemies[i] == null) continue;
            if (symbolEnemiesIsDead[i])
            {
                foreach (SymbolEncount symbol in symbolEncount)
                {
                    if (symbol == null) continue;
                    if (symbol.sceneName == symbolEnemies[i])
                    {
                        symbol.DestroySymbolEnemy();
                    }
                }
            }
        }
        if (doButtle)
        {
            this.transform.position = lastPlayerPos;
            doButtle = false;
        }
        PlayerController.hp = 5;
    }

    int GetDirection()
    {
        int dir;

        // キー入力から移動角度を求める
        Vector2 fromPt = transform.position;
        Vector2 toPt = new Vector2(fromPt.x + axisH, fromPt.y + axisV);
        angleZ = GetAngle(fromPt, toPt);

        if (angleZ >= -45 && angleZ < 45)
        {
            dir = 3;
        }
        else if (angleZ >= 45 && angleZ <= 135)
        {
            dir = 2;
        }
        else if (angleZ >= -135 && angleZ < -45)
        {
            dir = 0;
        }
        else 
        {
            dir = 1;
        }

        return dir;
    }

    // p1 から p2 の角度を返す
    float GetAngle(Vector2 p1, Vector2 p2)
    {
        float angle;
        if (axisH != 0 || axisV != 0)
        {
            // 移動中であれば角度を更新する
            // p1 から p2 への差分（原点を 0 にするため）
            float dx = p2.x - p1.x;
            float dy = p2.y - p1.y;
            // アークタンジェント 2 関数で角度（ラジアン）を求める
            float rad = Mathf.Atan2(dy,dx);
            // ラジアンを度に変換して返す
            angle = rad * Mathf.Rad2Deg;
        }
        else
        {
            // 停止中であれば以前の角度を維持
            angle = angleZ;
        }
        return angle;
    }

    void Encount()
    {
        if (enemies == null) return;
        var rateEncount = UnityEngine.Random.Range(0, rate);
        if (rateEncount == 50)
        {
            enemyScene = enemies[r.Next(enemies.Length)];
            StartButtle(enemyScene);
        }
    }

    public void StartButtle(string scene)
    {
        // 今のマップと座標を記録する
        lastScene = SceneManager.GetActiveScene().name;
        lastPlayerPos = new Vector2(this.transform.position.x, this.transform.position.y);
        doButtle = true;
        SceneManager.LoadScene(scene);
    }

    // 討伐済みシンボルモンスターを記録する
    public static void KillSymbolEnemy(string enemyName)
    {
        for (int i=0; i<symbolEnemies.Length; i++)
        {
            if (enemyName == symbolEnemies[i])
            {
                symbolEnemiesIsDead[i] = true;
                return;
            }
        }
    }
}
