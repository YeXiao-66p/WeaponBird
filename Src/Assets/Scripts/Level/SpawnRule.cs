using UnityEngine;

public class SpawnRule : MonoBehaviour
{
    public Unit Monster;
    public float InitTime;
    public float Period;
    public int MaxNum;

    public int HP;
    public int Attack;
   

    float timeSinceLevelStart = 0;  
    float levelStartTime = 0;

    int num = 0;

    float timer = 0;
    public ItemDropRule dropRule;
    ItemDropRule rule;
    private void Start()
    {
        this.levelStartTime = Time.realtimeSinceStartup;

        if(dropRule != null ) 
            rule = Instantiate<ItemDropRule>(dropRule);
    }
    private void Update()
    {
        timeSinceLevelStart = Time.realtimeSinceStartup - this.levelStartTime;

        if(num >= MaxNum )return;
        if(timeSinceLevelStart > InitTime)
        {
            //开始刷怪
            timer += Time.deltaTime;
            if(timer >= Period)
            {
                timer = 0;
                Enemy enemy = Manager.UnitManager.GenerateEnemy(this.Monster.gameObject);
                enemy.HPMax = this.HP;
                enemy.power = this.Attack;
                enemy.OnDeath += Enemy_OnDeath;
                num++;
            }
        }
    }

    private void Enemy_OnDeath(Unit sender)
    {
        if(rule != null)
            rule.Execute(sender.transform.position);
    }
}