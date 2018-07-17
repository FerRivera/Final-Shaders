// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SkillRoomGlow 1"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_AO("AO", 2D) = "white" {}
		_MetallicSmooth("Metallic&Smooth", 2D) = "white" {}
		_Normals("Normals", 2D) = "bump" {}
		_Albedo("Albedo", 2D) = "white" {}
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_TextureSample2("Texture Sample 2", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
			float2 texcoord_0;
		};

		uniform sampler2D _Normals;
		uniform float4 _Normals_ST;
		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform sampler2D _TextureSample2;
		uniform float4 _TextureSample2_ST;
		uniform sampler2D _TextureSample1;
		uniform sampler2D _TextureSample0;
		uniform sampler2D _MetallicSmooth;
		uniform float4 _MetallicSmooth_ST;
		uniform float _Smoothness;
		uniform sampler2D _AO;
		uniform float4 _AO_ST;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			o.texcoord_0.xy = v.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Normals = i.uv_texcoord * _Normals_ST.xy + _Normals_ST.zw;
			o.Normal = UnpackNormal( tex2D( _Normals,uv_Normals) );
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			o.Albedo = tex2D( _Albedo,uv_Albedo).xyz;
			float2 uv_TextureSample2 = i.uv_texcoord * _TextureSample2_ST.xy + _TextureSample2_ST.zw;
			float cos58 = cos( _Time[1] );
			float sin58 = sin( _Time[1] );
			float2 rotator58 = mul(distance( i.texcoord_0 , float2( 0.335,0.33 ) ) - float2( 0.5,0 ), float2x2(cos58,-sin58,sin58,cos58)) + float2( 0.5,0 );
			o.Emission = ( ( tex2D( _TextureSample2,uv_TextureSample2) * float4(1,1,1,0) ) * tex2D( _TextureSample1,tex2D( _TextureSample0,rotator58).xy) ).xyz;
			float2 uv_MetallicSmooth = i.uv_texcoord * _MetallicSmooth_ST.xy + _MetallicSmooth_ST.zw;
			float4 tex2DNode3 = tex2D( _MetallicSmooth,uv_MetallicSmooth);
			o.Metallic = tex2DNode3.x;
			o.Smoothness = ( tex2DNode3.a * _Smoothness );
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
7;67;727;503;2017.993;561.0063;3.4;True;False
Node;AmplifyShaderEditor.SamplerNode;3;-608,384;Float;True;Property;_MetallicSmooth;Metallic&Smooth;0;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.SamplerNode;4;-608,576;Float;True;Property;_AO;AO;0;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.SamplerNode;2;-608,192;Float;True;Property;_Normals;Normals;0;0;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;Standard;SkillRoomGlow 1;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;13;OBJECT;0.0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-204.8964,443.7003;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.SamplerNode;53;-1540.191,22.49394;Float;True;Property;_TextureSample0;Texture Sample 0;10;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.SamplerNode;54;-1190.391,9.293921;Float;True;Property;_TextureSample1;Texture Sample 1;11;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.SamplerNode;1;-263.5002,-428.8;Float;True;Property;_Albedo;Albedo;0;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.SamplerNode;59;-905.289,-533.5064;Float;True;Property;_TextureSample2;Texture Sample 2;12;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.ColorNode;61;-954.3888,-306.6064;Float;False;Constant;_Color0;Color 0;13;0;1,1,1,0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;63;-376.9906,-73.9066;Float;True;0;COLOR;0,0,0,0;False;1;FLOAT4;0.0;False
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;62;-612.7885,-288.2062;Float;True;0;FLOAT4;0.0;False;1;COLOR;0,0,0,0;False
Node;AmplifyShaderEditor.RotatorNode;58;-1468.891,273.2938;Float;True;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0;False;2;FLOAT;0.0;False
Node;AmplifyShaderEditor.DistanceOpNode;56;-1740.791,269.0938;Float;True;0;FLOAT2;0.0;False;1;FLOAT2;0,0;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;55;-2080.791,233.2939;Float;False;0;-1;2;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False
Node;AmplifyShaderEditor.Vector2Node;21;-2037.601,463.1994;Float;False;Constant;_Vector0;Vector 0;9;0;0.335,0.33
Node;AmplifyShaderEditor.RangedFloatNode;7;-591.2968,789.9002;Float;False;Property;_Smoothness;Smoothness;5;0;0;0;1
WireConnection;0;0;1;0
WireConnection;0;1;2;0
WireConnection;0;2;63;0
WireConnection;0;3;3;0
WireConnection;0;4;6;0
WireConnection;0;5;4;0
WireConnection;6;0;3;4
WireConnection;6;1;7;0
WireConnection;53;1;58;0
WireConnection;54;1;53;0
WireConnection;63;0;62;0
WireConnection;63;1;54;0
WireConnection;62;0;59;0
WireConnection;62;1;61;0
WireConnection;58;0;56;0
WireConnection;56;0;55;0
WireConnection;56;1;21;0
ASEEND*/
//CHKSM=1808B33F8F655D5B00D61FA74F46581B35C66EBA