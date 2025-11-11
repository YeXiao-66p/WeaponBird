using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance { get; private set; }

    [Header("Pool")]
    
    public int initialSize = 200;

    public GameObject bulletPrefab;
    public GameObject bulletList;
    Queue<GameObject> pool = new Queue<GameObject>();


    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this.gameObject);
        Instance = this;


        if (bulletPrefab == null)
        {
            Debug.LogError("BulletPool: bulletPrefab is null.");
            return;
        }


        for (int i = 0; i < initialSize; i++)
        {
            var go = Instantiate(bulletPrefab, transform);
            go.SetActive(false);
            pool.Enqueue(go);
        }
    }


    public GameObject Get()
    {
        GameObject go;
        if (pool.Count > 0)
        {
            go = pool.Dequeue();
            go.SetActive(true);
        }
        else
        {
            go = Instantiate(bulletPrefab, transform);
            go.SetActive(true);
        }
        return go;
    }


    public void Release(GameObject go)
    {
        go.SetActive(false);
        go.transform.SetParent(transform, false);
        pool.Enqueue(go);
    }
}