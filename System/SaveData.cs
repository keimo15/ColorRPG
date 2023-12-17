using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class SaveData : MonoBehaviour
{
    // 保存先
    string datapath;

    void Awake()
    {
        datapath = Application.persistentDataPath + "/SaveData.json";
    }

    public void Continue()
    {
        if (FindJsonfile())
        {
            LoadPlayerData();
        }
        else
        {
            Initialize();
        }
    }

    public void SavePlayerData(GameManager gameManager)
    {
        StreamWriter writer;

        string json = JsonUtility.ToJson(gameManager);

        writer = new StreamWriter(datapath, false);
        writer.Write(json);
        writer.Flush();
        writer.Close();

        Debug.Log("保存した");
    }

    public void LoadPlayerData()
    {
        string data = "";
        StreamReader reader;
        reader = new StreamReader(datapath);
        data = reader.ReadToEnd();
        reader.Close();

        JsonUtility.FromJsonOverwrite(data, GameManager.instance);
    }

    public void Initialize()
    {
        GameManager.instance.gameState           = GameState.Map;
        GameManager.instance.haveGold            = 0;
        GameManager.instance.canUseGreen         = false;
        GameManager.instance.canUseBlue          = false;
        GameManager.instance.canUseRed           = false;
        GameManager.instance.haveGreen           = 0;
        GameManager.instance.haveBlue            = 0;
        GameManager.instance.haveRed             = 0;
        GameManager.instance.haveApple           = 0;
        GameManager.instance.haveHerb            = 0;
        GameManager.instance.haveFlower          = 0;
        GameManager.instance.canJump             = false;
        GameManager.instance.canWalk             = false;
        GameManager.instance.canPunch            = false;
        GameManager.instance.sumGetDamage        = 0;
        GameManager.instance.sumUseItem          = 0;
        GameManager.instance.lastMapScene        = "MapGreenTown";
        GameManager.instance.lastPlayerPos       = new Vector2(8.5f, -5.0f);
        GameManager.instance.symbolEnemiesIsDead = new bool[4];
        GameManager.instance.doButtle            = false;

        SavePlayerData(GameManager.instance);
    }

    public bool FindJsonfile()
    {
        return File.Exists(datapath);
    }
}
