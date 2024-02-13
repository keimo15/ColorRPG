using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class ButtleManager : MonoBehaviour
{
    public static int nowStage = 0;             // 現在のステージ番号
    public int howStage = 1;                    // ステージ数
    public bool noButtle = false;               // オープニング or エンディング
    private bool first;                         // noButtle において最初のステージか
    GameObject[] itemCommandBoxes;

    System.Random r = new System.Random();

    [SerializeField] CameraManager camera;
    [SerializeField] UIManager ui;
    [SerializeField] PlayerButtle player;
    [SerializeField] EnemyController enemy;
    [SerializeField] PlayerCommand command;

    [SerializeField] StageInfo[] stages;

    void Start()
    {
        first = true;
        // ゲームの状態をアクションモードにする
        RandomNextStage();                                      // 次のステージをランダムに決定
        PosReset();                                             // プレイヤーとエネミーを初期位置に移動
        camera.ActionCamera(nowStage);                          // ステージに応じてカメラの移動
        itemCommandBoxes = GameObject.FindGameObjectsWithTag("ItemUse");
        ActiveItemCommand();                                    // 「アイテム」コマンドボックスの表示
        SelectBGM();                                            // BGM の切り替え
    }

    // アクションモードへの移行
    public IEnumerator ActionMode()
    {
        player.Stop();                                          // 速度のリセット
        // 0.5 秒待つ（攻撃エフェクトなどが入るため）
        yield return new WaitForSeconds(0.5f);
        player.invincibleTime(0.5f);                            // 開始 0.5 秒間は無敵にする
        RandomNextStage();                                      // 次のステージをランダムに決定
        camera.ActionCamera(nowStage);                          // ステージに応じてカメラの移動
        PosReset();                                             // プレイヤーとエネミーを初期位置に移動
        player.onGround = false;                                // 接地判定を off に
        destroyEnemyAttack();                                   // 敵の飛び道具を画面から削除
        GameManager.instance.gameState = GameState.Action;      // アクションモードに移行
    }

    // コマンドモードへの移行
    public void CommandMode()
    {
        player.Stop();                                          // 速度のリセット
        player.invincibleTime(1.5f);                            // 1.5 秒無敵にする
        camera.CommandCamera();                                 // コマンドエリアへカメラの移動
        ui.ActiveImage(ui.bigAttackImage);                      // 「こうげき」 UI の表示
        destroyEnemyAttack();                                   // 敵の飛び道具を画面から削除
        PosReset();                                             // プレイヤーとエネミーを初期位置に移動
        command.SetFirstCommand(0, ui.commandBoxBlack,ui.commandBoxRed, ui.commandBoxGreen, ui.commandBoxBlue);    // コマンドをデフォルトにセット
        // 1 秒待つ
        StartCoroutine(DelayMethod(1.0f, () =>
        {
            ui.InactiveImage(ui.bigAttackImage);                // 「こうげき」UI の非表示
            ui.ActiveCommandImage();                            // コマンド UI の表示
            GameManager.instance.gameState = GameState.Command; // コマンドモードへの移行
        }));
    }

    // アイテムコマンドモードへの移行
    public void ItemCommandMode()
    {
        player.Stop();                                          // 速度のリセット
        player.invincibleTime(1.5f);                            // 1.5 秒無敵にする
        camera.ItemCommandCamera();                             // コマンドエリアへのカメラの移動
        ui.ActiveImage(ui.bigItemImage);                        // 「アイテム」UI の表示
        destroyEnemyAttack();                                   // 敵の飛び道具を画面から削除
        PosReset();                                             // プレイヤーとエネミーを初期位置に移動
        command.SetFirstCommand(0, ui.commandBoxReturn, ui.commandBoxApple, ui.commandBoxHerb, ui.commandBoxFlower);    // コマンドをデフォルトにセット
        command.nameBox.GetComponent<Text>().text = "";
        command.textBox.GetComponent<Text>().text = "アイテムを使わずに戻る";
        // 1 秒待つ
        StartCoroutine(DelayMethod(1.0f, () =>
        {
            ui.InactiveImage(ui.bigItemImage);                  // 「アイテム」UI の非表示
            ui.ActiveItemCommandImage();                        // コマンド UI の表示
            command.ActiveTalkBox();                            // アイテム概要欄の表示
            GameManager.instance.gameState = GameState.UseItem; // アイテムコマンドモードへの移行
        }));
    }

    // リワードモードへの移行
    public void RewardMode()
    {
        player.Stop();                                          // 速度のリセット
        GameManager.instance.gameState = GameState.Reward;
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
        if (noButtle)
        {
            if (first)
            {
                Debug.Log("first");
                nowStage = 0;
                first = false;
            }
            else
            {
                nowStage++;
            }
            Debug.Log(nowStage);
            return;
        }
        if (GetComponent<StageController>() != null)
        {
            nowStage = GetComponent<StageController>().NextStage();
        }
        else
        {
            nowStage = r.Next(howStage);
        }
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

    // BGM の切り替え
    void SelectBGM()
    {
        // Opening と Ending は特定の BGM を設定
        if (SceneManager.GetActiveScene().name == "Opening") {
            SoundManager.soundManager.PlayBgm(BGMType.TownBright);
        }
        else if (SceneManager.GetActiveScene().name == "Ending") {
            SoundManager.soundManager.PlayBgm(BGMType.Clear);
        }
        // エネミーの名前が "Boss" を含むならボス戦用の BGM に
        else if (enemy.name.Contains("Boss")) {
            SoundManager.soundManager.PlayBgm(BGMType.Boss);
        }
        else
        {
            SoundManager.soundManager.PlayBgm(BGMType.Buttle);
        }
    }
}
