// 在另一个脚本中控制旋转
using UnityEngine;

public class RotationController : MonoBehaviour
{
    public AdvancedSphereRotator rotator;

    void Start()
    {
        // 设置斜向旋转
        rotator.SetDiagonalAxes(
            new Vector3(1, 1, 0).normalized,
            new Vector3(0, 1, 1).normalized
        );

        // 启用斜向旋转模式
        rotator.SetRotationMode(true, false, false, false, false);
    }

    void Update()
    {
        // 按空格键切换模式
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rotator.SetRotationMode(
                !rotator.useDiagonalRotation,
                !rotator.useSpiralRotation,
                !rotator.useWaveRotation,
                !rotator.useRandomRotation,
                !rotator.usePendulumRotation
            );
        }
    }
}