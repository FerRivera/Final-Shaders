Shader "Hidden/VesselReplacement" {
	Properties {
		//_Color ("Color", Color) = (1,1,1,1)
		[HideInInspector]_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Albedo("Albedo", 2D) = "white" {}
		//_Normal("Normal", 2D) = "bump" {}
		_AO("AO", 2D) = "white" {}
		//_LineText("LineText", 2D) = "white" {}
		//_Metallic("Metallic", 2D) = "white" {}
		//_Glossiness ("Smoothness", Range(0,1)) = 0.5
		//_Brigthness("Brigthness", Range(0,1)) = 0
		//_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader 
	{
		//Name "FORWARD"
		Tags 
		{
			"RenderType"="Opaque"
			"LightMode" = "ForwardBase"
			"Vessels" = "Vessels"
		}

		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite Off

		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows
		//#pragma surface surf Lambert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _Albedo;
		
		sampler2D _AO;

		struct Input 
		{
			float2 uv_Albedo;
			//float2 uv_Normal;
			float2 uv_AO;
		};
		//sampler2D _Normal;
		//fixed4 _Brigthness;

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D(_Albedo, IN.uv_Albedo);
			//fixed4 n = tex2D(_Normal, IN.uv_Normal);
			fixed4 ao = tex2D(_AO, IN.uv_AO);

			o.Albedo = c.rgb;
			//o.Normal = UnpackNormal(tex2D(_Normal, IN.uv_Normal));
			o.Occlusion = ao;
			//o.Normal = n;

			// Metallic and smoothness come from slider variables
			//o.Metallic = _Brigthness;
			//o.Smoothness = _Brigthness;
			//o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
