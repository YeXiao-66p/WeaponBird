using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Const;

public class Boss : Enemy
{
    public Animator pool1Ani;

    public GameObject danPre;
    private GameObject danPre2;     //改变弹幕种类
    private Queue<GameObject> pool = new Queue<GameObject>();

    private Queue<GameObject> pool2 = new Queue<GameObject>(); //改变弹幕种类

    public GameObject danmuRedPre;
    public GameObject danmuBluePre;
    public GameObject danmuPinkPre;
    public GameObject danpreList;
    public List<GameObject> danpreListGo;


    public GameObject danmuPinkPreMove;
    public GameObject danPreBlueMove;
    public GameObject danPreRedL2RMove;
    public GameObject danPreRedR2LMove;
    public GameObject danPreFireBox;
    public GameObject danPrePinkFireBox;




    public GameObject bulletList2;
    public Queue<GameObject> bulletPool2 = new Queue<GameObject>();
    public GameObject bulletTemplate2;

    public GameObject bulletList3;
    public Queue<GameObject> bulletPool3 = new Queue<GameObject>();
    public GameObject bulletTemplate3;

    public List<BossPhase> Phases;   // Boss 多阶段的配置
    public int currentPhaseIndex = 0;

    public bool isInvincible = false;
    public float invincibleTime = 0f;
    public float invincibleDuration = 4f;

    Vector3 dir;
    private Camera Camera;

    [Header("上下两个扇形8条弹幕旋转射击")]
    public float interTes = 0.04f;
    [Header("DrawStarAni and Coding")]

    public float interStarAni = 0.2f;
    public int bulletsPerRingStarAni = 120;
    public float interStar = 0.2f;
    public int bulletsPerRingStar = 84;
    [Header("Full Screen LowSpeed Bullet")]
    public float consistentBulletInterval = 0.35f;
    public float consistentInterval = 1f;
    public int maxRoundFullScreen = 300;
    public float spedFullScreen = 3f;

    [Header("持续Single移动弹幕球")]
    [SerializeField] private int bulletPerRoundSingle = 36;
    [SerializeField] private float intervalSingle = 0.2f;

    [Header("移动Once弹幕球")]
    [SerializeField] private int fireCircleNewbulletsPerRing = 36;
    [SerializeField] private int totalRound = 30;
    [SerializeField] private float fireCircleNewringInterval = 0.1f;

    [Header("弹幕圈FireRotOldNWay")]
    [SerializeField] private int bulletcnt = 18;
    [SerializeField] private float intervalShu = 0.1f;
    [SerializeField] private float angleNWay = 360f;

    [Header("五个一组弹幕圈")]
    [SerializeField] private int circleCount = 5;         // 圈数
    [SerializeField] private int groupCount = 36;         // 每圈的子弹组数（360° / groupCount = 角度间隔）
    [SerializeField] private float groupSpacing = 0.3f;   // 每组内部子弹间距
    [SerializeField] private float circleSpacing = 0.8f;  // 圈与圈之间的半径差
    [SerializeField] private float bulletSpeed = 4f;
    [SerializeField] private int ways = 6;               //Total rounds 6 条长链
    [SerializeField] private float inter = 0.2f;         //每圈间隔时间
    [SerializeField] private float second = 0.15f;      //Group5结束旋转的间隔时间


    [Header("随机一圈圆形弹幕")]
    [SerializeField] private int bulletsPerRing = 36;
    [SerializeField] private float spawnRadius = 3f;
    [SerializeField] private int ringCount = 50;
    [SerializeField] private float ringInterval = 0.4f;

    [Header("随机发射一圈圆形弹幕Quick")]
    [SerializeField] private int bulletsPerRingQuick = 36;
    [SerializeField] private float spawnRadiusQuick = 3f;
    [SerializeField] private int ringCountQuick = 50;
    [SerializeField] private float ringIntervalQuick = 0.2f;

    [Header("螺旋散射(boom version)")]
    [SerializeField] private float spinSpeed1 = 90f;
    [SerializeField] private float interval1 = 0.05f;
    [SerializeField] private int bulletsPerWave = 12;
    [SerializeField] private float sped = 200f;

    [Header("散射设置,中心向外多圈弹幕")]
    [SerializeField] private int bulletsPerShot = 36; // 每次发射的子弹数量
    public float FireScatter360Interval = 0.2f;
    public bool aimOneAtPlayer = true; // 是否有一发子弹瞄准玩家

    [Header("波形发射设置")]
    [SerializeField] private int bulletsWavePerShot = 36;             // 每次发射的子弹数量
    [SerializeField] private int waveRings = 36;                      // 波形圈数
    [SerializeField] private float waveInterval = 0.2f;              // 每圈间隔
    [SerializeField] private float waveAmplitudeDeg = 30f;            // 正弦角度振幅
    [SerializeField] private float waveFrequency = 2f;                 // 正弦频率
    [SerializeField] private float spiralOffsetSpeed = 90f;           // 螺旋角速度(度/秒)

    [Header("陀螺长链高速旋转")]
    [SerializeField] private float stepDealy = 0.02f;            //间隔Time
    [SerializeField] private float maxRotate = 3200;            //
    [SerializeField] private float minRotate = 120;             //
    [SerializeField] private float rotateAccel = 300;           //旋转减速度
    [SerializeField] private float rotateDel = 80;              //旋转加速度
    [SerializeField] private int LianRota_j1 = 150;            //一条链上的弹幕数
    private const int LianRota_i1 = 6;        //六个方向，六条长链
    [SerializeField] private float LianRota_timer11 = 0f;       //何时旋转

    [Header("陀螺开场高速旋转")]
    [SerializeField] private float stepRotateHighDealy = 0.02f;            //间隔Time
    [SerializeField] private float maxHighRotate = 3800;            //
    [SerializeField] private float minHighRotate = 50;             //
    [SerializeField] private float rotateHighAccel = 0;           //旋转加速度
    [SerializeField] private float rotateHighAccelMax = 250f;           //旋转加速度
    [SerializeField] private float rotateHighAccelAc = 6500f;              //旋转加速度加速度
    [SerializeField] private int rotateHigh_j1 = 150;            //一条链上的弹幕数
    private const int rotateHigh_i1 = 8;        //六个方向，六条长链

    [Header("间隔Time8个一组的大圆圈")]
    [SerializeField] private float fireScatterTimePerWave = 0.35f;
    [SerializeField] private float fireScatterTimePerRound = 1.25f;
    [SerializeField] private float fireScatterBulletsPerShot = 96;
    [SerializeField] private int fireScatterTotalRound = 8;

    [Header("直接8个一组的大圆圈")]
    [SerializeField] private float fireScatter3TimePerWave = 0.095f;
    [SerializeField] private float fireScatter3TimePerRound = 4f;
    [SerializeField] private float fireScatter3BulletsPerShot = 96;
    [SerializeField] private int fireScatter3TotalRound = 8;

    [Header("直接8个一组的大圆圈舒适模式")]
    [SerializeField] private float fireScatter4TimePerWave = 0.02f;
    [SerializeField] private float fireScatter4TimePerRound = 1f;
    [SerializeField] private float fireScatter4BulletsPerShot = 42;
    [SerializeField] private int fireScatter4TotalRound = 8;
    public float Value = 100f;

    [Header("FireRandomFiveToPlayer  Sub辅助射击")]
    private float singleBulletInterval = 0.1f;
    private float roundInterval = 0.6f;

    [Header("FireScatter360Sub  Sub辅助射击")]
    [SerializeField] private int bulletsPerShotSub = 64; // 每次发射的子弹数量
    private float FireScatter360IntervalSub = 1f;


    [Header("死亡掉落/结算")]
    public GameObject dropPrefab;                   // Boss 死亡掉落
    public Action onBossDefeated;                   // 关卡结算/过场触发

    public Collider2D col;                          //提供boss无敌效果

    //public GameObject enterFxPrefab; // 入场特效
    public GameObject leafParticlePrefab; // 粒子预制体

    // 存储当前活跃的特效ID
    private List<int> activeEffectIds = new List<int>();

    //public GameObject exitFxPrefab;  // 退场特效

    private Coroutine corNow;
    private Coroutine corNowSub;

    public float quickInter = 0.4f;
    public int pershot = 36;


    public override void OnStart()
    {
        this.Camera = Camera.main;
        if (this.col == null)
            this.col = GetComponent<Collider2D>();
        this.currentPhaseIndex = 0;
        this.HP = this.Phases[this.currentPhaseIndex].HPMax;

        this.BossFly();
        this.lifeTime = 200f;
        this.OnDeath += OnBossDeath;
        this.col.enabled = false;
        StartCoroutine(Enter());
        if(Manager.Player != null )
        {
            this.playerTarget = Manager.Player;
            this.player = playerTarget.GetComponent<Player>();
        }

        this.pool2 = this.pool;
        this.danPre2 = this.danPre;
        danPre = this.danmuBluePre;
        pool = this.bulletPool2;
    }

