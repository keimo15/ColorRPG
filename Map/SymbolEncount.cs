using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SymbolEncount : MonoBehaviour
{
    public Enemy sceneName;                 // シンボルエンカウントのボス名
    public int symbolNum;                   // 敵の番号
        // 0: ForestBoss, 1: IslandBoss, 2: CaveBoss, 3: LastBoss
    [SerializeField] MapManager mapManager;

    private bool haveAbility;

    void Start()
    {
        // 番号の設定
        switch (sceneName)
        {
          case Enemy.ForestBoss:
            symbolNum = 0;
            break;
          case Enemy.IslandBoss:
            symbolNum = 1;
            break;
          case Enemy.CaveBoss:
            symbolNum = 2;
            break;
          case Enemy.LastBoss:
            symbolNum = 3;
            break;
          default:
            break;
        }

        haveAbility = false;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        gameObject.GetComponent<Collider2D>().enabled = false;
    }

    void Update()
    {
        if (haveAbility) return;

        // シンボルボスに対応した能力を持っているならボスを表示する
        if (   (symbolNum == 0 && GameManager.instance.canJump)
            || (symbolNum == 1 && GameManager.instance.canWalk)
            || (symbolNum == 2 && GameManager.instance.canPunch) 
            || (symbolNum == 3))
        {
            haveAbility = true;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 1);
            gameObject.GetComponent<Collider2D>().enabled = true;
        }
    }

    // 接触
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // プレイヤーが接触したらバトルを開始する
            if (sceneName == null) return;
            mapManager.StartButtle(sceneName.ToString());
        }
    }

    // マップ上のシンボルエンカウントを削除する
    public void DestroySymbolEnemy()
    {
        if (gameObject == null) return;
        Destroy(gameObject);
    }
}
