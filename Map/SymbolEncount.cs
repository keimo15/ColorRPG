using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolEncount : MonoBehaviour
{
    public string sceneName;            // シンボルエンカウントのボス名
    [SerializeField] PlayerMap player;

    // 接触
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // プレイヤーが接触したらバトルを開始する
            if (sceneName == null) return;
            player.StartButtle(sceneName);
        }
    }

    // マップ上のシンボルエンカウントを削除する
    public void DestroySymbolEnemy()
    {
        if (gameObject == null) return;
        Destroy(gameObject);
    }
}
