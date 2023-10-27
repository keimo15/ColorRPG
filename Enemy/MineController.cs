using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MineController : MonoBehaviour
{
    public GameObject mineMarker;
    public Sprite mineMarker1;
    public Sprite mineMarker2;
    public Sprite[] bomb;
    
    public float explodeTime = 5.0f;
    public float passedTimes = 0;


    // Start is called before the first frame update
    void Start()
    {
        mineMarker.GetComponent<SpriteRenderer>().sprite = mineMarker1;
        gameObject.tag = "Untagged";
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameState != GameState.Action)
        {
            passedTimes = 0;
            return;
        }

        passedTimes += Time.deltaTime;
        if (passedTimes > explodeTime)
        {
            passedTimes = 0;
            StartCoroutine(Explode());
        }
    }

    IEnumerator Explode()
    {
        // 0.0 秒後 (点滅)
        mineMarker.GetComponent<SpriteRenderer>().sprite = mineMarker2;

        // 0.1 秒後
        yield return new WaitForSeconds(0.1f);
        mineMarker.GetComponent<SpriteRenderer>().sprite = mineMarker1;

        // 0.5 秒後（点滅）
        yield return new WaitForSeconds(0.4f);
        mineMarker.GetComponent<SpriteRenderer>().sprite = mineMarker2;

        // 0.6 秒後
        yield return new WaitForSeconds(0.1f);
        mineMarker.GetComponent<SpriteRenderer>().sprite = mineMarker1;

        // 1.0 秒後（爆発開始）
        yield return new WaitForSeconds(0.4f);
        mineMarker.GetComponent<SpriteRenderer>().sprite = bomb[0];
        gameObject.tag = "EnemyAttack";

        // 1.1 秒後
        yield return new WaitForSeconds(0.1f);
        mineMarker.GetComponent<SpriteRenderer>().sprite = bomb[1];

        // 1.2 秒後
        yield return new WaitForSeconds(0.1f);
        mineMarker.GetComponent<SpriteRenderer>().sprite = bomb[2];

        // 1.5 秒後（爆発終了）
        yield return new WaitForSeconds(0.3f);
        mineMarker.GetComponent<SpriteRenderer>().sprite = mineMarker1;
        gameObject.tag = "Untagged";
    }
}
