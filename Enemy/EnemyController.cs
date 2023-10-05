using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttributeColor
{
    Black,
    Red,
    Blue,
    Green,
}

public class EnemyController : MonoBehaviour
{
    public Transform enemyPos;

    // ステータス
    public string name  = "";
    public AttributeColor color = AttributeColor.Black;
    public int maxHp = 3;
    public int hp = 3;

    [SerializeField] EnemyHPBar hpBar;          // HP バー制御

    // ドロップアイテム
    public int dropGold;
    public int dropRed;
    public int dropGreen;
    public int dropBlue;

    public bool isSymbol = false;               // シンボルエンカウントの敵であるか
    public bool thereIsNext = false;            // 連戦があるか
    public string nextButtleSceneName;          // 連戦の場合、次の相手のシーン名

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        hpBar.SetHPBar( (float)hp / maxHp );
    }

    public void GetDamage(int damage)
    {
        hp -= damage;
    }
}
