// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Seminario/VesselsShaders"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_Albedo("Albedo", 2D) = "white" {}
		_Normals("Normals", 2D) = "bump" {}
		_AO("AO", 2D) = "white" {}
		_LineText("LineText", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "Vessels" = "Vessels" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
		};

		uniform sampler2D _Normals;
		uniform float4 _Normals_ST;
		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform sampler2D _LineText;
		uniform sampler2D _AO;
		uniform float4 _AO_ST;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Normals = i.uv_texcoord * _Normals_ST.xy + _Normals_ST.zw;
			o.Normal = UnpackNormal( tex2D( _Normals,uv_Normals) );
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			float2 temp_cast_0 = ( 1.0 - i.worldPos.y );
			o.Albedo = ( tex2D( _Albedo,uv_Albedo) + tex2D( _LineText,(abs( temp_cast_0+_Time[1] * float2(0.7,1 )))) ).xyz;
			float2 uv_AO = i.uv_texcoord * _AO_ST.xy + _AO_ST.zw;
			o.Occlusion = tex2D( _AO,uv_AO).x;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=5105
35;107;800;650;1246.697;565.7999;2.8;True;True
Node;AmplifyShaderEditor.SamplerNode;1;-400,-16;Float;True;Property;_Albedo;Albedo;0;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.SamplerNode;2;-400,192;Float;True;Property;_Normals;Normals;1;0;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.SamplerNode;3;-400,400;Float;True;Property;_AO;AO;2;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.WorldPosInputsNode;6;-690.6979,705.6003;Float;False
Node;AmplifyShaderEditor.SamplerNode;4;-366.4,656;Float;True;Property;_LineText;LineText;3;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.SimpleAddOpNode;10;6.102295,544.9999;Float;True;0;FLOAT4;0.0,0,0,0;False;1;FLOAT4;0.0;False
Node;AmplifyShaderEditor.PannerNode;5;-548.4988,985.4993;Float;False;0.7;1;0;FLOAT2;0,0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.OneMinusNode;11;-675.4958,877.7006;Float;False;0;FLOAT;0.0;False
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;Standard;Seminario/VesselsShaders;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;13;OBJECT;0.0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False
WireConnection;4;1;5;0
WireConnection;10;0;1;0
WireConnection;10;1;4;0
WireConnection;5;0;11;0
WireConnection;11;0;6;2
WireConnection;0;0;10;0
WireConnection;0;1;2;0
WireConnection;0;5;3;0
ASEEND*/
//CHKSM=A0DF25D4607F49A57FAEFB63ECB75143BD2AF95A