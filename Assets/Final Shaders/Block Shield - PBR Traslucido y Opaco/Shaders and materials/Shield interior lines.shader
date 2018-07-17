// Upgrade NOTE: replaced 'UNITY_INSTANCE_ID' with 'UNITY_VERTEX_INPUT_INSTANCE_ID'

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Shiled interior lines"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_Ray1("Ray 1", 2D) = "white" {}
		_Raycolor("Ray color", Color) = (0,0.751724,1,1)
		_ray2("ray 2", 2D) = "white" {}
		_Brigthness("Brigthness", Range( 0 , 8)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
			float2 texcoord_0;
		};

		uniform sampler2D _Ray1;
		uniform float4 _Ray1_ST;
		uniform sampler2D _ray2;
		uniform float4 _ray2_ST;
		uniform float4 _Raycolor;
		uniform float _Brigthness;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			o.texcoord_0.xy = v.texcoord.xy * float2( 2,2 ) + float2( -0.5,-0.5 );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Ray1 = i.uv_texcoord * _Ray1_ST.xy + _Ray1_ST.zw;
			float2 uv_ray2 = i.uv_texcoord * _ray2_ST.xy + _ray2_ST.zw;
			float4 temp_output_2_0 = ( lerp( tex2D( _Ray1,uv_Ray1).r , tex2D( _ray2,uv_ray2).r , (0.8 + (_SinTime.w - 0.0) * (1.5 - 0.8) / (1.0 - 0.0)) ) * _Raycolor );
			o.Emission = temp_output_2_0.rgb;
			float temp_output_17_0 = (1.0 + (_SinTime.w - 0.0) * (1.5 - 1.0) / (1.0 - 0.0));
			o.Alpha = ( ( temp_output_2_0 * _Brigthness ) * ( ( 1.0 - distance( i.texcoord_0 , float2( 0.5,0.5 ) ) ) * temp_output_17_0 ) ).a;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_instancing
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			# include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float3 worldPos : TEXCOORD6;
				float4 texcoords01 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.texcoords01 = float4( v.texcoord.xy, v.texcoord1.xy );
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			fixed4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.texcoords01.xy;
				float3 worldPos = IN.worldPos;
				fixed3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=5105
548;92;800;650;1111.292;821.849;3.4;True;False
Node;AmplifyShaderEditor.LerpOp;13;-641.9276,-495.2231;Float;True;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False
Node;AmplifyShaderEditor.SamplerNode;1;-1205.755,-741.131;Float;True;Property;_Ray1;Ray 1;0;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.DistanceOpNode;6;-973.097,337.876;Float;True;0;FLOAT2;0.0;False;1;FLOAT2;0.0,0;False
Node;AmplifyShaderEditor.OneMinusNode;8;-715.0973,375.876;Float;False;0;FLOAT;0.0;False
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-451.4465,411.6429;Float;True;0;FLOAT;0.0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;4;-1262.697,303.6758;Float;False;0;-1;2;0;FLOAT2;2,2;False;1;FLOAT2;-0.5,-0.5;False
Node;AmplifyShaderEditor.Vector2Node;5;-1237.2,495.3059;Float;False;Constant;_Vector0;Vector 0;1;0;0.5,0.5
Node;AmplifyShaderEditor.SinTimeNode;7;-1205.66,672.8081;Float;False
Node;AmplifyShaderEditor.SamplerNode;14;-1197.975,-533.8738;Float;True;Property;_ray2;ray 2;1;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.ClampOpNode;11;-755.584,482.9674;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.2;False;2;FLOAT;1.0;False
Node;AmplifyShaderEditor.TFHCRemap;19;-961.0935,-80.34872;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;0.8;False;4;FLOAT;1.5;False
Node;AmplifyShaderEditor.TFHCRemap;17;-970.3918,704.8511;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;1.0;False;4;FLOAT;1.5;False
Node;AmplifyShaderEditor.ColorNode;3;-539.4341,62.79909;Float;False;Property;_Raycolor;Ray color;1;0;0,0.751724,1,1
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;2;-309.3999,-264.6;Float;True;0;FLOAT;0.0,0,0,0;False;1;COLOR;0.0;False
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-42.19143,33.5509;Float;True;0;COLOR;0.0;False;1;FLOAT;0,0,0,0;False
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;166.0117,302.0799;Float;True;0;COLOR;0.0;False;1;FLOAT;0,0,0,0;False
Node;AmplifyShaderEditor.RangedFloatNode;21;-358.5914,271.5509;Float;False;Property;_Brigthness;Brigthness;3;0;0;0;8
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;839.3339,7.824251;Float;False;True;2;Float;ASEMaterialInspector;Standard;Shiled interior lines;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;0;False;0;0;Transparent;0.5;True;True;0;False;Transparent;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;13;OBJECT;0.0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False
Node;AmplifyShaderEditor.BreakToComponentsNode;18;472.3091,369.4511;Float;False;COLOR;0;COLOR;0.0;False
WireConnection;13;0;1;1
WireConnection;13;1;14;1
WireConnection;13;2;19;0
WireConnection;6;0;4;0
WireConnection;6;1;5;0
WireConnection;8;0;6;0
WireConnection;10;0;8;0
WireConnection;10;1;17;0
WireConnection;11;0;17;0
WireConnection;19;0;7;4
WireConnection;17;0;7;4
WireConnection;2;0;13;0
WireConnection;2;1;3;0
WireConnection;20;0;2;0
WireConnection;20;1;21;0
WireConnection;12;0;20;0
WireConnection;12;1;10;0
WireConnection;0;2;2;0
WireConnection;0;9;18;3
WireConnection;18;0;12;0
ASEEND*/
//CHKSM=546EAB30849725E9FB3D65F18FDAE9B40DEA6F86