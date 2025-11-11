Shader "MyCustom/SchlickFresnel"
{
    Properties
    {
        _FakeRimLightTex        ("_FakeRimLightTex",        2D)             = "white" {}
        _RimLightColor0         ("_RimLightColor0",          Color)         = (1, 0, 0, 1)
        _RimLightColor1         ("_RimLightColor1",          Color)         = (0, 0, 1, 1)
        _RimLightPower          ("_RimLightPower",          Range(0, 10))   = 1
        _RimLightIntensity      ("_RimLightIntensity",      Range(0, 10))   = 1
        _SchlickFresnelBias     ("_SchlickFresnelBias",     Range(0, 2))    = 0.6
        _SchlickFresnelEta      ("_SchlickFresnelEta",      Range(0, 10))   = 0.4
        _AngleMin               ("_AngleMin",               Range(0, 360))  = 0
        _AngleMax               ("_AngleMax",               Range(0, 360))  = 45
        _Inverse                ("_Inverse",                Range(-1, 1))  = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "MyCustomHeader.cginc"

            #define PI 3.14159265359

            float4 _RimLightColor0;
            float4 _RimLightColor1;
            float _RimLightPower;
            float _RimLightIntensity;
            sampler2D _FakeRimLightTex;

            float _SchlickFresnelBias;
            float _SchlickFresnelEta;

            float _AngleMin;
            float _AngleMax;
            float _Inverse;

            struct appdata
            {
                float4 vertex           : POSITION;
                float2 uv               : TEXCOORD0;
                float3 normal           : NORMAL;
            };

            struct v2f
            {
                float4 vertex           : SV_POSITION;
                float2 uv               : TEXCOORD0;
                float3 viewNormal       : TEXCOORD1;
                float3 worldNormal      : TEXCOORD2;
                float3 worldView        : TEXCOORD3;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.viewNormal = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, v.normal));
                o.worldNormal = UnityObjectToWorldNormal(v.normal);

                float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.worldView = normalize(_WorldSpaceCameraPos.xyz - worldPos.xyz);

                return o;
            }


            float funcSetRange(float2 input, float2 output, float value)
            {
                float v;
                Unity_Remap_float(value, input, output, v);
                return v;
            }

            float funcBias(float bias, float t)
            {
                return pow(t, -log2(bias));
            }

            float2 polarCoordinates(float2 uv)
            {
                float2 output;
                Unity_PolarCoordinates_float(uv, 0, 1, 1, output);
                return output;
            }

            float customLerp(float a, float b, float t)
            {
                return (step(a, t) - step(b, t)) * (smoothstep(a, a + (b - a) / 2, t) * (1 - smoothstep(a + (b - a) / 2, b, t)));
            }

            float3 funcSchlickFresnel(float bias, float eta, float intensity, float nv)
            {
                float a = funcSetRange(float2(0, 1), float2(0, 0.5), bias);
                float f0 = funcSetRange(float2(0, 1), float2(0, 0.2), eta);

                //float fresnelFactor = _FresnelRatio + (1 - _FresnelRatio) * pow(1 - dot(viewDir, i.worldNormal), 5);
                float fresnel = f0 + (1 - f0) * pow(1 - nv, 5);
                float SchlickFresnel = funcBias(a, fresnel) * intensity;
                return SchlickFresnel;
            }
            
            float3 funcFresnel(float3 worldNormal, float3 worldView)
            {
                float nv = max(0, dot(worldNormal, worldView));
                float3 fresnel = pow(1 - nv, _RimLightPower) * _RimLightIntensity * _RimLightColor0.rgb;
                return fresnel;
            }

            float3 schlickFresnel(float3 worldNormal, float3 worldView)
            {
                float nv = max(0, dot(worldNormal, worldView));
                float3 fresnel = funcSchlickFresnel(_SchlickFresnelBias, _SchlickFresnelEta, _RimLightIntensity, nv);
                return fresnel;
            }

            float3 schlickFresnelSegment(float3 viewNormal, float3 worldNormal, float3 worldView, float inverse)
            {
                // 计算fresnel的显示区域
                float2 uv = float2(viewNormal.x * inverse, viewNormal.y);
                float2 p = polarCoordinates(uv);
                
                float angleMin = funcSetRange(float2(0, 360), float2(0, 1), _AngleMin);
                float angleMax = funcSetRange(float2(0, 360), float2(0, 1), _AngleMax);
                float t = clamp(p.y, 0, 1);

                float v = customLerp(angleMin, angleMax, t);

                // 计算fresnel
                float3 fresnel = schlickFresnel(worldNormal, worldView);

                // 计算区域fresnel
                float3 fresnelSegment = fresnel * v * pow(p.x, 2);
                return fresnelSegment;
            }

            float3 schlickFresnelSegmentColor(float3 viewNormal, float3 worldNormal, float3 worldView)
            {
                float3 color0 = schlickFresnelSegment(viewNormal, worldNormal, worldView, _Inverse) * _RimLightColor0.rgb;
                float3 color1 = schlickFresnelSegment(viewNormal, worldNormal, worldView, -_Inverse) * _RimLightColor1.rgb;

                return color0 + color1;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //float3 fresnel = schlickFresnel(i.worldNormal, i.worldView);
                //float3 fresnel = funcFresnel(i.worldNormal, i.worldView);
                //float3 fresnel = schlickFresnelSegment(i.viewNormal, i.worldNormal, i.worldView, _Inverse);
                 float3 fresnel = schlickFresnelSegmentColor(i.viewNormal, i.worldNormal, i.worldView);

                return float4(fresnel, 1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
