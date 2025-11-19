using UnityEngine;
using System.Collections;

public class BossShieldBreakFX : MonoBehaviour
{
    [Header("效果Prefab")]
    public GameObject leafParticlePrefab; // 粒子预制体
    public GameObject ringPrefab;         // 红色冲击波环预制体

    [Header("参数")]
    public float ringExpandSpeed = 4f;
    public float ringFadeSpeed = 1.5f;
    public float destroyDelay = 2f;

    void Start()
    {
        StartCoroutine(PlayFX());
    }

    IEnumerator PlayFX()
    {
        // 1️⃣ 生成叶片粒子爆炸
        if (leafParticlePrefab)
        {
            var leaf = Instantiate(leafParticlePrefab, transform.position, Quaternion.identity);
            leaf.transform.SetParent(transform);
        }

        // 2️⃣ 生成红色冲击波环
        if (ringPrefab)
        {
            var ring = Instantiate(ringPrefab, transform.position, Quaternion.identity);
            ring.transform.SetParent(transform);
            StartCoroutine(AnimateRing(ring));
        }

        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }

    IEnumerator AnimateRing(GameObject ring)
    {
        var mat = ring.GetComponent<Renderer>().material;
        float radius = 0f;
        float fade = 0f;

        while (fade < 1f)
        {
            radius += ringExpandSpeed * Time.deltaTime;
            fade += ringFadeSpeed * Time.deltaTime;

            mat.SetFloat("_Radius", radius);
            mat.SetFloat("_Fade", fade);

            yield return null;
        }

        Destroy(ring);
    }

    // 方便测试
    [ContextMenu("BraidBoom Effect")]
    void TestEffect()
    {
        Instantiate(this, transform.position, Quaternion.identity);
    }
}
