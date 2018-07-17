// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Seminario/TesselationTest"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_Albedo("Albedo", 2D) = "white" {}
		_Normals("Normals", 2D) = "bump" {}
		_HeightMap("HeightMap", 2D) = "white" {}
		_AO("AO", 2D) = "white" {}
		_Displacement("Displacement", Range( 0 , 1)) = 0
		_Tesselation("Tesselation", Range( 0.1 , 10)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "Tessellation.cginc"
		#pragma target 4.6
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc tessellate:tessFunction nolightmap 
		struct Input
		{
			float2 uv_texcoord;
		};

		struct appdata
		{
			float4 vertex : POSITION;
			float4 tangent : TANGENT;
			float3 normal : NORMAL;
			float4 texcoord : TEXCOORD0;
			float4 texcoord1 : TEXCOORD1;
			float4 texcoord2 : TEXCOORD2;
			float4 texcoord3 : TEXCOORD3;
			fixed4 color : COLOR;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		uniform sampler2D _Normals;
		uniform float4 _Normals_ST;
		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform sampler2D _AO;
		uniform float4 _AO_ST;
		uniform sampler2D _HeightMap;
		uniform float4 _HeightMap_ST;
		uniform float _Displacement;
		uniform float _Tesselation;

		float4 tessFunction( appdata v0, appdata v1, appdata v2 )
		{
			float4 temp_cast_2 = _Tesselation;
			return temp_cast_2;
		}

		void vertexDataFunc( inout appdata v )
		{
			float4 uv_HeightMap = float4(v.texcoord * _HeightMap_ST.xy + _HeightMap_ST.zw, 0 ,0);
			v.vertex.xyz += ( ( tex2Dlod( _HeightMap,uv_HeightMap) * float4( v.normal , 0.0 ) ) * _Displacement ).xyz;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Normals = i.uv_texcoord * _Normals_ST.xy + _Normals_ST.zw;
			o.Normal = UnpackNormal( tex2D( _Normals,uv_Normals) );
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			o.Albedo = tex2D( _Albedo,uv_Albedo).xyz;
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
7;29;904;516;962.1332;-68.25107;1.770297;True;True
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,-1.927723;Float;False;True;6;Float;ASEMaterialInspector;Standard;Seminario/TesselationTest;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;True;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;13;OBJECT;0.0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-156.6744,509.6194;Float;False;0;FLOAT4;0.0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.RangedFloatNode;6;-496,576;Float;False;Property;_Displacement;Displacement;4;0;0;0;1
Node;AmplifyShaderEditor.NormalVertexDataNode;9;-752,560;Float;False
Node;AmplifyShaderEditor.SamplerNode;3;-896,320;Float;True;Property;_HeightMap;HeightMap;2;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-384,432;Float;False;0;FLOAT4;0.0,0,0;False;1;FLOAT3;0.0,0,0,0;False
Node;AmplifyShaderEditor.SamplerNode;1;-560,-544;Float;True;Property;_Albedo;Albedo;0;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.SamplerNode;2;-560,-336;Float;True;Property;_Normals;Normals;1;0;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.SamplerNode;4;-560,-128;Float;True;Property;_AO;AO;2;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.RangedFloatNode;11;-450.5264,768.3591;Float;False;Property;_Tesselation;Tesselation;5;0;0;0.1;10
WireConnection;0;0;1;0
WireConnection;0;1;2;0
WireConnection;0;5;4;0
WireConnection;0;11;7;0
WireConnection;0;14;11;0
WireConnection;7;0;10;0
WireConnection;7;1;6;0
WireConnection;10;0;3;0
WireConnection;10;1;9;0
ASEEND*/
//CHKSM=BE6C72449E86B2D3DC97BCCE06CAC8BD07731E83