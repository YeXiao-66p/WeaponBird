using UnityEngine;

public class AdvancedSphereRotator : MonoBehaviour
{
    [Header("基础旋转设置")]
    public float rotationSpeed = 30f;
    public Vector3 rotationAxis = Vector3.up;
    public bool useLocalSpace = true;

    [Header("斜向旋转模式")]
    public bool useDiagonalRotation = false;
    public Vector3 diagonalAxis1 = new Vector3(1, 1, 0); // 第一个斜向轴
    public Vector3 diagonalAxis2 = new Vector3(1, 0, 1); // 第二个斜向轴
    public float axisSwitchInterval = 3f; // 切换间隔

    [Header("螺旋旋转模式")]
    public bool useSpiralRotation = false;
    public float spiralRadius = 0.5f;
    public float spiralSpeed = 2f;
    public float spiralHeightChange = 1f;

    [Header("波浪旋转模式")]
    public bool useWaveRotation = false;
    public float waveFrequency = 1f;
    public float waveAmplitude = 15f;

    [Header("随机旋转模式")]
    public bool useRandomRotation = false;
    public float randomChangeInterval = 2f;
    public float maxRandomSpeed = 50f;

    [Header("摆锤旋转模式")]
    public bool usePendulumRotation = false;
    public float pendulumAngle = 45f;
    public float pendulumSpeed = 1f;

    // 私有变量
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private float timeAccumulator = 0f;
    private float diagonalTime = 0f;
    private float randomTime = 0f;
    private Vector3 currentRandomAxis;
    private float currentRandomSpeed;
    private float pendulumAngleAccumulator = 0f;

