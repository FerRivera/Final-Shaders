// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Shield Inside"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_Noise1("Noise 1", 2D) = "white" {}
		_ColorInside("Color Inside", Color) = (0,0.5656695,0.7132353,0)
		_Noise2("Noise 2", 2D) = "white" {}
		_Noise3("Noise 3", 2D) = "white" {}
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard alpha:fade keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float2 texcoord_0;
		};

		uniform sampler2D _Noise1;
		uniform sampler2D _Noise2;
		uniform sampler2D _Noise3;
		uniform float4 _ColorInside;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			o.texcoord_0.xy = v.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float cos18 = cos( _Time[1] );
			float sin18 = sin( _Time[1] );
			float2 rotator18 = mul(i.texcoord_0 - float2( 0.5,0.5 ), float2x2(cos18,-sin18,sin18,cos18)) + float2( 0.5,0.5 );
			float temp_output_15_0 = clamp( _SinTime.w , 0.0 , 1.0 );
			o.Emission = ( lerp( lerp( tex2D( _Noise1,rotator18).r , tex2D( _Noise2,rotator18).r , temp_output_15_0 ) , tex2D( _Noise3,rotator18).r , temp_output_15_0 ) * _ColorInside ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=5105
635;72;778;640;1227.5;854.5929;2.5;True;False
Node;AmplifyShaderEditor.LerpOp;17;-695.1192,-330.7628;Float;True;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False
Node;AmplifyShaderEditor.SamplerNode;1;-1391.547,-875.1664;Float;True;Property;_Noise1;Noise 1;0;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.SamplerNode;6;-1397.147,-652.6663;Float;True;Property;_Noise2;Noise 2;3;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.SamplerNode;16;-1394.148,-417.7658;Float;True;Property;_Noise3;Noise 3;4;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.LerpOp;7;-941.9468,-663.4664;Float;True;0;FLOAT;0.0;False;1;FLOAT;0;False;2;FLOAT;0.0;False
Node;AmplifyShaderEditor.SinTimeNode;9;-1336.701,-111.8997;Float;False
Node;AmplifyShaderEditor.ClampOpNode;15;-1082.001,-105.0992;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;19;-2128.575,-582.6045;Float;False;0;-1;2;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False
Node;AmplifyShaderEditor.RotatorNode;18;-1784.103,-581.6003;Float;False;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;0.005;False
Node;AmplifyShaderEditor.ColorNode;3;-676.1998,-71.50008;Float;False;Property;_ColorInside;Color Inside;1;0;0,0.5656695,0.7132353,0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-368.5998,-227.5004;Float;True;0;FLOAT;0,0,0,0;False;1;COLOR;0.0;False
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;16.4998,-320.8;Float;False;True;2;Float;ASEMaterialInspector;Standard;Shield Inside;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;0;False;0;0;Transparent;0.5;True;True;0;False;Transparent;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;13;OBJECT;0.0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False
WireConnection;17;0;7;0
WireConnection;17;1;16;1
WireConnection;17;2;15;0
WireConnection;1;1;18;0
WireConnection;6;1;18;0
WireConnection;16;1;18;0
WireConnection;7;0;1;1
WireConnection;7;1;6;1
WireConnection;7;2;15;0
WireConnection;15;0;9;4
WireConnection;18;0;19;0
WireConnection;4;0;17;0
WireConnection;4;1;3;0
WireConnection;0;2;4;0
ASEEND*/
//CHKSM=2C25779A9F301FF30024E28F07A58DAEF92A2204