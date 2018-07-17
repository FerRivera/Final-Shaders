// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Seminario/SkillRoom Glow"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_AO("AO", 2D) = "white" {}
		_MetallicSmooth("Metallic&Smooth", 2D) = "white" {}
		_Normals("Normals", 2D) = "bump" {}
		_Albedo("Albedo", 2D) = "white" {}
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		_Mask("Mask", 2D) = "white" {}
		_ColorEmission("ColorEmission", Color) = (0,0,0,0)
		_EmissionPower("EmissionPower", Range( 0 , 10)) = 0
		_Hardness("Hardness", Float) = 0
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
		uniform float _EmissionPower;
		uniform sampler2D _Mask;
		uniform float4 _Mask_ST;
		uniform float4 _ColorEmission;
		uniform float _Hardness;
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
			float2 uv_Mask = i.uv_texcoord * _Mask_ST.xy + _Mask_ST.zw;
			o.Emission = ( _EmissionPower * ( ( tex2D( _Mask,uv_Mask) * _ColorEmission ) * clamp( ( ( ( 1.0 - distance( i.texcoord_0 , float2( 0.335,0.33 ) ) ) * _Hardness ) - (0.45 + (_SinTime.w - 0.0) * (0.5 - 0.45) / (1.0 - 0.0)) ) , 0.0 , 1.0 ) ) ).rgb;
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
456;80;941;650;1299.779;240.8957;1.9;True;False
Node;AmplifyShaderEditor.SamplerNode;1;-608,0;Float;True;Property;_Albedo;Albedo;0;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.SamplerNode;3;-608,384;Float;True;Property;_MetallicSmooth;Metallic&Smooth;0;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.SamplerNode;4;-608,576;Float;True;Property;_AO;AO;0;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.SamplerNode;5;-608,768;Float;True;Property;_Heights;Heights;4;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.SamplerNode;2;-608,192;Float;True;Property;_Normals;Normals;0;0;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.SamplerNode;8;-608,960;Float;True;Property;_Mask;Mask;6;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;Standard;Seminario/SkillRoom Glow;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;13;OBJECT;0.0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False
Node;AmplifyShaderEditor.RangedFloatNode;7;-302.2965,592.7003;Float;False;Property;_Smoothness;Smoothness;5;0;0;0;1
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-204.8964,443.7003;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;19;-1216,496;Float;False;0;-1;2;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False
Node;AmplifyShaderEditor.Vector2Node;21;-1211.2,675.1997;Float;False;Constant;_Vector0;Vector 0;9;0;0.335,0.33
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;59.70772,881.5511;Float;True;0;FLOAT;0.0,0,0,0;False;1;COLOR;0.0;False
Node;AmplifyShaderEditor.RangedFloatNode;16;-305.8925,941.5997;Float;False;Property;_EmissionPower;EmissionPower;8;0;0;0;10
Node;AmplifyShaderEditor.DistanceOpNode;20;-908.1,512;Float;True;0;FLOAT2;0.0,0;False;1;FLOAT2;0.0;False
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-126.8931,711.0998;Float;True;0;COLOR;0.0;False;1;FLOAT;0.0,0,0,0;False
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-224,1056;Float;True;0;FLOAT4;0,0,0,0;False;1;COLOR;0.0,0,0,0;False
Node;AmplifyShaderEditor.OneMinusNode;42;-849.5137,765.5238;Float;False;0;FLOAT;0.0;False
Node;AmplifyShaderEditor.ClampOpNode;52;-679.8918,1146.894;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False
Node;AmplifyShaderEditor.RangedFloatNode;41;-1019.357,803.5875;Float;False;Property;_Hardness;Hardness;12;0;0;0;0
Node;AmplifyShaderEditor.RangedFloatNode;53;-1688.697,1122.794;Float;False;Property;_Abundance;Abundance;10;0;0;0;1
Node;AmplifyShaderEditor.SinTimeNode;43;-1326.47,951.3475;Float;False
Node;AmplifyShaderEditor.TFHCRemap;44;-1059.041,886.5873;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;0.45;False;4;FLOAT;0.5;False
Node;AmplifyShaderEditor.SimpleSubtractOpNode;40;-871.7029,1095.687;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;51;-828.9314,845.655;Float;True;0;FLOAT;0.0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.ColorNode;11;-500.3,1171.8;Float;False;Property;_ColorEmission;ColorEmission;7;0;0,0,0,0
WireConnection;0;0;1;0
WireConnection;0;1;2;0
WireConnection;0;2;15;0
WireConnection;0;3;3;0
WireConnection;0;4;6;0
WireConnection;0;5;4;0
WireConnection;6;0;3;4
WireConnection;6;1;7;0
WireConnection;15;0;16;0
WireConnection;15;1;17;0
WireConnection;20;0;19;0
WireConnection;20;1;21;0
WireConnection;17;0;10;0
WireConnection;17;1;52;0
WireConnection;10;0;8;0
WireConnection;10;1;11;0
WireConnection;42;0;20;0
WireConnection;52;0;40;0
WireConnection;44;0;43;4
WireConnection;40;0;51;0
WireConnection;40;1;44;0
WireConnection;51;0;42;0
WireConnection;51;1;41;0
ASEEND*/
//CHKSM=359D96AF64A8F0568976B306EB4D21BBDC843B64