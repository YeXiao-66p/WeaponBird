Shader "MyCustom/BasicRimLightRec"
{
    Properties
    {
       _RimLightColor          ("RimLightColor",       Color)              = (1,1,1,1)
       _RimLightPower          ("RimLightPower",       Range(0, 10))        = 1
       _RimLightIntensity      ("RimLightIntensity",   Range(0, 10))       = 1
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

            struct appdata
            {
                float4 vertex           :POSITION;
                float3 normal           :NORMAL;
            };

            struct v2f
            {
                
                float4 vertex           :SV_POSITION;         
                float3 worldNormal      :TEXCOORD0;
                float3 worldView        :TEXCOORD1;
            };


            v2f vert (appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);

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
                return float4(fresnel, 1);
            }
            ENDCG
        }
    }
}
