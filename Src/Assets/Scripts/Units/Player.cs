using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Const;

public class Player : Unit
{
    public static Player Instance;
    public CameraShake CameraShake;
    public PetFollow pet;
    public Boss Boss;
    public GameObject bulletSkillTemplate;
    public GameObject bulletSkillList;
    public Queue<GameObject> bulletSkillPool = new Queue<GameObject>();

    public GameObject bulletSkillDiv;
    public GameObject bulletSkillDivList;
    public Queue<GameObject> bulletSkillDivPool = new Queue<GameObject>();

    public GameObject bulletSkillBig;
    public GameObject bulletSkillBigList;
    public Queue<GameObject> bulletSkillBigPool = new Queue<GameObject>();


    private float coolTime1 = 2f;
    private float coolTime2 = 3f;

    private float coolTime4 = 3f;

    private bool qBurstRunning = false;

    // 无敌相关
    public float invincibleTime = 0f; // 无敌持续时间
    public float invincibleDuration = 10f;
    public float invincibleCooldown = 15f; // 无敌冷却
    private float invincibleTimer = 15f;   // 冷却计时
    private bool isInvincible = false;    // 当前无敌态

    public float skillTime1;
    public float skillTime2;
    
    public float skillTime4;

    [SerializeField] private float starTimeCool = 10f;
    [SerializeField] private float starTime = 5f;
    public float starTimeStart;
    public float starTimeDur = 5f;
    public bool isStarTime = false;
    [SerializeField] private int starCount = 50;

    [SerializeField] private float healTime = 0;
    [SerializeField] private float healTimeCool = 6f;


    [SerializeField] private float quickShootTimeCool = 15f;
    [SerializeField] private float quickShootRate = 15f;
    [SerializeField] private float quickShootTime = 10f;
    public float quickShootTimeStart;
    public float quickShootTimeDur = 10f;
    public bool isQuickShootTime = false;

    

    public bool isAI = false;

    public Vector2 pos;
    // 无敌时间更新事件
    public UnityAction<float, float> OnInvincibleTimeUpdate;

    public UnityAction<int> OnScore;

    // 存储当前活跃的特效ID
    private List<int> activeEffectIds = new List<int>();

    public Coroutine petSkill = null;
    private int effectIndex;
    public int skillAttack = 15;

    public float shiftSped = 4f;

    public float curSped = 10f;
    public override void OnStart()
    {
        base.OnStart();
        if (Manager.UnitManager != null)
        {
            if (Manager.UnitManager.boss != null)
                this.Boss = Manager.UnitManager.boss;
        }
       
    }

    public override void OnUpdate()
    {

        curSped = this.speed;
        if (!inputMode) return;
        if(!this.isFly) return;
        InitBullet();
        pos = this.transform.position;

        this.healTime += Time.deltaTime;
        this.skillTime1 += Time.deltaTime;
        this.skillTime2 += Time.deltaTime;
        this.skillTime4 += Time.deltaTime;
        this.starTime += Time.deltaTime;
        this.quickShootTime += Time.deltaTime;
        this.invincibleTimer += Time.deltaTime;
        if (Manager.UnitManager != null)
        {
            if (Manager.UnitManager.boss != null)
                this.Boss = Manager.UnitManager.boss;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            this.curSped = this.shiftSped;
        }

        pos.x += Input.GetAxis("Horizontal") * Time.deltaTime * curSped;
        pos.y += Input.GetAxis("Vertical") * Time.deltaTime * curSped;
        this.transform.position = pos;
        if (this.isAI)
        {
            AIPlayerDoAttack();
        }
        else
        {

            if (Input.GetButton("Fire1"))
            {
                this.FirePlayer();
            }
            else
            {
                this.Fly();
            }
        }
        PlayerAttack();

        CleanupFinishedEffects();
        OnLimitCamera();

        if(this.isQuickShootTime)
        {
            if (this.quickShootTimeStart <= this.quickShootTimeDur)
            {
                this.quickShootTimeStart += Time.deltaTime;
            }
            else
            {
                this.isQuickShootTime = false;
                this.quickShootTimeStart = 0f;
                //Debug.Log("QuickShootTime Exit");
            }
        }
        
        if (!this.isStarTime) return;
        if (starTimeStart <= this.starTimeDur) { this.starTimeStart += Time.deltaTime; }
        else
        {
            this.isStarTime = false;
            this.starTimeStart = 0f;
            //Debug.Log("StarTime Exit");
        }
    }

