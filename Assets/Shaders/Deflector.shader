Shader "Unlit/Deflector"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Col ("Color", Color) = (1,1,1,1)
		_Fade ("Fade", Float) = 0.1
		_Amb ("Ambient", Float) = 0.1
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" }
		LOD 100
		Blend One One

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
			fixed4 _Col;
			float _Fade;
			float _Amb;

			float tWave(float time) {
				return time % 1;
			}
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
			i.uv /= _MainTex_ST.xy;
			i.uv -= _MainTex_ST.zw;
				float t = tWave(_Time.y);
				float d = length(i.uv - float2(0.5, 0.5));

				if (d < t && d > t - _Fade) {
					d = 1 - (t - d) / (_Fade);
				}
				else d = 0;

				i.uv *= _MainTex_ST.xy;
				i.uv += _MainTex_ST.zw;

				return saturate(_Col * ((_SinTime.y*0.5)+0.5) + col * (_Amb + d));
			}
			ENDCG
		}
	}
}
