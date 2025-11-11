using UnityEngine;

public class UILevel : MonoBehaviour
{
    public GameObject LockPanel;
    public GameObject LevelPanel;
    public int level;

    void Start()
    {
        //if(UIGame.Instance.currentLevelId >= this.level)
        //{
        //    this.LockPanel.gameObject.SetActive(false);
        //}
        //else
            //this.LockPanel.gameObject.SetActive(true);
        this.LockPanel.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        //if (UIGame.Instance.currentLevelId >= this.level)
        //{
        //    this.LockPanel.gameObject.SetActive(false);
        //}
        //else
            this.LockPanel.gameObject.SetActive(false);
    }
    public void OnClickButtonPlay()
    {
        //if (UIGame.Instance.currentLevelId >= this.level)
        //{
        //    UIGame.Instance.StartGame(this.level);
        //    this.LevelPanel.gameObject.SetActive(false);
        //}
        //else
        //{
        //    SoundManager.Instance.PlaySound("ui_touch");
        //    var msg = MessageBox.Show("", string.Format("你还没有解锁该关卡哦！"), "确认", MessageBoxType.Confirm);
        //}
        UIGame.Instance.StartGame(this.level);
        this.LevelPanel.gameObject.SetActive(false);

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
