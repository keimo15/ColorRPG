using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StageController : MonoBehaviour
{
    public bool use;

    private int pointer;
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
            nextStage = r.Next(GetComponent<ButtleManager>().howStage);
            return nextStage;
        }

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
