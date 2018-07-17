// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Seminario/SwordFireShader"
{
	Properties
	{
		_ASEOutlineColor( "Outline Color", Color ) = (0.5588235,0.5983772,1,0)
		_ASEOutlineWidth( "Outline Width", Float ) = 0
		[HideInInspector] __dirty( "", Int ) = 1
		_Albedo("Albedo", 2D) = "white" {}
		_Normals("Normals", 2D) = "bump" {}
		_Mask("Mask", 2D) = "white" {}
		_EmissionPower("EmissionPower", Range( 0 , 1)) = 0.2812172
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
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

		uniform sampler2D _Normals;
		uniform float4 _Normals_ST;
		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform float _EmissionPower;
		uniform sampler2D _Mask;
		uniform float4 _Mask_ST;
		uniform float _Smoothness;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Normals = i.uv_texcoord * _Normals_ST.xy + _Normals_ST.zw;
			o.Normal = UnpackNormal( tex2D( _Normals,uv_Normals) );
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			o.Albedo = tex2D( _Albedo,uv_Albedo).xyz;
			float2 uv_Mask = i.uv_texcoord * _Mask_ST.xy + _Mask_ST.zw;
			float4 _EmissionColor2 = float4(1,0.9241379,0.5,1);
			o.Emission = ( _EmissionPower * ( tex2D( _Mask,uv_Mask) * _EmissionColor2 ) ).rgb;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=5105
486;108;1075;520;721.0005;170.0999;1.6;True;True
Node;AmplifyShaderEditor.CommentaryNode;14;-978,350;Float;False;733;956;Emission;6;4;3;5;10;12;11;Emission
Node;AmplifyShaderEditor.SamplerNode;1;-480,-192;Float;True;Property;_Albedo;Albedo;0;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.SamplerNode;2;-480,16;Float;True;Property;_Normals;Normals;1;0;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-480,624;Float;True;0;FLOAT4;0,0,0,0;False;1;COLOR;0.0,0,0,0;False
Node;AmplifyShaderEditor.SamplerNode;3;-800,400;Float;True;Property;_Mask;Mask;2;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.ColorNode;5;-928,640;Float;False;Constant;_EmissionColor;EmissionColor;3;0;1,0,0,1
Node;AmplifyShaderEditor.LerpOp;10;-656,768;Float;False;0;COLOR;0.0,0,0,0;False;1;COLOR;0.0;False;2;FLOAT;0.0;False
Node;AmplifyShaderEditor.SinTimeNode;12;-832,1104;Float;False
Node;AmplifyShaderEditor.ColorNode;11;-928,880;Float;False;Constant;_EmissionColor2;EmissionColor2;4;0;1,0.9241379,0.5,1
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;109.7988,749.7;Float;False;0;FLOAT;0.0,0,0,0;False;1;COLOR;0.0;False
Node;AmplifyShaderEditor.RangedFloatNode;13;-160,576;Float;False;Property;_EmissionPower;EmissionPower;4;0;0.2812172;0;1
Node;AmplifyShaderEditor.RangedFloatNode;9;-413.8997,278.4999;Float;False;Property;_Smoothness;Smoothness;4;0;0;0;1
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;308.8,0;Float;False;True;2;Float;ASEMaterialInspector;Standard;Seminario/SwordFireShader;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;True;0;0.5588235,0.5983772,1,0;VertexOffset;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;13;OBJECT;0.0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False
WireConnection;4;0;3;0
WireConnection;4;1;11;0
WireConnection;10;0;5;0
WireConnection;10;1;11;0
WireConnection;10;2;12;4
WireConnection;15;0;13;0
WireConnection;15;1;4;0
WireConnection;0;0;1;0
WireConnection;0;1;2;0
WireConnection;0;2;15;0
WireConnection;0;4;9;0
ASEEND*/
//CHKSM=150BA38BA3B0DAEA1B20DEE88DEA72C5BCCB9B0D