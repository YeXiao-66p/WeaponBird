Shader "Sprites/Default Instanced"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
        
        // 实例化属性
        [HideInInspector] _InstancePosition ("Instance Position", Vector) = (0,0,0,0)
        [HideInInspector] _InstanceRotation ("Instance Rotation", Float) = 0
        [HideInInspector] _InstanceScale ("Instance Scale", Vector) = (1,1,1,1)
        [HideInInspector] _InstanceColor ("Instance Color", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex SpriteVertInstanced
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_instancing
            #pragma multi_compile _ PIXELSNAP_ON
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA

            #include "UnityCG.cginc"

            #ifdef UNITY_INSTANCING_ENABLED

                UNITY_INSTANCING_BUFFER_START(PerDrawSprite)
                    // SpriteRenderer.Color while Non-Batched/Instanced.
                    UNITY_DEFINE_INSTANCED_PROP(fixed4, unity_SpriteRendererColorArray)
                    // this could be smaller but that's how bit each entry is regardless of type
                    UNITY_DEFINE_INSTANCED_PROP(fixed2, unity_SpriteFlipArray)
                UNITY_INSTANCING_BUFFER_END(PerDrawSprite)

                #define _RendererColor  UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, unity_SpriteRendererColorArray)
                #define _Flip           UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, unity_SpriteFlipArray)

            #endif // instancing

            CBUFFER_START(UnityPerDrawSprite)
            #ifndef UNITY_INSTANCING_ENABLED
                fixed4 _RendererColor;
                fixed2 _Flip;
            #endif
                float _EnableExternalAlpha;
            CBUFFER_END

            sampler2D _MainTex;
            sampler2D _AlphaTex;
            float4 _MainTex_TexelSize;

            fixed4 _Color;

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            // 实例化数据
            UNITY_INSTANCING_BUFFER_START(InstanceProperties)
                UNITY_DEFINE_INSTANCED_PROP(float4, _InstancePosition)
                UNITY_DEFINE_INSTANCED_PROP(float, _InstanceRotation)
                UNITY_DEFINE_INSTANCED_PROP(float4, _InstanceScale)
                UNITY_DEFINE_INSTANCED_PROP(fixed4, _InstanceColor)
            UNITY_INSTANCING_BUFFER_END(InstanceProperties)

            // 2D旋转矩阵
            float2x2 RotationMatrix(float angle)
            {
                float s, c;
                sincos(angle, s, c);
                return float2x2(c, -s, s, c);
            }

            v2f SpriteVertInstanced(appdata_t IN)
            {
                v2f OUT;
                
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_TRANSFER_INSTANCE_ID(IN, OUT);

                // 获取实例化数据
                float4 instancePos = UNITY_ACCESS_INSTANCED_PROP(InstanceProperties, _InstancePosition);
                float instanceRot = UNITY_ACCESS_INSTANCED_PROP(InstanceProperties, _InstanceRotation);
                float4 instanceScale = UNITY_ACCESS_INSTANCED_PROP(InstanceProperties, _InstanceScale);
                fixed4 instanceColor = UNITY_ACCESS_INSTANCED_PROP(InstanceProperties, _InstanceColor);

                // 应用实例化变换
                float3 worldPos = IN.vertex.xyz;
                
                // 缩放
                worldPos.xy *= instanceScale.xy;
                
                // 旋转
                if (instanceRot != 0)
                {
                    worldPos.xy = mul(RotationMatrix(instanceRot), worldPos.xy);
                }
                
                // 平移
                worldPos.xy += instancePos.xy;

                // 使用变换后的位置
                OUT.vertex = UnityObjectToClipPos(float4(worldPos, 1));

                // 处理翻转
                #ifdef UNITY_INSTANCING_ENABLED
                    IN.texcoord.xy = float2((IN.texcoord.x - 0.5f) * _Flip.x + 0.5f, (IN.texcoord.y - 0.5f) * _Flip.y + 0.5f);
                #endif

                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _Color * _RendererColor * instanceColor;

                #ifdef PIXELSNAP_ON
                OUT.vertex = UnityPixelSnap(OUT.vertex);
                #endif

                return OUT;
            }

            fixed4 SampleSpriteTexture(float2 uv)
            {
                fixed4 color = tex2D(_MainTex, uv);

                #if ETC1_EXTERNAL_ALPHA
                fixed4 alpha = tex2D(_AlphaTex, uv);
                color.a = lerp(color.a, alpha.r, _EnableExternalAlpha);
                #endif

                return color;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(IN);

                fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;
                c.rgb *= c.a;
                return c;
            }
            ENDCG
        }
    }
}