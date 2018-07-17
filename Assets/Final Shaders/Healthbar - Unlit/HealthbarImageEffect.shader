Shader "Hidden/Healthbar"
{
	Properties
	{
		[Hideininspector]_MainTex("Texture", 2D) = "white" {}
		_BloodTexture("BloodTexture", 2D) = "white" {}
		_BloodTexture2("BloodTexture2", 2D) = "white" {}
		_BloodTexture3("BloodTexture3", 2D) = "white" {}
		_Mask("Mask", 2D) = "white" {}
		//_Noise1("Noise1", 2D) = "white" {}
		//_Noise2("Noise2", 2D) = "white" {}
		_Noise("Noise", 2D) = "white" {}
		_Texture2Speed("Texture2Speed", Range(-10, 10)) = 1
		_NoiseSpeed("NoiseSpeed", Range(-10, 10)) = 1
		//_LerpSpeedNoise1and2("LerpSpeedNoise1and2", Range(-10, 10)) = 1
		_Brigthness("Brigthness", Range(-10, 10)) = 1
		//_YRange("YRange", Range(-10, 10)) = 1
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
			sampler2D _BloodTexture;
			sampler2D _BloodTexture2;
			sampler2D _BloodTexture3;
			sampler2D _Mask;
			//sampler2D _Noise1;
			//sampler2D _Noise2;
			sampler2D _Noise;
			fixed _Texture2Speed;
			fixed _NoiseSpeed;
			//fixed _LerpSpeedNoise1and2;
			fixed _Brigthness;
			//fixed _YRange;

			fixed4 frag (v2f i) : SV_Target
			{
				//fixed4 noise1 = tex2D(_Noise1, fixed2(i.uv.x, i.uv.y + _Time.y));
				//fixed4 noise2 = tex2D(_Noise2, fixed2(i.uv.x + _Time.x, i.uv.y + _Time.x));
				fixed4 noise = tex2D(_Noise, fixed2(i.uv.x , i.uv.y + _Time.y * _NoiseSpeed));

				//fixed4 finalNoise = lerp(noise2, noise1, _Time.x * _LerpSpeedNoise1and2);

				fixed4 col = tex2D(_BloodTexture, noise);
				fixed4 col2 = tex2D(_BloodTexture2, fixed2(i.uv.x, i.uv.y + _Time.y * _Texture2Speed));
				fixed4 col3 = tex2D(_BloodTexture3, noise);
				fixed4 mask = tex2D(_Mask, i.uv);

				col.rgba *= (col2.rgba * col3.rgba * _Brigthness);
				col *= mask;
				return col;
			}
			ENDCG
		}
	}
}
