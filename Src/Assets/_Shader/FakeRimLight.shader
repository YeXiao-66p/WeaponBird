Shader "MyCustom/FakeRimLight"
{
    Properties
    {
       _FakeRimLightTex        ("RimLightColor",       2D)                  = "white" {}
       _RimLightColor          ("RimLightColor",       Color)               = (1,1,1,1)
       _RimLightPower          ("RimLightPower",       Range(0, 10))        = 1
       _RimLightIntensity      ("RimLightIntensity",   Range(0, 10))        = 1
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
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            float _RimLightIntensity; 
            float _RimLightPower; 
            float4 _RimLightColor; 
            sampler2D _FakeRimLightTex;



            struct appdata
            {
                float4 vertex           :POSITION;
                float2 uv               :TEXCOORD0;
                float3 normal           :NORMAL;
            };

            struct v2f
            {
                
                float4 vertex           :SV_POSITION;      
                float2 uv               :TEXCOORD0;
                float3 worldNormal      :TEXCOORD1;
                float3 worldView        :TEXCOORD2;
            };


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

                o.worldNormal = UnityObjectToWorldNormal(v.normal);

                float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.worldView = normalize(_WorldSpaceCameraPos.xyz - worldPos.xyz);
                return o;
            }

            float3 funFresnel(float3 worldNormal, float3 worldView)
            { 
                float nv = max(0, dot(worldNormal, worldView));

                float3 fresnel = pow(1- nv, _RimLightPower) * _RimLightIntensity * _RimLightColor.rgb;

                return fresnel;
            }
            fixed4 frag (v2f i) : SV_Target
            {

                float3 fresnel = funFresnel(i.worldNormal, i.worldView);

                float u = (i.worldNormal.x * 0.5) + 0.5;

                float3 fakeRimLight = tex2D(_FakeRimLightTex, float2(u, i.uv.y)).rgb;

                float3 finalColor = fakeRimLight * fresnel;
                return float4(finalColor, 1);
            }
            ENDCG
        }
    }
}
