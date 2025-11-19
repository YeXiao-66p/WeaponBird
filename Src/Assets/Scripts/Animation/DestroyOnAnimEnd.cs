using UnityEngine;

public class DestroyOnAnimEnd : MonoBehaviour
{
    public void OnAnimationEnd()
    {
        //if (transform.parent != null) // 检查是否有父物体
        //{
        //    Destroy(transform.parent.gameObject); // 销毁父物体
        //}
        Destroy(gameObject);
    }
}
