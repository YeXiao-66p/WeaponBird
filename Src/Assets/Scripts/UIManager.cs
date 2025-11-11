using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject[] uiPanels;  // 存放所有UI面板
    public GameObject openPanel;

    public void CloseAllUI()
    {
        foreach (GameObject panel in uiPanels)
        {
            panel.SetActive(false);  // 关闭所有UI面板
        }
    }
    public void OpenUI()
    {
        if(UIGame.Instance.Status == Const.GAME_STATUS.InGame)
        {
            openPanel = uiPanels[1];
        }
        else
        {
            openPanel = uiPanels[9];
        }
            openPanel.SetActive(true);
    }
}