    public override void OnUpdate()
    {
        if (this.playerTarget != null)
            this.dir = (this.playerTarget.transform.position - this.transform.position).normalized;

        if (this.bulletPool2.Count <= this.maxBullet)
        {
            GameObject go = Instantiate(bulletTemplate2, bulletList2.transform);
            go.SetActive(false);
            this.bulletPool2.Enqueue(go);
        }
        if (this.bulletPool3.Count <= this.maxBullet)
        {
            GameObject go = Instantiate(bulletTemplate3, bulletList3.transform);
            go.SetActive(false);
            this.bulletPool3.Enqueue(go);
        }
    }
    IEnumerator Enter()
    {
        //this.transform.position = new Vector3(30, 7, 0);
        ////PlayFx(enterFxPrefab, this.transform.position);
        //yield return MoveTo(new Vector3(14.5f, 7, 0));
        //this.col.enabled = true;
        yield return new WaitForSeconds(6f);
        yield return Attack();
    }
    /// <summary>
    /// Boss的弹幕打击协程函数
    /// </summary>
    /// <returns></returns>
    IEnumerator Attack()
    {
        while (true)
        {
            //RandomColorBoom();

            //yield return DrawStarAni();
            //yield return new WaitForSeconds(200f);

            ColorBoomChange();
            yield return DrawStar();
            yield return new WaitForSeconds(200f);
            continue;

            if (this.currentPhaseIndex == 0)
            {
                RandomColorBoom();
                yield return new WaitForSeconds(0.5f);
                corNowSub = StartCoroutine(WaveRoutine());
                yield return new WaitForSeconds(0.5f);
                RandomColorBoom();
                corNow = StartCoroutine(FireAimedStream8());
                yield return new WaitForSeconds(37f);


                if(corNow != null) StopCoroutine(corNow);
                if(corNowSub != null) StopCoroutine(corNowSub);

            }

            //2
            if (this.currentPhaseIndex == 1)
            {
                ColorBoomChange();
                yield return DrawStarAni();
                yield return new WaitForSeconds(200f);

            }

            if (this.currentPhaseIndex == 2)
            {
                RandomColorBoom();
                yield return StartCoroutine(State_FullScreen());
            }

            if (this.currentPhaseIndex == 3)
            {
                ColorBoomChange();
                yield return DrawStar();
                yield return new WaitForSeconds(200f);
            }


            if (this.currentPhaseIndex == 4)
            {
                //corNow = StartCoroutine(Fire520());
                //yield return new WaitForSeconds(15f);
                //if (corNow != null) StopCoroutine(corNow);
                RandomColorBoom();
                yield return StartCoroutine(Fire520());
            }


            if (this.currentPhaseIndex == 5)
            {
                //corNow = StartCoroutine(State_DouPotWave());
                //yield return new WaitForSeconds(16f);
                //if (corNow != null) StopCoroutine(corNow);

                RandomColorBoom();
                yield return StartCoroutine(State_DouPotWave());
            }

            if (this.currentPhaseIndex == 6)
            {
                ////4
                //StartCoroutine(State_RandomCircle());
                //yield return new WaitForSeconds(44.1f);
                RandomColorBoom();
                yield return StartCoroutine(State_RandomCircle());
            }

            if (this.currentPhaseIndex == 7)
            {
                ////5
                //corNow = StartCoroutine(FireSpiralScatter(spinSpeed1, interval1, bulletsPerWave));
                //yield return new WaitForSeconds(15f);
                //if (corNow != null) StopCoroutine(corNow);

                RandomColorBoom();
                yield return StartCoroutine(FireSpiralScatter(spinSpeed1, interval1, bulletsPerWave));
            }


            if (this.currentPhaseIndex == 8)
            {
                if (!this.player.CameraShake.isShaking)
                    StartCoroutine(this.player.CameraShake.Shake());
                yield return new WaitForSeconds(1f);
                corNow = StartCoroutine(FireAimedStream(0.04f, 6f));
                yield return new WaitForSeconds(1f);
                corNowSub = StartCoroutine(FireScatter360Sub());
                yield return new WaitForSeconds(30f);
            }

            if (this.currentPhaseIndex == 9)
            {
                //State_RotateSub();
                //yield return new WaitForSeconds(151.1f);
                RandomColorBoom();
                yield return StartCoroutine(State_RotateSub());
            }

            if (this.currentPhaseIndex == 10)
            {
                //corNow = StartCoroutine(FireRotatingLianToPlayer(0.02f));
                //yield return new WaitForSeconds(100f);
                //if (corNow != null) StopCoroutine(corNow);
                RandomColorBoom();
                yield return StartCoroutine(FireRotatingLianToPlayer(0.02f));
            }
            if (this.currentPhaseIndex == 11)
            {
                if (!this.player.CameraShake.isShaking)
                    StartCoroutine(this.player.CameraShake.Shake());
                //corNow = StartCoroutine(FireWaveBurst());
                //yield return new WaitForSeconds(10f);
                //if (corNow != null) StopCoroutine(corNow);
                RandomColorBoom();
                yield return StartCoroutine(FireWaveBurst());
            }

            if (this.currentPhaseIndex == 12)
            {
                //StartCoroutine(State_RotateNWay());
                //yield return new WaitForSeconds(91f);
                //RandomColorBoom();
                RandomColorBoom();
                yield return StartCoroutine(State_RotateNWay());
            }
            if (this.currentPhaseIndex == 13)
            {
                yield return StartCoroutine(State_Group5());
            }
            if(this.currentPhaseIndex == 14)
            {
                RandomColorBoom();
                yield return State_FireScatter360();

            }
            if (this.currentPhaseIndex == 15)
            {
                RandomColorBoom();
                yield return StartCoroutine(State_FireAniMove());
            }
            yield return null;
        }
    }

    IEnumerator DrawStarAni()
    {
        while(true)
        {
            GameObject box = Instantiate(this.danPreBlueMove, this.transform.position + new Vector3(0, 5, 0), Quaternion.identity, this.danpreList.transform);
            Vector3 dir;
            List<Element> ringBullets = new List<Element>();
            Animator animator = box.GetComponent<Animator>();
            if (animator != null) animator.SetTrigger("Star");

            //5 的简单绘制
            for (int j = 1; j < 6; j++)
            {

                for (int i = 0; i < bulletsPerRingStarAni; i++)
                {
                    GameObject go = Instantiate(danPre, box.transform.position, Quaternion.identity, this.danpreList.transform);
                    float angle = i * bulletsPerRingStarAni;
                    dir = GetBulletFromPool(box.transform.position, ringBullets, angle, this.pool);
                    yield return new WaitForSeconds(interStarAni / bulletsPerRingStarAni);
                }
            }
            // 整圈延迟启动
            StartCoroutine(ActivateRing(ringBullets, 4f, 0.5f));
            Destroy(box);
            yield return new WaitForSeconds(6f);
        }
        
    }
    IEnumerator DrawStar()
    {
        float R = 3;
        Vector3 spawnPos = this.transform.position + new Vector3(0, R, 0);
        Vector3 spawnPos2 = this.transform.position + new Vector3(-4, R, 0);
        Vector3 spawnPosTemp = spawnPos;
        Vector3 dir;
        List<Element> ringBullets1 = new List<Element>();
        List<Element> ringBullets2 = new List<Element>();
        // 计算旋转45度后的五角星边向量，并每个点向左偏移2f
        float rotation = 45f * Mathf.Deg2Rad;
        // 创建旋转矩阵函数
        Vector3 RotatePoint(Vector3 point, float angle)
        {
            float cos = Mathf.Cos(angle);
            float sin = Mathf.Sin(angle);
            return new Vector3(
                point.x * cos - point.y * sin,
                point.x * sin + point.y * cos,
                0
            );
        }

        // 原始五角星顶点（未旋转）
        Vector3[] originalPoints = {
    new Vector3(0, R, 0),          // 顶部
    new Vector3(0.5878f * R,  - 0.809f * R, 0), //左下
    new Vector3(- 0.951f * R,  0.309f * R, 0), // 右上
    new Vector3(0.951f * R,  0.309f * R, 0),  // 左上
    new Vector3(-0.5878f * R,  - 0.809f * R, 0)  // 右下
};
//        Vector3[] originalPoints = {
//    new Vector3(0, R, 0),          // 顶部
//    new Vector3(0.5878f * R, R - 0.809f * R, 0), //左下
//    new Vector3(- 0.951f * R, R + 0.309f * R, 0), // 右上
//    new Vector3(0.951f * R, R + 0.309f * R, 0),  // 左上
//    new Vector3(-0.5878f * R, R - 0.809f * R, 0)  // 右下
//};


        // 旋转并偏移所有点
        Vector3[] rotatedPoints = new Vector3[5];
        for (int i = 0; i < 5; i++)
        {
            rotatedPoints[i] = RotatePoint(originalPoints[i], rotation);
        }

        // 计算边向量
        Vector3[] disF = new Vector3[5];
        Vector3[] disS = new Vector3[5];
        for (int i = 0; i < 5; i++)
        {
            int nextIndex = (i + 1) % 5;
            disF[i] = (rotatedPoints[i] - rotatedPoints[nextIndex]) / bulletsPerRingStar;
            disS[i] = (originalPoints[i] - originalPoints[nextIndex]) / bulletsPerRingStar;
        }
        float angle = 0;
        while (true)
        {
            // 第一种颜色
            for (int j = 0; j < 5; j++)
            {
                for (int i = 0; i < bulletsPerRingStar; i++)
                {
                    GameObject go = Instantiate(danPre, spawnPos2, Quaternion.identity, this.danpreList.transform);
                    angle = i * bulletsPerRingStar;
                    spawnPos2 -= disF[j];
                    dir = GetBulletFromPool(spawnPos2, ringBullets1, angle, this.pool);
                    yield return new WaitForSeconds(interStar / bulletsPerRingStar);
                }
            }

            StartCoroutine(ActivateRing(ringBullets1, 6f, interStar * 5 * 5));
            yield return new WaitForSeconds(1f);
            if (this.danPre == this.danmuBluePre)
            {
                this.danPre2 = this.danmuRedPre;
                this.pool2 = this.bulletPool;
            }
            else
            {
                this.danPre2 = this.danmuBluePre;
                this.pool2 = this.bulletPool2;
            }
            // 第二种颜色
            for (int j = 0; j < 5; j++)
            {
                for (int i = 0; i < bulletsPerRingStar; i++)
                {
                    GameObject go = Instantiate(danPre2, spawnPosTemp, Quaternion.identity, this.danpreList.transform);
                    angle = i * bulletsPerRingStar;
                    spawnPosTemp -= disS[j];

                    dir = GetBulletFromPool(spawnPosTemp, ringBullets2, angle, this.pool2);
                    yield return new WaitForSeconds(interStar / bulletsPerRingStar);
                }
            }

            StartCoroutine(ActivateRing(ringBullets2, 6f, interStar * 5));
            yield return new WaitForSeconds(6f);
        }
    }
    IEnumerator State_RotateSub()
    {
        yield return new WaitForSeconds(1f);
        corNow = StartCoroutine(FireRotatingLianAni(stepDealy));
        yield return new WaitForSeconds(1f);
        corNowSub = StartCoroutine(FireScatter360Sub());
        yield return new WaitForSeconds(100f);
    }

