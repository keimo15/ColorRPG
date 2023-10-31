using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundStretcher : MonoBehaviour
{
    public int stageNum;
    public Transform targetObject;
    public float stretchSpeed = 1.0f;
    public float defaultTimes = 0f;     // 初期時間
    float passedTimes = 0f;             // 経過時間
    public float startTime = 0f;        // 伸び始める時間
    public float backTime = 0f;         // 縮み始める時間
    public bool doStretch = true;
    bool isDefault;
    private Vector3 originalScale;

    void Start()
    {
        originalScale = targetObject.localScale;
        passedTimes = defaultTimes;
        isDefault = true;
    }

    void Update()
    {
        if (GameManager.instance.gameState != GameState.Action || stageNum != ButtleManager.nowStage)
        {
            if (!isDefault)
            {
                targetObject.localScale = originalScale;
                isDefault = true;
                passedTimes = defaultTimes;
            }
            return;
        }

        isDefault = false;

        if (passedTimes >= backTime)
        {
            doStretch = false;
        }
        else if (passedTimes <= startTime)
        {
            doStretch = true;
        }

        if (doStretch)
        {
            passedTimes += Time.deltaTime;
        }
        else
        {
            passedTimes -= Time.deltaTime;
        }

        float newScaleY = originalScale.y + stretchSpeed * passedTimes;
        targetObject.localScale = new Vector3(originalScale.x, newScaleY, originalScale.z);
    }
}