    private void AIPlayerDoAttack()
    {
        this.FirePlayer();
        if (this.quickShootTime > quickShootTimeCool)
        {
            this.isQuickShootTime = true;
            this.quickShootTime = 0;
        }
        if (this.starTime > starTimeCool)
        {
            this.isStarTime = true;
            this.starTime = 0;
        }
        if (this.HP <= this.HPMax / 2)
        {
            this.InvinceMode();
        }
    }

    private void PlayerAttack()
    {
        //------------------特效技能-------------------
        if (Input.GetButton("Fire2") && this.skillTime1 > coolTime1)
        {
            this.effectIndex = UnityEngine.Random.Range(0, 5);
            Manager.EffectManager.effectIndex = effectIndex;
            this.skillTime1 = 0;
            Manager.UnitManager.OnDamageSkill(this.skillAttack);

            // 播放特效，并注册完成回调
            int effectId = Manager.EffectManager.PlayEffect(
                effectIndex,
                this.transform.position,
                this.transform.rotation,
                3f,
                false,
                (id) => OnEffectComplete(id) // 特效完成时的回调
            );
            activeEffectIds.Add(effectId);
        }
        if (Input.GetKeyDown(KeyCode.Q) && this.skillTime2 > coolTime2 && !qBurstRunning)
        {
            this.skillTime2 = 0;
            StartCoroutine(QBurst(3, 0.3f, this.skillAttack));
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            InvinceMode();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (this.petSkill != null && this.pet.isInput)
            {
                StopCoroutine(this.petSkill);
                this.petSkill = null;
                return;
            }
            this.petSkill = StartCoroutine(this.pet.FireWaveBurst());

        }
        if (Input.GetKeyDown(KeyCode.T) && this.skillTime4 > coolTime4)
        {
            this.skillTime4 = 0;
            StartCoroutine(TBurst(4, 1, this.skillAttack));
        }


        //------------------攻击状态--------------------
        if (Input.GetKeyDown(KeyCode.Alpha2) && this.starTime > starTimeCool)
        {
            this.isStarTime = true;
            this.starTime = 0;
            //Debug.Log("进入技能时间");
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) && this.quickShootTime > quickShootTimeCool)
        {
            this.isQuickShootTime = true;
            this.quickShootTime = 0;
            //Debug.Log("进入技能时间");
        }