    IEnumerator FireRotatingLianAni(float stepDelay)
    {
        yield return new WaitForSeconds(1f);
        GameObject Box = Instantiate(danPrePinkFireBox, this.transform.position + new Vector3(-5f, 3f, 0), Quaternion.identity, this.danpreList.transform);
        danpreListGo.Add(Box);
        Box.GetComponent<Animator>().SetTrigger("Rotate");
        while (true)
        {
            for (int j = 0; j < 150; j++)
            {
                for (int i = 0; i < 13; i++)
                {
                    // 计算当前子弹的角度，从0度开始，均匀分布
                    float currentAngle = i * 30;
                    Vector3 shootDirection = Quaternion.Euler(0, 0, currentAngle) * Box.transform.right;
                    GameObject ob;
                    Element bu;
                    GameUtil.BulletPoolGet(out ob, out bu, this.pool);

                    ob.transform.position = Box.transform.position;
                    bu.side = SIDE.BOSS; // 明确标记为Boss子弹，启用Element的dir移动
                    bu.dir = shootDirection;
                    bu.speed = 6f;
                    ob.transform.rotation = Quaternion.Euler(0, 0, currentAngle);

                }
                yield return new WaitForSeconds(stepDelay);
            }
            yield return new WaitForSeconds(stepDelay);
        }
    }
    IEnumerator FireRotatingLianToPlayer(float stepDelay)
    {
        while (true)
        {
            for (int j = 0; j < 50; j++)
            {
                for (int i = 0; i < 13; i++)
                {
                    // 计算当前子弹的角度，从0度开始，均匀分布
                    float currentAngle = i * 30 + AngleUtil.DirectionToAngle(this.dir);
                    Vector3 shootDirection = Quaternion.Euler(0, 0, currentAngle) * Vector3.right;
                    GameObject ob;
                    Element bu;
                    GameUtil.BulletPoolGet(out ob, out bu, this.pool);

                    ob.transform.position = this.transform.position;
                    bu.side = SIDE.BOSS; // 明确标记为Boss子弹，启用Element的dir移动
                    bu.dir = shootDirection;
                    bu.speed = 6f;
                    ob.transform.rotation = Quaternion.Euler(0, 0, currentAngle);

                }
                yield return new WaitForSeconds(stepDelay);
            }
            yield return new WaitForSeconds(stepDelay);
        }
    }
    //--------------RandomCircle发射--------------------------------------
    IEnumerator State_RandomCircle()
    {
        yield return new WaitForSeconds(1f);
        corNow = StartCoroutine(FireRandomCircleBursts(this.ringCount, this.spawnRadius, this.bulletsPerRing, this.ringInterval));
        yield return new WaitForSeconds(1f);
        corNowSub = StartCoroutine(FireRandomFiveToPlayer(8, roundInterval));
        yield return new WaitForSeconds(22f);
        if (corNow != null) StopCoroutine(corNow);


        RandomColorBoom();

        corNow = StartCoroutine(FireRandomCircleImmedia(this.ringCountQuick, this.spawnRadiusQuick, this.bulletsPerRingQuick, this.ringIntervalQuick));
        yield return new WaitForSeconds(10.5f);
        if (corNow != null) StopCoroutine(corNow);

        RandomColorBoom();
        yield return new WaitForSeconds(1f);

        corNow = StartCoroutine(FireRandomCircleImmediaHighSped(this.ringCountQuick, this.spawnRadiusQuick, 60, this.ringIntervalQuick));
        yield return new WaitForSeconds(10.5f);
        if (corNowSub != null) StopCoroutine(corNowSub);
        if (corNow != null) StopCoroutine(corNow);
    }    // 🎲 （1）随机在Boss附近生成一圈圆形弹幕（可扩展加快释放速度的协程）
    IEnumerator FireRandomCircleBursts(int ringCount = 5, float spawnRadius = 3f, int bulletsPerRing = 24, float ringInterval = 1.2f)
    {
        //Debug.Log("随机圆形弹幕发射");

        for (int ring = 0; ring < ringCount; ring++)
        {
            SoundManager.Instance.PlaySound(SoundDefine.Biu);
            // 随机偏移Boss位置
            Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPos = this.transform.position + new Vector3(randomOffset.x, randomOffset.y, 0);

            List<Element> ringBullets = new List<Element>();
            GameObject go = Instantiate(danPre, spawnPos, Quaternion.identity, this.danpreList.transform);
            // 先生成但不移动
            float angleStep = 360f / bulletsPerRing;
            for (int i = 0; i < bulletsPerRing; i++)
            {
                float angle = i * angleStep;
                Vector3 dir = Quaternion.Euler(0, 0, angle) * Vector3.right;

                GameObject ob;
                Element bu;
                GameUtil.BulletPoolGet(out ob, out bu, this.pool);
                ob.transform.position = spawnPos;
                bu.side = SIDE.BOSS;
                bu.dir = dir;
                bu.speed = 0f;
                ob.transform.rotation = Quaternion.Euler(0, 0, angle);

                ringBullets.Add(bu);
            }

            // 整圈延迟启动
            StartCoroutine(ActivateRingOrigin(ringBullets, 5f, 1.0f));

            yield return new WaitForSeconds(ringInterval);
        }
    }
    IEnumerator FireRandomCircleImmedia(int ringCount, float spawnRadius, int bulletsPerRing, float ringInterval)
    {
        //Debug.Log("随机圆形弹幕发射");

        for (int ring = 0; ring < ringCount; ring++)
        {
            SoundManager.Instance.PlaySound(SoundDefine.Biu);
            // 随机偏移Boss位置
            Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPos = this.transform.position + new Vector3(randomOffset.x, randomOffset.y, 0);
            GameObject go = Instantiate(danPre, spawnPos, Quaternion.identity, this.danpreList.transform);
            RandomColorBoom();
            // 先生成但不移动
            float angleStep = 360f / bulletsPerRing;
            for (int i = 0; i < bulletsPerRing; i++)
            {
                float angle = i * angleStep;
                Vector3 dir = Quaternion.Euler(0, 0, angle) * Vector3.right;

                GameObject ob;
                Element bu;
                GameUtil.BulletPoolGet(out ob, out bu, this.pool);
                ob.transform.position = spawnPos;
                bu.side = SIDE.BOSS;
                bu.dir = dir;
                bu.speed = 5f;
                ob.transform.rotation = Quaternion.Euler(0, 0, angle);

            }

            yield return new WaitForSeconds(ringInterval);
        }
    }
    IEnumerator FireRandomCircleImmediaHighSped(int ringCount, float spawnRadius, int bulletsPerRing, float ringInterval)
    {
        //Debug.Log("随机圆形弹幕发射");

        for (int ring = 0; ring < ringCount; ring++)
        {
            SoundManager.Instance.PlaySound(SoundDefine.Biu);
            // 随机偏移Boss位置
            Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPos = this.transform.position + new Vector3(randomOffset.x, randomOffset.y, 0);
            GameObject go = Instantiate(danPre, spawnPos, Quaternion.identity, this.danpreList.transform);
            RandomColorBoom();
            // 先生成但不移动
            float angleStep = 360f / bulletsPerRing;
            for (int i = 0; i < bulletsPerRing; i++)
            {
                float angle = i * angleStep;
                Vector3 dir = Quaternion.Euler(0, 0, angle) * Vector3.right;

                GameObject ob;
                Element bu;
                GameUtil.BulletPoolGet(out ob, out bu, this.pool);
                ob.transform.position = spawnPos;
                bu.side = SIDE.BOSS;
                bu.dir = dir;
                bu.speed = 10f;
                ob.transform.rotation = Quaternion.Euler(0, 0, angle);

            }

            yield return new WaitForSeconds(ringInterval);
        }
    }
    IEnumerator ActivateRing(List<Element> ring, float finalSpeed, float delay)
    {
        yield return new WaitForSeconds(delay);
        foreach (var bullet in ring)
        {
            if (bullet != null && bullet.gameObject.activeInHierarchy)
                bullet.speed = finalSpeed;
        }
        yield return new WaitForSeconds(0.5f);
        foreach (var bullet in ring)
        {
            if (bullet != null && bullet.gameObject.activeInHierarchy)
                bullet.speed = 0;
        }
        yield return new WaitForSeconds(1f);
        foreach (var bullet in ring)
        {
            if (bullet != null && bullet.gameObject.activeInHierarchy)
                bullet.speed = finalSpeed - 2;
        }
    }
    IEnumerator ActivateRingOrigin(List<Element> ring, float finalSpeed, float delay)
    {
        yield return new WaitForSeconds(delay);
        foreach (var bullet in ring)
        {
            if (bullet != null && bullet.gameObject.activeInHierarchy)
                bullet.speed = finalSpeed;
        }
        
    }
    //-------------------------------------------------------------------

    IEnumerator State_FireAniMove()
    {
        yield return new WaitForSeconds(1.5f);
        corNow = StartCoroutine(FireCircleCurveSingle(bulletPerRoundSingle, intervalSingle));
        yield return new WaitForSeconds(8.7f);
        if (corNow != null) StopCoroutine(corNow);
        RandomColorBoom();

        yield return new WaitForSeconds(0.5f);
        //1
        corNow = StartCoroutine(FireCircleCurve(fireCircleNewbulletsPerRing, fireCircleNewringInterval));
        yield return new WaitForSeconds(19f);
        if (corNow != null) StopCoroutine(corNow);

        yield return new WaitForSeconds(0.5f);

        RandomColorBoom();

        corNow = StartCoroutine(FireCirStraight(fireCircleNewbulletsPerRing, fireCircleNewringInterval));
        yield return new WaitForSeconds(22f);
        if (corNow != null) StopCoroutine(corNow);

        corNow = StartCoroutine(FireCircleCurveQuick(pershot, quickInter));
        yield return new WaitForSeconds(20f);
        if (corNow != null) StopCoroutine(corNow);
    }

