using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Const;

public class PetFollow : MonoBehaviour
{
    [Header("跟随目标")]
    public Transform target;               // 主角
    public Player player;

    [Header("轨道参数")]
    public float orbitRadius = 1.5f;       // 半径
    public float orbitSpeed = 60f;         // 每秒旋转角度（度）
    public float smoothTime = 0.15f;       // 平滑时间

    private float currentAngle = 0f;       // 当前角度
    private Vector3 velocity = Vector3.zero;

    public bool isInput = false;
    public float fireRate = 1f;

    public GameObject bulletTemplate;

    public GameObject bulletList;

    public Queue<GameObject> bulletPool = new Queue<GameObject>();


    public GameObject bulletSkillTemplate;
    public GameObject bulletSkillDiv;
    public GameObject bulletSkillBig;
    public GameObject bulletSkillDivList;
    public GameObject bulletSkillBigList;
    public GameObject bulletSkillList;
    public Queue<GameObject> bulletSkillPool = new Queue<GameObject>();
    public Queue<GameObject> bulletSkillDivPool = new Queue<GameObject>();
    public Queue<GameObject> bulletSkillBigPool = new Queue<GameObject>();


    public int power = 1; // 子弹威力

    protected float fireTimer = 5f;
    private SIDE side = SIDE.PET;


    [Header("波形发射设置")]
    public int waveRings = 6;                 // 波形圈数
    public float waveInterval = 0.12f;        // 每圈间隔
    public float waveAmplitudeDeg = 20f;      // 正弦角度振幅
    public float waveFrequency = 5f;          // 正弦频率
    public float spiralOffsetSpeed = 90f;     // 螺旋角速度(度/秒)
    public float bulletsPerShot = 18;

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("宠物缺少跟随目标！");
            enabled = false;
            return;
        }
        else this.player = target.GetComponent<Player>();
            // 随机一个起始角度
        currentAngle = Random.Range(0f, 360f);

    }

    void Update()
    {
        if (!isInput) return;
        if (target == null) return;
        InitBullet();
        // 更新角度（始终旋转）
        currentAngle += orbitSpeed * Time.deltaTime;
        if (currentAngle > 360f) currentAngle -= 360f;

        // 计算期望位置（围绕 target 的圆周）
        float rad = currentAngle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f) * orbitRadius;

        Vector3 desiredPosition = target.position + offset;

        // 宠物不能在角色正下方（如果在下方，就往上抬一点）
        if (desiredPosition.y < target.position.y)
        {
            desiredPosition.y = target.position.y + 0.5f;
        }

        // 平滑移动到目标点
        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref velocity,
            smoothTime
        );

        fireTimer += Time.deltaTime;
        if (this.bulletPool.Count <= 600)
        {
            GameObject go = Instantiate(bulletTemplate, bulletList.transform);
            go.SetActive(false);
            this.bulletPool.Enqueue(go);
        }
        if (fireTimer >= 1 / fireRate) this.Fire();
    }
    public void Fire()
    {
        if (fireTimer >= 1 / fireRate)
        {
            fireTimer = 0;

            GameObject ob;
            Element bu;
            GameUtil.BulletPoolGet(out ob, out bu, this.bulletPool);

            ob.transform.position = this.transform.position;
            bu.direction = 1;
            this.bulletPool.Enqueue(ob);
        }

    }
    public void CameraCenter()
    {

    }

    public IEnumerator FireWaveBurst()
    {
        while (true) 
        {
            float baseStep = 360f / bulletsPerShot;
            for (int ring = 0; ring < waveRings; ring++)
            {
                if (this.bulletSkillPool.Count < bulletsPerShot)
                {
                    Debug.LogWarning($"波形圈{ring}子弹池不足: 需要{bulletsPerShot}，当前{this.bulletSkillPool.Count}");
                }

                float t = Time.timeSinceLevelLoad;
                float waveOffset = Mathf.Sin(t * waveFrequency + ring) * waveAmplitudeDeg;
                float spiralOffset = t * spiralOffsetSpeed;
                float totalOffset = waveOffset + spiralOffset;

                for (int i = 0; i < bulletsPerShot; i++)
                {
                    float angle = i * baseStep + totalOffset;
                    SpawnBulletAtAngle(angle, 5);
                }
                yield return new WaitForSeconds(waveInterval);
            }
            yield return null;
        }

        

    }
    void SpawnBulletAtAngle(float angleDeg, float sped)
    {
        Vector3 shootDirection = Quaternion.Euler(0, 0, angleDeg) * Vector3.right;
        GameObject ob;
        Element bu;



        if (GameUtil.Random.NextDouble() <= 0.9f)
        {
            ob = this.bulletSkillDivPool.Dequeue();
            bu = ob.GetComponent<Element>();
            this.bulletSkillDivPool.Enqueue(ob);
            bu.isSkill = false;
            bu.isSkillDiv = true;
        }
        else
        {
            ob = this.bulletSkillBigPool.Dequeue();
            bu = ob.GetComponent<Element>();
            this.bulletSkillBigPool.Enqueue(ob);
            bu.power += 10;

            bu.isSkill = true;
            bu.isSkillDiv = false;
        }
        ob.SetActive(true);
        bu.timer = 0f;
        ob.transform.position = this.transform.position;
        bu.dir = shootDirection;
        bu.speed = sped;
        ob.transform.rotation = Quaternion.Euler(0, 0, angleDeg);
    }

    void SpawnBulletAtAngle(float angleDeg, Vector2 pos)
    {

        GameObject ob;
        Element bu;
        GameUtil.BulletPoolGet(out ob, out bu, this.bulletSkillDivPool);

        ob.transform.position = pos;
        bu.isSkill = false;
        bu.isSkillDiv = true;
        ob.transform.rotation = Quaternion.Euler(0, 0, angleDeg);
        bu.dir = AngleUtil.AngleToDirection(angleDeg);

    }

    private void InitBullet()
    {
        if (this.bulletSkillPool.Count <= 300)
        {
            GameObject go = Instantiate(bulletSkillTemplate, bulletSkillList.transform);
            go.SetActive(false);
            this.bulletSkillPool.Enqueue(go);
        }

        if (this.bulletSkillBigPool.Count <= 300)
        {
            GameObject go = Instantiate(bulletSkillBig, bulletSkillBigList.transform);
            go.SetActive(false);
            this.bulletSkillBigPool.Enqueue(go);
        }

        if (this.bulletSkillDivPool.Count <= 500)
        {
            GameObject go = Instantiate(bulletSkillDiv, bulletSkillDivList.transform);
            go.SetActive(false);
            this.bulletSkillDivPool.Enqueue(go);
        }
    }
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
        else
        {
            Debug.Log("子弹池数量不足");
        }
    }

}
