using System;
using UnityEngine;
using static Const;

public class Enemy : Unit
{
    public ENEMY_TYPE enemyType;
    public float lifeTime = 5f;
    public int scoreGet = 2;
    public GameObject playerTarget;
    public Player player;

    float y;

    public Transform effect;

    private Action<int> onEnemyDefeated;
    public Action<Vector3, float> OnDamagePop;
    public Action<int> OnScore;
    public float offx;
    public float offy;

    public override void OnStart()
    {
        this.OnDeath += this.OnEnemyDeath;
        this.onEnemyDefeated += UIWorldElementManager.Instance.OnPlayerScore;
        y =  UnityEngine.Random.Range(1.2f, 10f);
        Destroy(gameObject, lifeTime);
        this.Fly();
        this.transform.localPosition += new Vector3(0, y, 0);
        if(Manager.Player!=null) this.playerTarget = Manager.Player;
        if (this.playerTarget != null) this.player = Manager.Player.GetComponent<Player>();

    }

    private void OnEnemyDeath(Unit sender)
    {
        // 通知外部系统
        if (onEnemyDefeated != null)
        {
            onEnemyDefeated.Invoke(this.scoreGet);
        }
    }
    private void OnDisable()
    {
        this.OnDeath -= this.OnEnemyDeath;
        this.onEnemyDefeated -= UIWorldElementManager.Instance.OnPlayerScore;
    }
    public override void OnUpdate()
    {
        float z = 0;
        if (this.enemyType == ENEMY_TYPE.SWING_ENEMY)
        {
            z = Mathf.Sin(Time.timeSinceLevelLoad) * 3f;
        }
       this.transform.position = new Vector3(this.transform.position.x - Time.deltaTime * speed, y + z);
        if (fireTimer >= 1 / fireRate) this.Fire();
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        Element bullet = collision.gameObject.GetComponent<Element>();
        if (bullet == null) return;
        if (bullet.side == SIDE.PLAYER)
        {
            if (bullet.isSkill)
            {
                if(bullet.isPet)
                {
                    this.player.pet.FireScatter360(this.transform.position);
                    this.OnDamage(bullet.power, bullet.side, true);
                    bullet.gameObject.SetActive(false);
                    return;
                }
                if (bullet.isBig)
                {
                    GameObject go = Instantiate(UIWorldElementManager.Instance.boom, UIWorldElementManager.Instance.gamePanel.transform);
                    SoundManager.Instance.PlaySound(SoundDefine.AttackStar);
                    go.transform.position = new Vector3(offx * this.transform.position.x, offy * this.transform.position.y, this.transform.position.z);
                    Destroy(go, 0.6f);
                }
                
                this.player.FireScatter360(this.transform.position);
                if(GameUtil.Random.NextDouble() < 0.5)
                {
                    SoundManager.Instance.PlaySound(SoundDefine.CoinDrop);
                }
                this.OnDamage(bullet.power, bullet.side, true);
                bullet.gameObject.SetActive(false);
                return;
            }
            if (bullet.isSkillDiv)
            {
                this.OnDamage(bullet.power, bullet.side, false);
                return;
            }

            this.OnDamage(bullet.power, bullet.side, false);
            bullet.gameObject.SetActive(false);
        }
        else if (bullet.side == SIDE.PET)
        {
            this.OnDamage(bullet.power, bullet.side, false);
            bullet.gameObject.SetActive(false);
        }
    }

    public virtual void OnDamage(float power, SIDE side, bool isSkill)
    {
        if (this.HP <= 0)
        {
            this.HP = 0;
            this.Die();
            return;
        }
        bool isCrit = false;
        if (side == SIDE.PLAYER || side == SIDE.SKILL)
        {
            isCrit = IsCrit(0.5f);
            if (isCrit)
                power = power * 1.3f;
               
            //随机浮动
            power = power * (1 + ((float)GameUtil.Random.NextDouble() * 0.1f - 0.05f));
        }
        //if(isSkill && isCrit) SoundManager.Instance.PlaySound(SoundDefine.AttackStar);
        this.HP -= power;
        UIWorldElementManager.Instance.ShowPopupText(this.transform.position, power, side, isCrit,isSkill);
        OnDamagePop?.Invoke(this.transform.position, power);
        if(this.side != SIDE.BOSS)
            this.ani.SetTrigger("OnDamage");
    }
    private bool IsCrit(float crit)
    {
        return GameUtil.Random.NextDouble() < crit;
    }
    public override void Die()
    {
        base.Die();
        Destroy(this.gameObject);
    }
}