    void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        // 初始化随机旋转
        if (useRandomRotation)
        {
            GenerateNewRandomRotation();
        }
    }

    void Update()
    {
        timeAccumulator += Time.deltaTime;

        // 保存原始位置和旋转，防止多个模式互相干扰
        Vector3 finalPosition = originalPosition;
        Quaternion finalRotation = originalRotation;

        // 基础旋转
        if (!useDiagonalRotation && !useSpiralRotation && !useWaveRotation &&
            !useRandomRotation && !usePendulumRotation)
        {
            ApplyBasicRotation();
        }

        // 斜向旋转模式
        if (useDiagonalRotation)
        {
            ApplyDiagonalRotation();
        }

        // 螺旋旋转模式
        if (useSpiralRotation)
        {
            ApplySpiralRotation(ref finalPosition);
        }

        // 波浪旋转模式
        if (useWaveRotation)
        {
            ApplyWaveRotation();
        }

        // 随机旋转模式
        if (useRandomRotation)
        {
            ApplyRandomRotation();
        }

        // 摆锤旋转模式
        if (usePendulumRotation)
        {
            ApplyPendulumRotation();
        }

        // 应用最终位置（主要用于螺旋模式）
        transform.position = finalPosition;
    }

    // 基础旋转方法
    private void ApplyBasicRotation()
    {
        float rotationAmount = rotationSpeed * Time.deltaTime;

        if (useLocalSpace)
        {
            transform.Rotate(rotationAxis, rotationAmount, Space.Self);
        }
        else
        {
            transform.Rotate(rotationAxis, rotationAmount, Space.World);
        }
    }

    // 斜向旋转 - 在两个斜轴之间切换
    private void ApplyDiagonalRotation()
    {
        diagonalTime += Time.deltaTime;

        // 计算当前使用的轴（在两个斜轴之间插值）
        float t = Mathf.PingPong(diagonalTime / axisSwitchInterval, 1f);
        Vector3 currentAxis = Vector3.Slerp(diagonalAxis1.normalized, diagonalAxis2.normalized, t);

        float rotationAmount = rotationSpeed * Time.deltaTime;

        if (useLocalSpace)
        {
            transform.Rotate(currentAxis, rotationAmount, Space.Self);
        }
        else
        {
            transform.Rotate(currentAxis, rotationAmount, Space.World);
        }
    }

    // 螺旋旋转 - 同时旋转并上下移动
    private void ApplySpiralRotation(ref Vector3 position)
    {
        // 基础旋转
        float rotationAmount = rotationSpeed * Time.deltaTime;
        transform.Rotate(rotationAxis, rotationAmount, useLocalSpace ? Space.Self : Space.World);

        // 螺旋运动
        float angle = timeAccumulator * spiralSpeed;
        float x = Mathf.Cos(angle) * spiralRadius;
        float z = Mathf.Sin(angle) * spiralRadius;
        float y = Mathf.Sin(angle * 2f) * spiralHeightChange; // 上下运动

        position = originalPosition + new Vector3(x, y, z);
    }

    // 波浪旋转 - 旋转速度呈波浪变化
    private void ApplyWaveRotation()
    {
        // 使用正弦波调整旋转速度
        float wave = Mathf.Sin(timeAccumulator * waveFrequency) * waveAmplitude;
        float currentSpeed = rotationSpeed + wave;

        float rotationAmount = currentSpeed * Time.deltaTime;

        if (useLocalSpace)
        {
            transform.Rotate(rotationAxis, rotationAmount, Space.Self);
        }
        else
        {
            transform.Rotate(rotationAxis, rotationAmount, Space.World);
        }
    }

    // 随机旋转 - 随机改变旋转轴和速度
    private void ApplyRandomRotation()
    {
        randomTime += Time.deltaTime;

        // 定期生成新的随机旋转参数
        if (randomTime >= randomChangeInterval)
        {
            GenerateNewRandomRotation();
            randomTime = 0f;
        }

        float rotationAmount = currentRandomSpeed * Time.deltaTime;

        if (useLocalSpace)
        {
            transform.Rotate(currentRandomAxis, rotationAmount, Space.Self);
        }
        else
        {
            transform.Rotate(currentRandomAxis, rotationAmount, Space.World);
        }
    }

    // 生成新的随机旋转参数
    private void GenerateNewRandomRotation()
    {
        currentRandomAxis = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ).normalized;

        currentRandomSpeed = Random.Range(10f, maxRandomSpeed);
    }

    // 摆锤旋转 - 在一定角度范围内来回摆动
    private void ApplyPendulumRotation()
    {
        pendulumAngleAccumulator += Time.deltaTime * pendulumSpeed;

        // 使用正弦函数创建摆动效果
        float angle = Mathf.Sin(pendulumAngleAccumulator) * pendulumAngle;

        // 应用旋转
        if (useLocalSpace)
        {
            transform.rotation = originalRotation * Quaternion.AngleAxis(angle, rotationAxis);
        }
        else
        {
            transform.rotation = Quaternion.AngleAxis(angle, rotationAxis) * originalRotation;
        }
    }

    // 公共方法 - 用于外部控制

    // 设置斜向旋转轴
    public void SetDiagonalAxes(Vector3 axis1, Vector3 axis2)
    {
        diagonalAxis1 = axis1.normalized;
        diagonalAxis2 = axis2.normalized;
    }

    // 设置螺旋参数
    public void SetSpiralParameters(float radius, float speed, float height)
    {
        spiralRadius = radius;
        spiralSpeed = speed;
        spiralHeightChange = height;
    }

    // 设置波浪参数
    public void SetWaveParameters(float frequency, float amplitude)
    {
        waveFrequency = frequency;
        waveAmplitude = amplitude;
    }

    // 设置摆锤参数
    public void SetPendulumParameters(float angle, float speed)
    {
        pendulumAngle = angle;
        pendulumSpeed = speed;
    }

    // 重置到初始状态
    public void ResetRotation()
    {
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        timeAccumulator = 0f;
        diagonalTime = 0f;
        randomTime = 0f;
        pendulumAngleAccumulator = 0f;
    }

    // 切换旋转模式
    public void SetRotationMode(bool diagonal, bool spiral, bool wave, bool random, bool pendulum)
    {
        useDiagonalRotation = diagonal;
        useSpiralRotation = spiral;
        useWaveRotation = wave;
        useRandomRotation = random;
        usePendulumRotation = pendulum;
    }
}