using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class PlayerButtle : MonoBehaviour
{
    Rigidbody2D rbody;                                              // 当たり判定
    public Transform playerPos;                                     // Player の座標
    float axisH = 0.0f;                                             // 左右入力
    public bool onGround = false;                                   // 地上判定
    bool inDamage = false;                                          // ダメージ中フラグ
    public LayerMask groundLayer;                                   // 着地できるレイヤー
    public LayerMask moveGroundLayer;                               // 着地できるレイヤー（動く床）

    // 移動床に関しては慣性をつける
    private GroundMoverStraight moveStraightObj = null;
    private GroundMoverLoop moveLoopObj = null;
    private string moveStraightGroundTag = "MoveStraightGround";
    private string moveLoopGroundTag = "MoveLoopGround";

    // RedBlock 関連
    private bool nearRedBlock;
    private string redBlockTag = "RedBlock";
    private Collision2D redBlock;

    [SerializeField] ButtleManager buttleManager;
    [SerializeField] UIManager ui;
    [SerializeField] StageInfo[] stages;
    [SerializeField] PlayerJump jump;
    [SerializeField] PlayerPunchBattle punch;
    [SerializeField] PlayerHP hpSprite;
    [SerializeField] PlayerAnimation animation;

    // 体力
    public int hp = 5;
    // 移動速度
    public float buttleSpeed = 3.0f;
    // 攻撃力
    public int power = 1;
    
    // 一時的なバフ
    public int plusPower;
    public float plusSpeed;

    // 外的要因による強制ジャンプ
    public bool goJumpByJumpStand = false;
    public float plusJumpHeight = 0;

    // Start is called before the first frame update
    void Start()
    {
        rbody = this.GetComponent<Rigidbody2D>();
        GameManager.instance.gameState = GameState.Action;
        // バフリセット
        hp = 5;
        plusPower = 0;
        plusSpeed = 0;
        redBlock = null;
        goJumpByJumpStand = false;
        plusJumpHeight = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject == null) return;

        ui.UpdateItemCount();

        // アクション中は何もしない
        if (GameManager.instance.gameState != GameState.Action)
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
        stages[ButtleManager.nowStage].OutRangePlayerMoveToStartPos(playerPos);

        // ジャンプ
        if ((Input.GetButtonDown("Jump") && GameManager.instance.canJump && onGround) || goJumpByJumpStand)
        {
            jump.Jump(plusJumpHeight);
            goJumpByJumpStand = false;
            plusJumpHeight = 0;
        }

        // パンチ
        if (Input.GetKeyDown(KeyCode.Return) && GameManager.instance.canPunch && redBlock != null && ui.punchTimerOK)
        {
            // パンチされた RedBlock を探す
            GameObject[] redBlocks = GameObject.FindGameObjectsWithTag(redBlockTag);
            foreach (GameObject red in redBlocks)
            {
                if (red == redBlock.gameObject)
                {
                    StartCoroutine(punch.Punch(red));
                    StartCoroutine(ui.PunchTimer());
                    break;
                }
            }
            redBlock = null;
            redBlocks = null;
        }
    }

    void FixedUpdate()  // 物理系
    {
        // アクション中以外は何もしない
        if (GameManager.instance.gameState != GameState.Action || gameObject == null)
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
                  case Direction.Right:
                    addVelocity = new Vector2(moveStraightObj.speed, 0);
                    break;
                  case Direction.Left:
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
                  case Direction.Right:
                    addVelocity = new Vector2(moveLoopObj.speed, 0);
                    break;
                  case Direction.Left:
                    addVelocity = new Vector2(-moveLoopObj.speed, 0);
                    break;
                  default:
                    break;
                }
            }
            rbody.velocity = new Vector2(axisH * (buttleSpeed + plusSpeed), rbody.velocity.y) + addVelocity;
        }
    }

    // 接触
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == moveStraightGroundTag)
        {
            moveStraightObj = collision.gameObject.GetComponent<GroundMoverStraight>();
        }
        else if (collision.collider.tag == moveLoopGroundTag)
        {
            moveLoopObj = collision.gameObject.GetComponent<GroundMoverLoop>();
        }
        else if (collision.collider.tag == redBlockTag)
        {
            redBlock = collision;
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
        else if (collision.collider.tag == redBlockTag)
        {
            redBlock = null;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Attack")
        {
            // コマンドモードへ移行
            buttleManager.CommandMode();
        }
        else if (collision.gameObject.tag == "ItemUse")
        {
            // アイテムコマンドモードへ移行
            buttleManager.ItemCommandMode();
        }
        else if (collision.gameObject.tag == "Escape")
        {
            if (GameManager.instance.lastMapScene == null) return;    
            // 逃げる
            SceneManager.LoadScene(GameManager.instance.lastMapScene);
        }
        else if (collision.gameObject.tag == "Next")
        {
            // 次のアクションモードへ移行
            StartCoroutine(buttleManager.ActionMode());
        }
        else if (collision.gameObject.tag == "Start")
        {  
            // ゲームスタート
            SceneManager.LoadScene(GameManager.instance.lastMapScene);
        }
        else if (collision.gameObject.tag == "End")
        {  
            // タイトルに戻る
            SceneManager.LoadScene("TitleMain");
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyAttack")
        {
            GetDamage();
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyAttack")
        {
            GetDamage();
        }
    }

    // 被ダメージ
    public void GetDamage()
    {
        if (GameManager.instance.gameState == GameState.Action && !inDamage)
        {
            hp--;
            GameManager.instance.sumGetDamage++;
            if (hp > 0)
            {
                invincibleTime(1.0f);  // 無敵時間
            }
            else
            {
                // ゲームオーバー
                hpSprite.SetHP();
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

    // 速度のリセット
    public void Stop()
    {
        rbody.velocity = new Vector2(0, 0);
    }

    // ダメージ終了
    void DamageEnd()
    {
        // ダメージフラグ OFF
        inDamage = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }

    // ゲームオーバー
    void GameOver()
    {
        GameManager.instance.gameState = GameState.GameOver;
        GetComponent<CapsuleCollider2D>().enabled = false;
        if (rbody == null) return;
        rbody.velocity = new Vector2(0, 0);
        rbody.gravityScale = 1;
        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
        StartCoroutine(DelayMethod(3.0f, () =>
        {
            Destroy(gameObject, 1.0f);
            SceneManager.LoadScene("GameOver");
        }));
    }

    IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }
}
