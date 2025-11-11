using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Level : MonoBehaviour  
{
    public int levelID;
    public string Name;
    public GameObject Boss;
    public List<SpawnRule> Rules = new List<SpawnRule>();
    public GameObject RuleList;
    public List<GameObject> RulesList;
    private float timeSinceLevelStart;
    private float levelStartTime;
    private float bossTime;
    Boss boss = null;

    public enum LEVEL_RESULT
    { 
        NONE,
        SUCCESS,
        FAILD,
    }
    public LEVEL_RESULT result = LEVEL_RESULT.NONE;
    public UnityAction<LEVEL_RESULT> OnLevelEnd;

    private void Start()
    {
        StartCoroutine(RunLevel());
        if(Rules.Count > 0)
        {
            for (int i = 0; i < Rules.Count; i++)
            {
                SpawnRule rule = Instantiate<SpawnRule>(Rules[i], this.RuleList.transform);
                RulesList.Add(rule.gameObject);
                rule.gameObject.SetActive(true);
            }
        }
        
    }
    IEnumerator RunLevel() 
    {
        UIWorldElementManager.Instance.ShowLevelStart(string.Format("LEVEL. {0}  {1}", Manager.LevelManager.level.levelID, Manager.LevelManager.level.Name));
        yield return new WaitForSeconds(2f);

    }
    private void Update()
    {
        timeSinceLevelStart = Time.realtimeSinceStartup - this.levelStartTime;
        if(this.result != LEVEL_RESULT.NONE)return;
        if(timeSinceLevelStart > bossTime)
        {
            if(boss == null)
            {
                boss = (Boss)Manager.UnitManager.GenerateBoss(Boss);
                boss.OnDeath += Boss_OnDeath;
            }
        }
    }

    private void Boss_OnDeath(Unit sender)
    {
        this.result = LEVEL_RESULT.SUCCESS;

        this.OnLevelEnd?.Invoke(this.result);
        ClearEnemy();
        boss.OnDeath -= Boss_OnDeath;
    }

    public void ClearEnemy()
    {
        for (int i = 0; i < RulesList.Count; i++)
        {
            RulesList[i].SetActive(false);
        }
    }
}
