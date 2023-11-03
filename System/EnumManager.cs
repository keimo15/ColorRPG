using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumManager : MonoBehaviour{}

// ゲームの状態
public enum GameState
{
    Action,
    Command,
    UseItem,
    Reward,
    Map,
    Talking,
    GameOver,
    Title,
    HowToPlay,
}

// マップのシーン名
public enum MapSceneName
{
    MapBlueTown,
    MapBlueIsland,
    MapBoss,
    MapFirstRoad,
    MapGreenTown,
    MapGreenForest,
    MapRedTown,
    MapRedCave1,
    MapRedCave2,
    MapRedCave3,
    MapSecondRoad,
    MapThirdRoad,
    MapWhiteTown,
}

// 敵の名前
public enum Enemy
{
    BlueCat,
    BlueSlime,
    BlueSoccerFish,
    CaveBoss,
    ForestBoss,
    GreenDevil,
    GreenSlime,
    GreenWood,
    IslandBoss,
    RedBat,
    RedDragon,
    RedSlime,
    RedWizard,
    Slime,
    LastBoss,
}

// 能力
public enum Ability
{
    Jump,
    Walk,
    Punch,
}

// 属性色
public enum AttributeColor
{
    Black,
    Red,
    Blue,
    Green,
}

// アイテム
public enum Item
{
    Apple,
    Herb,
    Flower,
}

// 方向
public enum Direction
{
    Right,
    Left,
    Up,
    Down,
}

// 描画状態
public enum DrawState
{
    Draw,
    Exist,
    Blink,
    Clear,
}

