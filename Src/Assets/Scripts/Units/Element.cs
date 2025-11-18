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

}
