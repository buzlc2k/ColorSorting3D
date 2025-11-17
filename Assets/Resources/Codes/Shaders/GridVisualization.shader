Shader "Custom/GridVisulization"
{
    Properties
    {
        _GridColor ("Grid Color", Color) = (1, 1, 1, 1)
        _BackgroundColor ("Background Color", Color) = (0, 0, 0, 1)
        _CellWidth ("Cell Width", Float) = 1.0
        _CellHeight ("Cell Height", Float) = 1.0
        _LineWidth ("Line Width", Range(0.01, 0.1)) = 0.05
    }
    
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        
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
                float2 worldScale : TEXCOORD1;
            };
            
            float4 _GridColor;
            float4 _BackgroundColor;
            float _CellWidth;
            float _CellHeight;
            float _LineWidth;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                
                // Lấy world scale từ transform matrix
                float3 worldScale = float3(
                    length(unity_ObjectToWorld._m00_m10_m20),
                    length(unity_ObjectToWorld._m01_m11_m21),
                    length(unity_ObjectToWorld._m02_m12_m22)
                );
                o.worldScale = worldScale.xz;
                
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // Center UV coordinates (-0.5 to 0.5)
                float2 centeredUV = (i.uv - 0.5) * i.worldScale;
                float2 grid = frac(centeredUV / float2(_CellWidth, _CellHeight));
                
                float2 lines = step(grid, _LineWidth) + step(1.0 - _LineWidth, grid);
                float gridMask = saturate(lines.x + lines.y);
                
                return lerp(_BackgroundColor, _GridColor, gridMask);
            }
            
            ENDCG
        }
    }
}