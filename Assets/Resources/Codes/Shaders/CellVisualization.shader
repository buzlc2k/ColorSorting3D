Shader "Unlit/CellVisualization"
{
    Properties
    {
        _BorderColor ("Border Color", Color) = (0, 1, 0, 1)
        _FillColor ("Fill Color", Color) = (0.2, 0.2, 0.2, 1)
        _BorderWidth ("Border Width", Range(0.01, 0.5)) = 0.1
        _EdgeSoftness ("Edge Softness", Range(0.001, 0.1)) = 0.02
    }
    
    SubShader
    {
        Tags { 
            "RenderType"="Opaque" 
            "Queue"="Geometry"
            "LightMode"="Always"
        }
        
        LOD 100
        
        ZWrite On
        Cull Off
        Lighting Off
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            
            struct Attributes
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            struct Varyings
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };
            
            fixed4 _BorderColor;
            fixed4 _FillColor;
            float _BorderWidth;
            float _EdgeSoftness;
            
            Varyings vert(Attributes v)
            {
                Varyings o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            fixed4 frag(Varyings i) : SV_Target
            {
                float2 uv = i.uv;
                
                // Tính khoảng cách từ UV đến các cạnh
                float2 distToEdge = min(uv, 1.0 - uv);
                float minDistToEdge = min(distToEdge.x, distToEdge.y);
                
                // Tạo border với smoothstep để có viền mượt
                float borderMask = 1.0 - smoothstep(_BorderWidth - _EdgeSoftness, _BorderWidth + _EdgeSoftness, minDistToEdge);
                
                // Kết hợp màu border và màu fill
                fixed4 finalColor = lerp(_FillColor, _BorderColor, borderMask);
                
                return finalColor;
            }
            ENDCG
        }
    }
    
    // Fallback cho Built-in Render Pipeline
    SubShader
    {
        Tags { 
            "RenderType"="Opaque" 
            "Queue"="Geometry"
        }
        
        LOD 100
        
        ZWrite On
        Cull Off
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };
            
            fixed4 _BorderColor;
            fixed4 _FillColor;
            float _BorderWidth;
            float _EdgeSoftness;
            
            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;
                
                // Tính khoảng cách từ UV đến các cạnh
                float2 distToEdge = min(uv, 1.0 - uv);
                float minDistToEdge = min(distToEdge.x, distToEdge.y);
                
                // Tạo border với smoothstep để có viền mượt
                float borderMask = 1.0 - smoothstep(_BorderWidth - _EdgeSoftness, _BorderWidth + _EdgeSoftness, minDistToEdge);
                
                // Kết hợp màu border và màu fill
                fixed4 finalColor = lerp(_FillColor, _BorderColor, borderMask);
                
                return finalColor;
            }
            ENDCG
        }
    }
    
    FallBack "Diffuse"
}