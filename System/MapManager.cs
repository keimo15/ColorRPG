using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public enum Enemy
{
    BlueSlime,
    BlueSoccerFish,
    CaveBoss,
    ForestBoss,
    GreenDevil,
    IslandBoss,
    RedBat,
    RedSlime,
    RedWizard,
    Slime,
}

public class MapManager : MonoBehaviour
{
    public bool canEncount = true;                  // エンカウントするか否か
    public int rate = 1000;                         // エンカウント率
    public Enemy[] enemies;                         // ランダムエンカウント一覧
    private string enemyScene;                      // エンカウント先のシーン
    private System.Random r = new System.Random();  // ランダムエンカウントに使用する乱数

    [SerializeField] PlayerMap player;
    [SerializeField] SymbolEncount symbolEncount;   // シンボルエンカウント一覧（現在のマップの）

    void Start()
    {
        GameManager.instance.gameState = GameState.Map;

        // マップを開くたびに討伐済みのシンボルエンカウントを削除する
        if (symbolEncount != null && GameManager.instance.symbolEnemiesIsDead[symbolEncount.symbolNum])
        {
            symbolEncount.DestroySymbolEnemy();
        }
        // バトル後にマップに戻ってきたなら、座標をバトル前の位置にする
        if (GameManager.instance.doButtle) 
        {
            player.transform.position = GameManager.instance.lastPlayerPos;
            GameManager.instance.doButtle = false;
        }
    }

    public void Encount()
    {
        if (enemies == null) return;
        var rateEncount = UnityEngine.Random.Range(0, rate);
        if (rateEncount == 50)
        {
            enemyScene = enemies[r.Next(enemies.Length)].ToString();
            StartButtle(enemyScene);
        }
    }

    public void StartButtle(string scene)
    {
        // 今のマップと座標を記録する
        GameManager.instance.lastMapScene = SceneManager.GetActiveScene().name;
        GameManager.instance.lastPlayerPos = new Vector2(player.transform.position.x, player.transform.position.y);
        GameManager.instance.doButtle = true;
        MapChanger.ChangeScene(scene, -1);
    }
}
