using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
public class DataManager : Singleton<DataManager>
{
    public string DataPath { get; private set; }
    public Dictionary<int, AchievementDefine> Achievements { get; private set; }

    public DataManager()
    {
        DataPath = "Assets/Data/";
        Load();
    }

    public void Load()
    {
        // 加载成就数据
        LoadAchievements();

        //path = Application.persistentDataPath + "/ToolbarData.json";
        //if (File.Exists(path))// 存在文件
        //{
        //    string json = File.ReadAllText(path);
        //    this.toolbarData = JsonConvert.DeserializeObject<InventoryDataJson>(json);
        //}
        //else
        //{
        //    toolbarData = new InventoryDataJson(Resources.Load<InventoryData>("SavedData/Toolbar"));
        //}
        //path = Application.persistentDataPath + "/BackpackData.json";
        //if (File.Exists(path))// 存在文件
        //{
        //    string json = File.ReadAllText(path);
        //    this.backpackData = JsonConvert.DeserializeObject<InventoryDataJson>(json);
        //}
        //else
        //{
        //    backpackData = new InventoryDataJson(Resources.Load<InventoryData>("SavedData/Backpack"));
        //}
    }
    void LoadAchievements()
    {
        string path = Path.Combine(DataPath, "AchievementDefine.txt");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Achievements = JsonConvert.DeserializeObject<Dictionary<int, AchievementDefine>>(json);
        }
        else
        {
            Debug.LogError($"成就文件未找到: {path}");
            Achievements = new Dictionary<int, AchievementDefine>();
        }
    }
}
