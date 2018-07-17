Shader "Hidden/LowHealth"
{
	Properties
	{
		[Hideininspector]_MainTex("Texture", 2D) = "white" {}
		_BloodTexture("BloodTexture", 2D) = "white" {}
		//_MaskRange("MaskRange", float) = 0
		_GreyRange("GreyRange", Range(0, 10)) = 1
		_FinalGreyscale("FinalGreyscale", Range(0, 1)) = 0
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
			sampler2D _BloodTexture;
			fixed _GreyRange;
			fixed _FinalGreyscale;
			//fixed _MaskRange;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 blood = tex2D(_BloodTexture, i.uv);
				fixed4 black = (0, 0, 0, 1);

				fixed4 greyscale = ((col.r + col.g + col.b) * _GreyRange);

				blood = lerp(blood, black, (sin(_Time.w*0.5)));//_MaskRange
				greyscale = greyscale * blood;
				fixed4 finalColor = lerp(col, greyscale, _FinalGreyscale);
				return finalColor;
			}
			ENDCG
		}
	}
}
