using System;
using System.Collections.Generic;
using UnityEngine;

class GameUtil : Singleton<GameUtil>
{
    public static float GetRandomSpeedWithMinAbsValue(float value)
    {
        bool isPositive = UnityEngine.Random.Range(0, 1) == 0;
        return isPositive ?
            UnityEngine.Random.Range(1f, value) :
            UnityEngine.Random.Range(-value, -1f);
    }
    public static System.Random Random = new System.Random();
    public static int RoundToInt(float f)
    {
        return (int)Math.Round((double)f);
    }
    public static float Clamp01(float value)
    {
        if (value < 0f)
        {
            return 0f;
        }
        if (value > 1f)
        {
            return 1f;
        }
        return value;
    }
    public static float Clamp(float value, float min, float max)
    {
        if (value < min)
        {
            value = min;
        }
        else if (value > max)
        {
            value = max;
        }
        return value;
    }
    public static Vector2 LimitCamera(GameObject go, Vector2 pos, Camera cam)
    {
        if (cam != null)
        {
            Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0f, 0f, Mathf.Abs(cam.transform.position.z - go.transform.position.z)));
            Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1f, 1f, Mathf.Abs(cam.transform.position.z - go.transform.position.z)));

            float padX = 0f;
            float padY = 0f;
            var sprite = go.GetComponent<SpriteRenderer>();
            if (sprite != null)
            {
                padX = sprite.bounds.extents.x;
                padY = sprite.bounds.extents.y;
            }
            else
            {
                var collider = go.GetComponent<Collider2D>();
                if (collider != null)
                {
                    padX = collider.bounds.extents.x;
                    padY = collider.bounds.extents.y;
                }
            }

            pos.x = Mathf.Clamp(pos.x, bottomLeft.x + padX, topRight.x - padX);
            pos.y = Mathf.Clamp(pos.y, bottomLeft.y + padY, topRight.y - padY);
           
        }
        return pos;
    }
    public static void BulletPoolGet(out GameObject ob, out Element bu, Queue<GameObject> bulletPool)
    {
        ob = bulletPool.Dequeue();
        ob.SetActive(true);
        bu = ob.GetComponent<Element>();
        bu.timer = 0f;
        bulletPool.Enqueue(ob);
    }
    public static Item GetRandom(List<Item> list)
    {
        if (list == null || list.Count == 0)
        {
            Debug.LogWarning("列表为空或为null！");
            return default(Item);
        }

        return list[UnityEngine.Random.Range(0, list.Count)];
    }
}
