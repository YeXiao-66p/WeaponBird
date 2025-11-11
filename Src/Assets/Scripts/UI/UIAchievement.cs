using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAchievement : MonoBehaviour
{
    public static UIAchievement Instance { get; private set; }
    public Button exitButton;
    public GameObject UIachievement;
    public Transform Content; // 成就列表父物体
    public GameObject achievementPre; // 成就项预制体
    public Button[] categoryButtons;     // 分类按钮数组
    public int currentProgress;
    public int targetProgress;

    private Category currentCategory = Category.All;

    private void Start()
    {
        // 绑定分类按钮事件
        for (int i = 0; i < categoryButtons.Length; i++)
        {
            Category type = (Category)i;
            categoryButtons[i].onClick.AddListener(() => ShowCategory(type));
        }
        // 默认显示全部成就
        RefreshUI(Category.All);
    }

    // 显示指定分类
    private void ShowCategory(Category type)
    {
        currentCategory = type;
        RefreshUI(type);
    }
    // 刷新UI列表
    private void RefreshUI(Category type)
    {
        // 清空现有项
        foreach (Transform child in Content)
        {
            Destroy(child.gameObject);
        }

        // 获取当前分类成就
        Dictionary<int, AchievementDefine> achievements = AchievementManager.Instance
            .GetAchievementsByType(type);
        if (type == Category.All)
        {
            foreach (var achievement in achievements)
            {
                GameObject item = Instantiate(achievementPre, Content);
                AchievementPre ach = item.GetComponent<AchievementPre>();
                ach.SetUp(achievement.Value);
                ach.Complete.SetActive(false);
                if (achievement.Value.isCompleted == true)
                {
                    ach.Complete.SetActive(true);
                }
            }
        }
        else
        {
            // 动态生成UI项
            foreach (var achievement in achievements)
            {
                GameObject item = Instantiate(achievementPre, Content);
                AchievementPre ach = item.GetComponent<AchievementPre>();
                ach.SetUp(achievement.Value);
                ach.Complete.SetActive(false);
                if (achievement.Value.isCompleted == true)
                {
                    ach.Complete.SetActive(true);
                }
            }
        }

    }

    //private void OnExitButtonClicked()
    //{
    //    Destroy(gameObject);
    //}
}