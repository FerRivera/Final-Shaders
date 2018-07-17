// Upgrade NOTE: replaced 'UNITY_INSTANCE_ID' with 'UNITY_VERTEX_INPUT_INSTANCE_ID'

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Seminario/HollowGround"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Float0("Float 0", Float) = 2.53
		_refractionmultiplier("refractionmultiplier", Range( 0 , 1)) = 0
		[Header(Refraction)]
		_ChromaticAberration("Chromatic Aberration", Range( 0 , 0.3)) = 0.1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Back
		GrabPass{ "RefractionGrab0" }
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 texcoord_0;
			float4 screenPos;
			float3 worldPos;
			float2 texcoord_1;
		};

		uniform sampler2D _TextureSample0;
		uniform float _Float0;
		uniform float _refractionmultiplier;
		uniform sampler2D RefractionGrab0;
		uniform float _ChromaticAberration;
		uniform sampler2D _TextureSample1;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			o.texcoord_0.xy = v.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
			o.texcoord_1.xy = v.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
		}

		inline float4 Refraction( Input i, SurfaceOutputStandard o, float indexOfRefraction, float chomaticAberration ) {
			float3 worldNormal = o.Normal;
			float4 screenPos = i.screenPos;
			#if UNITY_UV_STARTS_AT_TOP
				float scale = -1.0;
			#else
				float scale = 1.0;
			#endif
			float halfPosW = screenPos.w * 0.5;
			screenPos.y = ( screenPos.y - halfPosW ) * _ProjectionParams.x * scale + halfPosW;
			#if SHADER_API_D3D9 || SHADER_API_D3D11
				screenPos.w += 0.0000001;
			#endif
			float2 projScreenPos = ( screenPos / screenPos.w ).xy;
			float3 worldViewDir = normalize( UnityWorldSpaceViewDir( i.worldPos ) );
			float3 refractionOffset = ( ( ( ( indexOfRefraction - 1.0 ) * mul( UNITY_MATRIX_V, float4( worldNormal, 0.0 ) ) ) * ( 1.0 / ( screenPos.z + 1.0 ) ) ) * ( 1.0 - dot( worldNormal, worldViewDir ) ) );
			float2 cameraRefraction = float2( refractionOffset.x, -( refractionOffset.y * _ProjectionParams.x ) );
			float4 redAlpha = tex2D( RefractionGrab0, ( projScreenPos + cameraRefraction ) );
			float green = tex2D( RefractionGrab0, ( projScreenPos + ( cameraRefraction * ( 1.0 - chomaticAberration ) ) ) ).g;
			float blue = tex2D( RefractionGrab0, ( projScreenPos + ( cameraRefraction * ( 1.0 + chomaticAberration ) ) ) ).b;
			return float4( redAlpha.r, green, blue, redAlpha.a );
		}

		void RefractionF( Input i, SurfaceOutputStandard o, inout fixed4 color )
		{
			#ifdef UNITY_PASS_FORWARDBASE
			float cos3 = cos( _Time[1] );
			float sin3 = sin( _Time[1] );
			float2 rotator3 = mul(i.texcoord_1 - float2( 0.5,0.5 ), float2x2(cos3,-sin3,sin3,cos3)) + float2( 0.5,0.5 );
			float temp_output_13_0 = ( 1.0 - ( distance( i.texcoord_1 , float2( 0.5,0.5 ) ) * _Float0 ) );
				color.rgb = color.rgb + Refraction( i, o, ( tex2D( _TextureSample1,rotator3).a * ( rotator3 * temp_output_13_0 ) ), _ChromaticAberration ) * ( 1 - color.a );
				color.a = 1;
			#endif
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Normal = float3(0,0,1);
			float cos3 = cos( _Time[1] );
			float sin3 = sin( _Time[1] );
			float2 rotator3 = mul(i.texcoord_0 - float2( 0.5,0.5 ), float2x2(cos3,-sin3,sin3,cos3)) + float2( 0.5,0.5 );
			o.Albedo = tex2D( _TextureSample0,rotator3).xyz;
			float temp_output_13_0 = ( 1.0 - ( distance( i.texcoord_0 , float2( 0.5,0.5 ) ) * _Float0 ) );
			o.Alpha = ( 1.0 - ( temp_output_13_0 * _refractionmultiplier ) );
			o.Normal = o.Normal + 0.00001 * i.screenPos * i.worldPos;
		}

		ENDCG
		CGPROGRAM
		#pragma multi_compile _ALPHAPREMULTIPLY_ON
		#pragma surface surf Standard alpha:fade keepalpha finalcolor:RefractionF vertex:vertexDataFunc 

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
				float4 tSpace0 : TEXCOORD1;
				float4 tSpace1 : TEXCOORD2;
				float4 tSpace2 : TEXCOORD3;
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
				fixed3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				fixed tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				fixed3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
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
				float3 worldPos = IN.worldPos;
				fixed3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
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
95;53;1053;645;735.933;-48.79416;1.66776;True;True
Node;AmplifyShaderEditor.Vector2Node;7;-1047.2,456.5;Float;False;Constant;_Vector0;Vector 0;1;0;0.5,0.5
Node;AmplifyShaderEditor.OneMinusNode;13;-314.7997,835.8008;Float;True;0;FLOAT;0.0;False
Node;AmplifyShaderEditor.SamplerNode;20;-388.5647,-80.02155;Float;True;Property;_TextureSample1;Texture Sample 1;1;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;4;-655.0694,85.45168;Float;False;0;-1;2;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-641.9485,833.8138;Float;True;0;FLOAT;0.0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.DistanceOpNode;6;-776.8326,486.5898;Float;False;0;FLOAT2;0.0,0;False;1;FLOAT2;0.0;False
Node;AmplifyShaderEditor.RotatorNode;3;-435.2968,240.4776;Float;False;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;0.0;False
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;215.4834,0.1710422;Float;False;True;2;Float;ASEMaterialInspector;Standard;Seminario/HollowGround;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Transparent;0.5;True;True;0;False;Transparent;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;13;OBJECT;0.0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False
Node;AmplifyShaderEditor.SamplerNode;23;-166.2079,-244.3191;Float;True;Property;_TextureSample0;Texture Sample 0;1;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-159.2792,154.9406;Float;True;0;FLOAT;0.0;False;1;FLOAT2;0.0;False
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-234.8161,475.3835;Float;False;0;FLOAT2;0.0;False;1;FLOAT;0,0;False
Node;AmplifyShaderEditor.RangedFloatNode;25;-230.5755,394.0205;Float;False;Property;_refractionmultiplier;refractionmultiplier;2;0;0;0;1
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;61.28228,440.7178;Float;True;0;FLOAT;0.0;False;1;FLOAT;0.0,0;False
Node;AmplifyShaderEditor.OneMinusNode;28;329.7653,499.0894;Float;True;0;FLOAT;0.0;False
Node;AmplifyShaderEditor.RangedFloatNode;12;-874.7991,882.1996;Float;False;Property;_Float0;Float 0;1;0;2.53;0;0
WireConnection;13;0;11;0
WireConnection;20;1;3;0
WireConnection;11;0;6;0
WireConnection;11;1;12;0
WireConnection;6;0;4;0
WireConnection;6;1;7;0
WireConnection;3;0;4;0
WireConnection;0;0;23;0
WireConnection;0;8;21;0
WireConnection;0;9;28;0
WireConnection;23;1;3;0
WireConnection;21;0;20;4
WireConnection;21;1;22;0
WireConnection;22;0;3;0
WireConnection;22;1;13;0
WireConnection;24;0;13;0
WireConnection;24;1;25;0
WireConnection;28;0;24;0
ASEEND*/
//CHKSM=0555083701CE820ECCAF3F7A6E90B77992D03506