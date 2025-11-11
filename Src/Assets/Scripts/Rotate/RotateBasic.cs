using UnityEngine;

public class RotateBasic : MonoBehaviour
{
    public Transform target;  // 目标物体
    public float rotateSpeed = 30f; // 旋转速度，度/秒
    public float selfRotateSpeed = 60f; // 自身旋转速度，度/秒
    public bool isRect;
    public int dir = 1;  // 旋转方向（1为顺时针，-1为逆时针）
    public int selfRotateDir = 1; // 自身旋转方向

    void Update()
    {
        if (this.isRect)
        {
            // 如果需要绕 X 轴旋转
            transform.Rotate(dir * rotateSpeed * Time.deltaTime, 0, 0);
            return;
        }

        if (target == null)
            return;

        // 绕目标旋转（绕世界的Y轴）
        transform.RotateAround(target.position, Vector3.up, dir * rotateSpeed * Time.deltaTime);
        transform.RotateAround(this.transform.position, Vector3.up, dir * selfRotateSpeed * Time.deltaTime);

        //// 同时自身绕自己的Y轴旋转
        //transform.Rotate(0, selfRotateDir * selfRotateSpeed * Time.deltaTime, 0, Space.Self);
    }
}