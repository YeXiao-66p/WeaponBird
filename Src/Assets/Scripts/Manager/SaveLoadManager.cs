using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance { get; private set; }
    private string saveFilePath;
    public List<PlayerInfo> characterList = null;
    public bool hasIcon = true;
    private void Awake()
    {
        // 单例模式初始化
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        // 设置保存文件的路径
        saveFilePath = Application.persistentDataPath + "/characterData.json";
        // 加载已有的角色信息
        LoadCharacters();
    }
    public void SavePlayerData()
    {
        //// 获取输入的角色名称
        //string characterName = UILogin.Instance.GetName();
        //string farmName = UILogin.Instance.GetFarmName();
        //string favoriteItem = UILogin.Instance.GetFavoriteItem();
        //string icon = UILogin.Instance.icon;
        //if (string.IsNullOrEmpty(icon))
        //{
        //    var msg = MessageBox.Show("",string.Format("Icon没有选择哦! OvO "), "确认", MessageBoxType.Confirm);
        //    msg.OnYes = () =>
        //    {

        //    };
        //    Debug.LogError("icon不能为空！");
        //    this.hasIcon = false;
        //    return;
        //}
        Dictionary<int, AchievementDefine> allAchievements = new Dictionary<int, AchievementDefine>();
        foreach (var ach in DataManager.Instance.Achievements.Values)
        {
            allAchievements.Add(ach.ID, ach);
        }
        // 创建角色信息
        //PlayerInfo newCharacter = new PlayerInfo(characterName, farmName, favoriteItem, 100f, 100f, 100f, 6, 0, 1, 1, "船难海滩", 0f, 0f, 1, 0, allAchievements, icon, 0);

        //CharacterDataHolder.SelectedCharacter = newCharacter;
        //if (this.characterList == null)
        //{
        //    characterList = new List<PlayerInfo>();
        //    this.characterList.Add(newCharacter);
        //    SaveCharacters(this.characterList);
        //    return;
        //}
        //this.characterList.Add(newCharacter);
        //SaveCharacters(this.characterList);
    }
    public Dictionary<int, AchievementDefine> LoadAchievementData()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            var playerInfo = JsonConvert.DeserializeObject<PlayerInfo>(json);
            return playerInfo.achievements;
        }
        else return null;
    }

    public void SaveCharacters(List<PlayerInfo> characterList)
    {
        CharacterListWrapper wrapper = new CharacterListWrapper(characterList);
        string jsonData = JsonConvert.SerializeObject(wrapper);
        File.WriteAllText(saveFilePath, jsonData);
        Debug.Log("CharacterList data saved to " + saveFilePath);

    }
    public void SaveCharacter(PlayerInfo playerInfo)
    {
        // 加载已有的角色信息
        List<PlayerInfo> characterList = LoadCharactersList();

        // 检查是否已存在同名角色
        bool exists = false;
        for (int i = 0; i < characterList.Count; i++)
        {
            if (characterList[i].playerName == playerInfo.playerName)
            {
                characterList[i] = playerInfo; // 更新角色信息
                exists = true;
                break;
            }
        }

        // 如果角色不存在，添加到列表
        if (!exists)
        {
            characterList.Add(playerInfo);
        }

        SaveCharacters(characterList);
    }
    public List<PlayerInfo> LoadCharactersList()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            Debug.Log("Loaded JSON data: " + json);

            CharacterListWrapper wrapper = JsonConvert.DeserializeObject<CharacterListWrapper>(json);
            return wrapper.characterList;
        }
        else
        {
            Debug.LogWarning("No saved character data found.");
            return new List<PlayerInfo>();
        }
    }
    public void LoadCharacters()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            CharacterListWrapper wrapper = JsonConvert.DeserializeObject<CharacterListWrapper>(json);
            if (this.characterList == null)
            {
                characterList = new List<PlayerInfo>(wrapper.characterList);
            }
            else
            {
                this.characterList = wrapper.characterList;
            }
            Debug.Log("Characters loaded from " + saveFilePath);
        }
        else
        {
            Debug.LogWarning("No saved character data found.");
        }
    }


    [Serializable]
    private class CharacterListWrapper
    {
        public List<PlayerInfo> characterList;

        public CharacterListWrapper(List<PlayerInfo> list)
        {
            characterList = list;
        }
    }
}

