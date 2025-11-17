Shader "RepeatTexture"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BaseScale ("Base Scale", Vector) = (1, 1, 1, 0)
        _Color ("Color", Color) = (1,1,1,1)
        _Transparency ("Transparency", Range(0, 1)) = 1
    }
    
    SubShader
    {
        Tags { "Queue"="AlphaTest" "RenderType"="TransparentCutout" "IgnoreProjector"="True" }
        LOD 100
        
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite On
        Cull Back

        Pass
        {
            Tags {"LightMode"="ForwardBase"}
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float2 scaledUV : TEXCOORD1;
                float3 worldNormal : TEXCOORD2;
                float3 worldPos : TEXCOORD3;
                SHADOW_COORDS(4)
                UNITY_FOG_COORDS(5)
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _BaseScale;
            float4 _Color;
            float _Transparency;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                
                float3 currentScale = float3(
                    length(unity_ObjectToWorld._m00_m10_m20),
                    length(unity_ObjectToWorld._m01_m11_m21),
                    length(unity_ObjectToWorld._m02_m12_m22)
                );
                
                float2 scaleRatio = float2(
                    currentScale.x / _BaseScale.x,
                    currentScale.y / _BaseScale.y
                );
                
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                
                o.scaledUV = v.uv * scaleRatio;
                
                TRANSFER_SHADOW(o)
                
                UNITY_TRANSFER_FOG(o, o.vertex);
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.scaledUV) * _Color;
                
                col.a *= _Transparency;
                
                float3 worldNormal = normalize(i.worldNormal);
                float3 worldLightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));
                
                float NdotL = max(0, dot(worldNormal, worldLightDir));
                fixed3 diffuse = col.rgb * _LightColor0.rgb * NdotL;
                
                fixed3 ambient = col.rgb * UNITY_LIGHTMODEL_AMBIENT.rgb;
                
                UNITY_LIGHT_ATTENUATION(atten, i, i.worldPos)
                
                fixed3 lighting = ambient + diffuse * atten;
                fixed4 finalColor = fixed4(lighting, col.a);
                
                UNITY_APPLY_FOG(i.fogCoord, finalColor);
                
                return finalColor;
            }
            ENDCG
        }
        
        Pass
        {
            Tags {"LightMode"="ForwardAdd"}
            Blend SrcAlpha One
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdadd_fullshadows
            
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float2 scaledUV : TEXCOORD1;
                float3 worldNormal : TEXCOORD2;
                float3 worldPos : TEXCOORD3;
                SHADOW_COORDS(4)
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _BaseScale;
            float4 _Color;
            float _Transparency;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                
                float3 currentScale = float3(
                    length(unity_ObjectToWorld._m00_m10_m20),
                    length(unity_ObjectToWorld._m01_m11_m21),
                    length(unity_ObjectToWorld._m02_m12_m22)
                );
                
                float2 scaleRatio = float2(
                    currentScale.x / _BaseScale.x,
                    currentScale.y / _BaseScale.y
                );
                
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                
                o.scaledUV = v.uv * scaleRatio;
                
                TRANSFER_SHADOW(o)
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.scaledUV) * _Color;
                
                col.a *= _Transparency;
                
                float3 worldNormal = normalize(i.worldNormal);
                float3 worldLightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));
                
                float NdotL = max(0, dot(worldNormal, worldLightDir));
                fixed3 diffuse = col.rgb * _LightColor0.rgb * NdotL;
                
                UNITY_LIGHT_ATTENUATION(atten, i, i.worldPos)
                
                return fixed4(diffuse * atten, col.a);
            }
            ENDCG
        }
        
        Pass
        {
            Tags {"LightMode"="ShadowCaster"}
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_shadowcaster
            
            #include "UnityCG.cginc"

            struct v2f
            {
                V2F_SHADOW_CASTER;
            };

            v2f vert(appdata_base v)
            {
                v2f o;
                TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    
    FallBack "Transparent/Cutout/VertexLit"
}