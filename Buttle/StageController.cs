using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// 次のステージ番号を決定する
public class StageController : MonoBehaviour
{
    public bool use;            // 敵の HP によってステージ番号を任意に設定するか（false ならランダム選択になる）

    private int pointer;        // 下の配列のインデックス
    public int[] hpBorders;     // 敵のステージを切り替える HP のボーダー
    public int[] stageUps;      // ステージ番号の上限
    public int[] stageDowns;    // ステージ番号の下限

    public EnemyController enemy;

    private int nextStage;
    System.Random r = new System.Random();

    void Start()
    {
        pointer = 0;
    }

    public int NextStage()
    {
        if (!use)
        {
            // 全体からランダム選択
            nextStage = r.Next(GetComponent<ButtleManager>().howStage);
            return nextStage;
        }

        // 任意の範囲からランダム選択
        for (int i=0; i<hpBorders.Length; i++)
        {
            if (enemy.hp <= hpBorders[i])
            {
                pointer = i;
            }
        }

        nextStage = r.Next(stageDowns[pointer], stageUps[pointer]+1);

        return nextStage;
    }
}
