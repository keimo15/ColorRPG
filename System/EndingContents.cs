using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// エンディングのメッセージ管理
public class EndingContents : MonoBehaviour
{
    public int stageNum;        // ステージ番号
    public string content;      // メッセージ

    private bool sameStage;     // ステージ判定フラグ

    public bool displayDamage;  // ダメージ数を表示するか
    public bool displayItem;    // アイテム使用数を表示するか

    void Start()
    {
        // 最初は透明に
        this.GetComponent<Text>().color = new Color(255, 255, 255, 0);
        Addcontent();
        this.GetComponent<Text>().text = content;
        sameStage = false;
    }

    void Update()
    {
        // stageNum と今のステージが同じなら表示する
        if (stageNum == ButtleManager.nowStage && !sameStage)
        {
            this.GetComponent<Text>().color = new Color(255, 255, 255, 1);
            sameStage = true;
        }

        if (stageNum != ButtleManager.nowStage && sameStage)
        {
            this.GetComponent<Text>().color = new Color(255, 255, 255, 0);
            sameStage = false;
        }
    }

    // メッセージを追加する
    void Addcontent()
    {
        if (displayDamage)    content += GameManager.instance.sumGetDamage.ToString();
        else if (displayItem) content += GameManager.instance.sumUseItem.ToString();
    }
}
