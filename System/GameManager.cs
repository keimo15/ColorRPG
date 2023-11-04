using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public GameState gameState = GameState.Action;

    // ステータス
        // 所持金
        public int haveGold = 0;
        // 属性の解放状況
        public bool canUseGreen = true;
        public bool canUseBlue  = true;
        public bool canUseRed   = true;
        // 属性エネルギーの所持数
        public int haveGreen = 0;
        public int haveBlue  = 0;
        public int haveRed   = 0;
        // アイテムの所持数
        public int haveApple  = 0;
        public int haveHerb   = 0;
        public int haveFlower = 0;
        // 能力の開放状況
        public bool canJump  = false;
        public bool canWalk  = false;
        public bool canPunch = true;
    
    // 最終位置
    public string lastMapScene;                                 // エンカウント前のシーン
    public Vector2 lastPlayerPos;                               // エンカウント前の座標
    public bool[] symbolEnemiesIsDead = new bool[3];            // シンボルエンカウント討伐状況
    public bool doButtle;                                       // バトル後にこのシーンに遷移してきたかどうか

    // シングルトン
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
