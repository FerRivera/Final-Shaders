// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "WallTestShader"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_TextureSample1("Texture Sample 1", 2D) = "bump" {}
		_TextureSample2("Texture Sample 2", 2D) = "white" {}
		_TextureSample3("Texture Sample 3", 2D) = "white" {}
		_Float0("Float 0", Range( 0 , 1)) = 0.4533497
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _TextureSample1;
		uniform float4 _TextureSample1_ST;
		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform sampler2D _TextureSample2;
		uniform float4 _TextureSample2_ST;
		uniform float _Float0;
		uniform sampler2D _TextureSample3;
		uniform float4 _TextureSample3_ST;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float4 uv_TextureSample3 = float4(v.texcoord * _TextureSample3_ST.xy + _TextureSample3_ST.zw, 0 ,0);
			v.vertex.xyz += ( _Float0 * ( tex2Dlod( _TextureSample3,uv_TextureSample3).r * v.normal ) );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TextureSample1 = i.uv_texcoord * _TextureSample1_ST.xy + _TextureSample1_ST.zw;
			o.Normal = UnpackNormal( tex2D( _TextureSample1,uv_TextureSample1) );
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			o.Albedo = tex2D( _TextureSample0,uv_TextureSample0).xyz;
			float2 uv_TextureSample2 = i.uv_texcoord * _TextureSample2_ST.xy + _TextureSample2_ST.zw;
			o.Occlusion = tex2D( _TextureSample2,uv_TextureSample2).x;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=5105
528;494;1268;824;1277.492;-527.2996;1.6;True;True
Node;AmplifyShaderEditor.SamplerNode;1;-520,-106;Float;True;Property;_TextureSample0;Texture Sample 0;0;0;Assets/Art/3D/Scenary (New)/Textures/Room1/Walls/JPEG/Wall_Albedo.jpg;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.SamplerNode;2;-512,96;Float;True;Property;_TextureSample1;Texture Sample 1;1;0;Assets/Art/3D/Scenary (New)/Textures/Room1/Walls/JPEG/Wall8_normals.jpg;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.SamplerNode;3;-544,288;Float;True;Property;_TextureSample2;Texture Sample 2;2;0;Assets/Art/3D/Scenary (New)/Textures/Room1/Walls/JPEG/Wall8_occlusion.jpg;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-9.499999,72.2;Float;False;True;2;Float;ASEMaterialInspector;Standard;WallTestShader;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;13;OBJECT;0.0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False
Node;AmplifyShaderEditor.SamplerNode;5;-574.5001,624.4999;Float;True;Property;_TextureSample3;Texture Sample 3;3;0;Assets/Art/3D/Scenary (New)/Textures/Room1/Walls/JPEG/Wall8_heights.jpg;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-212.4997,712.8002;Float;False;0;FLOAT;0.0,0,0;False;1;FLOAT3;0.0;False
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-51.59589,626.7999;Float;False;0;FLOAT;0.0;False;1;FLOAT3;0.0;False
Node;AmplifyShaderEditor.RangedFloatNode;4;-567.8001,500.5009;Float;False;Property;_Float0;Float 0;3;0;0.4533497;0;1
Node;AmplifyShaderEditor.NormalVertexDataNode;11;-376.6917,885.6995;Float;False
WireConnection;0;0;1;0
WireConnection;0;1;2;0
WireConnection;0;5;3;0
WireConnection;0;11;9;0
WireConnection;6;0;5;1
WireConnection;6;1;11;0
WireConnection;9;0;4;0
WireConnection;9;1;6;0
ASEEND*/
//CHKSM=D5D4BC4587C74D17FFDFE6C68FB98BA36CCC19B8