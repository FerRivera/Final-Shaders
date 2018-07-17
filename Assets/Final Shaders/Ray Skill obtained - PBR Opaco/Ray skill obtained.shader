// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Ray skill obtained"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_vertexoffset("vertex offset", Range( 0 , 1)) = 0
		_Tessellation("Tessellation", Range( 0 , 30)) = 0
		_ray("ray", 2D) = "white" {}
		_Heightmap("Heightmap", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
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

		uniform sampler2D _ray;
		uniform float4 _ray_ST;
		uniform sampler2D _Heightmap;
		uniform float _vertexoffset;
		uniform float _Tessellation;

		float4 tessFunction( appdata v0, appdata v1, appdata v2 )
		{
			float4 temp_cast_3 = _Tessellation;
			return temp_cast_3;
		}

		void vertexDataFunc( inout appdata v )
		{
			v.texcoord.xy = v.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
			v.vertex.xyz += ( ( tex2Dlod( _Heightmap,float4( (abs( v.texcoord.xy+_Time[1] * float2(0.2,0.2 ))), 0.0 , 0.0 )) * float4( v.normal , 0.0 ) ) * _vertexoffset ).xyz;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_ray = i.uv_texcoord * _ray_ST.xy + _ray_ST.zw;
			o.Emission = tex2D( _ray,uv_ray).xyz;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=5105
518;99;778;625;2505.903;715.4235;2.5;True;False
Node;AmplifyShaderEditor.SamplerNode;3;-901.4067,-264.4733;Float;True;Property;_ray;ray;1;0;Assets/Final Shaders/Ray Skill obtained/ray.jpg;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-650.3128,91.25549;Float;False;0;FLOAT4;0.0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-810.9623,74.00575;Float;False;0;FLOAT4;0,0,0;False;1;FLOAT3;0.0,0,0,0;False
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-334.4949,-25.67951;Float;False;True;6;Float;ASEMaterialInspector;Standard;Ray skill obtained;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;True;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;13;OBJECT;0.0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False
Node;AmplifyShaderEditor.RangedFloatNode;9;-1039.916,244.2172;Float;False;Property;_vertexoffset;vertex offset;1;0;0;0;1
Node;AmplifyShaderEditor.RangedFloatNode;10;-688.5063,287.5668;Float;False;Property;_Tessellation;Tessellation;1;0;0;0;30
Node;AmplifyShaderEditor.SamplerNode;11;-1354.684,-53.54404;Float;True;Property;_Heightmap;Heightmap;3;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.NormalVertexDataNode;6;-1053.897,92.76214;Float;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;5;-1751.417,-263.2733;Float;False;0;-1;2;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False
Node;AmplifyShaderEditor.PannerNode;4;-1439.417,-263.2733;Float;False;0.2;0.2;0;FLOAT2;0,0;False;1;FLOAT;0.0;False
WireConnection;8;0;7;0
WireConnection;8;1;9;0
WireConnection;7;0;11;0
WireConnection;7;1;6;0
WireConnection;0;2;3;0
WireConnection;0;11;8;0
WireConnection;0;14;10;0
WireConnection;11;1;4;0
WireConnection;4;0;5;0
ASEEND*/
//CHKSM=7D409F52A1D5732F5676463CDA70EE9CC0E8149C