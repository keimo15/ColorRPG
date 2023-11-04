using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

public class SaveData : MonoBehaviour
{
    // 保存先
    string datapath;

    void Awake()
    {
        datapath = Application.dataPath + "/SaveData.json";
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
        GameManager.instance.lastMapScene        = "MapGreenTown";
        GameManager.instance.lastPlayerPos       = new Vector2(8.5f, -5.0f);
        GameManager.instance.symbolEnemiesIsDead = new bool[3];
        GameManager.instance.doButtle            = false;

        SavePlayerData(GameManager.instance);
    }

    public bool FindJsonfile()
    {
        string[] assets = AssetDatabase.FindAssets("SaveData");
        Debug.Log(assets.Length);
        if (assets.Length != 0)
        {
            string[] paths = assets.Select(guid => AssetDatabase.GUIDToAssetPath(guid)).ToArray();
            Debug.Log($"検索結果:\n{string.Join("\n", paths)}");
            return true;
        }
        else
        {
            Debug.Log("Jsonファイルが無かった");
            return false;
        }
    }
}
