Shader "Hidden/NewImageEffectShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Mask("Mask", 2D) = "white" {}
		_HorizontalLines("HorizontalLines", 2D) = "white" {}
		_Bubbles("Bubles", 2D) = "white" {}
		_NoiseLines("NoiseLines", 2D) = "white" {}
		_LinesLerp("LinesLerp", Range(-2, 2)) = 0
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha

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

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _Mask;
			sampler2D _HorizontalLines;
			sampler2D _Bubbles;
			sampler2D _NoiseLines;
			fixed _LinesLerp;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 mask = tex2D(_Mask, i.uv);
				fixed4 noiseLines = tex2D(_NoiseLines, fixed2(i.uv.x, i.uv.y + _Time.y * 0.1));
				fixed4 bubbles = tex2D(_Bubbles, fixed2(i.uv.x + _Time.w * 0.05, i.uv.y + _Time.y * 0.08));

				col += noiseLines * col;
				col += bubbles * col * 0.5;
				//col *=  + 0.2;
				//col *= ;
				col *= mask;
				return col;
			}
			ENDCG
		}
	}
}
