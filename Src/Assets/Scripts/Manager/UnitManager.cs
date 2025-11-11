using System.Collections.Generic;
using UnityEngine;
public class UnitManager : MonoSingleton<UnitManager>
{
    public Stack<Unit> unitStack;   
    public GameObject enemyPar;
    public UIGame game;
    [Header("头顶血条")]
    public Canvas worldCanvas; // Screen Space - Overlay 也可
    public GameObject hpBarPrefab;
    public Boss boss;
    public List<Enemy> enemyList = new List<Enemy>();

    public void Init()
    {
       
    }
    public GameObject player;
    public void OnDamageSkill(int damage)
    {
        if(enemyList != null && enemyList.Count > 0)
        {
            foreach (var enemy in enemyList)
            {
                if (enemy != null)
                {
                    enemy.OnDamage(damage, Const.SIDE.SKILL, true);
                }
            }
        }
        
    }
    public Boss GenerateBoss(GameObject enemyBoss)
    {
        if (enemyBoss == null)
        {
            return null;
        }
        int[] array = new int[100];
        for(int i = 0; i < array.Length; i++)
        if (Manager.Boss != null) return Manager.Boss.GetComponent<Boss>();
        GameObject bs = Instantiate(enemyBoss, this.enemyPar.transform);
        var boss = bs.GetComponent<Boss>();

        boss.playerTarget = this.player;
        this.boss = boss;   
        // 绑定头顶血条
        TryBindHpBar(boss);
        return boss;
    }

    public Enemy GenerateEnemy(GameObject templates)
    {
        GameObject enmy = Instantiate(templates, this.enemyPar.transform);
        var enemy = enmy.GetComponent<Enemy>();
        this.enemyList.Add(enemy);

        // 绑定头顶血条
        TryBindHpBar(enemy);
        return enemy;
    }

    private void TryBindHpBar(Unit unit)
    {
        if (hpBarPrefab == null || worldCanvas == null || unit == null)
        {
            return;
        }
        GameObject bar = Instantiate(hpBarPrefab, worldCanvas.transform);
        HpBar comp = bar.GetComponent<HpBar>();
        if (comp != null)
        {
            comp.Bind(unit);
        }
    }
}
