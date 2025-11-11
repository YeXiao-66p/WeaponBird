// 单独的设置菜单控制器
using UnityEngine;
public class SettingMenuController : MonoBehaviour
{
    public GameObject settingsPanel;

    void Start()
    {
        // 注册事件
        GameStateManager.Instance.OnGamePaused.AddListener(ShowSettings);
        GameStateManager.Instance.OnGameResumed.AddListener(HideSettings);

        // 初始隐藏设置面板
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    public void OpenSettings()
    {
        GameStateManager.Instance.PauseGame();
    }

    public void CloseSettings()
    {
        SoundManager.Instance.PlaySound("ui_touch");
        GameStateManager.Instance.ResumeGame();
    }

    public void ShowSettings()
    {
        SoundManager.Instance.PlaySound("ui_touch");
        if (settingsPanel != null)
            settingsPanel.SetActive(true);
        this.OpenSettings();
    }


    public void HideSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
        this.CloseSettings();
    }

    void OnDestroy()
    {
        // 取消注册事件
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnGamePaused.RemoveListener(ShowSettings);
            GameStateManager.Instance.OnGameResumed.RemoveListener(HideSettings);
        }
    }
}