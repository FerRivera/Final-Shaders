// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "GoldShader"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_EmissionColor("EmissionColor", Color) = (0,0,0,0)
		_LineTexture("LineTexture", 2D) = "white" {}
		_CoinColor("CoinColor", Color) = (0,0,0,0)
		_GoldIntesnity("GoldIntesnity", Float) = 1.2
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha  noshadow 
		struct Input
		{
			float3 worldPos;
		};

		uniform float4 _CoinColor;
		uniform float4 _EmissionColor;
		uniform sampler2D _LineTexture;
		uniform float _GoldIntesnity;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Albedo = _CoinColor.rgb;
			o.Emission = ( ( _EmissionColor * tex2D( _LineTexture,(abs( i.worldPos.xy+_Time[1] * float2(1,0 )))) ) * _GoldIntesnity ).xyz;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=5105
632;192;1266;765;1270.199;422.7284;1.7;True;True
Node;AmplifyShaderEditor.WorldPosInputsNode;43;-866.0251,534.0228;Float;False
Node;AmplifyShaderEditor.PannerNode;49;-637.4253,735.3238;Float;False;1;0;0;FLOAT2;0,0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.SamplerNode;45;-606.5266,494.4238;Float;True;Property;_LineTexture;LineTexture;4;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.RangedFloatNode;53;-217.829,784.8239;Float;False;Property;_GoldIntesnity;GoldIntesnity;5;0;1.2;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;50;-227.9261,413.3239;Float;False;0;COLOR;0.0,0,0,0;False;1;FLOAT4;0.0,0,0,0;False
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;52;-105.829,576.8238;Float;False;0;FLOAT4;0.0;False;1;FLOAT;0.0,0,0,0;False
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;203.5,1.3;Float;False;True;2;Float;ASEMaterialInspector;Standard;GoldShader;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Opaque;0.5;True;False;0;False;Opaque;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;False;0;SrcAlpha;OneMinusSrcAlpha;0;SrcAlpha;OneMinusSrcAlpha;Add;Add;0;False;0.001;0,0,0,0;VertexOffset;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;13;OBJECT;0.0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False
Node;AmplifyShaderEditor.ColorNode;2;-193.2999,164.1;Float;False;Property;_EmissionColor;EmissionColor;1;0;0,0,0,0
Node;AmplifyShaderEditor.ColorNode;56;-196.8001,-57.3;Float;False;Property;_CoinColor;CoinColor;4;0;0,0,0,0
WireConnection;49;0;43;0
WireConnection;45;1;49;0
WireConnection;50;0;2;0
WireConnection;50;1;45;0
WireConnection;52;0;50;0
WireConnection;52;1;53;0
WireConnection;0;0;56;0
WireConnection;0;2;52;0
ASEEND*/
//CHKSM=71DF9659729CB3CE2B667B8829B2D40ADF04F94E