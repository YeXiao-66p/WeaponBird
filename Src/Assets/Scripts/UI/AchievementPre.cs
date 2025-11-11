using UnityEngine;
using UnityEngine.UI;

public class AchievementPre : MonoBehaviour
{
    public static AchievementPre Instance;
    public GameObject Description;
    public GameObject Complete;
    public Button exitButton;
    public Button descriptionButton;
    public Slider sliderProgress;
    public Text targetProgress;
    //public Text textProgressPercent;
    public Text textReward;
    public Text description;
    public Text achName;
    public Text currentProgress;
    public Text progressPercent;
    public Category category;
    public Image achievementIcon;
    public int index;
    public string playerName;


    // Start is called before the first frame update
    void Awake()
    {
        if(Instance == null)
            Instance = this;
        achievementIcon = transform.Find("Icon").GetComponent<Image>();
        textReward = transform.GetChild(1).GetComponent<Text>();
        sliderProgress = transform.GetChild(2).GetComponent<Slider>();
        targetProgress = transform.GetChild(3).GetComponent<Text>();
        currentProgress = transform.GetChild(4).GetComponent<Text>();
        progressPercent = transform.GetChild(6).GetComponent<Text>();
        Description = transform.Find("Description").gameObject;
        description = transform.Find("Description/Bgi/Content").GetComponent<Text>();
        achName = transform.GetChild(7).GetComponent<Text>();
        this.exitButton.onClick.AddListener(OnExitButtonClicked);
        this.descriptionButton.onClick.AddListener(OnDescriptionButtonClicked);
    }
    public void SetUp(AchievementDefine def)
    {
        this.achievementIcon.overrideSprite = Resources.Load<Sprite>(def.AchievementIcon);
        this.targetProgress.text = def.TargetProgress.ToString() + " / ";
        this.currentProgress.text = def.CurrentProgress.ToString();
        this.textReward.text = def.TextReward;
        this.description.text = def.Description;
        this.achName.text = def.Name;
        this.category = def.Category;
        this.index = def.ID;
        this.progressPercent.text = (def.CurrentProgress * 1.0 / def.TargetProgress * 100).ToString() + "%";
        if (def.CurrentProgress == 100) this.progressPercent.text = "100%";
        sliderProgress.value = (float)def.CurrentProgress / def.TargetProgress;
        this.Description .SetActive(false);
        this.playerName = def.playerName;

    }
    private void OnDescriptionButtonClicked()
    {
        this.Description.SetActive(true);
    }

    private void OnExitButtonClicked()
    {
        this.Description.SetActive(false);
    }

}
