Shader "Unlit/Shield"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Fade("Fade", Float) = 0.1
		_Col ("Color", Color) = (1,1,1,1)
		_Rim ("Rim light strength", Float) = 2
			_ScSpd ("UV Scroll Speed", Float) = 1
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "RenderQueue"="Transparent" }
		LOD 100
		Blend One One
		ZWrite Off

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
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float3 objPos : TEXCOORD1;
				float3 normal : TEXCOORD2;
				float3 worldPos : TEXCOORD3;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Fade;
			float _Rim;
			fixed4 _Col;
			float _ScSpd;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.objPos = v.vertex;
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.normal = UnityObjectToWorldNormal(v.normal);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				float3 worldVDir = normalize(UnityWorldSpaceViewDir(i.worldPos));
				fixed4 col = _Col * tex2D(_MainTex, i.uv + float2(_Time.y * _ScSpd, 0));
				float yDist = 1-abs(i.objPos.y) * 2;

				float t = frac(_Time.y) * (1+_Fade);
				float d = yDist;
				float dd = 0;

				if (d < t && d > t - _Fade) {
					dd = 1-((t - d) / _Fade);
				}

				float rimL = saturate(dot(worldVDir, i.normal) * _Rim);
				rimL = 1 - rimL;

				return col + col * dd + col*rimL;
			}
			ENDCG
		}
	}
}
