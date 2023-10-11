using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerCommand : MonoBehaviour
{
    public GameObject nameBox;
    public GameObject textBox;
    public GameObject talkBox;

    [SerializeField] GameManager gameManager;
    [SerializeField] UIManager ui;
    [SerializeField] EnemyController enemy;
    [SerializeField] PlayerController player;

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
        
        if (GameManager.gameState != GameState.Command && GameManager.gameState != GameState.UseItem)
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
        if (GameManager.gameState == GameState.Command)
        {
            if (axisV > 0 && directionCommand != 0)
            {   // 上入力
                SetFirstCommand(0, ui.commandBoxBlack, ui.commandBoxRed, ui.commandBoxGreen, ui.commandBoxBlue);
            }
            else if (axisH < 0 && directionCommand != 1 && PlayerController.canUseRed && PlayerController.haveRed > 0)
            {   // 左入力
                SetFirstCommand(1, ui.commandBoxRed, ui.commandBoxBlack, ui.commandBoxGreen, ui.commandBoxBlue);
            }
            else if (axisV < 0 && directionCommand != 2 && PlayerController.canUseGreen && PlayerController.haveGreen > 0)
            {   // 下入力
                SetFirstCommand(2, ui.commandBoxGreen, ui.commandBoxBlack, ui.commandBoxRed, ui.commandBoxBlue);
            }
            else if (axisH > 0 && directionCommand != 3 && PlayerController.canUseBlue && PlayerController.haveBlue > 0)
            {   // 右入力
                SetFirstCommand(3, ui.commandBoxBlue, ui.commandBoxBlack, ui.commandBoxRed, ui.commandBoxGreen);
            }

            // 攻撃
            if (Input.GetButtonDown("Jump"))
            {
                haveChosen = true;
                Attack(directionCommand);
                // バフのリセット
                player.plusPower = 0;
                player.plusSpeed = 0;
                gameManager.ActiveItemCommand();
                ui.InactiveCommandImage();

                // 敵を倒した
                if (enemy.hp <= 0)
                {
                    ui.InactiveImage(ui.stopEnemy);
                    if (enemy.isSymbol)
                    {
                        // 連戦があるなら次のシーンを読み込む
                        if (enemy.thereIsNext)
                        {
                            SceneManager.LoadScene(enemy.nextButtleSceneName);
                            return;
                        }
                        // 連戦がないなら敵を討伐リストに加える
                        else
                        {
                            PlayerMap.KillSymbolEnemy(enemy.name);
                        }
                    }
                    if (PlayerMap.lastScene != null)
                    {
                        gameManager.RewardMode();
                    }
                }
                // 敵をまだ倒していない
                else
                {
                    gameManager.ActionMode();
                }
            }
        }

        // アイテムを選択
        else if (GameManager.gameState == GameState.UseItem)
        {
            if (axisV > 0 && directionCommand != 0)
            {   // 上入力
                SetFirstCommand(0, ui.commandBoxReturn, ui.commandBoxApple, ui.commandBoxHerb, ui.commandBoxFlower);
                nameBox.GetComponent<Text>().text = "";
                textBox.GetComponent<Text>().text = "アイテムを使わずに戻る";
            }
            else if (axisH < 0 && directionCommand != 1 && PlayerController.canUseRed && PlayerController.haveApple > 0)
            {   // 左入力
                SetFirstCommand(1, ui.commandBoxApple, ui.commandBoxReturn, ui.commandBoxHerb, ui.commandBoxFlower);
                nameBox.GetComponent<Text>().text = "所持数: " + PlayerController.haveApple.ToString();
                textBox.GetComponent<Text>().text = "効果: 敵に与えるダメージが 1 増える";
            }
            else if (axisV < 0 && directionCommand != 2 && PlayerController.canUseGreen && PlayerController.haveHerb > 0)
            {   // 下入力
                SetFirstCommand(2, ui.commandBoxHerb, ui.commandBoxReturn, ui.commandBoxApple, ui.commandBoxFlower);
                nameBox.GetComponent<Text>().text = "所持数: " + PlayerController.haveHerb.ToString();
                textBox.GetComponent<Text>().text = "効果: 自分の HP が 3 回復する";
            }
            else if (axisH > 0 && directionCommand != 3 && PlayerController.canUseBlue && PlayerController.haveFlower > 0)
            {   // 右入力
                SetFirstCommand(3, ui.commandBoxFlower, ui.commandBoxReturn, ui.commandBoxApple, ui.commandBoxHerb);
                nameBox.GetComponent<Text>().text = "所持数: " + PlayerController.haveFlower.ToString();
                textBox.GetComponent<Text>().text = "効果: 自分のスピードが 2 倍になる";
            }

            // アイテムを使う
            if (Input.GetButtonDown("Jump"))
            {
                haveChosen = true;
                Item(directionCommand);
                InactiveTalkBox();
                ui.InactiveItemCommandImage();
                gameManager.InactiveItemCommand();

                // アクションモードへ戻る
                gameManager.ActionMode();
            }
        }
    }

    // 2つ目の引数の GameObject を強調する
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
                enemy.GetDamage(PlayerController.power + player.plusPower);
                return;
            case 1:
                // 赤攻撃
                PlayerController.haveRed--;
                if (enemy.color == AttributeColor.Green)
                {
                    enemy.GetDamage(PlayerController.power * 3 + player.plusPower);
                }
                else
                {
                    enemy.GetDamage(PlayerController.power + player.plusPower);
                }
                return;
            case 2:
                // 緑攻撃
                PlayerController.haveGreen--;
                if (enemy.color == AttributeColor.Blue)
                {
                    enemy.GetDamage(PlayerController.power * 3 + player.plusPower);
                }
                else
                {
                    enemy.GetDamage(PlayerController.power + player.plusPower);
                }
                return;
            case 3:
                // 青攻撃
                PlayerController.haveBlue--;
                if (enemy.color == AttributeColor.Red)
                {
                    enemy.GetDamage(PlayerController.power * 3 + player.plusPower);
                }
                else
                {
                    enemy.GetDamage(PlayerController.power + player.plusPower);
                }
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
                PlayerController.haveApple--;
                player.plusPower = 1;
                return;
            case 2:
                // やくそう（回復）を使う
                PlayerController.haveHerb--;
                PlayerController.hp += 3;
                // HP は5より大きくはならない
                if (PlayerController.hp > 5) PlayerController.hp = 5;
                return;
            case 3:
                // はな（スピードバフ）を使う
                PlayerController.haveFlower--;
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
