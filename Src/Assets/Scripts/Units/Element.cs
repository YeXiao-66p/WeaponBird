using System.Collections;
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
    }
    void FixedUpdate()
    {
        this.timer += Time.deltaTime;
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
            //×Óµ¯×·×Ù£¨¿ÉÀ©Õ¹£©
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
}
