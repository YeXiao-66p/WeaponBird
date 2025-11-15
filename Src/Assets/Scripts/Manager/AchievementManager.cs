using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AchievementManager : MonoSingleton<AchievementManager>
{
    //public Dictionary<int, AchievementDefine> allAchievements = CharacterDataHolder.SelectedCharacter.achievements;
    public Dictionary<int, AchievementDefine> allAchievements;
    private AchievementDefine achievement;

    private void Awake()
    {
        if (this.allAchievements == null)
        {
            this.allAchievements = new Dictionary<int, AchievementDefine>();
            foreach (var ach in DataManager.Instance.Achievements.Values)
            {
                allAchievements.Add(ach.ID, ach);
            }
        }
        if(CharacterDataHolder.SelectedCharacter != null)
        {
            foreach (var a in this.allAchievements)
            {
                a.Value.playerName = CharacterDataHolder.SelectedCharacter.playerName;
            }
        }
        
    }


    // 从JSON/ScriptableObject初始化

    // 按类型筛选成就
    public Dictionary<int, AchievementDefine> GetAchievementsByType(Category type)
    {
        if (type == Category.All) return allAchievements;

        return allAchievements
            .Where(ach => ach.Value.Category == type)
            .ToDictionary(ach => ach.Key, ach => ach.Value);
    }

    // 更新成就进度
    public void UpdateAchievement(int id, int progress)
    {
        //PlayerInfo playerInfo = CharacterDataHolder.SelectedCharacter;

        if (allAchievements.TryGetValue(id, out achievement))
        //foreach (var achievement in allAchievements)
        {
            if (achievement != null && achievement.ID == id && !achievement.isCompleted)
            {
                //成就进度增加

                achievement.CurrentProgress = Mathf.Min(achievement.CurrentProgress + progress,
                    achievement.TargetProgress);

                //成就完成
                if (achievement.CurrentProgress == achievement.TargetProgress)
                {
                    AchievementReward(achievement);
                }
            }
            SaveAchievement();
        }

    }
    //}


    public void AchievementReward(AchievementDefine achievement)
    {
        //完成
        achievement.isCompleted = true;


        ////奖励
        //AttributeChangeController.Instance.UpdateExpCoin(achievement.RewardExp, achievement.RewardGold);

        var msg = MessageBox.Show(achievement.AchievementIcon, string.Format(achievement.Name), null, MessageBoxType.notice);
        SaveAchievement();

    }
    public void SaveAchievement()
    {
        PlayerInfo selectedCharacter = CharacterDataHolder.SelectedCharacter;

        if (selectedCharacter != null)
        {
            if (selectedCharacter.achievements == null)
            {
                selectedCharacter.achievements = new Dictionary<int, AchievementDefine>();

            }
            selectedCharacter.achievements = this.allAchievements;
        }
        

    }

}