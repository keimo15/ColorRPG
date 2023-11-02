using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyColorChanger : MonoBehaviour
{
    private EnemyController enemy;
    public AttributeColor[] changeColors;
    public int beforeStage;

    public Sprite[] enemyImages;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<EnemyController>();
        beforeStage = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.gameState != GameState.Action)
        {
            return;
        }

        if (beforeStage != ButtleManager.nowStage)
        {
            beforeStage = ButtleManager.nowStage;
            if (beforeStage < changeColors.Length)
            {
                enemy.color = changeColors[beforeStage];
            }
            enemy.GetComponent<SpriteRenderer>().sprite = enemyImages[beforeStage];
        }
    }
}
