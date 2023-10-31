using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolEncount : MonoBehaviour
{
    public Enemy sceneName;                 // シンボルエンカウントのボス名
    public int symbolNum;                   // 敵の番号
        // 0: ForestBoss, 1: IslandBoss, 2: CaveBoss
    [SerializeField] MapManager mapManager;

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
          default:
            break;
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