    // ===================== 攻击模式（）中为难度等级1，2，3递增 =====================
    //--------------（3）利用动画Animation作为弹幕圈发射点--------------------------------------
    IEnumerator FireCircleCurve(int bulletsPerRing, float ringInterval)
    {
        Vector3 shootDirection = new Vector3(0, 0, 0);
        while (true)
        {
            GameObject go = Instantiate(this.danmuPinkPreMove, this.transform.position, Quaternion.identity, this.danpreList.transform);
            danpreListGo.Add(go);
            go.GetComponent<Animator>().SetTrigger("Once");
            Vector3 goPos =  go.transform.position;
            if (go != null)
            {
                for (int ring = 0; ring < totalRound; ring++)
                {
                    if (go != null)
                    {
                        GameObject danmu = Instantiate(danPre, goPos, Quaternion.identity, this.danpreList.transform);
                    }

                    for (int i = 0; i < bulletsPerRing; i++)
                    {
                        float angle = i * 360f / bulletsPerRing;
                        GameObject ob;
                        Element bu;
                        GameUtil.BulletPoolGet(out ob, out bu, pool);
                        if (go != null)
                        {
                            goPos = go.transform.position;
                            ob.transform.position = go.transform.position;
                            shootDirection = Quaternion.Euler(0, 0, angle) * Vector3.right;
                        }
                        bu.side = SIDE.BOSS; // 明确标记为Boss子弹，启用Element的dir移动
                        bu.dir = shootDirection;
                        bu.speed = 3;
                        ob.transform.rotation = Quaternion.Euler(0, 0, angle);
                    }
                    yield return new WaitForSeconds(ringInterval);
                }
            }
            yield return new WaitForSeconds(2f);

            GameObject danper = Instantiate(this.danPreBlueMove, goPos, Quaternion.identity, this.danpreList.transform);
            if (this.danPre == this.danmuBluePre)
            {
                this.danPre2 = this.danmuRedPre;
                this.pool2 = this.bulletPool;
            }
            else
            {
                this.danPre2 = this.danmuBluePre;
                this.pool2 = this.bulletPool2;
            }

            if (danper != null)
            {
                for (int ring = 0; ring < totalRound; ring++)
                {
                    if (danper != null)
                    {
                        GameObject danmu = Instantiate(danPre2, danper.transform.position, Quaternion.identity, this.danpreList.transform);
                    }
                    for (int i = 0; i < bulletsPerRing; i++)
                    {
                        float angle = i * 360f / bulletsPerRing;
                        GameObject ob;
                        Element bu;
                        GameUtil.BulletPoolGet(out ob, out bu, this.pool2);
                        if (danper != null)
                        {
                            ob.transform.position = danper.transform.position;
                            shootDirection = Quaternion.Euler(0, 0, angle) * Vector3.right;
                        }
                        bu.side = SIDE.BOSS; // 明确标记为Boss子弹，启用Element的dir移动
                        bu.dir = shootDirection;
                        bu.speed = 3;
                        ob.transform.rotation = Quaternion.Euler(0, 0, angle);
                    }
                    yield return new WaitForSeconds(ringInterval);
                }
            }
            yield return new WaitForSeconds(2f);
        }
        

    }
    IEnumerator FireCircleCurveQuick(int bulletsPerRing, float ringInterval)
    {
        Vector3 shootDirection = new Vector3(0, 0, 0);
        GameObject go = Instantiate(this.danmuPinkPreMove, this.transform.position, Quaternion.identity, this.danpreList.transform);
        danpreListGo.Add(go);
        go.GetComponent<Animator>().SetTrigger("Quick");
        Vector3 goPos = go.transform.position;
        while (true)
        {
            if (go != null)
            {
                for (int ring = 0; ring < 3; ring++)
                {
                    if (go != null)
                    {
                        GameObject danmu = Instantiate(danPre, goPos, Quaternion.identity, this.danpreList.transform);
                    }

                    for (int i = 0; i < bulletsPerRing; i++)
                    {
                        float angle = i * 360f / bulletsPerRing;
                        GameObject ob;
                        Element bu;
                        GameUtil.BulletPoolGet(out ob, out bu, pool);
                        if (go != null)
                        {
                            goPos = go.transform.position;
                            ob.transform.position = go.transform.position;
                            shootDirection = Quaternion.Euler(0, 0, angle) * go.transform.right;
                        }
                        bu.side = SIDE.BOSS; // 明确标记为Boss子弹，启用Element的dir移动
                        bu.dir = shootDirection;
                        bu.speed = 6;
                        ob.transform.rotation = Quaternion.Euler(0, 0, angle);
                    }
                    yield return new WaitForSeconds(ringInterval);
                }
            }
        }
    }
    IEnumerator FireCircleCurveSingle(int bulletsPerRing, float ringInterval)
    {
        Vector3 shootDirection = new Vector3(0, 0, 0);
        while (true)
            {
            GameObject go = Instantiate(this.danmuPinkPreMove, this.transform.position + new Vector3(-5, 0, 0), Quaternion.identity, this.danpreList.transform);
            Animator ani = go.GetComponent<Animator>();
            danpreListGo.Add(go);
            if (ani != null)
            {
                ani.SetTrigger("Single");
            }
            Vector3 goPos = go.transform.position;
            if (go != null)
            {
                for (int ring = 0; ring < 36; ring++)
                {
                    if (go != null)
                    {
                        GameObject danmu = Instantiate(danPre, goPos, Quaternion.identity, this.danpreList.transform);
                    }

                    for (int i = 0; i < bulletsPerRing; i++)
                    {
                        float angle = i * 360f / bulletsPerRing;
                        GameObject ob;
                        Element bu;
                        GameUtil.BulletPoolGet(out ob, out bu, pool);
                        if (go != null)
                        {
                            goPos = go.transform.position;
                            ob.transform.position = go.transform.position;
                            shootDirection = Quaternion.Euler(0, 0, angle) * Vector3.right;
                        }
                        bu.side = SIDE.BOSS; // 明确标记为Boss子弹，启用Element的dir移动
                        bu.dir = shootDirection;
                        bu.speed = 3;
                        ob.transform.rotation = Quaternion.Euler(0, 0, angle);
                    }
                    yield return new WaitForSeconds(ringInterval);
                }
            }
            yield return new WaitForSeconds(1.5f);
        }


    }
    IEnumerator FireCirStraight(int bulletsPerRing, float ringInterval)
    {
        Vector3 shootDirection = new Vector3(0, 0, 0);
        while(true)
        {
            GameObject go = Instantiate(this.danPreRedL2RMove, this.transform.position - new Vector3(33, 0, 0), Quaternion.identity, this.danpreList.transform);
            if (go != null)
            {
                for (int ring = 0; ring < totalRound; ring++)
                {
                    if (go != null)
                    {
                        GameObject danmu = Instantiate(danPre, go.transform.position, Quaternion.identity, this.danpreList.transform);
                    }

                    for (int i = 0; i < bulletsPerRing; i++)
                    {
                        float angle = i * 360f / bulletsPerRing;
                        GameObject ob;
                        Element bu;
                        GameUtil.BulletPoolGet(out ob, out bu, pool);
                        if (go != null)
                        {
                            ob.transform.position = go.transform.position;
                            shootDirection = Quaternion.Euler(0, 0, angle) * Vector3.right;
                        }
                        bu.side = SIDE.BOSS; // 明确标记为Boss子弹，启用Element的dir移动
                        bu.dir = shootDirection;
                        bu.speed = 3;
                        ob.transform.rotation = Quaternion.Euler(0, 0, angle);
                    }
                    yield return new WaitForSeconds(ringInterval);
                }
            }
            yield return new WaitForSeconds(2.5f);
            GameObject danper = Instantiate(this.danPreRedR2LMove, this.transform.position - new Vector3(6, 0, 0), Quaternion.identity, this.danpreList.transform);
            if (this.danPre == this.danmuBluePre)
            {
                this.danPre2 = this.danmuRedPre;
                this.pool2 = this.bulletPool;
            }
            else
            {
                this.danPre2 = this.danmuBluePre;
                this.pool2 = this.bulletPool2;
            }

            if (danper != null)
            {
                for (int ring = 0; ring < totalRound; ring++)
                {
                    if (danper != null)
                    {
                        GameObject danmu = Instantiate(danPre2, danper.transform.position, Quaternion.identity, this.danpreList.transform);
                    }
                    for (int i = 0; i < bulletsPerRing; i++)
                    {
                        float angle = i * 360f / bulletsPerRing;
                        GameObject ob;
                        Element bu;
                        GameUtil.BulletPoolGet(out ob, out bu, this.pool2);
                        if (danper != null)
                        {
                            ob.transform.position = danper.transform.position;
                            shootDirection = Quaternion.Euler(0, 0, angle) * Vector3.right;
                        }
                        bu.side = SIDE.BOSS; // 明确标记为Boss子弹，启用Element的dir移动
                        bu.dir = shootDirection;
                        bu.speed = 3;
                        ob.transform.rotation = Quaternion.Euler(0, 0, angle);
                    }
                    yield return new WaitForSeconds(ringInterval);
                }
            }
            yield return new WaitForSeconds(2.5f);
        }
        

    }
    //--------------利用动画Animation作为弹幕圈发射点， 可使用Animation扩展多个弹幕函数--------------------------------------
    //--------------（2）Fire520在屏幕绘画--------------------------------------
    IEnumerator Fire520(int bulletsPerRing = 36, float ringInterval = 0.2f)
    {
        // 随机偏移Boss位置
        Vector2 offsetBoss = new Vector2(-8, 2);
        Vector3 spawnPos = this.transform.position + new Vector3(offsetBoss.x, offsetBoss.y, 0);
        Vector3 spawnPosTemp = spawnPos;
        Vector3 dir;
        List<Element> ringBullets = new List<Element>();
        
        // 先生成但不移动
        //float angleStep = 360f / bulletsPerRing;
        float dis = 2.0f / bulletsPerRing;
        //5 的简单绘制
        for (int j = 1; j < 6; j++)
        {
            for (int i = 0; i < bulletsPerRing; i++)
            {
                GameObject go = Instantiate(danPre, spawnPos, Quaternion.identity, this.danpreList.transform);
                float angle = i * bulletsPerRing;
                if (j == 1 || j == 5 )
                {
                    spawnPos.x -= dis;
                }
                if(j % 2 == 0)
                {
                    spawnPos.y -= dis;
                }
                if(j == 3)
                {
                    spawnPos.x += dis;
                }
                dir = GetBulletFromPool(spawnPos, ringBullets, angle, this.pool);
                yield return new WaitForSeconds(ringInterval / bulletsPerRing);
            }
            yield return new WaitForSeconds(ringInterval - 0.15f);
            
        }
        spawnPos = spawnPosTemp + new Vector3(1,0,0);
        if (this.danPre == this.danmuBluePre)
        {
            this.danPre2 = this.danmuRedPre;
            this.pool2 = this.bulletPool;
        }
        else
        {
            this.danPre2 = this.danmuBluePre;
            this.pool2 = this.bulletPool2;
        }
        //2 的简单绘制
        for (int j = 1; j < 6; j++)
        {
            SoundManager.Instance.PlaySound(SoundDefine.Biu);
            GameObject go = Instantiate(danPre, spawnPos, Quaternion.identity, this.danpreList.transform);
            for (int i = 0; i < bulletsPerRing; i++)
            {
                float angle = i * bulletsPerRing;
                if (j == 1 || j == 5)
                {
                    spawnPos.x += dis;
                }
                if (j % 2 == 0)
                {
                    spawnPos.y -= dis;
                }
                if (j == 3)
                {
                    spawnPos.x -= dis;
                }
                dir = GetBulletFromPool(spawnPos, ringBullets, angle, this.pool2);
                yield return new WaitForSeconds(ringInterval / bulletsPerRing);
            }
            yield return new WaitForSeconds(0.04f);

        }
        spawnPos = spawnPosTemp + new Vector3(4, 0, 0);
        //0 的简单绘制
        for (int j = 1; j < 7; j++)
        {
            SoundManager.Instance.PlaySound(SoundDefine.Biu);
            GameObject go = Instantiate(danPre, spawnPos, Quaternion.identity, this.danpreList.transform);
            for (int i = 0; i < bulletsPerRing; i++)
            {
                float angle = i * bulletsPerRing;
                if (j == 1)
                {
                    spawnPos.x += dis;
                }
                if (j == 2 || j == 3)
                {
                    spawnPos.y -= dis;
                }
                if (j == 4)
                {
                    spawnPos.x -= dis;
                }
                if (j == 5 || j == 6)
                    spawnPos.y += dis;

                dir = GetBulletFromPool(spawnPos, ringBullets, angle, this.pool);
                yield return new WaitForSeconds(ringInterval / bulletsPerRing);
            }
            yield return new WaitForSeconds(0.04f);

        }
        // 整圈延迟启动
        StartCoroutine(ActivateRingOrigin(ringBullets, 5f, 0.5f));

        yield return new WaitForSeconds(1f);
       
    }
    //--------------上述为Fire520在屏幕绘画--------------------------------------

