Shader "Hidden/Stun"
{
	Properties
	{
		[Hideininspector]_MainTex ("Texture", 2D) = "white" {}
		_ShockTexture("ShockTexture", 2D) = "white" {}
		_Black("Black", 2D) = "black" {}
		_MaskRange("MaskRange", Range(0, 1)) = 0
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

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

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _ShockTexture;
			sampler2D _Black;
			fixed _MaskRange;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 maintext = tex2D(_MainTex, i.uv);
				fixed4 col = tex2D(_ShockTexture, i.uv);
				fixed4 black = tex2D(_Black, i.uv);

				//float sine = sin(_Time.w*0.5);
				fixed4 finalColor = lerp(col, black, _MaskRange);
				// just invert the colors
				//col = 1 - col;
				return finalColor + maintext;
			}
			ENDCG
		}
	}
}
