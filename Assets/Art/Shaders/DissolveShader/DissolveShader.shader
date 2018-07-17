// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Seminario/DissolveShader"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_MaskClipValue( "Mask Clip Value", Float ) = 0.5
		_Noise("Noise", 2D) = "white" {}
		_Dissolve("Dissolve", Range( -0.1 , 0.6)) = 0
		_Ramp("Ramp", 2D) = "white" {}
		_Albedo("Albedo", 2D) = "white" {}
		_Normal("Normal", 2D) = "bump" {}
		_Mettalic("Mettalic", 2D) = "white" {}
		_Occlusion("Occlusion", 2D) = "white" {}
		_Metallic("Metallic", Range( 0 , 1)) = 0
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Normal;
		uniform float4 _Normal_ST;
		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform sampler2D _Ramp;
		uniform float _Dissolve;
		uniform sampler2D _Noise;
		uniform float4 _Noise_ST;
		uniform sampler2D _Mettalic;
		uniform float4 _Mettalic_ST;
		uniform float _Metallic;
		uniform float _Smoothness;
		uniform sampler2D _Occlusion;
		uniform float4 _Occlusion_ST;
		uniform float _MaskClipValue = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Normal = i.uv_texcoord * _Normal_ST.xy + _Normal_ST.zw;
			o.Normal = UnpackNormal( tex2D( _Normal,uv_Normal) );
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			o.Albedo = tex2D( _Albedo,uv_Albedo).xyz;
			float4 temp_cast_1 = (-0.5 + (( 1.0 - _Dissolve ) - 0.0) * (0.5 - -0.5) / (1.0 - 0.0));
			float2 uv_Noise = i.uv_texcoord * _Noise_ST.xy + _Noise_ST.zw;
			float4 temp_output_49_0 = ( temp_cast_1 + tex2D( _Noise,uv_Noise) );
			o.Emission = tex2D( _Ramp,( 1.0 - (float4( -6,0,0,0 ) + (temp_output_49_0 - float4( 0,0,0,0 )) * (float4( 6,0,0,0 ) - float4( -6,0,0,0 )) / (float4( 1,0,0,0 ) - float4( 0,0,0,0 ))) ).xy).xyz;
			float2 uv_Mettalic = i.uv_texcoord * _Mettalic_ST.xy + _Mettalic_ST.zw;
			o.Metallic = ( tex2D( _Mettalic,uv_Mettalic) * _Metallic ).x;
			o.Smoothness = _Smoothness;
			float2 uv_Occlusion = i.uv_texcoord * _Occlusion_ST.xy + _Occlusion_ST.zw;
			o.Occlusion = tex2D( _Occlusion,uv_Occlusion).x;
			o.Alpha = 1;
			clip( temp_output_49_0 - ( _MaskClipValue ).xxxx );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=5105
7;29;600;350;1224.084;-110.474;2.724942;True;False
Node;AmplifyShaderEditor.CommentaryNode;79;-1136.27,839.6543;Float;False;1229.027;582.1885;Dissolve;8;53;73;15;49;7;58;54;8;Dissolve
Node;AmplifyShaderEditor.CommentaryNode;77;-777.172,-14.44522;Float;False;455.8405;821.9193;Textures;4;76;61;75;62;Textures
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-152.6003,191.4999;Float;False;True;2;Float;ASEMaterialInspector;Standard;Seminario/DissolveShader;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;0;False;0;0;Masked;0.5;True;True;0;False;TransparentCutout;AlphaTest;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;13;OBJECT;0.0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False
Node;AmplifyShaderEditor.SamplerNode;76;-656,608;Float;True;Property;_Occlusion;Occlusion;6;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.SamplerNode;75;-656,416;Float;True;Property;_Mettalic;Mettalic;5;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.SamplerNode;62;-656,224;Float;True;Property;_Normal;Normal;4;0;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.SamplerNode;61;-656,32;Float;True;Property;_Albedo;Albedo;3;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.OneMinusNode;54;-853.4279,955.3277;Float;False;0;FLOAT;0.0;False
Node;AmplifyShaderEditor.TFHCRemap;58;-689.949,958.3092;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;-0.5;False;4;FLOAT;0.5;False
Node;AmplifyShaderEditor.SamplerNode;7;-1035.758,1156.056;Float;True;Property;_Noise;Noise;0;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.SimpleAddOpNode;49;-500.1872,1124.9;Float;False;0;FLOAT;0.0,0,0,0;False;1;FLOAT4;0.0;False
Node;AmplifyShaderEditor.SamplerNode;15;-313.0384,915.3489;Float;True;Property;_Ramp;Ramp;2;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.OneMinusNode;73;-341.1316,1141.899;Float;False;0;FLOAT4;0.0;False
Node;AmplifyShaderEditor.TFHCRemap;53;-462.3158,1245.732;Float;False;0;FLOAT4;0.0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT4;1,0,0,0;False;3;FLOAT4;-6,0,0,0;False;4;FLOAT4;6,0,0,0;False
Node;AmplifyShaderEditor.RangedFloatNode;8;-1117.007,929.9678;Float;False;Property;_Dissolve;Dissolve;1;0;0;-0.1;0.6
Node;AmplifyShaderEditor.RangedFloatNode;80;-287.2053,626.5139;Float;False;Property;_Metallic;Metallic;7;0;0;0;1
Node;AmplifyShaderEditor.RangedFloatNode;81;-291.2545,710.2347;Float;False;Property;_Smoothness;Smoothness;8;0;0;0;1
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;82;-295.5043,399.9931;Float;False;0;FLOAT4;0.0;False;1;FLOAT;0.0,0,0,0;False
WireConnection;0;0;61;0
WireConnection;0;1;62;0
WireConnection;0;2;15;0
WireConnection;0;3;82;0
WireConnection;0;4;81;0
WireConnection;0;5;76;0
WireConnection;0;10;49;0
WireConnection;54;0;8;0
WireConnection;58;0;54;0
WireConnection;49;0;58;0
WireConnection;49;1;7;0
WireConnection;15;1;73;0
WireConnection;73;0;53;0
WireConnection;53;0;49;0
WireConnection;82;0;75;0
WireConnection;82;1;80;0
ASEEND*/
//CHKSM=BFDC3C640B53FAFB6E089F35F0CED2AE4FCF5389