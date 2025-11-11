using UnityEngine;
using static Const;

public class UIGame : MonoSingleton<UIGame>
{
    internal int currentLevelId = 1;
    public Player player;
    public PetFollow pet;
    public Manager Manager;
    public float time;
    private GAME_STATUS status;
    public GAME_STATUS Status
    {
        get { return status; }
        set
        {
            status = value;
            UIWorldElementManager.Instance.UpdateUI();
        }
    }
    void Start()
    {
        Manager.Init();
        this.Status = GAME_STATUS.Ready;
        PlayerInit();
    }
    public void StartGame(int levelId)
    {
        this.Status = GAME_STATUS.InGame;
        player.Fly();
        this.player.inputMode = true;
        this.pet.isInput = true;
        this.player.isFly = true;
        LoadLevel(levelId);
    }

    public void LoadLevel(int levelId)
    {
        UIWorldElementManager.Instance.Score = 0;
        UIWorldElementManager.Instance.player.HP = 600;
        this.player.isAI = false;
        Manager.LevelManager.LoadLevel(levelId);
        Manager.LevelManager.level.OnLevelEnd = OnLevelEnd;
    }
    private void OnLevelEnd(Level.LEVEL_RESULT result)
    {
        //if (result == Level.LEVEL_RESULT.SUCCESS && this.currentLevelId < Manager.LevelManager.levels.Count)
        //{
        //    this.currentLevelId++;
        //    this.LoadLevel(this.currentLevelId);
        //}
        //else if (this.currentLevelId == Manager.LevelManager.levels.Count && result == Level.LEVEL_RESULT.SUCCESS)
        //{
        //    this.Status = GAME_STATUS.GameOver;
        //    this.player.inputMode = false;
        //    this.pet.isInput = false;
        //    UIWorldElementManager.Instance.OnGameSuc();
        //}
        this.currentLevelId += 1;
        this.Status = GAME_STATUS.GameOver;
        this.player.inputMode = false;
        this.pet.isInput = false;
        this.player.isFly = false;
        if (this.player.petSkill != null)
        {
            StopCoroutine(this.player.petSkill);
        }
        UIWorldElementManager.Instance.OnGameSuc();
    }
    private void PlayerInit()
    {
        this.player.OnDeath += this.GameOver;
        this.player.OnScore += UIWorldElementManager.Instance.OnPlayerScore;
        this.player.OnInvincibleTimeUpdate += UIWorldElementManager.Instance.OnInvincibleTimeUpdate;
    }


    public void Restart()
    {
        this.player.Init();

        UIWorldElementManager.Instance.Score = 0;
        Manager.UnitManager.enemyList.Clear();
        if (Manager.Boss != null)
        {
            Destroy(Manager.Boss);
            Manager.Boss = null;    
        }
        this.StartGame(this.currentLevelId);
    }
    public void ReturnSelect()
    {
        this.Status = GAME_STATUS.SelectLevel;
    }
    public void ReturnHome()
    {
        if(this.Status == GAME_STATUS.InGame)
        {
            UIWorldElementManager.Instance.Score = 0;
            Manager.UnitManager.enemyList.Clear();
            if (Manager.Boss != null)
            {
                Destroy(Manager.Boss);
                Manager.Boss = null;
            }
            this.player.inputMode = false;
            this.pet.isInput = false;
            if(this.player.petSkill != null)
            {
                StopCoroutine(this.player.petSkill);
            }
            Manager.LevelManager.ClearLevel();
        }
        this.Status = GAME_STATUS.Ready;

    }
    public void GameOver(Unit sender)
    {
        this.Status = GAME_STATUS.GameOver;
        this.player.inputMode = false;
        this.pet.isInput = false;
        UIWorldElementManager.Instance.bs.ClearBoss();
        Manager.LevelManager.level.ClearEnemy();
        Manager.LevelManager.ClearLevel();
        UIWorldElementManager.Instance.OnGameOver();
    }
}