    private Vector3 GetBulletFromPool(Vector3 spawnPos, List<Element> ringBullets, float angle, Queue<GameObject> poolCur)
    {
        Vector3 dir = Quaternion.Euler(0, 0, angle) * Vector3.right;
        GameObject ob;
        Element bu;
        GameUtil.BulletPoolGet(out ob, out bu, poolCur);
        ob.transform.position = spawnPos;
        bu.side = SIDE.BOSS;
        bu.dir = dir;
        bu.speed = 0f;
        ob.transform.rotation = Quaternion.Euler(0, 0, angle);

        ringBullets.Add(bu);
        return dir;
    }

    //--------------（2）FireGroup5弹幕地狱！！，有旋转大球！！--------------------------------------
    IEnumerator State_Group5()
    {
        corNow = StartCoroutine(FireGroup(48, ways, inter));
        if (this.danPre == this.danmuBluePre)
        {
            this.danPre2 = this.danmuRedPre;
            this.pool2 = this.bulletPool;
        }
        else
        {
            this.danPre2 = this.danmuBluePre;
            this.pool2 = this.bulletPool2;
        }
        corNowSub = StartCoroutine(DelayedSpiral());
        yield return new WaitForSeconds(10f);
    }
    IEnumerator DelayedSpiral()
    {
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(FireGroup2(36, ways, inter - 0.02f));
    }
    IEnumerator FireGroup(int waves, int ways, float stepDelay)
    {
        float baseAngle = 360 / ways;
        baseAngle %= 360;
        for (int j = 0; j < waves; j++)
        {
            GameObject go = Instantiate(danPre, this.transform.position, Quaternion.identity, this.danpreList.transform);
            for (int i = 0; i < ways; i++)
            {
                float currentAngle = i * baseAngle + AngleUtil.DirectionToAngle(this.dir);
                SpawnGroupOf5(currentAngle, this.pool);

            }
            yield return new WaitForSeconds(stepDelay);
        }
    }
    IEnumerator FireGroup2(int waves, int ways, float stepDelay)
    {
        float baseAngle = 360 / ways;
        baseAngle %= 360;
        for (int j = 0; j < waves; j++)
        {
            GameObject go = Instantiate(this.danPre2, this.transform.position, Quaternion.identity, this.danpreList.transform);
            for (int i = 0; i < ways; i++)
            {
                float currentAngle = i * baseAngle + 30 + AngleUtil.DirectionToAngle(this.dir);
                SpawnGroupOf5(currentAngle, this.pool2);

            }
            yield return new WaitForSeconds(stepDelay);
        }
    }
    public void SpawnGroupOf5(float angleDeg, Queue<GameObject> poolCur)
    {
        Vector2[] offsets = new Vector2[5]
        {
            new Vector2(0f, 0.0f), // front: center
            new Vector2(0.2f, -0.2f), // middle left
            new Vector2(0.2f, 0.2f), // middle right
            new Vector2(0.5f, 0.35f), // back left
            new Vector2(0.5f, -0.35f) // back right
        };

        for (int i = 0; i < 5; i++)
        {
            // For each bullet in the formation we spawn it with the same shooting direction but apply a small position offset
            Vector3 shootDirection = Quaternion.Euler(0, 0, angleDeg) * Vector3.right;
            GameObject ob;
            Element bu;
            GameUtil.BulletPoolGet(out ob, out bu, poolCur);

            ob.transform.position = this.transform.position + (Vector3)offsets[i];
            bu.side = SIDE.BOSS; // 明确标记为Boss子弹，启用Element的dir移动
            bu.dir = shootDirection;
            bu.speed = Mathf.Lerp(8, 9.5f, Time.smoothDeltaTime * Value); ;
            ob.transform.rotation = Quaternion.Euler(0, 0, angleDeg);

        }
    }
    //-------------------------上述为FireGroup5弹幕地狱！！---------------------------


    //-------------------------（3）DouPotWave两个弹幕圈生成点，弹幕地狱！！---------------------------
   IEnumerator State_DouPotWave()
    {
        // 随机偏移Boss位置
        Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPos = this.transform.position + new Vector3(randomOffset.x, randomOffset.y, 0);
        StartCoroutine(Fire2WaveBurst(spawnPos));

        spawnPos = this.transform.position + new Vector3(-randomOffset.x, -randomOffset.y, 0);

        ColorBoomChange();
        StartCoroutine(DelayedWaveBurst(spawnPos));
        yield return new WaitForSeconds(10f);
    }
    IEnumerator DelayedWaveBurst(Vector3 pos)
    {
        yield return new WaitForSeconds(1f);
        StartCoroutine(Fire3WaveBurst(pos, this.pool2, this.danPre2));
    }
    IEnumerator Fire3WaveBurst(Vector3 pos, Queue<GameObject> pool, GameObject danpre1)
    {
        float baseStep = 360f / bulletsWavePerShot;
        for (int ring = 0; ring < waveRings; ring++)
        {
            SoundManager.Instance.PlaySound(SoundDefine.Biu);
            GameObject go = Instantiate(danpre1, pos, Quaternion.identity, this.danpreList.transform);
            float t = Time.timeSinceLevelLoad;
            float waveOffset = Mathf.Sin(t * waveFrequency + ring) * waveAmplitudeDeg;
            float spiralOffset = t * spiralOffsetSpeed;
            float totalOffset = waveOffset + spiralOffset;
            for (int i = 0; i < bulletsWavePerShot; i++)
            {

                float angle = i * baseStep + totalOffset;
                Vector3 shootDirection = Quaternion.Euler(0, 0, angle) * Vector3.right;

                GameObject ob;
                Element bu;
                GameUtil.BulletPoolGet(out ob, out bu, pool);

                ob.transform.position = pos;
                bu.side = SIDE.BOSS; // 明确标记为Boss子弹，启用Element的dir移动
                bu.dir = -shootDirection;
                bu.speed = 7;
                ob.transform.rotation = Quaternion.Euler(0, 0, angle);
            }
            yield return new WaitForSeconds(waveInterval);
        }

    }
    IEnumerator Fire2WaveBurst(Vector3 pos)
    {
        float baseStep = 360f / bulletsWavePerShot;
        for (int ring = 0; ring < waveRings; ring++)
        {
            SoundManager.Instance.PlaySound(SoundDefine.Biu);
            GameObject go = Instantiate(danPre, pos, Quaternion.identity, this.danpreList.transform);
            float t = Time.timeSinceLevelLoad;
            float waveOffset = Mathf.Sin(t * waveFrequency + ring) * waveAmplitudeDeg;
            float spiralOffset = t * spiralOffsetSpeed;
            float totalOffset = waveOffset + spiralOffset;
            for (int i = 0; i < bulletsWavePerShot; i++)
            {
               
                float angle = i * baseStep + totalOffset;
                Vector3 shootDirection = Quaternion.Euler(0, 0, angle) * Vector3.right;

                GameObject ob;
                Element bu;
                GameUtil.BulletPoolGet(out ob, out bu, this.pool);

                ob.transform.position = pos;
                bu.side = SIDE.BOSS; // 明确标记为Boss子弹，启用Element的dir移动
                bu.dir = -shootDirection;
                bu.speed = 7;
                ob.transform.rotation = Quaternion.Euler(0, 0, angle);
            }
            yield return new WaitForSeconds(waveInterval);
        }

    }
    IEnumerator FireWaveBurst()
    {
        float baseStep = 360f / bulletsWavePerShot;
        for (int ring = 0; ring < waveRings; ring++)
        {
            SoundManager.Instance.PlaySound(SoundDefine.Biu);
            GameObject go = Instantiate(danPre, this.transform.position, Quaternion.identity, this.danpreList.transform);
            float t = Time.timeSinceLevelLoad;
            float waveOffset = Mathf.Sin(t * waveFrequency + ring) * waveAmplitudeDeg;
            float spiralOffset = t * spiralOffsetSpeed;
            float totalOffset = waveOffset + spiralOffset;

            for (int i = 0; i < bulletsWavePerShot; i++)
            {
                float angle = i * baseStep + totalOffset;
                SpawnBulletAtAngle(angle, 9);
            }
            yield return new WaitForSeconds(waveInterval);
        }

    }
    //-------------------------上述为 DouPotWave两个弹幕圈生成点，弹幕地狱！！---------------------------

