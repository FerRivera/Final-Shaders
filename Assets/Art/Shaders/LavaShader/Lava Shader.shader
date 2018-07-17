// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Seminario/Lava Shader"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_Noise("Noise", 2D) = "white" {}
		_Ramp("Ramp", 2D) = "white" {}
		_Noise2("Noise 2", 2D) = "white" {}
		_Noise3("Noise 3", 2D) = "white" {}
		_LavaTexture("Lava Texture", 2D) = "white" {}
		_Displacement("Displacement", Range( 0 , 1)) = 0
		_Waves("Waves", 2D) = "white" {}
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 4.6
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float2 texcoord_0;
			float2 texcoord_1;
		};

		uniform sampler2D _Ramp;
		uniform sampler2D _Noise2;
		uniform sampler2D _Noise;
		uniform sampler2D _Noise3;
		uniform sampler2D _LavaTexture;
		uniform sampler2D _Waves;
		uniform float _Displacement;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			o.texcoord_0.xy = v.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
			o.texcoord_1.xy = v.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
			float2 temp_output_8_0 = (abs( o.texcoord_1+_Time[1] * float2(0,0.05 )));
			v.vertex.xyz += ( ( tex2Dlod( _Waves,float4( temp_output_8_0, 0.0 , 0.0 )) * float4( v.normal , 0.0 ) ) * _Displacement ).xyz;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 temp_output_8_0 = (abs( i.texcoord_0+_Time[1] * float2(0,0.05 )));
			float4 temp_output_21_0 = lerp( lerp( tex2D( _Noise2,temp_output_8_0) , tex2D( _Noise,temp_output_8_0) , _SinTime.w ) , tex2D( _Noise3,temp_output_8_0) , (0.5 + (_SinTime.w - 0.0) * (1.5 - 0.5) / (1.0 - 0.0)) );
			float4 tex2DNode2 = tex2D( _Ramp,( temp_output_21_0 + tex2D( _LavaTexture,temp_output_8_0) ).xy);
			o.Albedo = tex2DNode2.xyz;
			o.Emission = tex2DNode2.xyz;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=5105
235;205;904;516;253.9802;155.0287;1.3;True;False
Node;AmplifyShaderEditor.CommentaryNode;48;-315.7485,534.7372;Float;False;556.7485;607.2628;Displacement;5;34;28;27;33;32;Displacement
Node;AmplifyShaderEditor.CommentaryNode;47;-1275.178,253.4883;Float;False;825.5095;528.2457;Lava/Noise Intensify;5;11;4;14;13;3;Lava/Noise Intensify
Node;AmplifyShaderEditor.CommentaryNode;46;-1411.738,-696.1949;Float;False;1462.73;853.421;Texture;15;8;43;41;30;2;21;18;25;26;1;20;19;29;42;9;Texture
Node;AmplifyShaderEditor.SinTimeNode;19;-1376,-272;Float;False
Node;AmplifyShaderEditor.TFHCRemap;20;-1200,-272;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;0.5;False;4;FLOAT;1.5;False
Node;AmplifyShaderEditor.SamplerNode;1;-912,-192;Float;True;Property;_Noise;Noise;0;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.SamplerNode;26;-912,-656;Float;True;Property;_Noise3;Noise 3;5;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.SamplerNode;25;-912,-432;Float;True;Property;_Noise2;Noise 2;4;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.WireNode;41;-362.9766,-261.4543;Float;False;0;FLOAT4;0.0;False
Node;AmplifyShaderEditor.WireNode;43;-143.8085,-275.4437;Float;False;0;FLOAT4;0.0;False
Node;AmplifyShaderEditor.SamplerNode;30;-393.7739,-534.8213;Float;True;Property;_LavaTexture;Lava Texture;6;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.SamplerNode;2;-384,-208;Float;True;Property;_Ramp;Ramp;1;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.SimpleAddOpNode;29;-89.02629,-377.1269;Float;False;0;FLOAT4;0.0;False;1;FLOAT4;0;False
Node;AmplifyShaderEditor.WireNode;42;-31.37684,-251.7491;Float;False;0;FLOAT4;0.0;False
Node;AmplifyShaderEditor.SinTimeNode;13;-1225.178,455.989;Float;False
Node;AmplifyShaderEditor.TFHCRemap;14;-1074.378,476.7904;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;1.5;False;4;FLOAT;1.9;False
Node;AmplifyShaderEditor.RangedFloatNode;4;-1182.475,303.4883;Float;False;Property;_LavaIntensity;Lava Intensity;2;0;1;0;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-885.8771,376.689;Float;True;0;FLOAT;0.0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.SamplerNode;34;-80,912;Float;True;Property;_Waves;Waves;8;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.NormalVertexDataNode;28;-265.7485,587.3375;Float;False
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-101.9486,584.7372;Float;False;0;FLOAT4;0.0,0,0;False;1;FLOAT3;0.0,0,0,0;False
Node;AmplifyShaderEditor.RangedFloatNode;33;-218.1237,809.4846;Float;False;Property;_Displacement;Displacement;7;0;0;0;1
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;-28.49207,703.4424;Float;False;0;FLOAT4;0.0;False;1;FLOAT;0.0,0,0,0;False
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-672,384;Float;True;0;FLOAT4;0.0;False;1;FLOAT;0.0,0,0,0;False
Node;AmplifyShaderEditor.LerpOp;21;-563.4949,-390.621;Float;False;0;FLOAT4;0.0;False;1;FLOAT4;0.0;False;2;FLOAT;0.0;False
Node;AmplifyShaderEditor.LerpOp;18;-579.1263,-260.6895;Float;False;0;FLOAT4;0.0,0,0,0;False;1;FLOAT4;0.0;False;2;FLOAT;0.0;False
Node;AmplifyShaderEditor.PannerNode;8;-1096.588,-71.30033;Float;False;0;0.05;0;FLOAT2;0,0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;9;-1359.7,26.8;Float;False;0;-1;2;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;493.8,-32;Float;False;True;6;Float;ASEMaterialInspector;Standard;Seminario/Lava Shader;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;0;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;1;4;10;25;True;0.49;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;13;OBJECT;0.0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False
WireConnection;20;0;19;4
WireConnection;1;1;8;0
WireConnection;26;1;8;0
WireConnection;25;1;8;0
WireConnection;41;0;43;0
WireConnection;43;0;42;0
WireConnection;30;1;8;0
WireConnection;2;1;41;0
WireConnection;29;0;21;0
WireConnection;29;1;30;0
WireConnection;42;0;29;0
WireConnection;14;0;13;4
WireConnection;11;0;4;0
WireConnection;11;1;14;0
WireConnection;34;1;8;0
WireConnection;27;0;34;0
WireConnection;27;1;28;0
WireConnection;32;0;27;0
WireConnection;32;1;33;0
WireConnection;3;0;21;0
WireConnection;3;1;11;0
WireConnection;21;0;18;0
WireConnection;21;1;26;0
WireConnection;21;2;20;0
WireConnection;18;0;25;0
WireConnection;18;1;1;0
WireConnection;18;2;19;4
WireConnection;8;0;9;0
WireConnection;0;0;2;0
WireConnection;0;2;2;0
WireConnection;0;11;32;0
ASEEND*/
//CHKSM=08B727982ABA9A6EEE0EA98D6A67B4E94E73010D