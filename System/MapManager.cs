using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

// マップ管理
public class MapManager : MonoBehaviour
{
    public bool canEncount = true;                  // エンカウントするか否か
    public int rate = 1000;                         // エンカウント率
    public Enemy[] enemies;                         // ランダムエンカウント一覧
    private string enemyScene;                      // エンカウント先のシーン
    private System.Random r = new System.Random();  // ランダムエンカウントに使用する乱数

    [SerializeField] PlayerMap player;
    [SerializeField] SymbolEncount symbolEncount;   // シンボルエンカウント一覧（現在のマップの）

    public GameObject redBlock;                     // レッドブロック（1 マップに 1 つまで）

    public SaveData saveData;

    void Start()
    {
        GameManager.instance.gameState = GameState.Map;

        SaveDataAuto();

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

        SelectBGM();
    }

    // ランダムエンカウント
    public void Encount()
    {
        // 配列からランダムに選択しエンカウントする
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

    private void SaveDataAuto()
    {
        // 町に入ったときにオートセーブをする
        string nowSceneName = SceneManager.GetActiveScene().name;
        switch (nowSceneName)
        {
          case "MapBlueTown":
            GameManager.instance.lastPlayerPos = new Vector2(-3.5f, 3.0f);
            break;
          case "MapGreenTown":
            GameManager.instance.lastPlayerPos = new Vector2(8.5f, -5.0f);
            break;
          case "MapRedTown":
            GameManager.instance.lastPlayerPos = new Vector2(-7.5f, 0.0f);
            break;
          case "MapWhiteTown":
            GameManager.instance.lastPlayerPos = new Vector2(8.0f, -10.0f);
            break;
          // 町以外ならセーブしない
          default:
            return;
        }
        GameManager.instance.lastMapScene = nowSceneName;
        saveData.SavePlayerData(GameManager.instance);
    }

    void SelectBGM()
    {
        // マップの名前が "Town" を含むなら町の BGM に
        if (SceneManager.GetActiveScene().name.Contains("Town")) {
            SoundManager.soundManager.PlayBgm(BGMType.TownBright);
        }
        // それ以外ならダンジョン用の BGM に
        else
        {
            SoundManager.soundManager.PlayBgm(BGMType.Dungeon);
        }
    }
}
