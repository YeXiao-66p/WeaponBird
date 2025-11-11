Shader "MyCustom/BossLeafParticle"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color Tint", Color) = (1,1,1,1)
        _Soft ("Soft Edge", Range(0,1)) = 0.3
        _Glow ("Glow Strength", Range(0,3)) = 1
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend One OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _Color;
            float _Soft;
            float _Glow;

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv * 2 - 1;
                float r = length(uv);
                float edge = smoothstep(1, 1 - _Soft, r);

                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                col.rgb *= (1 - edge) * _Glow;
                col.a *= (1 - edge);
                return col;
            }
            ENDCG
        }
    }
}
