using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Const;

public class UIWorldElementManager : MonoSingleton<UIWorldElementManager>
{
    Queue<GameObject> popTextPool = new Queue<GameObject>();
    Queue<GameObject> popTextActivePool = new Queue<GameObject>();
    public GameObject popList;
    public GameObject popupTextPrefab;

    public Slider hpSlide;
    public Slider bossSlide;
    public Slider hpSubSlide;
    public Slider bossSubSlide;
    public Image fillImage;     // 血条填充部分（Slider 的 Fill）
    public Text bsHpText;
    public Text playerHpText;
    public Text bsHPCnt;
    public Text namePhase;
    public Boss bs;
    public bool isInit = false;
    public bool bossInit = false;
    public AchievementManager AchievementManager;
    private int best = 0;
    private int score = 0;
    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            this.curtScore.text = this.Score.ToString();
        }
    }
    public void OnPlayerScore(int score)
    {
        this.Score += score;
        this.AchievementManager.UpdateAchievement(2, 1);
    }

    public GameObject readyPanel;
    public GameObject levelPanel;
    public GameObject gamePanel;
    public GameObject setPanel;
    public GameObject overPanel;
    public GameObject boss;
    public GameObject blackOver;
    public GameObject blackSuccess;
    public GameObject uiLevelStart;
    public GameObject uiLevelEnd;
    public Player player;

    public Slider invincibleTimeSlider; // 无敌时间进度条


    public List<int> ScoreHistory = new List<int>();

    public Text curtScore;
    public Text finalScore;
    public Text bestScore;
    private bool isInvincibleActive = false;
    public Text invincibleTimeText; // 无敌时间文本显示
    public Text levelName;
    public Text levelStartName;
    public GameObject boom;
    void Start()
    {
        popupTextPrefab.SetActive(false);
        this.readyPanel.SetActive(true);
        InitUI();
    }

    public void ShowLevelStart(string name)
    {
        this.levelName.text = name;
        levelStartName.text = name;  
        uiLevelStart.SetActive(true);
        this.isInit = false;
    }

    // Update is called once per frame
    private float pHp;
    private float bHp;
    void Update()
    {
        if (this.popTextPool.Count <= 800)
        {
            GameObject go = Instantiate(popupTextPrefab, this.popList.transform);
            go.SetActive(false);
            this.popTextPool.Enqueue(go);
            //this.popTextActivePool.Enqueue(go);
        }
        if (!this.isInvincibleActive)
        {
            InitUI();
        }
        if (Manager.Boss != null && !bossInit)
        {
            this.bs = Manager.GetBossComponent<Boss>();
            bossInit = true;
        }
        if(bs == null)
        {
            this.bs = Manager.GetBossComponent<Boss>();
        }

        this.hpSlide.maxValue = player.HPMax;
        this.hpSubSlide.maxValue = player.HPMax;
        this.hpSubSlide.value = Mathf.Lerp(this.hpSubSlide.value, UIGame.Instance.player.HP, 0.6f * Time.deltaTime);

        if(this.pHp != this.player.HP)
        {
            this.pHp = this.player.HP;
            this.hpSlide.value = this.pHp;
            this.playerHpText.text = this.pHp.ToString() + " / " + this.player.HPMax.ToString();
        }

        if (this.bs != null && !isInit)
        {
            this.bsHPCnt.text = "X" + (bs.Phases.Count - bs.currentPhaseIndex).ToString();
            this.namePhase.text = this.bs.Phases[bs.currentPhaseIndex].PhaseName;
            this.fillImage.color = this.bs.Phases[bs.currentPhaseIndex].HPColor;
            isInit = true;
        }

        if(bs==null) return;

        this.bossSubSlide.value = Mathf.Lerp(this.bossSubSlide.value, this.bs.HP, 0.6f * Time.deltaTime);
        if (bs.currentPhaseIndex <= 14)
        {
            this.bossSubSlide.maxValue = this.bs.Phases[bs.currentPhaseIndex].HPMax;
            this.bossSlide.maxValue = this.bs.Phases[bs.currentPhaseIndex].HPMax;
            if(this.bHp != this.bs.HP)
            {
                this.bHp = this.bs.HP;
                this.bossSlide.value = this.bHp;
                this.bsHpText.text = ((int)this.bHp).ToString() + " / " + this.bs.Phases[bs.currentPhaseIndex].HPMax.ToString();
            }

        }

       

        //if (this.Score % 50 == 0)
        //{
        //    UIGame.Instance.player.AddPower();
        //    Debug.Log("AddPower + 5");
        //}
            

    }
    public void OnGameOver()
    {
        this.finalScore.text = this.Score.ToString();
        blackOver.SetActive(true);
        blackSuccess.SetActive(false);

        this.ScoreHistory.Add(this.Score);
        int best = 0;
        foreach (int res in ScoreHistory)
        {
            if (res >= best) best = res;
        }
        this.bestScore.text = best.ToString();
        //this.boss.GetComponent<Boss>().Init();
        // 隐藏无敌时间UI
        InitUI();
    }
    public void OnGameSuc()
    {
        
        blackSuccess.SetActive(true);
        blackOver.SetActive(false);
        this.OnPlayerScore(200);
        this.finalScore.text = this.Score.ToString();
        this.ScoreHistory.Add(this.Score);

        if(this.best < this.Score) 
            this.best = this.Score; 
        this.bestScore.text = this.best.ToString();
        // 隐藏无敌时间UI
        InitUI();
    }
    /// <summary>
    /// 处理无敌时间更新
    /// </summary>
    /// <param name="remainingTime">剩余无敌时间</param>
    /// <param name="totalTime">总无敌时间</param>
    public void OnInvincibleTimeUpdate(float remainingTime, float totalTime)
    {
        isInvincibleActive = remainingTime > 0;

        // 更新进度条
        if (invincibleTimeSlider != null)
        {
            invincibleTimeSlider.gameObject.SetActive(isInvincibleActive);
            if (isInvincibleActive)
            {
                // 计算进度值（0-1）
                float progress = Mathf.Clamp01(remainingTime / totalTime) * 100;
                invincibleTimeSlider.value = progress;
            }
        }

        // 更新文本显示
        if (invincibleTimeText != null)
        {
            invincibleTimeText.gameObject.SetActive(isInvincibleActive);
            if (isInvincibleActive)
            {
                // 格式化时间显示，保留1位小数
                invincibleTimeText.text = string.Format("无敌time: {0:F1}s", remainingTime);
            }
        }
    }
    private void InitUI()
    {
        // 初始化无敌时间UI元素
        if (invincibleTimeSlider != null)
        {
            invincibleTimeSlider.gameObject.SetActive(false);
            invincibleTimeSlider.value = 0;
        }
        if (invincibleTimeText != null)
        {
            invincibleTimeText.gameObject.SetActive(false);
            invincibleTimeText.text = "";
        }
    }
    public void UpdateUI()
    {
        this.readyPanel.SetActive(UIGame.Instance.Status == GAME_STATUS.Ready);
        this.gamePanel.SetActive(UIGame.Instance.Status == GAME_STATUS.InGame);
        this.overPanel.SetActive(UIGame.Instance.Status == GAME_STATUS.GameOver);
        this.levelPanel.SetActive(UIGame.Instance.Status == GAME_STATUS.SelectLevel);
        
    }


    public int x;
    public int y;
    public void ShowPopupText(Vector3 pos, float damage, Const.SIDE side, bool isCrit, bool isSkill)
    {
        //GameObject goPopup = Instantiate(popupTextPrefab, new Vector3(x + pos.x, y +pos.y,0), Quaternion.identity, this.transform);
        //if(this.popTextActivePool.Count >= 0)
        //{
        //    //GameObject goPopup = this.popTextActivePool.Dequeue();
        //    GameObject goPopup = this.popTextPool.Dequeue();
        //    goPopup.transform.position = new Vector3(x + pos.x, y + pos.y, 0);
        //    //goPopup.transform.LookAt(Camera.main.transform);
        //    goPopup.name = "Popup";
        //    goPopup.GetComponent<UIPopupText>().InitPopup(damage, side, isCrit, isSkill);
        //    goPopup.SetActive(true);
        //    this.popTextPool.Enqueue(goPopup);
        //}

        GameObject goPopup = this.popTextPool.Dequeue();
        if (goPopup != null)
        {
            goPopup.transform.position = new Vector3(x + pos.x, y + pos.y, 0);
            //goPopup.transform.LookAt(Camera.main.transform);
            goPopup.name = "Popup";
            goPopup.GetComponent<UIPopupText>().InitPopup(damage, side, isCrit, isSkill);
            goPopup.SetActive(true);
            this.popTextPool.Enqueue(goPopup);
        }
       
    }
    public void OnStartPhase()
    {
        this.bsHPCnt.text = "X" + (bs.Phases.Count - bs.currentPhaseIndex).ToString();
        this.fillImage.color = this.bs.Phases[bs.currentPhaseIndex].HPColor;
        this.namePhase.text = this.bs.Phases[bs.currentPhaseIndex].PhaseName;

    }



    public void OnClickButtonExit()
    {
        SoundManager.Instance.PlaySound("ui_touch");
#if UNITY_EDITOR
        var msg = MessageBox.Show("", string.Format("你要离开了吗QAQ, 感谢您的体验"), "确认", MessageBoxType.Confirm);
        msg.OnYes = () =>
        {
            UnityEditor.EditorApplication.isPlaying = false;
        };
        return;

#else
        var msg = MessageBox.Show("",string.Format("你要离开了吗QAQ, 感谢您的体验 "), "ȷ��", MessageBoxType.Confirm);
        msg.OnYes = () =>
        {
             Application.Quit();
        };
        return;
       
#endif

    }
}