    IEnumerator WaveRoutine(int bulletsPerWave = 15, int waves = 8, float angleAmplitude = 20f, float angleFreq = 3, float intervalBetweenBullets = 0.05f)
    {
        for (int w = 0; w < waves; w++)
        {
            SoundManager.Instance.PlaySound(SoundDefine.Biu);
            GameObject go = Instantiate(danPre, this.transform.position, Quaternion.identity, this.danpreList.transform);
            for (int i = 0; i < bulletsPerWave; i++)
            {
                float t = Time.time * angleFreq + i * 0.2f;
                float angle = Mathf.Sin(t) * angleAmplitude + AngleUtil.DirectionToAngle(this.dir);

                SpawnBulletAtAngle(angle, 8, 1);
                yield return new WaitForSeconds(intervalBetweenBullets);
            }
            yield return null;
        }
    }

    //---------------（3）满屏旋转弹幕地狱！！！-------------------------

    IEnumerator State_RotateNWay()
    {
        yield return new WaitForSeconds(1f);
        corNow = StartCoroutine(FireRotOldNWay(bulletcnt, intervalShu, angleNWay));
        yield return new WaitForSeconds(10f);
        if (corNow != null) StopCoroutine(corNow);


        corNow = StartCoroutine(RotateHighSpeed(stepRotateHighDealy, minHighRotate, maxHighRotate, rotateHighAccel));
        yield return new WaitForSeconds(30f);
        if (corNow != null) StopCoroutine(corNow);

        corNow = StartCoroutine(FireRotatingNWay(24, 0.05f, 30, 720, 60, 80, 1.2f));
        yield return new WaitForSeconds(10f);
        if (corNow != null) StopCoroutine(corNow);

        corNow = StartCoroutine(FireRotatingLian(stepDealy, minRotate, maxRotate, rotateAccel, rotateDel, 1.2f));
        yield return new WaitForSeconds(40f);
        if (corNow != null) StopCoroutine(corNow);
        
    }

    IEnumerator FireRotOldNWay(int ways, float stepDelay, float rotSpeedDeg)
    {
        float baseAngle = 0f;
        while (true)
        {
            SoundManager.Instance.PlaySound(SoundDefine.Biu);
            GameObject go = Instantiate(danPre, this.transform.position, Quaternion.Euler(0, 0, baseAngle), this.danpreList.transform);
            FireCircleOffset(ways, baseAngle);
            baseAngle += rotSpeedDeg * stepDelay;
            yield return new WaitForSeconds(stepDelay);
        }
    }
    IEnumerator FireRotatingNWay(int ways, float stepDelay, float minSpeed, float maxSpeed, float accel, float decel, float pauseTime)
    {
        float baseAngle = 0f;
        float currentSpeed = minSpeed;
        bool accelerating = true; // 当前是加速还是减速阶段
        float stateTimer = 0f;    // 控制状态切换（暂停计时）

        //go.GetComponent<Animator>().enabled = false;
        while (true)
        {
            SoundManager.Instance.PlaySound(SoundDefine.Biu);
            GameObject go = Instantiate(danPre, this.transform.position, Quaternion.Euler(0,0, baseAngle), this.danpreList.transform);
            // 发射一轮弹幕
            FireCircleOffset(ways, baseAngle);
            baseAngle += currentSpeed * stepDelay;

            // =======================
            // 阶段 1：加速到最大速度
            // =======================
            if (accelerating)
            {
                currentSpeed += accel * stepDelay;
                if (currentSpeed >= maxSpeed)
                {
                    currentSpeed = maxSpeed;
                    accelerating = false;
                }
            }
            // =======================
            // 阶段 2：减速到最小速度
            // =======================
            else
            {
                currentSpeed -= decel * stepDelay;
                if (currentSpeed <= minSpeed)
                {
                    currentSpeed = minSpeed;
                    accelerating = true;
                    stateTimer = 0f;
                    // =======================
                    // 阶段 3：暂停一段时间再加速
                    // =======================
                    while (stateTimer < pauseTime)
                    {
                        stateTimer += stepDelay;
                        yield return new WaitForSeconds(stepDelay);
                    }
                }
            }

            yield return new WaitForSeconds(stepDelay);
        }

    }

    IEnumerator RotateHighSpeed(float stepDelay, float minSpeed, float maxSpeed, float accel)
    {
        float baseAngle = 0f;
        float currentSpeed = 0f;

        while (true)
        {
            this.LianRota_timer11 += stepDelay;
            currentSpeed = GameUtil.GetRandomSpeedWithMinAbsValue(20f);
            if (Math.Abs(currentSpeed) < 2f)
            {
                currentSpeed *= 100f;
            }
            else
            {
                currentSpeed *= 10f;
            }
            for (int j = 0; j < rotateHigh_j1; j++)
            {
                GameObject go = Instantiate(danPre, this.transform.position, Quaternion.Euler(0, 0, baseAngle), this.danpreList.transform);
                Destroy(go, 0.05f);  /// 减轻眼部疲劳光效带来的眼部折磨
                for (int i = 0; i < rotateHigh_i1; i++)
                {
                    // 计算当前子弹的角度，从0度开始，均匀分布
                    float currentAngle = i * 60;
                    SpawnBulletAtAngle(baseAngle + currentAngle, 8);
                }
                baseAngle += currentSpeed * stepDelay;
                if (LianRota_timer11 > 4 * stepDelay)
                {
                    if (currentSpeed == 0)
                    {
                        currentSpeed = minSpeed;
                    }

                    if (accel >= rotateHighAccelMax)
                    {
                        accel = rotateHighAccelMax;
                    }
                    else
                    {
                        accel += rotateHighAccelAc * stepDelay;
                    }
                    currentSpeed += accel * stepDelay;
                    if (currentSpeed >= maxSpeed)
                    {
                        currentSpeed = maxSpeed;
                    }

                }
                yield return new WaitForSeconds(stepDelay);
            }
            yield return new WaitForSeconds(stepDelay);
        }

    }
    // 辅助：按偏移角进行 n-way 散射
    void FireCircleOffset(int ways, float angleOffsetDeg)
    {
        if (ways <= 0) return;
        float step = 360f / ways;
        for (int i = 0; i < ways; i++)
        {

            float a = angleOffsetDeg + i * step;
            SpawnBulletAtAngle(a, 13);
        }
    }

    IEnumerator FireRotatingLian(float stepDelay, float minSpeed, float maxSpeed, float accel, float decel, float pauseTime)
    {
        float baseAngle = 0f;
        float currentSpeed = minSpeed;
        bool accelerating = true; // 当前是加速还是减速阶段
        float stateTimer = 0f;    // 控制状态切换（暂停计时）
        while (true)
        {
            SoundManager.Instance.PlaySound(SoundDefine.Biu);
            this.LianRota_timer11 += stepDelay;
            for (int j = 0; j < LianRota_j1; j++)
            {
                GameObject go = Instantiate(danPre, this.transform.position, Quaternion.Euler(0, 0, baseAngle), this.danpreList.transform);
                Destroy(go, 0.05f);  /// 减轻眼部疲劳光效带来的眼部折磨
                for (int i = 0; i < LianRota_i1; i++)
                {
                    // 计算当前子弹的角度，从0度开始，均匀分布
                    float currentAngle = i * 60;
                    SpawnBulletAtAngle(baseAngle + currentAngle, 8);

                }
                if(LianRota_timer11 > 1.5 *stepDelay)
                {
                    baseAngle += currentSpeed * stepDelay;
                    if (accelerating)
                    {
                        currentSpeed += accel * stepDelay;
                        if (currentSpeed >= maxSpeed)
                        {
                            currentSpeed = maxSpeed;
                            accelerating = false;
                        }
                    }
                    else
                    {
                        currentSpeed -= decel * stepDelay;
                        if (currentSpeed <= minSpeed)
                        {
                            currentSpeed = minSpeed;
                            accelerating = true;
                            stateTimer = 0f;
                            while (stateTimer < pauseTime)
                            {
                                stateTimer += stepDelay;
                                yield return new WaitForSeconds(stepDelay);
                            }
                        }
                    }
                    yield return new WaitForSeconds(stepDelay);
                }
                yield return new WaitForSeconds(stepDelay - 0.05f);
            }
            yield return new WaitForSeconds(stepDelay);
        }
        
    }
    //-----------------------上述为弹幕地狱旋转陀螺式--------------------------
    // 🌪️ （2）螺旋散射（持续旋转发射，东方风格）
    IEnumerator FireSpiralScatter(float spinSpeed = 90f, float interval = 0.05f, int bulletsPerWave = 12)
    {
        //Debug.Log("开始螺旋散射");
        float spiralAngle = 0f;

        while (true)
        {
            float angleStep = 360f / bulletsPerWave;

            GameObject go = Instantiate(danPre, this.transform.position, Quaternion.identity, this.danpreList.transform);
            //Destroy(go, 0.05f);
            for (int i = 0; i < bulletsPerWave; i++)
            {
                

                float currentAngle = spiralAngle + i * angleStep;
                SpawnBulletAtAngle(currentAngle, 8);
            }

            // 累积旋转角度，形成螺旋
            spiralAngle += spinSpeed * interval * Time.deltaTime * sped;
            spiralAngle %= 360f;
            yield return new WaitForSeconds(interval);
        }
    }

    // 协程：螺旋发射（可双螺旋）
    IEnumerator FireSpiral(int stepDeg, float interval, bool dual)
    {
        int angle = 0;
        while (true)
        {
            SpawnBulletAtAngle(angle, 8);
            if (dual)
            {
                int opposite = (angle + 180) % 360;
                SpawnBulletAtAngle(opposite, 8);
            }
            angle = (angle + stepDeg) % 360;
            yield return new WaitForSeconds(interval);
        }
    }

