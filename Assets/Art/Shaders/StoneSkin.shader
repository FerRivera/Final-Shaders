// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "StoneSkin"
{
	Properties
	{
		_ASEOutlineColor( "Outline Color", Color ) = (1,1,0,0)
		_ASEOutlineWidth( "Outline Width", Float ) = 0.03
		[HideInInspector] __dirty( "", Int ) = 1
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_TextureSample1("Texture Sample 1", 2D) = "bump" {}
		_Float0("Float 0", Range( 0 , 1)) = 0
		_Color0("Color 0", Color) = (0,0,0,0)
		_Float1("Float 1", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}

	SubShader
	{
		Tags{ }
		Cull Front
		CGPROGRAM
		#pragma target 3.0
		#pragma surface outlineSurf Standard keepalpha noshadow noambient novertexlights nolightmap nodynlightmap nodirlightmap nofog nometa noforwardadd vertex:outlineVertexDataFunc
		struct Input
		{
			fixed filler;
		};
		uniform fixed4 _ASEOutlineColor;
		uniform fixed _ASEOutlineWidth;
		void outlineVertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			v.vertex.xyz += ( v.normal * _ASEOutlineWidth );
		}
		void outlineSurf( Input i, inout SurfaceOutputStandard o ) { o.Emission = _ASEOutlineColor.rgb; o.Alpha = 1; }
		ENDCG
		

		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _TextureSample1;
		uniform float4 _TextureSample1_ST;
		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform float4 _Color0;
		uniform float _Float1;
		uniform float _Float0;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TextureSample1 = i.uv_texcoord * _TextureSample1_ST.xy + _TextureSample1_ST.zw;
			o.Normal = UnpackNormal( tex2D( _TextureSample1,uv_TextureSample1) );
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float4 tex2DNode1 = tex2D( _TextureSample0,uv_TextureSample0);
			o.Albedo = tex2DNode1.xyz;
			o.Emission = ( tex2DNode1 * _Color0 ).rgb;
			o.Metallic = _Float1;
			o.Smoothness = _Float0;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=5105
547;144;935;389;1504.8;404.6;2.6;True;True
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;Standard;StoneSkin;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;True;0.03;1,1,0,0;VertexOffset;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;13;OBJECT;0.0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False
Node;AmplifyShaderEditor.RangedFloatNode;4;-446.3999,525.2;Float;False;Property;_Float1;Float 1;2;0;0;0;1
Node;AmplifyShaderEditor.RangedFloatNode;3;-424.3999,647.2;Float;False;Property;_Float0;Float 0;2;0;0;0;1
Node;AmplifyShaderEditor.SamplerNode;1;-701.1997,-185.2;Float;True;Property;_TextureSample0;Texture Sample 0;0;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.ColorNode;5;-712.6002,26.80005;Float;False;Property;_Color0;Color 0;2;0;0,0,0,0
Node;AmplifyShaderEditor.SamplerNode;2;-987.2,264;Float;True;Property;_TextureSample1;Texture Sample 1;1;0;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-360.8,-50.99998;Float;False;0;FLOAT4;0.0;False;1;COLOR;0.0,0,0,0;False
WireConnection;0;0;1;0
WireConnection;0;1;2;0
WireConnection;0;2;6;0
WireConnection;0;3;4;0
WireConnection;0;4;3;0
WireConnection;6;0;1;0
WireConnection;6;1;5;0
ASEEND*/
//CHKSM=F2652AF261B6004EF0053E7ED64AB9F4BD5AEB60