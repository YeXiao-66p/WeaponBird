using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using static Const;

public class Element : MonoBehaviour
{
    public float speed;
    public float timer = 0;
    public int direction = 1; // 1:right, -1:left
    public SIDE side;
    public Vector3 dir = new(1, 0, 0);
    public float power;
    public GameObject target;
    internal bool isSkill = false;
    internal bool isSkillDiv = false;
    public bool isPet = false;

    public BulletPool pool;
    public bool isHardMode = true;
    internal bool isBig = false;
    private float desTime = 0;
    // 屏幕边界（根据您的IsScreen方法中的值）

    public bool enableBounce = false; // 是否启用反弹
    private float xMin = -1f;
    private float xMax = 20f;
    private float yMin = 1f;
    private float yMax = 13f;
    public void SetPool(BulletPool bulletPool)
    {
        this.pool = bulletPool;
    }
    void Start()
    {
        this.transform.rotation = Quaternion.Euler(0, 0, -90);
    }
    private void OnDisable()
    {
        this.isSkill = false;
        this.transform.rotation = Quaternion.Euler(0, 0, -90);
        this.enableBounce = false;
    }
    void FixedUpdate()
    {
        this.timer += Time.deltaTime;
        if (enableBounce)
        {
            HandleBounce();
            return;
        }
        if (!this.IsScreen())
        {
            desTime += Time.deltaTime;
            if(desTime > 0.3f)
            {
                this.gameObject.SetActive(false);

                desTime = 0;
            } 
        }
            

        if (this.side == SIDE.BOSS && this.isHardMode)
        {
            //子弹追踪（可扩展）
            this.transform.position += (Vector3)(dir * speed * Time.deltaTime);
        }

        else
            this.transform.position += (this.speed * dir * this.direction) * Time.deltaTime;
    }
    IEnumerator ActivateBullet()
    {
        yield return new WaitForSeconds(2f);
        this.gameObject.SetActive(false);
    }
    bool IsScreen()
    {
        return this.transform.position.x >= -1 && this.transform.position.x < 20 && this.transform.position.y >= 1 && this.transform.position.y < 13;
    }
    // 新增：处理反弹逻辑
    private void HandleBounce()
    {
        Vector3 currentPos = this.transform.position;
        bool bounced = false;
        this.transform.position += (this.speed * dir * this.direction) * Time.deltaTime;
        // 检查左右边界
        if (currentPos.x <= xMin || currentPos.x >= xMax)
        {
            // 反转X方向
            dir.x = -dir.x;
            bounced = true;

            // 确保不会卡在边界外
            currentPos.x = Mathf.Clamp(currentPos.x, xMin + 0.1f, xMax - 0.1f);
            this.transform.position = currentPos;
        }

        // 检查上下边界
        if (currentPos.y <= yMin || currentPos.y >= yMax)
        {
            // 反转Y方向
            dir.y = -dir.y;
            bounced = true;

            // 确保不会卡在边界外
            currentPos.y = Mathf.Clamp(currentPos.y, yMin + 0.1f, yMax - 0.1f);
            this.transform.position = currentPos;
        }

        if (bounced)
        {
            this.enableBounce = false;
        }
    }
    //IEnumerator Test()
    //{
    //    while (true)
    //    {
    //        for (int j = 1; j < 11; j++)
    //        {
    //            tests[j] = StartCoroutine(TenLianR(j));
    //        }
    //        yield return new WaitForSeconds(0.5f);
    //        for (int j = 1; j < 11; j++)
    //        {
    //            tests2[j] = StartCoroutine(TenLianL(j));
    //        }

    //        yield return new WaitForSeconds(100f);
    //    }
    //}

    //IEnumerator TenLianR(int j)
    //{
    //    float angle1;
    //    float angle;
    //    while (true)
    //    {
    //        ColorBoomChange();
    //        angle1 = j * (360f / 10);

    //        for (int i = 0; i < 18; i++)
    //        {
    //            GameObject go = Instantiate(this.danmuRedPre, this.transform.position, Quaternion.identity, this.danpreList.transform);
    //            angle = i * (135.0f / 18) + angle1;
    //            Vector3 shootDirection = Quaternion.Euler(0, 0, angle) * Vector3.right;

    //            GameObject ob;
    //            Element bu;
    //            GameUtil.BulletPoolGet(out ob, out bu, this.bulletPool);

    //            ob.transform.position = this.transform.position;
    //            bu.side = SIDE.BOSS;
    //            bu.dir = shootDirection;

    //            // 修改速度曲线，实现甩辫子效果
    //            if (i < 6)
    //            {
    //                // 前6个子弹：速度从0.5逐渐增加到4（加速）
    //                bu.speed = Mathf.Lerp(3f, 3.5f, i / 6f);
    //            }
    //            else if (i < 12)
    //            {
    //                // 中间6个子弹：保持高速4
    //                bu.speed = 4f;
    //            }
    //            else
    //            {
    //                // 后6个子弹：速度从4逐渐减小到2（减速）
    //                bu.speed = Mathf.Lerp(4f, 2f, (i - 12) / 6f);
    //            }

    //            ob.transform.rotation = Quaternion.Euler(0, 0, angle);
    //            yield return new WaitForSeconds(0.06f);
    //        }
    //        yield return new WaitForSeconds(0.6f);
    //    }
    //}

    //IEnumerator TenLianL(int j)
    //{
    //    float angle1;
    //    float angle;
    //    while (true)
    //    {
    //        ColorBoomChange();
    //        angle1 = j * (360f / 10);

    //        for (int i = 0; i < 18; i++)
    //        {
    //            GameObject go = Instantiate(this.danmuBluePre, this.transform.position, Quaternion.identity, this.danpreList.transform);
    //            angle = angle1 - i * (135.0f / 18);
    //            Vector3 shootDirection = Quaternion.Euler(0, 0, angle) * Vector3.right;

    //            GameObject ob;
    //            Element bu;
    //            GameUtil.BulletPoolGet(out ob, out bu, this.bulletPool2);

    //            ob.transform.position = this.transform.position;
    //            bu.side = SIDE.BOSS;
    //            bu.dir = shootDirection;

    //            // 使用相同的速度曲线，实现对称的甩辫子效果
    //            if (i < 6)
    //            {
    //                // 前6个子弹：速度从0.5逐渐增加到4（加速）
    //                bu.speed = Mathf.Lerp(3f, 3.5f, i / 6f);
    //            }
    //            else if (i < 12)
    //            {
    //                // 中间6个子弹：保持高速4
    //                bu.speed = 4f;
    //            }
    //            else
    //            {
    //                // 后6个子弹：速度从4逐渐减小到2（减速）
    //                bu.speed = Mathf.Lerp(4f, 2f, (i - 12) / 6f);
    //            }

    //            ob.transform.rotation = Quaternion.Euler(0, 0, angle);
    //            yield return new WaitForSeconds(0.06f);
    //        }
    //        yield return new WaitForSeconds(0.6f);
    //    }
    //}
}
