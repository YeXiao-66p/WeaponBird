[System.Serializable]
public enum Category
{
    All,        // 全部
    Fight,     // 战斗
    Collection  // 收集
}
[System.Serializable]
public class AchievementDefine
{

    public string playerName;
    public string AchievementIcon;
    public int ID;// 成就的唯一ID
    public string Name;// 成就名称
    public string Description; // 成就描述
    public string TextReward;
    public Category Category;
    public bool isUnlocked ;
    public bool isCompleted;
    public int CurrentProgress;
    public int RewardGold;
    public int RewardExp;
    //[NonSerialized]
    public int TargetProgress; // 目标进度

}

