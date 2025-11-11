using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager Instance { get; private set; }
    public GameObject UILoading;
    public GameObject UILoadingA;
    public GameObject UISetting;
    public GameObject UILogin;
    public GameObject UIFirstGraphic;
    public GameObject UISecond;
    public GameObject UILoadPlayerInfo;
    public GameObject UIFirstPlay;
    public GameObject UIFirst;
    public Slider progressBar;
    public Text progressNumber;
    public GameObject UICreateCharacter;
    public Text skipText;
    private bool isSkipped = false;
    private float skipTextBlinkInterval = 0.5f;
    private float skipTextTimer;


    void Awake()
    {
        // 单例模式初始化
        if (Instance == null)
        {
            Instance = this;
        }
    }
    IEnumerator Start()
    {
        progressNumber.text = progressValue.ToString();
        skipTextTimer = skipTextBlinkInterval;
        DataManager.Instance.Load();
        UISecond.SetActive(false);
        UIFirstPlay.SetActive(false);
        UILoadingA.SetActive(false);
        UIFirst.SetActive(false);
        UIFirstGraphic.SetActive(true);
        UILoadPlayerInfo.SetActive(false);
        UISetting.SetActive(false);
        UILogin.SetActive(false);
        UICreateCharacter.SetActive(false);
        UILoading.SetActive(false);
        yield return new WaitForSeconds(3f);
        UISecond.SetActive(true);
        yield return WaitForKeyPress();
        isSkipped = true;
        if (skipText != null) skipText.enabled = false;
        yield return new WaitForSeconds(1.5f);
        UILoading.SetActive(true);
        yield return new WaitForSeconds(2f);
        UIFirstGraphic.SetActive(false);
        UISecond.SetActive(false);

        // Fake Loading Simulate
        for (float i = 50f; i < 100;)
        {
            i += Random.Range(0.1f, 1.5f);
            progressBar.value = i;
            progressNumber.text = ((int)i).ToString() + "%";
            yield return new WaitForEndOfFrame();
        }

        UILoading.SetActive(false);
        UILogin.SetActive(true);
        yield return null;
        
    }


    [Header("UI References")]
    public Text loadingText;
    //[Header("Settings")]
    //public float loadingDelay = 1f; // 最小加载时间（避免闪屏）

    //private AsyncOperation loadingOperation;
    public float progressValue;
    //public IEnumerator LoadGameAsync(string sceneName, bool isDataLoaded)
    //{

    //    UILoading.SetActive(true);
    //    progressBar.value = 0;
    //    loadingOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
    //    loadingOperation.allowSceneActivation = false;

    //    float timer = 0f;
    //    while (!loadingOperation.isDone || timer < loadingDelay)
    //    {
    //        timer += Time.deltaTime;

    //        // 计算进度（0-0.9是加载进度，0.9-1.0是激活准备）
    //        progressValue = Mathf.Clamp01(loadingOperation.progress / 0.9f) * 0.7f * 100;

    //        // 如果存档已加载，增加进度
    //        if (isDataLoaded)
    //        {
    //            progressValue += 0.3f * 100;
    //        }

    //        // 平滑过渡进度条值
    //        progressBar.value = Mathf.Lerp(progressBar.value, progressValue, Time.deltaTime * 5f);

    //        // 更新加载文本
    //        loadingText.text = GetLoadingText(progressBar.value);

    //        // 当进度接近完成且达到最小加载时间时
    //        if (progressBar.value >= 0.9f * 100 && timer >= loadingDelay)
    //        {
    //            // 在场景切换前隐藏加载界面
    //            UILoading.SetActive(false);

    //            yield return null;
    //            loadingOperation.allowSceneActivation = true;
    //        }
    //        yield return new WaitForEndOfFrame();
    //    }
    //    yield return null;
    //}
    //string GetLoadingText(float progress)
    //{
    //    int dotsCount = Mathf.FloorToInt(Time.time * 2f) % 4;
    //    string dots = new string('.', dotsCount);

    //    string phase = progress < 0.7f * 100 ? "加载地图" : "初始化角色";

    //    return $"{phase}中{dots} ({Mathf.FloorToInt(progress * 100)}%)";
    //}
    private IEnumerator WaitForKeyPress()
    {
        while (!Input.anyKeyDown)
        {
            yield return null;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!isSkipped && skipText != null)
        {
            skipTextTimer -= Time.deltaTime;
            if (skipTextTimer <= 0)
            {
                skipText.enabled = !skipText.enabled;
                skipTextTimer = skipTextBlinkInterval;
            }
        }
    }
}

