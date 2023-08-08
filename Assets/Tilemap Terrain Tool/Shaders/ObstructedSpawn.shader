Shader "Anthony/Unlit/ObstructedSpawn"
{
    Properties
    {
        _Color ("Color", Color) = (1, 0.0, .1, 0.5)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 wPos : TEXCOORD0;
                float3 normal : TEXCOORD1;
            };

            float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.wPos = mul(UNITY_MATRIX_M, float4(v.vertex.xyz, 1)); // tangent to world
                o.normal = mul((float3x3)UNITY_MATRIX_M, v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Fresnel
                float3 viewDir = normalize(_WorldSpaceCameraPos - i.wPos);

                float3 normal = normalize(i.normal);

                float fresnel = pow(1 - dot(viewDir, normal), 2);
                fresnel = lerp(0.3, .8, fresnel);
                
                return float4(_Color.xyz, fresnel);
            }
            ENDCG
        }
    }
}
