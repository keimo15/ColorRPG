using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    // アニメーション
    Animator animator;              // アニメーター
    public string stopAnime = "PlayerStop";
    public string moveAnime = "PlayerMove";
    string nowAnime = "";
    string oldAnime = "";

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        // 停止中の画像に初期化
        nowAnime = stopAnime;
        oldAnime = stopAnime;
    }

    public void UpdateAnimation(float axisH)
    {
        // 向きの調整
        if (axisH > 0.0f)
        {
            // 右移動
            transform.localScale = new Vector2(1, 1);
        }
        else if (axisH < 0.0f)
        {
            // 左移動
            transform.localScale = new Vector2(-1, 1);  // 左右反転させる
        }

        // アニメーション更新
        if (axisH == 0)
        {
            nowAnime = stopAnime;
        }
        else
        {
            nowAnime = moveAnime;
        }
        if (nowAnime != oldAnime)
        {
            oldAnime = nowAnime;
            animator.Play(nowAnime);    // アニメーション
        }
    }
}
