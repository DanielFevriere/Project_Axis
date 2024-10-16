Shader "Infraxis/UI/Health"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Health("Health", Range(0,1)) = .5
		_LowHealthColor("LowHealthColor", Color) = (1,0,0,1)
		_HighHealthColor("HighHealthColor", Color) = (0,1,0,1)
		_Opacity ("Opacity", Range(0, 1)) = 1

	}
		SubShader
	{
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" "PreviewType" = "Plane" }
		LOD 100

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			struct MeshData
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct Interpolators
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST; //describes the resolution of the texture because diff textures will have diff resolutions
			float _Health;
			float4 _LowHealthColor;
			float4 _HighHealthColor;
			float minHealth;
			float _Opacity;


			float InverseLerp(float a, float b, float t) 
			{
				return (t - a) / (b - a);
			}

			Interpolators vert(MeshData v)
			{
				Interpolators o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			//Fragment shader
			fixed4 frag(Interpolators i) : SV_Target
			{
				minHealth = 0.2;

				// sample the texture
				fixed4 col;
				col.rg = i.uv;
				col.a = 1;
				col.rgb = 0; //This changes the color of the background hp bar

				//This changes the color of the filled hp bar
				if (_Health > i.uv.x)
				{
					float t = saturate(InverseLerp(0.2, 0.7, _Health));

					float3 hh = _HighHealthColor * smoothstep(0.3, 1, i.uv.y);
					float3 lh = _LowHealthColor * smoothstep(0.3, 1, i.uv.y);

					col.rgb = lerp(lh, hh, t);

					//If the Health reaches the minHealth value, it automatically becomes the lowhealth color
					if (_Health <= 0.2)
					{
						col.rgb = lh;
						col.rgb *= (1 + (cos(2 * 3.141592 * _Time.y * 1.5) * 0.5 + 0.5) * 0.75);
					}
				}
				else 
				{
					col.a = _Opacity;
				}

				// apply fog
				return col;
		}
		ENDCG
	}
	}
}
