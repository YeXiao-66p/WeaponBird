inline float unity_noise_randomValue (float2 uv)
{
    return frac(sin(dot(uv, float2(12.9898, 78.233)))*43758.5453);
}

inline float unity_noise_interpolate (float a, float b, float t)
{
    return (1.0-t)*a + (t*b);
}

inline float unity_valueNoise (float2 uv)
{
    float2 i = floor(uv);
    float2 f = frac(uv);
    f = f * f * (3.0 - 2.0 * f);

    uv = abs(frac(uv) - 0.5);
    float2 c0 = i + float2(0.0, 0.0);
    float2 c1 = i + float2(1.0, 0.0);
    float2 c2 = i + float2(0.0, 1.0);
    float2 c3 = i + float2(1.0, 1.0);
    float r0 = unity_noise_randomValue(c0);
    float r1 = unity_noise_randomValue(c1);
    float r2 = unity_noise_randomValue(c2);
    float r3 = unity_noise_randomValue(c3);

    float bottomOfGrid = unity_noise_interpolate(r0, r1, f.x);
    float topOfGrid = unity_noise_interpolate(r2, r3, f.x);
    float t = unity_noise_interpolate(bottomOfGrid, topOfGrid, f.y);
    return t;
}

void Unity_SimpleNoise_float(float2 UV, float Scale, out float Out)
{
    float t = 0.0;

    float freq = pow(2.0, float(0));
    float amp = pow(0.5, float(3-0));
    t += unity_valueNoise(float2(UV.x*Scale/freq, UV.y*Scale/freq))*amp;

    freq = pow(2.0, float(1));
    amp = pow(0.5, float(3-1));
    t += unity_valueNoise(float2(UV.x*Scale/freq, UV.y*Scale/freq))*amp;

    freq = pow(2.0, float(2));
    amp = pow(0.5, float(3-2));
    t += unity_valueNoise(float2(UV.x*Scale/freq, UV.y*Scale/freq))*amp;

    Out = t;
}

void Unity_RandomRange_float(float2 Seed, float Min, float Max, out float Out)
{
    float randomno =  frac(sin(dot(Seed, float2(12.9898, 78.233)))*43758.5453);
    Out = lerp(Min, Max, randomno);
}

void Unity_Rectangle_Fastest_float(float2 UV, float Width, float Height, out float Out)
{
    float2 d = abs(UV * 2 - 1) - float2(Width, Height);
#if defined(SHADER_STAGE_RAY_TRACING)
    d = saturate((1 - saturate(d * 1e7)));
#else
    d = saturate(1 - d / fwidth(d));
#endif
    Out = min(d.x, d.y);
}

void Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax, out float Out)
{
    Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
}

void Unity_PolarCoordinates_float(float2 UV, float2 Center, float RadialScale, float LengthScale, out float2 Out)
{
    //float2 delta = UV - Center;
    //float radius = length(delta) * 2 * RadialScale;
    //float angle = atan2(delta.x, delta.y) * 1.0/6.28 * LengthScale;
    // atan2: -PI ~ PI, angle : -0.5 ~ -.0.5
    //Out = float2(radius, angle);

    float2 delta = UV - Center;
    float radius = length(delta) * 2 * RadialScale;
    float angle = (atan2(delta.x, delta.y) * 1.0/6.28  + 0.5) * LengthScale;
    // angle : 0 ~ 1
    Out = float2(radius, angle);
}

void Unity_Rotate_Degrees_float(float2 UV, float2 Center, float Rotation, out float2 Out)
{
    Rotation = Rotation * (3.1415926f/180.0f);
    UV -= Center;
    float s = sin(Rotation);
    float c = cos(Rotation);
    float2x2 rMatrix = float2x2(c, -s, s, c);
    rMatrix *= 0.5;
    rMatrix += 0.5;
    rMatrix = rMatrix * 2 - 1;
    UV.xy = mul(UV.xy, rMatrix);
    UV += Center;
    Out = UV;
}

void Unity_TriangleWave_float(float In, out float Out)
{
    // 
    Out = 2.0 * abs( 2 * (In - floor(0.5 + In)) ) - 1.0;
}

void Unity_SquareWave_float(float In, out float Out)
{   
    // -1 ~ 1
    Out = 1.0 - 2.0 * round(frac(In));
}

void Unity_SawtoothWave_float(float In, out float Out)
{
    Out = 2 * (In - floor(0.5 + In));
}

void Unity_NoiseSineWave_float(float In, float2 MinMax, out float Out)
{
    float sinIn = sin(In);
    float sinInOffset = sin(In + 1.0);
    float randomno =  frac(sin((sinIn - sinInOffset) * (12.9898 + 78.233))*43758.5453);
    float noise = lerp(MinMax.x, MinMax.y, randomno);
    Out = sinIn + noise;
}