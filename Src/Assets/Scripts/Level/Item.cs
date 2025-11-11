using UnityEngine;

public class Item : MonoBehaviour
{
    public float AddHp;
    public GameObject bullet;
    public float lifeTime = 5f;

    internal void Use(Unit player)
    {
        player.AddHp(this.AddHp);
        Destroy(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += new Vector3(0, -1f * Time.deltaTime, 0);
        if (!this.IsScreen())
            Destroy(this.gameObject);
        Destroy(this.gameObject, this.lifeTime);
    }
    bool IsScreen()
    {
        return this.transform.position.x >= -1 && this.transform.position.x < 20 && this.transform.position.y >= 1 && this.transform.position.y < 13;
    }
}
