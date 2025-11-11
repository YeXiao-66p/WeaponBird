using UnityEngine;
public class Pipeline : MonoBehaviour
{
    public float speed = 6;
    // 生命周期时间（秒）
    private void Start()
    {
        this.Init();
        // 启动生命周期协程
        Destroy(this.gameObject, 5f);
    }

    public void Init()
    {
        float y = Random.Range(-2, 3);
        this.transform.localPosition += new Vector3(0, y, 0);
    }

}
