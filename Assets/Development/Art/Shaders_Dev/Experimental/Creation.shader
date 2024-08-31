Shader "Experimental/Creation"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Resolution ("Resolution (XY)", Vector) = (16, 9, 0, 0)
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

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float4 _Resolution;
            fixed4 frag (v2f IN) : SV_Target
            {
                float3 c;
                float l; 
                float z = _Time.y;
                float2 r = _Resolution.xy;
                float2 uv0 = IN.uv;

                /*uv0 *= 8.0;
                uv0 = floor(uv0);
                uv0 /= 8.0;*/


                for (int i = 0; i < 3; i++) 
                {
                    float2 uv = 0;
                    float2 p = uv0 / r;
                    uv = p;
                    p -= .5;

                    p.x *= r.x / r.y; // resolution
                    z += .07;
                    l = length(p);
                    uv += p / l * (sin(z) + 1.) * abs(sin(l * 9. - z - z));
                    c[i] = .01 / length(fmod(uv, 1.) - .5);
                }
                
                return float4(c / l, 1);
            }
            ENDCG
        }
    }
}
