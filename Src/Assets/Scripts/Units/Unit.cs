using System;
using System.Collections.Generic;
using UnityEngine;
using static Const;
public class Unit : MonoBehaviour
{
    public Camera main;
    public Animator ani;
    public Rigidbody2D rb;
    public SIDE side;

    public GameObject bulletTemplate;
    public GameObject bulletList;
    public Queue<GameObject> bulletPool = new Queue<GameObject>();

    public bool isDeath = false;
    protected Vector2 InitPos;

    public float speed;

    protected float fireTimer = 5f;
    public float fireRate = 1f;

    public float HP;
    public float HPMax;

    public bool inputMode = false;
    public int maxBullet;
    public float power; // 子弹威力

    public event Action <Unit> OnDeath;

    void Start()
    {
        this.ani = this.GetComponent<Animator>();
        this.Idle();
        this.HP = this.HPMax;
        InitPos = this.transform.position;
        OnStart();
    }
    public virtual void OnStart()
    {

    }
    public virtual void Init()
    {
        this.gameObject.SetActive(true);
        this.Idle();
        this.transform.position = InitPos;
        this.isDeath = false;
        this.HP = this.HPMax;
    }
    private void Update()
    {
        if (this.isDeath) return;

        fireTimer += Time.deltaTime;
        if (this.bulletPool.Count <= this.maxBullet)
        {
            GameObject go = Instantiate(bulletTemplate, bulletList.transform);

            go.SetActive(false);
            this.bulletPool.Enqueue(go);
        }
        OnUpdate();

    }
    public virtual void OnUpdate()
    {

    }

    public bool isFly = false;
    public virtual void Die()
    {
        this.isDeath = true;
        this.inputMode = false;
        this.Idle();
        this.isFly = false;
        this.OnDeath?.Invoke(this);
    }
    public void Idle()
    {
        this.rb.simulated = false;
        this.isFly = false;
        this.ani.SetTrigger("Idle");
    }
    public void Fly()
    {
        this.rb.simulated = true;
        this.isFly = true;
        this.ani.SetTrigger("Fly");
    }
    protected void Fire()
    {
        if (this.bulletPool.Count > 0 && fireTimer >= 1 / fireRate)
        {
            fireTimer = 0;

            GameObject ob;
            Element bu;
            GameUtil.BulletPoolGet(out ob, out bu, this.bulletPool);

            ob.transform.position = this.transform.position;
            bu.direction = this.side == SIDE.PLAYER ? 1 : -1;
            bu.dir = new Vector3(1, 0, 0);  
        }
    }


    public void AddHp(float hp)
    {
        this.HP += hp;
        if (this.HP > this.HPMax)
            this.HP = this.HPMax;

    }
}