        //------------------宠物状态--------------------
        if (Input.GetKeyDown(KeyCode.Alpha3) && this.healTime > healTimeCool)
        {
            StartCoroutine(this.HealSelf(5, 1f));
            this.healTime = 0;
        }
        if (Input.GetKeyDown(KeyCode.Y) && this.quickShootTime > quickShootTimeCool)
        {
            this.pet.CameraCenter();
        }

    }

    IEnumerator HealSelf(int times, float interval)
    {
        for (int i = 0; i < times; i++)
        {
            this.AddHp(10);

            UIWorldElementManager.Instance.ShowPopupText(this.transform.position, 10f, SIDE.HEAL, false, false);
            if (i < times - 1)
            {
                yield return new WaitForSeconds(interval);
            }
        }
    }

    IEnumerator TBurst(int times, float interval, int damagePerHit)
    {     
        // 播放特效，并注册完成回调
        int range = UnityEngine.Random.Range(0, 3);
        int effectId = Manager.EffectManager.PlayEffect(
            33 + range,
            this.transform.position + new Vector3(7, 0, 0.5f),
            this.transform.rotation,
            times * interval,
            false,
            (id) => OnEffectComplete(id) // 特效完成时的回调
        );
        activeEffectIds.Add(effectId);

        for (int i = 0; i < times; i++)
        {
            if (Manager.UnitManager != null)
            {
                Manager.UnitManager.OnDamageSkill(damagePerHit);
            }
            if (i < times - 1)
            {
                yield return new WaitForSeconds(interval);
            }
        }
    }
    IEnumerator QBurst(int times, float interval, int damagePerHit)
    {
        qBurstRunning = true;
        // Q技能播放特效
        int qEffectId = Manager.EffectManager.PlayEffect(
            42,
            this.transform.position,
            this.transform.rotation,
            times * interval, // 特效持续时间与技能总时间一致
            false,
            (id) => OnEffectComplete(id)
        );
        activeEffectIds.Add(qEffectId);

        for (int i = 0; i < times; i++)
        {
            if (Manager.UnitManager != null)
            {
                Manager.UnitManager.OnDamageSkill(damagePerHit);
            }
            if (i < times - 1)
            {
                yield return new WaitForSeconds(interval);
            }
        }
        qBurstRunning = false;
    }
    public int effectIn;
    private void InvinceMode()
    {
        if (invincibleTimer < invincibleCooldown || isInvincible) return;
        Manager.EffectManager.effectIndex = effectIn;
        invincibleTimer = 0f;
        this.invincibleTime = this.invincibleDuration;

        // 播放无敌特效，并注册完成回调
        int effectId = Manager.EffectManager.PlayEffect(
            effectIn,
            this.transform.position,
            this.transform.rotation,
            invincibleDuration, // 特效持续时间与无敌时间一致
            false,
            (id) => OnEffectComplete(id) // 特效完成时的回调
        );
        activeEffectIds.Add(effectId);

        StartCoroutine(InvincibleWindow());
    }

    public int starCntNow = 0;
    private void FirePlayer()
    {
        float rate = this.fireRate;
        if (this.isQuickShootTime) rate = this.quickShootRate;
        if (fireTimer >= 1 / rate)
        {
            this.SetAttack();
            fireTimer = 0;

            GameObject ob;
            Element bu;
            if (this.isStarTime && starCntNow <= this.starCount)
            {
               
                if (GameUtil.Random.NextDouble() <= 0.95f)
                {
                    GameUtil.BulletPoolGet(out ob, out bu, this.bulletSkillPool);
                    starCntNow++;
                    bu.power = this.power;
                    bu.speed = 20;
                }
                else
                {
                    GameUtil.BulletPoolGet(out ob, out bu, this.bulletSkillBigPool);
                    bu.isBig = true;
                    bu.power = this.power + 5;
                    bu.power = this.power * 1.5f;
                    starCntNow++;
                }
                bu.isSkill = true;
            }
            else
            {
                this.isStarTime = false;
                starCntNow = 0;
                this.starTimeStart = 0f;
                GameUtil.BulletPoolGet(out ob, out bu, this.bulletPool);
                bu.power = this.power;
                bu.speed = 20;
                bu.isSkill = false;
                if (this.isQuickShootTime) bu.power = this.power * 1.0f / 1.15f;
            }
            ob.transform.position = this.transform.position;
            bu.isSkillDiv = false;
            bu.direction = 1;
            bu.dir = new Vector3(1, 0, 0);
            //追踪Boss的弹道
            if(this.Boss != null)
            {
                GameUtil.BulletPoolGet(out ob, out bu, this.bulletPool);
                bu.power = this.power;
                bu.isSkill = false;
                bu.speed = 20;
                bu.transform.position = this.transform.position;
                bu.dir = (Boss.transform.position - this.transform.position).normalized;
            }
        }
    }

    public override void Init()
    {
        this.gameObject.SetActive(true);
        this.transform.position = InitPos;
        this.isDeath = false;
        this.HP = this.HPMax;
        this.pet.gameObject.SetActive(true);
    }

    public int bulletsPerShot = 4;
    internal float lastHP;

    void SetAttack()
    {
        this.ani.SetTrigger("Attack");
    }

    // ===================== 攻击模式 =====================
    public void FireScatter360(Vector2 pos)
    {
        // 360度散射：将360度平均分配给所有子弹
        float angleStep = 360f / bulletsPerShot; // 每个子弹之间的角度间隔

        if (this.bulletPool.Count >= bulletsPerShot)
        {
            for (int i = 0; i < bulletsPerShot; i++)
            {
                // 计算当前子弹的角度，从0度开始，均匀分布
                float currentAngle = i * angleStep;
                SpawnBulletAtAngle(currentAngle, pos);
            }
        }
        //else
        //{
        //    //Debug.Log("子弹池数量不足");
        //}
    }
    void SpawnBulletAtAngle(float angleDeg, Vector2 pos)
    {
        GameObject ob;
        Element bu;
        GameUtil.BulletPoolGet(out ob, out bu, bulletSkillDivPool);

        ob.transform.position = pos;
        bu.isSkill = false;
        bu.isSkillDiv = true;
        bu.power = this.power;
        ob.transform.rotation = Quaternion.Euler(0, 0, angleDeg);
        bu.dir = AngleUtil.AngleToDirection(angleDeg);

    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        Element bullet = col.gameObject.GetComponent<Element>();

        Item item = col.gameObject.GetComponent<Item>();
        if (item != null)
        {
            item.Use(this);
        }
        if (isInvincible)
        {
            if (bullet != null && bullet.side != SIDE.PLAYER)
                //bullet.gameObject.SetActive(false);
            return;
        }
        if (bullet == null) return;

        if (bullet.side == SIDE.ENEMY || bullet.side == SIDE.BOSS)
        {
            this.HP -= bullet.power;
            if (this.HP < 0)
            {
                this.HP = 0;
                this.Die();
            }
            //bullet.gameObject.SetActive(false);
            return;
        }
        
    }


    public override void Die()
    {
        base.Die();
        activeEffectIds.Clear();
        this.gameObject.SetActive(false);
        // 清理所有与玩家相关的特效

        this.pet.gameObject.SetActive(false);
    }

    // 场景切换或对象禁用时清理
    private void OnDisable()
    {
        CleanupAllEffects();
    }

    // 清理已完成特效的ID
    private void CleanupFinishedEffects()
    {
        if (Time.frameCount % 1000 == 0)
        {
            List<int> toRemove = new List<int>();

            foreach (int effectId in activeEffectIds)
            {
                if (!Manager.EffectManager.IsEffectActive(effectId))
                {
                    toRemove.Add(effectId);
                }
            }

            foreach (int effectId in toRemove)
            {
                activeEffectIds.Remove(effectId);
            }

            if (activeEffectIds.Count > 20)
            {
                Debug.LogWarning($"活跃特效ID列表过大: {activeEffectIds.Count}，可能存在未正确清理的特效");
            }
        }
    }
    private void OnEffectComplete(int effectId)
    {
        if (activeEffectIds.Contains(effectId))
        {
            activeEffectIds.Remove(effectId);
        }
    }
    // 强制清理所有特效
    public void CleanupAllEffects()
    {
        if (this.activeEffectIds.Count > 0)
        {
            activeEffectIds.Clear();
        }

    }
    IEnumerator InvincibleWindow()
    {
        isInvincible = true;
        invincibleTime = invincibleDuration; // 确保每次都使用完整的无敌时间

        // 触发初始无敌状态更新
        OnInvincibleTimeUpdate?.Invoke(invincibleTime, invincibleDuration);

        while (this.invincibleTime > 0)
        {
            this.invincibleTime -= Time.deltaTime;
            if (this.invincibleTime < 0) this.invincibleTime = 0; // 确保不会出现负值
            // 每帧更新无敌时间
            OnInvincibleTimeUpdate?.Invoke(invincibleTime, invincibleDuration);
            yield return null;
        }

        isInvincible = false;
        // 最后确认一次无敌状态结束更新，确保UI隐藏
        OnInvincibleTimeUpdate?.Invoke(0, invincibleDuration);
    }

    private void InitBullet()
    {
        if (this.bulletSkillPool.Count <= 200)
        {
            GameObject go = Instantiate(bulletSkillTemplate, bulletSkillList.transform);
            go.SetActive(false);
            this.bulletSkillPool.Enqueue(go);
        }

        if (this.bulletSkillBigPool.Count <= 1000)
        {
            GameObject go = Instantiate(bulletSkillBig, bulletSkillBigList.transform);
            go.SetActive(false);
            this.bulletSkillBigPool.Enqueue(go);
        }

        if (this.bulletSkillDivPool.Count <= 800)
        {
            GameObject go = Instantiate(bulletSkillDiv, bulletSkillDivList.transform);
            go.SetActive(false);
            this.bulletSkillDivPool.Enqueue(go);
        }
    }
    public void OnLimitCamera()
    {
        this.transform.position = GameUtil.LimitCamera(this.gameObject, this.pos, this.main);
    }
    public static GameObject GetInstance()
    {
        return Instance.gameObject;
    }
    public void SetAI()
    {
        SoundManager.Instance.PlaySound("ui_touch");
        if (!this.isAI)
            this.isAI = true;
        else
            this.isAI = false;
    }
    public void AddPower()
    {
        this.power += 5;
    }
}
