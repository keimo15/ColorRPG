using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

// バトル中のパンチ
public class PlayerPunchBattle : MonoBehaviour
{
    public Sprite punchMark;

    public IEnumerator Punch(GameObject redBlock)
    {
        // 対象のブロックをパンチする演出、判定をなくす
        SoundManager.soundManager.PlaySE(SEType.Punch);
        redBlock.GetComponent<SpriteRenderer>().sprite = punchMark;
        redBlock.GetComponent<Collider2D>().enabled = false;

        // 0.5 秒後にパンチ演出を非表示にする
        yield return new WaitForSeconds(0.5f);
        redBlock.GetComponent<SpriteRenderer>().sprite = null;
    }
}