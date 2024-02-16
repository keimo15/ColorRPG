using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 敵の基本情報
public class EnemyController : MonoBehaviour
{
    public Transform enemyPos;                              // 敵の座標

    // ステータス
    public string name  = "";                               // 名前
    public AttributeColor color = AttributeColor.Black;     // 色
    public int maxHp = 3;                                   // 最大 HP
    public int hp = 3;                                      // 現在の HP

    [SerializeField] EnemyHPBar hpBar;                      // HP バー制御

    // ドロップアイテム
    public int dropGold;
    public int dropRed;
    public int dropGreen;
    public int dropBlue;

    public bool isSymbol = false;                           // シンボルエンカウントの敵であるか
    public bool thereIsNext = false;                        // 連戦があるか
    public string nextButtleSceneName;                      // 連戦の場合、次のシーン名
    public bool lastBoss = false;                           // ラスボスであるか

    void Start()
    {
        hp = maxHp;
    }

    void Update()
    {
        // hp バーの更新
        hpBar.SetHPBar( (float)hp / maxHp );
    }

    // ダメージを受けたら hp を減少させる
    public void GetDamage(int damage)
    {
        hp -= damage;
    }
}
