using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class PlayerCommand : MonoBehaviour
{
    public GameObject nameBox;
    public GameObject textBox;
    public GameObject talkBox;

    [SerializeField] ButtleManager buttleManager;
    [SerializeField] UIManager ui;
    [SerializeField] PlayerButtle player;
    [SerializeField] EnemyController enemy;

    float light = 0.1f;         // 非選択オブジェクトの色の薄さ

    // キー入力
    float axisH = 0.0f;
    float axisV = 1.0f;

    int directionCommand;       // 0:黒 1:赤 2:緑 3:青
    bool haveChosen;            // 攻撃済みか否か

    // Update is called once per frame
    void Update()
    {
        if (gameObject == null || enemy == null) return;
        
        if (GameManager.instance.gameState != GameState.Command && GameManager.instance.gameState != GameState.UseItem)
        {
            haveChosen = false;
            return;
        }

        // コマンド選択済みなら何もしない
        if (haveChosen)
        {
            return;
        }

        axisH = Input.GetAxisRaw("Horizontal");     // 左右キー入力
        axisV = Input.GetAxisRaw("Vertical");       // 上下キー入力

        // 攻撃を選択
        if (GameManager.instance.gameState == GameState.Command)
        {
            if (axisV > 0 && directionCommand != 0)
            {   // 上入力
                SetFirstCommand(0, ui.commandBoxBlack, ui.commandBoxRed, ui.commandBoxGreen, ui.commandBoxBlue);
            }
            else if (axisH < 0 && directionCommand != 1 && GameManager.instance.canUseRed && GameManager.instance.haveRed > 0)
            {   // 左入力
                SetFirstCommand(1, ui.commandBoxRed, ui.commandBoxBlack, ui.commandBoxGreen, ui.commandBoxBlue);
            }
            else if (axisV < 0 && directionCommand != 2 && GameManager.instance.canUseGreen && GameManager.instance.haveGreen > 0)
            {   // 下入力
                SetFirstCommand(2, ui.commandBoxGreen, ui.commandBoxBlack, ui.commandBoxRed, ui.commandBoxBlue);
            }
            else if (axisH > 0 && directionCommand != 3 && GameManager.instance.canUseBlue && GameManager.instance.haveBlue > 0)
            {   // 右入力
                SetFirstCommand(3, ui.commandBoxBlue, ui.commandBoxBlack, ui.commandBoxRed, ui.commandBoxGreen);
            }

            // 攻撃
            if (Input.GetButtonDown("Jump"))
            {
                SoundManager.soundManager.PlaySE(SEType.Attack);
                haveChosen = true;
                Attack(directionCommand);
                // バフのリセット
                player.plusPower = 0;
                player.plusSpeed = 0;
                buttleManager.ActiveItemCommand();
                ui.InactiveCommandImage();

                // 敵を倒した
                if (enemy.hp <= 0)
                {
                    ui.InactiveImage(ui.stopEnemy);
                    if (enemy.lastBoss)
                    {
                        SceneManager.LoadScene("Ending");
                    }
                    if (enemy.isSymbol)
                    {
                        // 連戦があるなら次のシーンを読み込む
                        if (enemy.thereIsNext)
                        {
                            SceneManager.LoadScene(enemy.nextButtleSceneName);
                            return;
                        }
                    }
                    if (GameManager.instance.lastMapScene != null)
                    {
                        buttleManager.RewardMode();
                    }
                }
                // 敵をまだ倒していない
                else
                {
                    StartCoroutine(buttleManager.ActionMode());
                }
            }
        }

        // アイテムを選択
        else if (GameManager.instance.gameState == GameState.UseItem)
        {
            if (axisV > 0 && directionCommand != 0)
            {   // 上入力
                SetFirstCommand(0, ui.commandBoxReturn, ui.commandBoxApple, ui.commandBoxHerb, ui.commandBoxFlower);
                nameBox.GetComponent<Text>().text = "";
                textBox.GetComponent<Text>().text = "アイテムを使わずに戻る";
            }
            else if (axisH < 0 && directionCommand != 1 && GameManager.instance.canUseRed && GameManager.instance.haveApple > 0)
            {   // 左入力
                SetFirstCommand(1, ui.commandBoxApple, ui.commandBoxReturn, ui.commandBoxHerb, ui.commandBoxFlower);
                nameBox.GetComponent<Text>().text = "所持数: " + GameManager.instance.haveApple.ToString();
                textBox.GetComponent<Text>().text = "効果: 敵に与えるダメージが 1 増える";
            }
            else if (axisV < 0 && directionCommand != 2 && GameManager.instance.canUseGreen && GameManager.instance.haveHerb > 0)
            {   // 下入力
                SetFirstCommand(2, ui.commandBoxHerb, ui.commandBoxReturn, ui.commandBoxApple, ui.commandBoxFlower);
                nameBox.GetComponent<Text>().text = "所持数: " + GameManager.instance.haveHerb.ToString();
                textBox.GetComponent<Text>().text = "効果: 自分の HP が 3 回復する";
            }
            else if (axisH > 0 && directionCommand != 3 && GameManager.instance.canUseBlue && GameManager.instance.haveFlower > 0)
            {   // 右入力
                SetFirstCommand(3, ui.commandBoxFlower, ui.commandBoxReturn, ui.commandBoxApple, ui.commandBoxHerb);
                nameBox.GetComponent<Text>().text = "所持数: " + GameManager.instance.haveFlower.ToString();
                textBox.GetComponent<Text>().text = "効果: 自分のスピードが 2 倍になる";
            }

            // アイテムを使う
            if (Input.GetButtonDown("Jump"))
            {
                SoundManager.soundManager.PlaySE(SEType.Item);
                haveChosen = true;
                Item(directionCommand);
                InactiveTalkBox();
                ui.InactiveItemCommandImage();
                buttleManager.InactiveItemCommand();

                // アクションモードへ戻る
                StartCoroutine(buttleManager.ActionMode());
            }
        }
    }

    // 選択中の GameObject を強調する
    public void SetFirstCommand(int command, GameObject choose, GameObject other1, GameObject other2, GameObject other3)
    {
        directionCommand = command;
        ui.NoLightenColor(choose);
        ui.LightenColor(other1, light);
        ui.LightenColor(other2, light);
        ui.LightenColor(other3, light);
    }

    void Attack(int directionCommand)
    {
        switch(directionCommand)
        {
            case 0:
                // 黒攻撃
                enemy.GetDamage(player.power + player.plusPower);
                StartCoroutine(ui.AttackEffect(AttributeColor.Black));
                return;
            case 1:
                // 赤攻撃
                GameManager.instance.haveRed--;
                if (enemy.color == AttributeColor.Green)
                {
                    enemy.GetDamage(player.power * 3 + player.plusPower);
                }
                else
                {
                    enemy.GetDamage(player.power + 1 + player.plusPower);
                }
                StartCoroutine(ui.AttackEffect(AttributeColor.Red));
                return;
            case 2:
                // 緑攻撃
                GameManager.instance.haveGreen--;
                if (enemy.color == AttributeColor.Blue)
                {
                    enemy.GetDamage(player.power * 3 + player.plusPower);
                }
                else
                {
                    enemy.GetDamage(player.power + 1 + player.plusPower);
                }
                StartCoroutine(ui.AttackEffect(AttributeColor.Green));
                return;
            case 3:
                // 青攻撃
                GameManager.instance.haveBlue--;
                if (enemy.color == AttributeColor.Red)
                {
                    enemy.GetDamage(player.power * 3 + player.plusPower);
                }
                else
                {
                    enemy.GetDamage(player.power + 1 + player.plusPower);
                }
                StartCoroutine(ui.AttackEffect(AttributeColor.Blue));
                return;
            default:
                return;
        }
    }

    void Item(int directionCommand)
    {
        switch(directionCommand)
        {
            case 1:
                // リンゴ（パワーアップ）を使う
                GameManager.instance.haveApple--;
                GameManager.instance.sumUseItem++;
                player.plusPower = 1;
                return;
            case 2:
                // やくそう（回復）を使う
                GameManager.instance.haveHerb--;
                GameManager.instance.sumUseItem++;
                player.hp += 3;
                // HP は5より大きくはならない
                if (player.hp > 5) player.hp = 5;
                return;
            case 3:
                // はな（スピードバフ）を使う
                GameManager.instance.haveFlower--;
                GameManager.instance.sumUseItem++;
                player.plusSpeed = 3.0f;
                return;
            default:
                // 0 の時は何もしない
                return;
        }
    }

    // talkBox を表示する
    public void ActiveTalkBox()
    {
        ui.ActiveImage(talkBox);
        ui.ActiveImage(nameBox);
        ui.ActiveImage(textBox);
    }

    // talkBox を非表示にする
    public void InactiveTalkBox()
    {
        ui.InactiveImage(talkBox);
        ui.InactiveImage(nameBox);
        ui.InactiveImage(textBox);
    }
}