    //------------------------------（2）360度多圈弹幕圈 8 个圈一个周期-------------------------------
    IEnumerator State_FireScatter360()
    {
        corNowSub = StartCoroutine(FireRandomFiveToPlayer(8, roundInterval));
        yield return new WaitForSeconds(0.5f);

        corNow = StartCoroutine(FireScatter3602());
        yield return new WaitForSeconds(10f);
        if (corNow != null) StopCoroutine(corNow);

        RandomColorBoom();
        yield return new WaitForSeconds(0.3f);

        corNow = StartCoroutine(FireScatter360());
        yield return new WaitForSeconds(10f);
        if (corNow != null) StopCoroutine(corNow);

        RandomColorBoom();
        yield return new WaitForSeconds(0.3f);

        corNow = StartCoroutine(FireScatter3603());
        yield return new WaitForSeconds(13f);
        if (corNow != null) StopCoroutine(corNow);

        RandomColorBoom();
        yield return new WaitForSeconds(0.3f);

        if (corNowSub != null) StopCoroutine(corNowSub);

        corNow = StartCoroutine(FireScatter3604());
        yield return new WaitForSeconds(14.2f);
        if (corNow != null) StopCoroutine(corNow);



    }
    IEnumerator FireScatter3603()
    {
        float angleStep = 360f / fireScatter3BulletsPerShot;
        while (true)
        {
            SoundManager.Instance.PlaySound(SoundDefine.Biu);
            GameObject go = Instantiate(danPre, this.transform.position, Quaternion.identity, this.danpreList.transform);
            for (int j = 0; j < fireScatter3TotalRound; j++)
            {
                for (int i = 0; i < fireScatter3BulletsPerShot; i++)
                {
                   
                    float currentAngle = i * angleStep;

                    SpawnBulletAtAngle(currentAngle, 7);
                }
                yield return new WaitForSeconds(fireScatter3TimePerWave);
            }
            yield return new WaitForSeconds(fireScatter3TimePerRound);
        }
    }
    IEnumerator FireScatter3602()
    {
        float angleStep = 360f / fireScatterBulletsPerShot;
        while (true)
        {
            for (int j = 0; j < fireScatterTotalRound; j++)
            {
                SoundManager.Instance.PlaySound(SoundDefine.Biu);
                GameObject go = Instantiate(danPre, this.transform.position, Quaternion.identity, this.danpreList.transform);
                for (int i = 0; i < fireScatterBulletsPerShot; i++)
                {
                    float currentAngle = i * angleStep;

                    SpawnBulletAtAngle(currentAngle, 7);
                }
                yield return new WaitForSeconds(fireScatterTimePerWave);
            }
            yield return new WaitForSeconds(fireScatterTimePerRound);
        }

    }

    IEnumerator FireScatter3604()
    {
        float angleStep = 360f / fireScatter4BulletsPerShot;
        while (true)
        {
            SoundManager.Instance.PlaySound(SoundDefine.Biu);
            GameObject go = Instantiate(danPre, this.transform.position, Quaternion.identity, this.danpreList.transform);
            for (int j = 0; j < fireScatter4TotalRound; j++)
            {

                for (int i = 0; i < fireScatter4BulletsPerShot; i++)
                {

                    float currentAngle = i * angleStep;

                    Vector3 shootDirection = Quaternion.Euler(0, 0, currentAngle) * Vector3.right;

                    GameObject ob;
                    Element bu;
                    GameUtil.BulletPoolGet(out ob, out bu, this.pool);

                    ob.transform.position = this.transform.position;
                    bu.side = SIDE.BOSS; // 明确标记为Boss子弹，启用Element的dir移动
                    bu.dir = shootDirection;
                    bu.speed = Mathf.Lerp(18f, 6.5f, Time.smoothDeltaTime * Value);
                    ob.transform.rotation = Quaternion.Euler(0, 0, currentAngle);
                }
                yield return new WaitForSeconds(fireScatter4TimePerWave);
            }
            yield return new WaitForSeconds(fireScatter4TimePerRound);
        }

    }
    IEnumerator FireScatter360()
    {
        // 360度散射：将360度平均分配给所有子弹
        float angleStep = 360f / bulletsPerShot; // 每个子弹之间的角度间隔

        for (int j = 0; j < 24; j++)
        {
            SoundManager.Instance.PlaySound(SoundDefine.Biu);
            GameObject go = Instantiate(danPre, this.transform.position, Quaternion.identity, this.danpreList.transform);
            for (int i = 0; i < bulletsPerShot; i++)
            {
                // 计算当前子弹的角度，从0度开始，均匀分布
                float currentAngle = i * angleStep;

                //// 如果启用瞄准玩家且这是第一发子弹，计算朝向玩家的角度
                //if (aimOneAtPlayer && playerTarget != null && i == 0)
                //{
                //    Vector3 directionToPlayer = this.dir.normalized;
                //    currentAngle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
                //}

                SpawnBulletAtAngle(currentAngle, 8);
            }
            yield return new WaitForSeconds(FireScatter360Interval);
        }
    }
    //------------------------------上述为360度多圈弹幕圈 8 个圈一个周期-------------------------------
    // （1）协程：朝向玩家的连射
    IEnumerator FireAimedStream(float interval, float noiseDeg)
    {
        while (true)
        {
            SoundManager.Instance.PlaySound(SoundDefine.Biu);
            GameObject go = Instantiate(danPre, this.transform.position, Quaternion.identity, this.danpreList.transform);
            Destroy(go, 0.05f);
            if (playerTarget != null)
            {
                Vector3 d = (playerTarget.transform.position - transform.position).normalized;
                float a = Mathf.Atan2(d.y, d.x) * Mathf.Rad2Deg + UnityEngine.Random.Range(-noiseDeg, noiseDeg);
                SpawnBulletAtAngle(a, 18);
            }
            yield return new WaitForSeconds(interval);
        }
    }

    IEnumerator FireAimedStream8()
    {
        // 创建上下两个box
        GameObject topBox = Instantiate(this.danPreFireBox, this.transform.position + new Vector3(0, 3, 0), Quaternion.identity, this.danpreList.transform);
        GameObject bottomBox = Instantiate(this.danPreFireBox, this.transform.position + new Vector3(0, -3, 0), Quaternion.identity, this.danpreList.transform);

        topBox.GetComponent<Animator>().SetTrigger("RotateRound");
        bottomBox.GetComponent<Animator>().SetTrigger("RotateRound");

        while (true)
        {
            // 第一阶段：上下同时发射8条纵向弹幕
            yield return StartCoroutine(FireVerticalStreams(topBox, bottomBox, 80, 15f, interTes));

            yield return new WaitForSeconds(interTes * 80);

            // 第二阶段：可以调整参数再次发射
            yield return StartCoroutine(FireVerticalStreams(topBox, bottomBox, 80, 15f, interTes));

            yield return new WaitForSeconds(interTes * 80);

            yield return new WaitForSeconds(1f);
        }
    }

    // 发射纵向排列的弹幕
    IEnumerator FireVerticalStreams(GameObject topBox, GameObject bottomBox,int bulletsPerStream, float speed, float bulletInterval)
    {
        float[] offsets = { -0.8f, -0.6f, -0.3f, 0f, 0.3f, 0.6f, 0.9f, 1.15f }; // 8个偏移位置
        GameObject[] buls = new GameObject[8];
        Element[] elements= new Element[8];
        while (true) 
        {


            // 发射该条弹幕的子弹
            for (int j = 0; j < bulletsPerStream; j++)
            {
                // 创建特效
                if (topBox != null)
                {
                    Vector3 topEffectPos = topBox.transform.position;
                    GameObject topEffect = Instantiate(danPre, topEffectPos, Quaternion.identity, this.danpreList.transform);
                }
                if (bottomBox != null)
                {
                    Vector3 bottomEffectPos = bottomBox.transform.position;
                    GameObject bottomEffect = Instantiate(danPre, bottomEffectPos, Quaternion.identity, this.danpreList.transform);
                }
                // 从上方box发射
                if (topBox != null)
                {
                    Vector3 topBulletPos = topBox.transform.position;
                    for(int i = 0;i <buls.Length;i++)
                    {
                        GameUtil.BulletPoolGet(out buls[i], out elements[i], this.pool);
                        buls[i].transform.position = topBulletPos + new Vector3(0, offsets[i], 0);
                        elements[i].side = SIDE.BOSS;
                        elements[i].dir = -1 * topBox.transform.right; // 纵向向下
                        elements[i].speed = Mathf.Lerp(8f, speed, Time.smoothDeltaTime * Value);
                    }
                }

                // 从下方box发射
                if (bottomBox != null)
                {
                    Vector3 bottomBulletPos = bottomBox.transform.position;
                    for (int i = 0; i < buls.Length; i++)
                    {
                        GameUtil.BulletPoolGet(out buls[i], out elements[i], this.pool);
                        buls[i].transform.position = bottomBulletPos + new Vector3(0, offsets[i], 0);
                        elements[i].side = SIDE.BOSS;
                        elements[i].dir = topBox.transform.right; // 纵向向下
                        elements[i].speed = Mathf.Lerp(8f, speed, Time.smoothDeltaTime * Value);
                    }
                }

                yield return new WaitForSeconds(bulletInterval);
            }
        }
    }
        
    
    IEnumerator FireScatter360Sub()
    {
        // 360度散射：将360度平均分配给所有子弹
        float angleStep = 360f / bulletsPerShotSub; // 每个子弹之间的角度间隔

        while(true)
        {
            SoundManager.Instance.PlaySound(SoundDefine.Biu);
            GameObject go = Instantiate(danPre, this.transform.position, Quaternion.identity, this.danpreList.transform);
            for (int i = 0; i < bulletsPerShotSub; i++)
            {
                float currentAngle = i * angleStep;
                SpawnBulletAtAngle(currentAngle, 4);
            }
            yield return new WaitForSeconds(FireScatter360IntervalSub);
        }
    }


    private Coroutine[] fullScreenCoroutines;
    private readonly Vector2[] leftPositions = {
    new Vector2(-1f, 7.4f), new Vector2(-1f, 9.2f), new Vector2(-1f, 11f),
    new Vector2(-1f, 12.7f), new Vector2(-1f, 5.6f), new Vector2(-1f, 3.8f), new Vector2(-1f, 2f)
};

    private readonly Vector2[] rightPositions = {
    new Vector2(20.5f, 7.4f), new Vector2(20.5f, 9.2f), new Vector2(20.5f, 11f),
    new Vector2(20.5f, 12.7f), new Vector2(20.5f, 5.6f), new Vector2(20.5f, 3.8f), new Vector2(20.5f, 2f)
};

    IEnumerator State_FullScreen()
    {
        // 初始化协程数组
        fullScreenCoroutines = new Coroutine[14];
        int index = 0;

        while (true)
        {
            // 启动左侧协程
            for (int i = 0; i < leftPositions.Length; i++)
            {
                fullScreenCoroutines[index++] = StartCoroutine(FireAniFullScreen(leftPositions[i], 1));
            }

            // 启动右侧协程
            for (int i = 0; i < rightPositions.Length; i++)
            {
                fullScreenCoroutines[index++] = StartCoroutine(FireAniFullScreen(rightPositions[i], -1));
            }
            index = 0; // 重置索引
            yield return new WaitForSeconds(105f);

            // 停止所有协程，准备下一轮
            StopFullScreen();
        }
    }

