using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ステージ番号に応じて敵の色（見た目と属性）を変化させる
public class EnemyColorChanger : MonoBehaviour
{
    private EnemyController enemy;              // 敵
    public AttributeColor[] changeColors;       // 変更する色
    public int beforeStage;                     // 直前のステージ番号
    public Sprite[] enemyImages;                // 敵の見た目

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<EnemyController>();
        beforeStage = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.gameState != GameState.Action) return;

        // 以前のステージと異なるなら更新する
        if (beforeStage != ButtleManager.nowStage)
        {
            beforeStage = ButtleManager.nowStage;
            if (beforeStage < changeColors.Length) enemy.color = changeColors[beforeStage];
            enemy.GetComponent<SpriteRenderer>().sprite = enemyImages[beforeStage];
        }
    }
}
