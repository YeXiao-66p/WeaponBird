using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance; // 单例模式
    public GameObject characterPrefab; // 角色的Prefab
    private string saveFilePath;
    public  GameObject characterInstance; // 动态创建的角色实例
    public string currentMap;
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
        // 获取选中的角色信息
        PlayerInfo selectedCharacter = CharacterDataHolder.SelectedCharacter;
        // 创建Prefab并赋值角色信息
        if (selectedCharacter != null)
        {
            CreateCharacterPrefab(selectedCharacter);
         
        }
        else
        {
            Debug.LogWarning("No character selected!");
        }
        
        // 设置保存文件的路径
        saveFilePath = Application.persistentDataPath + "/characterData.json";
    }

    private void CreateCharacterPrefab(PlayerInfo character)
    {
        Vector2 pos = new Vector2(character.x, character.y);
        // 实例化Prefab
        if (this.characterInstance == null)
            this.characterInstance = Instantiate(characterPrefab, pos, Quaternion.identity); ;
            //character.currentMap = "船难海滩";
    }

    public void SaveCharacterInfo()
    {
        if (File.Exists(saveFilePath))
        {
            PlayerInfo selectedCharacter = CharacterDataHolder.SelectedCharacter;
            selectedCharacter.currentMap = this.currentMap;

            AchievementManager.Instance.SaveAchievement();
            SaveLoadManager.Instance.SaveCharacter(selectedCharacter);

        }
    }
    private void OnDestroy()
    {
        SaveCharacterInfo();

        // 在编辑器退出时销毁动态创建的对象
        if (!Application.isPlaying && characterInstance != null)
        {
            SaveCharacterInfo();
        }
    }

    //public void OnApplicationQuit()
    //{
    //    // 在应用程序退出时销毁动态创建的对象
    //    if (characterInstance != null)
    //    {
    //        Destroy(characterInstance);
    //    }
    //}

    [Serializable]
    private class CharacterListWrapper
    {
        public List<PlayerInfo> characterList;
    }
}