    void StopFullScreen()
    {
        if (fullScreenCoroutines != null)
        {
            for (int i = 0; i < fullScreenCoroutines.Length; i++)
            {
                if (fullScreenCoroutines[i] != null)
                {
                    StopCoroutine(fullScreenCoroutines[i]);
                    fullScreenCoroutines[i] = null; // 清除引用
                }
            }
        }
    }

    // 通用的全屏射击方法
    IEnumerator FireAniFullScreen(Vector2 position, int dir = 1)
    {
        while (true)
        {
            GameObject go = Instantiate(danPreFireBox, position, Quaternion.identity, this.danpreList.transform);
            danpreListGo.Add(go);
            go.GetComponent<Animator>().SetTrigger("Box");

            for (int i = 0; i < maxRoundFullScreen; i++)
            {
                if (go != null)
                {
                    GameObject boom = Instantiate(danPre, go.transform.position, Quaternion.identity, this.danpreList.transform);
                    GameObject ob;
                    Element bu;
                    GameUtil.BulletPoolGet(out ob, out bu, this.pool);
                    ob.transform.position = go.transform.position;
                    bu.side = SIDE.BOSS;
                    bu.dir = go.transform.right * dir;
                    bu.speed = spedFullScreen;
                    yield return new WaitForSeconds(consistentBulletInterval);
                }
            }
            Destroy(go);
        }
    }
    IEnumerator FireRandomFiveToPlayer(int bulletsPerRing, float ringInterval)
    {
        //Debug.Log("随机圆形弹幕发射");

        while (true)
        {
            SoundManager.Instance.PlaySound(SoundDefine.Biu);
            // 随机偏移Boss位置
            Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPos = this.transform.position + new Vector3(randomOffset.x, randomOffset.y, 0);
            GameObject go = Instantiate(danPre, spawnPos, Quaternion.identity, this.danpreList.transform);
            Vector3 dirSingle = (this.playerTarget.transform.position - this.transform.position).normalized;
            for (int i = 0; i < bulletsPerRing; i++)
            {
                GameObject ob;
                Element bu;
                GameUtil.BulletPoolGet(out ob, out bu, this.pool);
                ob.transform.position = spawnPos;
                bu.side = SIDE.BOSS;
                bu.dir = dirSingle;
                bu.speed = 8f;
                yield return new WaitForSeconds(singleBulletInterval);
            }
            yield return new WaitForSeconds(ringInterval);

        }


    }

    //--------------------下述为Boss的基本属性函数-----------------------
    IEnumerator MoveTo(Vector3 pos)
    {
        while (true)
        {
            Vector3 dir = pos - this.transform.position;
            if (dir.magnitude < 0.1)  //距离   
            {
                Manager.UnitManager.enemyList.Add(this);
                break;
            }
            this.transform.position += dir.normalized * speed * Time.deltaTime;
            yield return null;
        }

    }
    public void StartPhase()
    {
        this.HP = this.Phases[this.currentPhaseIndex].HPMax;
        //Debug.Log("进入阶段：" + this.Phases[this.currentPhaseIndex].PhaseName);

        // 播放特效，并注册完成回调
        Manager.EffectManager.effectIndex = 43 + currentPhaseIndex;
        int effectId = Manager.EffectManager.PlayEffect(
            43 + currentPhaseIndex,
            this.transform.position,
            this.transform.rotation,
            3f,
            false,
            (id) => OnEffectComplete(id) // 特效完成时的回调
        );
        this.isInvincible = true;
        this.invincibleTime = 2f;
        ClearCoroutine();
        StopAllCoroutines();
        StartCoroutine(Attack());
        UIWorldElementManager.Instance.OnStartPhase();
        if (!this.player.CameraShake.isShaking)
            StartCoroutine(this.player.CameraShake.Shake());
        StartCoroutine(PlayFX());
        StartCoroutine(InvincibleWindow());
    }

    private void ClearCoroutine()
    {
        if (this.corNow != null)
        {
            StopCoroutine(this.corNow);
            this.corNow = null;
        }
        if (this.corNowSub != null) StopCoroutine(this.corNowSub);
        if (fullScreenCoroutines != null) StopFullScreen();
        if (this.danpreListGo.Count > 0) this.danpreListGo.Clear();
        for (int i = 0; i < danpreList.transform.childCount; i++)
        {
            Transform child = danpreList.transform.GetChild(i);
            GameObject go = child.gameObject;
            Destroy(go);
        }
    }

    void SpawnBulletAtAngle(float angleDeg, float sped, int x = 1)
    {
        Vector3 shootDirection = Quaternion.Euler(0, 0, angleDeg) * Vector3.right;

        GameObject ob;
        Element bu;
        GameUtil.BulletPoolGet(out ob, out bu, this.pool);

        ob.transform.position = this.transform.position;
        bu.side = SIDE.BOSS; // 明确标记为Boss子弹，启用Element的dir移动
        bu.dir = x * shootDirection;
        bu.speed = sped;    
        ob.transform.rotation = Quaternion.Euler(0, 0, angleDeg);
    }
    IEnumerator PlayFX()
    {
        // 1️⃣ 生成叶片粒子爆炸
        var leaf = Instantiate(leafParticlePrefab, transform.position, Quaternion.identity);
        leaf.transform.SetParent(transform);

        yield return new WaitForSeconds(3f);
        Destroy(leaf.gameObject);
    }
    private void OnEffectComplete(int effectId)
    {
        if (activeEffectIds.Contains(effectId))
        {
            activeEffectIds.Remove(effectId);
        }
    }
    IEnumerator InvincibleWindow()
    {
        isInvincible = true;
        invincibleTime = invincibleDuration; // 确保每次都使用完整的无敌时间

        while (this.invincibleTime > 0)
        {
            this.invincibleTime -= Time.deltaTime;
            if (this.invincibleTime < 0) this.invincibleTime = 0; // 确保不会出现负值
            yield return null;
        }

        isInvincible = false;
    }

    void ClearBossBullets()
    {
        // 假设所有子弹都在 bulletList 下
        if (bulletList == null | bulletList2 == null) return;
        
        for (int i = 0; i < bulletList.transform.childCount; i++)
        {
            Transform child = bulletList.transform.GetChild(i);
            GameObject go = child.gameObject;
            if (!go.activeSelf) continue;
            Element e = go.GetComponent<Element>();
            if (e != null && e.side == SIDE.BOSS)
            {
                go.SetActive(false);
            }
        }
        for (int i = 0; i < bulletList2.transform.childCount; i++)
        {
            Transform child = bulletList2.transform.GetChild(i);
            GameObject go = child.gameObject;
            if (!go.activeSelf) continue;
            Element e = go.GetComponent<Element>();
            if (e != null && e.side == SIDE.BOSS)
            {
                go.SetActive(false);
            }
        }
    }

    private void RandomColorBoom()
    {
        float x = UnityEngine.Random.Range(-2, 1);
        if (x < 0)
        {
            if(x < -1)
            {
                danPre = this.danmuRedPre;
                pool = this.bulletPool;
            }
            else
            {
                danPre = this.danmuPinkPre;
                pool = this.bulletPool3;
            }
            
        }

        else
        {
            danPre = this.danmuBluePre;
            pool = this.bulletPool2;
        }
    }
    private void ColorBoomChange()
    {
        float x = UnityEngine.Random.Range(-2, 2);

        if (this.danPre == this.danmuBluePre)
        {
            if(x < 0)
            {
                this.danPre2 = this.danmuRedPre;
                this.pool2 = this.bulletPool;
            }
            else
            {
                this.danPre2 = this.danmuPinkPre;
                this.pool2 = this.bulletPool3;
            }
        }
        else if(this.danPre == this.danmuRedPre)
        {
            if (x < 0)
            {
                this.danPre2 = this.danmuBluePre;
                this.pool2 = this.bulletPool2;
            }
            else
            {
                this.danPre2 = this.danmuPinkPre;
                this.pool2 = this.bulletPool3;
            }
              
        }
        else
        {
            if (x < 0)
            {
                this.danPre2 = this.danmuBluePre;
                this.pool2 = this.bulletPool2;
            }
            else
            {
                this.danPre2 = this.danmuRedPre;
                this.pool2 = this.bulletPool;
            }
        }
        
    }
    public void BossFly()
    {
        this.rb.simulated = true;
        this.ani.SetTrigger("BossFly");
    }
    void OnDisable()
    {
        this.OnDeath -= OnBossDeath;
            //this.onBossDefeated -= Manager.UnitManager.game.GameSuc;
        }
        // Boss 死亡退出清理
    void OnBossDeath(Unit sender)
    {
        ClearBoss();
        // 掉落
        if (dropPrefab != null)
        {
            Instantiate(dropPrefab, this.transform.position, Quaternion.identity);
        }
        // 通知外部系统
        if (onBossDefeated != null)
        {
            onBossDefeated.Invoke();
        }
    }

    public void ClearBoss()
    {
        StopAllCoroutines();
        // 清屏并播放退场特效
        ClearBossBullets();
        StartCoroutine(PlayFX());
    }

    public Vector3 offset = new Vector3 (0, -4f, 0);


    public override void OnDamage(float power, SIDE side, bool isSkill)
    {
        if (isInvincible)
        {
            //Debug.Log("Boss当前剩余无敌时间： " + invincibleTime);
            return;
        }
        if (this.HP <= 0)
        {
            if(this.currentPhaseIndex < this.Phases.Count - 1)
            {
                this.HP = 0;
                this.currentPhaseIndex++;
                this.StartPhase();
                if(this.corNow != null) StopCoroutine(this.corNow);
                ClearBossBullets() ;
            }
            else
            {
                this.HP = 0;
                this.Die();
                Destroy(this.gameObject);
                return;
            }
        }
        if(this.currentPhaseIndex >= 2)
        {
            this.HP -= power * 1.0f / 1.5f;
        }
        else
        {
            this.HP -= power;
        }
            UIWorldElementManager.Instance.ShowPopupText(this.transform.position + offset, power, side, false, false);
    }
    void SetRotate()
    {
        this.ani.SetTrigger("Rotate");
    }
    public override void Die()
    {
        base.Die();
        Destroy(this.gameObject);
    }

}
