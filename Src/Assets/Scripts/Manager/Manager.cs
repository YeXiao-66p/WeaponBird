using UnityEngine;

public class Manager : MonoSingleton<Manager>
{
    private static UnitManager unitManager;
    public static UnitManager UnitManager 
    { 
        get { return unitManager; }
    }

    private static EffectManager effectManager;
    public static EffectManager EffectManager
    {
        get { return effectManager; }
    }


    private static LevelManager levelManager;
    public static LevelManager LevelManager
    {
        get { return levelManager; }
    }
    private static GameObject player;
    public static GameObject Player
    {
        get
        {
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player");
            }
            return player;
        }
    }
    private static GameObject boss;
    public static GameObject Boss
    {
        get
        {
            if (boss == null)
            {
                boss = GameObject.FindGameObjectWithTag("Boss");
            }
            return boss;
        }
        set
        {
            boss = value;
        }
    }
    public static T GetPlayerComponent<T>() where T : Component
    {
        if (player != null)
            return player.GetComponent<T>();
        return null;
    }
    public static T GetBossComponent<T>() where T : Component
    {
        if (boss != null)
            return boss.GetComponent<T>();
        return null;
    }

    public void Init()
    {
        unitManager = this.GetComponent<UnitManager>();
        effectManager = this.GetComponent<EffectManager>();
        levelManager = this.GetComponent<LevelManager>();
    }


}
