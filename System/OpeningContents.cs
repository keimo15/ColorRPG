using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpeningContents : MonoBehaviour
{
    public string[] contents;
    public GameObject contentBox;

    void Update()
    {
        contentBox.GetComponent<Text>().text = contents[ButtleManager.nowStage];
    }
}
