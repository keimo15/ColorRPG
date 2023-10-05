using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum GameState
{
    Action,
    Command,
    UseItem,
    Reward,
    Map,
    Talking,
    GameOver,
}

public class GameManager : MonoBehaviour
{
    public static GameState gameState = GameState.Action;
    public int nowStage = 0;
    public int howStage = 1;
    GameObject[] itemCommandBoxes;

    System.Random r = new System.Random();

    [SerializeField] CameraManager camera;
    [SerializeField] UIManager ui;
    [SerializeField] PlayerController player;
    [SerializeField] EnemyController enemy;
    [SerializeField] PlayerCommand command;

    [SerializeField] StageInfo[] stages;

    // Start is called before the first frame update
    void Start()
    {
        // ゲームの状態をアクションモードにする（とりあえず）
        RandomNextStage();
        PosReset();
        camera.ActionCamera(nowStage);
        itemCommandBoxes = GameObject.FindGameObjectsWithTag("ItemUse");
        ActiveItemCommand();
    }

    // アクションモードへの移行
    public void ActionMode()
    {
        RandomNextStage();
        camera.ActionCamera(nowStage);
        PosReset();
        destroyEnemyAttack();
        gameState = GameState.Action;
    }

    // コマンドモードへの移行
    public void CommandMode()
    {
        camera.CommandCamera();
        player.invincibleTime(1.0f);
        PosReset();
        ui.ActiveImage(ui.bigAttackImage);
        destroyEnemyAttack();
        command.SetFirstCommand(0, ui.commandBoxBlack, ui.commandBoxRed, ui.commandBoxGreen, ui.commandBoxBlue);
        StartCoroutine(DelayMethod(1.0f, () =>
        {
            ui.InactiveImage(ui.bigAttackImage);
            ui.ActiveCommandImage();
            gameState = GameState.Command;
        }));
    }

    // アイテムコマンドモードへの移行
    public void ItemCommandMode()
    {
        camera.ItemCommandCamera();
        player.invincibleTime(1.0f);
        PosReset();
        ui.ActiveImage(ui.bigItemImage);
        destroyEnemyAttack();
        command.SetFirstCommand(0, ui.commandBoxReturn, ui.commandBoxApple, ui.commandBoxHerb, ui.commandBoxFlower);
        command.nameBox.GetComponent<Text>().text = "";
        command.textBox.GetComponent<Text>().text = "アイテムを使わずに戻る";
        StartCoroutine(DelayMethod(1.0f, () =>
        {
            ui.InactiveImage(ui.bigItemImage);
            ui.ActiveItemCommandImage();
            command.ActiveTalkBox();
            gameState = GameState.UseItem;
        }));
    }

    public void RewardMode()
    {
        gameState = GameState.Reward;
    }

    public IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }

    // 座標のリセット
    public void PosReset()
    {
        if (stages[nowStage] == null) return;
        stages[nowStage].PlayerMoveToStartPos(player.playerPos);
        stages[nowStage].EnemyMoveToStartPos(enemy.enemyPos);
    }

    // 次のステージをランダムで決定
    void RandomNextStage()
    {
        nowStage = r.Next(howStage);
    }

    // 敵の飛び道具を削除する
    void destroyEnemyAttack()
    {
        GameObject[] enemyAttacks = GameObject.FindGameObjectsWithTag("EnemyAttack");
        foreach (GameObject enemyAttack in enemyAttacks)
        {
            Destroy(enemyAttack);
        }
        enemyAttacks = null;
    }

    // 色を解禁する
    public void unlockColor(string enemyName)
    {
        switch(enemyName)
        {
          case "ForestBoss":
            PlayerController.canUseGreen = true;
            break;
          default:
            break;
        }
    }

    // 画面上のアイテムコマンドを表示にする
    public void ActiveItemCommand()
    {
        foreach (GameObject itemCommandBox in itemCommandBoxes)
        {
            if (itemCommandBox != null)
            {
                itemCommandBox.SetActive(true);
            }
        }
    }

    // 画面上のアイテムコマンドを非表示にする
    public void InactiveItemCommand()
    {
        foreach (GameObject itemCommandBox in itemCommandBoxes)
        {
            if (itemCommandBox != null)
            {
                itemCommandBox.SetActive(false);
            }
        }
    }
}
