//// (c) Copyright 2013 Luke Light&Magic. All rights reserved.

//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;

//public class EffectManager : MonoBehaviour
//{
//    public Transform surround_point, ground;
//    public Transform[] effects = { null };

//    public int effectIndex, goIndex, fpsNumber;
//    private float repeatTime = 4;
//    private float timer;
//    private float fpsTimer;
//    public Transform effect;
//    private float fps, fpsTotal;
//    private bool isRepeat = true;
//    private string effectName;


//    //void Start()
//    //{
//    //    InitEffect();
//    //}
//    //public void Init()
//    //{

//    //}

//    //void OnGUI()
//    //{
//    //    GUILayout.Label("Effect Name : " + effectName.Replace("(Clone)", ""));
//    //    GUILayout.Label("Effect Index : " + effectIndex);
//    //    GUILayout.Label("Total Effects : " + effects.Length);

//    //    GUILayout.BeginHorizontal();
//    //    if (GUILayout.Button("Go Effect Index", GUILayout.Width(120)))
//    //    {
//    //        if (goIndex == effectIndex || goIndex < 0 || goIndex >= effects.Length)
//    //            return;
//    //        if (effect != null)
//    //            Destroy(effect.gameObject);
//    //        effectIndex = goIndex;
//    //        InitEffect();
//    //    }
//    //    GUILayout.Label(" ==> ", GUILayout.Width(30));
//    //    goIndex = int.Parse(GUILayout.TextField(goIndex.ToString(), GUILayout.MaxWidth(50)));
//    //    GUILayout.EndHorizontal();

//    //    GUILayout.BeginHorizontal();
//    //    GUILayout.Label("Repeat Time : ", GUILayout.MaxWidth(85));
//    //    repeatTime = float.Parse(GUILayout.TextField(repeatTime.ToString(), GUILayout.MaxWidth(50)));
//    //    GUILayout.EndHorizontal();

//    //    if (GUILayout.Button("Show/Hide Ground", GUILayout.Width(120)))
//    //    {
//    //        if (ground != null)
//    //        {
//    //            ground.gameObject.SetActive(!ground.gameObject.activeSelf);
//    //        }
//    //    }
//    //    GUILayout.Space(6);

//    //    if (GUILayout.Button("Previous Effect", GUILayout.Width(120)))
//    //    {
//    //        if (effect != null)
//    //            Destroy(effect.gameObject);
//    //        --effectIndex;
//    //        if (effectIndex < 0)
//    //            effectIndex = effects.Length - 1;
//    //        InitEffect();
//    //    }
//    //    else if (GUILayout.Button("Next Effect", GUILayout.Width(120)))
//    //    {
//    //        if (effect != null)
//    //            Destroy(effect.gameObject);
//    //        ++effectIndex;
//    //        if (effectIndex >= effects.Length)
//    //            effectIndex = 0;
//    //        InitEffect();
//    //    }
//    //    GUILayout.Space(6);
//    //    GUILayout.Label("fps = " + fps);
//    //}

//    //void Update()
//    //{
//    //    if (timer >= repeatTime && isRepeat)
//    //    {
//    //        if (effect != null)
//    //        {
//    //            effect.gameObject.SetActive(false);
//    //            StartCoroutine(DelayActive(effect.gameObject));
//    //        }
//    //        timer = 0;
//    //    }
//    //    else
//    //        timer += Time.deltaTime;

//    //    if (Time.time > fpsTimer)
//    //    {
//    //        fps = fpsTotal / fpsNumber;
//    //        fpsTimer = Time.time + 0.5f;
//    //    }
//    //    else
//    //    {
//    //        fpsTotal += 1 / Time.deltaTime;
//    //        ++fpsNumber;
//    //    }
//    //}
//    public void InitEffect()
//    {
//        effect = null;
//        effect = effects[effectIndex];
//        if (effect != null)
//        {
//            effect = Instantiate(effect) as Transform;
//            effect.gameObject.SetActive(true);
//        }
//        //if (effectIndex >= 8 && effectIndex <= 28)
//        //{
//        //    if (surround_point != null)
//        //    {
//        //        effect.parent = surround_point;
//        //        if (effectIndex >= 8 && effectIndex <= 12)
//        //            effect.localPosition = new Vector3(0, effect.localPosition.y, 0);
//        //        else
//        //            effect.localPosition = Vector3.zero;
//        //        isRepeat = false;
//        //    }
//        //}
//        //else if (effectIndex >= 22 && effectIndex <= 24 || effectIndex >= 47)
//        //    isRepeat = false;
//        //else
//        //    isRepeat = true;
//        //if (effectIndex >= 38 && effectIndex <= 42)
//        //    effect.localEulerAngles = new Vector3(0, -45, 0);
//        timer = 0;
//        effectName = effect.name;
//    }

//    IEnumerator DelayActive(GameObject effect)
//    {
//        yield return new WaitForSeconds(0.1f);
//        if (effect != null)
//            effect.SetActive(true);
//    }

