using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rbody;                  // 当たり判定
    public Transform playerPos;         // Player の座標
    float axisH = 0.0f;                 // 左右入力
    public bool onGround = false;       // 地上判定
    bool inDamage = false;              // ダメージ中フラグ
    public LayerMask groundLayer;       // 着地できるレイヤー
    public LayerMask moveGroundLayer;

    private GroundMoverStraight moveStraightObj = null;
    private GroundMoverLoop moveLoopObj = null;
    private string moveStraightGroundTag = "MoveStraightGround";
    private string moveLoopGroundTag = "MoveLoopGround";

    [SerializeField] GameManager gameManager;
    [SerializeField] UIManager ui;
    [SerializeField] StageInfo[] stages;
    [SerializeField] PlayerJump jump;
    // [SerializeField] PlayerDash dash;
    // [SerializeField] PlayerPunch punch;
    [SerializeField] PlayerHP hpSprite;
    [SerializeField] PlayerAnimation animation;

    // ステータス
        // プレイヤーの HP
        public static int hp = 5;
        // 移動速度
        public float speed = 3.0f;
        // 攻撃力
        public static int power = 1;
        // 所持金
        public static int haveGold = 0;
        // 属性の解放状況
        public static bool canUseGreen = true;
        public static bool canUseBlue  = false;
        public static bool canUseRed   = false;
        // 属性エネルギーの所持数
        public static int haveGreen = 3;
        public static int haveBlue  = 3;
        public static int haveRed   = 3;
        // アイテムの所持数
        public static int haveApple  = 3;
        public static int haveHerb   = 3;
        public static int haveFlower = 3;
        // 能力の開放状況
        public static bool canJump  = true;
        public static bool canWalk  = true;
        public static bool canBreak = false;
    
    // 一時的なバフ
    public int plusPower;
    public float plusSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rbody = this.GetComponent<Rigidbody2D>();
        GameManager.gameState = GameState.Action;
        // バフリセット
        plusPower = 0;
        plusSpeed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject == null) return;

        ui.UpdateItemCount();

        // アクション中は何もしない
        if (GameManager.gameState != GameState.Action)
        {
            return;
        }

        // 水平方向の入力チェック
        axisH = Input.GetAxisRaw("Horizontal");

        // アニメーションの更新
        animation.UpdateAnimation(axisH);

        // HP UI の更新
        hpSprite.SetHP();

        // 画面外に出たらリスポーン地点に戻す & ダメージを受ける
        playerPos = this.transform;
        stages[gameManager.nowStage].OutRangePlayerMoveToStartPos(playerPos);

        // ジャンプ
        if (Input.GetButtonDown("Jump") && canJump && onGround)
        {
            jump.Jump();
        }
    }

    void FixedUpdate()  // 物理系
    {
        // アクション中以外は何もしない
        if (GameManager.gameState != GameState.Action || gameObject == null)
        {
            return;
        }

        // ダメージ中点滅させる
        if (inDamage)
        {
            float val = Mathf.Sin(Time.time * 50);
            if (val > 0)
            {
                // スプライトを表示
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }
            else
            {
                // スプライトを非表示
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
        }

        // 地上判定
        onGround = Physics2D.CircleCast(transform.position,    // 発射位置
                                        0.2f,                  // 円の半径
                                        Vector2.down,          // 発射方向
                                        0.0f,                  // 発射距離
                                        groundLayer)           // 検出するレイヤー
                || Physics2D.CircleCast(transform.position,    // 発射位置
                                        0.2f,                  // 円の半径
                                        Vector2.down,          // 発射方向
                                        0.0f,                  // 発射距離
                                        moveGroundLayer);      // 検出するレイヤー
        
        // 速度の更新
        if (onGround || axisH != 0)
        {
            // 地面の上 or 速度が 0 ではない
            Vector2 addVelocity = Vector2.zero;
            if (moveStraightObj != null)
            {
                switch(moveStraightObj.direction)
                {
                  case Direction.right:
                    addVelocity = new Vector2(moveStraightObj.speed, 0);
                    break;
                  case Direction.left:
                    addVelocity = new Vector2(-moveStraightObj.speed, 0);
                    break;
                  default:
                    break;
                }
            }
            else if (moveLoopObj != null)
            {
                switch(moveLoopObj.direction)
                {
                  case Direction.right:
                    addVelocity = new Vector2(moveLoopObj.speed, 0);
                    break;
                  case Direction.left:
                    addVelocity = new Vector2(-moveLoopObj.speed, 0);
                    break;
                  default:
                    break;
                }
            }
            rbody.velocity = new Vector2(axisH * (speed + plusSpeed), rbody.velocity.y) + addVelocity;
        }
    }

    // 接触
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            GetDamage(collision.gameObject);
        }
        else if (collision.collider.tag == moveStraightGroundTag)
        {
            moveStraightObj = collision.gameObject.GetComponent<GroundMoverStraight>();
        }
        else if (collision.collider.tag == moveLoopGroundTag)
        {
            moveLoopObj = collision.gameObject.GetComponent<GroundMoverLoop>();
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == moveStraightGroundTag)
        {
            moveStraightObj = null;
        }
        else if (collision.collider.tag == moveLoopGroundTag)
        {
            moveLoopObj = null;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyAttack")
        {
            GetDamage(collision.gameObject);
        }
        else if (collision.gameObject.tag == "Attack")
        {
            // コマンドモードへ移行
            invincibleTime(2.0f);
            gameManager.CommandMode();
        }
        else if (collision.gameObject.tag == "ItemUse")
        {
            // アイテムコマンドモードへ移行
            invincibleTime(2.0f);
            gameManager.ItemCommandMode();
        }
        else if (collision.gameObject.tag == "Escape")
        {
            if (PlayerMap.lastScene == null) return;
            // 逃げる
            SceneManager.LoadScene(PlayerMap.lastScene);
        }
    }

    // 被ダメージ
    public void GetDamage(GameObject gameObject)
    {
        if (GameManager.gameState == GameState.Action && !inDamage)
        {
            hp--;
            if (hp > 0)
            {
                // 移動停止（調整中）
                // rbody.velocity = new Vector2(0, 0);

                // ダメージ要因の反対方向にヒットバックさせる
                // Vector3 v = (transform.position - gameObject.transform.position).normalized;
                // rbody.AddForce(new Vector2(v.x * 10, v.y), ForceMode2D.Impulse);
                invincibleTime(1.0f);  // 無敵時間
            }
            else
            {
                // ゲームオーバー
                GameOver();
            }
        }
    }

    // 無敵時間
    public void invincibleTime(float time)
    {
        inDamage = true;
        Invoke("DamageEnd", time);
    }

    // ダメージ終了
    void DamageEnd()
    {
        // ダメージフラグ OFF
        inDamage = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }

    // ゲームオーバー（仮）
    void GameOver()
    {
        GameManager.gameState = GameState.GameOver;
        GetComponent<CapsuleCollider2D>().enabled = false;
        if (rbody == null) return;
        rbody.velocity = new Vector2(0, 0);
        rbody.gravityScale = 1;
        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
        Destroy(gameObject, 1.0f);
    }
}
