using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GameData
{
    public PlayerInfo characterData;
    public Dictionary<int, AchievementDefine> achievementData;
}

[System.Serializable]
public class PlayerInfo
{

    public string playerName;
    public string farmName;
    public string favoriteItem;
    [JsonIgnore] public Vector2 Position;
    public float x;
    public float y;
    public string currentMap;

    public int level;
    public int exp;
    public int coin;
    public bool isFirst = true;
    public Dictionary<int, AchievementDefine> achievements; // 角色的成就列表
    public PlayerInfo(string playerName, string farmName, string favoriteItem, float currentHealth, float currentEndurance, float currentHunger, int currentTime_Hours, int currentTime_Minutes,int currentDay, int seasonType, string currentMap, float x, float y, int level, int coin, Dictionary<int, AchievementDefine> achs, string icon, int exp)
    {
        this.playerName = playerName;
        this.farmName = farmName;
        this.favoriteItem = favoriteItem;
        this.currentMap = currentMap;
        this.x = x;
        this.y = y;
        this.Position = new Vector2(x, y);
        this.level = level;
        this.coin = coin;
        this.achievements = achs;
        this.exp = exp;
    }
}