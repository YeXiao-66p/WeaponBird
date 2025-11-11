using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Transform ThisTransform;
    public bool isShaking = false;   // 标志位

    public float ShakeTime = 2.0f;
    public float ShakeAmount = 3.0f;
    public float ShakeSpeed = 2.0f;

    void Start()
    {
        ThisTransform = GetComponent<Transform>();
    }

    public void StartShake()
    {
        if (!isShaking)   // 只有不在抖动时才开启
            StartCoroutine(Shake());
    }

    public IEnumerator Shake()
    {

        isShaking = true;  // 标记为正在抖动

        Vector3 OrigPosition = ThisTransform.localPosition;
        float ElapsedTime = 0f;

        while (ElapsedTime < ShakeTime)
        {
            Vector3 RandomPoint = OrigPosition + Random.insideUnitSphere * ShakeAmount;
            ThisTransform.localPosition = Vector3.Lerp(ThisTransform.localPosition, RandomPoint, Time.deltaTime * ShakeSpeed);

            yield return null;
            ElapsedTime += Time.deltaTime;
        }

        ThisTransform.localPosition = OrigPosition;
        isShaking = false; // 抖动结束，解锁
    }
}
