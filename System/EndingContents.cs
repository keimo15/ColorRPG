using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingContents : MonoBehaviour
{
    public int stageNum;
    public string content;

    private bool sameStage;

    public bool displayDamage;
    public bool displayItem;

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

    void Addcontent()
    {
        if (displayDamage)    content += GameManager.instance.sumGetDamage.ToString();
        else if (displayItem) content += GameManager.instance.sumUseItem.ToString();
    }
}
