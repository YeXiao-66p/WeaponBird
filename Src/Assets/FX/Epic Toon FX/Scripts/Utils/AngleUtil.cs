using UnityEngine;

public static class AngleUtil
{
    // 角度转方向向量
    public static Vector2 AngleToDirection(float angleDegrees)
    {
        float angleRadians = angleDegrees * Mathf.Deg2Rad;   // 转弧度制
        return new Vector2(Mathf.Cos(angleRadians), Mathf.Sin(angleRadians));
    }
    public static Vector3 AngleToV3Direction(float angleDegrees)
    {
        float angleRadians = angleDegrees * Mathf.Deg2Rad;   // 转弧度制
        return new Vector3(Mathf.Cos(angleRadians), Mathf.Sin(angleRadians), 0);
    }
    public static Vector2 DirFromAngle(float deg)
    {
        float r = deg * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(r), Mathf.Sin(r));
    }
    // 方向向量转角度
    public static float DirectionToAngle(Vector2 direction)
    {
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }

    // 两点之间计算角度（A指向B）
    public static float AngleBetweenPoints(Vector2 pointA, Vector2 pointB)
    {
        Vector2 direction = (pointB - pointA).normalized;
        return DirectionToAngle(direction);
    }

    // 确保角度在0-360范围内
    public static float NormalizeAngle(float angle)
    {
        angle %= 360;
        if (angle < 0) angle += 360;
        return angle;
    }
    // 获取随机侧向方向（左上或右上）
    public static Vector3 GetRandomSideDirection()
    {
        return UnityEngine.Random.Range(0, 2) == 0 ?
            new Vector3(-1.5f, 0.7f).normalized :
            new Vector3(1.5f, 0.7f).normalized;
    }
}