//}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EffectManager : MonoSingleton<EffectManager>
{
    public Transform surround_point, ground;
    public Transform[] effects = { null };

    public int effectIndex, goIndex, fpsNumber;
    private float repeatTime = 4;
    private float timer;
    private float fpsTimer;
    private float fps, fpsTotal;
    private bool isRepeat = true;
    private string effectName;

    // 存储当前激活的特效实例
    private List<ActiveEffect> activeEffects = new List<ActiveEffect>();

    [System.Serializable]
    public class ActiveEffect
    {
        public Transform effectTransform;
        public float duration; // 特效预计持续时间
        public float elapsedTime; // 已播放时间
        public bool isLooping; // 是否循环播放
        public int effectId; // 特效唯一标识
        public System.Action <int>onComplete; // 播放完成回调

        public ActiveEffect(Transform effect, float dur, bool loop, int id)
        {
            effectTransform = effect;
            duration = dur;
            isLooping = loop;
            effectId = id;
            elapsedTime = 0f;
        }
    }

    private int nextEffectId = 0;

    void Update()
    {
        if (timer >= repeatTime && isRepeat)
        {
            // 这里可以保留原有的重复逻辑，或者根据需求修改
            timer = 0;
        }
        else
        {
            timer += Time.deltaTime;
        }

        // 更新所有活跃的特效
        UpdateActiveEffects();

        // FPS 计算（保留原有逻辑）
        if (Time.time > fpsTimer)
        {
            fps = fpsTotal / fpsNumber;
            fpsTimer = Time.time + 0.5f;
        }
        else
        {
            fpsTotal += 1 / Time.deltaTime;
            ++fpsNumber;
        }
    }

    void UpdateActiveEffects()
    {
        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            ActiveEffect activeEffect = activeEffects[i];

            if (activeEffect.effectTransform == null)
            {
                activeEffects.RemoveAt(i);
                continue;
            }

            activeEffect.elapsedTime += Time.deltaTime;

            // 如果特效有持续时间且不是循环播放，检查是否应该销毁
            if (!activeEffect.isLooping && activeEffect.duration > 0 &&
                activeEffect.elapsedTime >= activeEffect.duration)
            {
                DestroyEffect(activeEffect);
                activeEffects.RemoveAt(i);
            }
        }
    }

    // 播放特效（返回特效ID用于后续控制）
    public int PlayEffect(int index, Vector3 position, Quaternion rotation,
                         float duration = 0f, bool loop = false, System.Action<int> onComplete = null)
    {
        if (index < 0 || index >= effects.Length || effects[index] == null)
        {
            Debug.LogWarning($"特效索引 {index} 无效或特效为空");
            return -1;
        }

        Transform newEffect = Instantiate(effects[index], position, rotation);
        newEffect.gameObject.SetActive(true);

        ActiveEffect activeEffect = new ActiveEffect(newEffect, duration, loop, nextEffectId);
        activeEffect.onComplete = onComplete;
        activeEffects.Add(activeEffect);

        effectName = newEffect.name;

        return nextEffectId++;
    }

    // 为Player类提供的便捷方法
    public int PlayEffectForPlayer(int index, Transform playerTransform)
    {
        return PlayEffect(index, playerTransform.position, playerTransform.rotation, 3f, false);
    }

    // 停止特定特效
    public void StopEffect(int effectId)
    {
        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            if (activeEffects[i].effectId == effectId)
            {
                DestroyEffect(activeEffects[i]);
                activeEffects.RemoveAt(i);
                break;
            }
        }
    }

    // 停止所有特效
    public void StopAllEffects()
    {
        foreach (var activeEffect in activeEffects)
        {
            DestroyEffect(activeEffect);
        }
        activeEffects.Clear();
    }

    private void DestroyEffect(ActiveEffect activeEffect)
    {
        if (activeEffect.effectTransform != null)
        {
            if (activeEffect.onComplete != null)
                activeEffect.onComplete.Invoke(activeEffect.effectId);

            Destroy(activeEffect.effectTransform.gameObject);
        }
    }

    // 保留原有的InitEffect方法用于兼容性
    public void InitEffect()
    {
        // 这个方法现在只播放一个特效，不干扰其他正在播放的特效
        PlayEffect(effectIndex, Vector3.zero, Quaternion.identity, 5f, false);
    }

    IEnumerator DelayActive(GameObject effect)
    {
        yield return new WaitForSeconds(0.1f);
        if (effect != null)
            effect.SetActive(true);
    }
    // 检查特定ID的特效是否还在活跃列表中
    public bool IsEffectActive(int effectId)
    {
        foreach (var activeEffect in activeEffects)
        {
            if (activeEffect.effectId == effectId)
            {
                return activeEffect.effectTransform != null;
            }
        }
        return false;
    }

    // 获取当前活跃特效数量（用于调试）
    public int GetActiveEffectCount()
    {
        return activeEffects.Count;
    }
}