// Upgrade NOTE: replaced 'UNITY_INSTANCE_ID' with 'UNITY_VERTEX_INPUT_INSTANCE_ID'

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Seminario/SkillSphereShader2"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_Power("Power", Float) = 0
		_Bias("Bias", Float) = 0
		_SphereColor("SphereColor", Color) = (0,0,0,0)
		_Scale("Scale", Float) = 0
		_Noise1("Noise1", 2D) = "white" {}
		_Noise2("Noise2", 2D) = "white" {}
		_Noise3("Noise3", 2D) = "white" {}
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) fixed3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float2 texcoord_0;
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
		};

		uniform float4 _SphereColor;
		uniform sampler2D _Noise3;
		uniform sampler2D _Noise1;
		uniform sampler2D _Noise2;
		uniform float _Bias;
		uniform float _Scale;
		uniform float _Power;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			o.texcoord_0.xy = v.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Normal = float3(0,0,1);
			float temp_output_21_0 = (0.5 + (_SinTime.w - 0.0) * (1.0 - 0.5) / (1.0 - 0.0));
			float4 temp_output_25_0 = ( _SphereColor * lerp( tex2D( _Noise3,(abs( i.texcoord_0+_Time[1] * float2(-0.5,-0.5 )))) , lerp( tex2D( _Noise1,(abs( i.texcoord_0+_Time[1] * float2(0.5,0 )))) , tex2D( _Noise2,(abs( i.texcoord_0+_Time[1] * float2(0.5,0.5 )))) , temp_output_21_0 ) , temp_output_21_0 ) );
			o.Albedo = temp_output_25_0.xyz;
			float3 worldViewDir = normalize( UnityWorldSpaceViewDir( i.worldPos ) );
			float3 worldNormal = WorldNormalVector( i, float3(0,0,1) );
			float3 vertexNormal = mul( unity_WorldToObject, float4( worldNormal, 0 ) );
			float fresnelFinalVal2 = (_Bias + _Scale*pow( 1.0 - dot( vertexNormal, worldViewDir ) , _Power));
			o.Emission = ( ( _SphereColor * fresnelFinalVal2 ) + temp_output_25_0 ).xyz;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha vertex:vertexDataFunc 

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
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				fixed3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
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
532;124;1266;765;1244.403;188.9002;1.7;True;True
Node;AmplifyShaderEditor.RangedFloatNode;3;-988.5005,216.1001;Float;False;Property;_Bias;Bias;0;0;0;0;0
Node;AmplifyShaderEditor.NormalVertexDataNode;6;-988.5005,72.09997;Float;False
Node;AmplifyShaderEditor.RangedFloatNode;4;-988.5005,376.1001;Float;False;Property;_Power;Power;0;0;0;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-556.4999,120.1001;Float;True;0;COLOR;0.0;False;1;FLOAT;0.0,0,0,0;False
Node;AmplifyShaderEditor.FresnelNode;2;-812.5003,200.1001;Float;False;0;FLOAT3;0,0,0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;5.0;False
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;Standard;Seminario/SkillSphereShader2;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;13;OBJECT;0.0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False
Node;AmplifyShaderEditor.RangedFloatNode;5;-988.5005,296.1001;Float;False;Property;_Scale;Scale;0;0;0;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;13;-960,512;Float;False;0;-1;2;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False
Node;AmplifyShaderEditor.SamplerNode;16;-672,672;Float;True;Property;_Noise2;Noise2;5;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.LerpOp;18;-231.6021,686.2998;Float;False;0;FLOAT4;0.0;False;1;FLOAT4;0.0,0,0,0;False;2;FLOAT;0.0;False
Node;AmplifyShaderEditor.SinTimeNode;17;-704,1104;Float;False
Node;AmplifyShaderEditor.TFHCRemap;21;-544,1104;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;0.5;False;4;FLOAT;1.0;False
Node;AmplifyShaderEditor.SamplerNode;22;-672,880;Float;True;Property;_Noise3;Noise3;6;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.LerpOp;24;-30.40186,897.8998;Float;False;0;FLOAT4;0.0;False;1;FLOAT4;0.0,0,0,0;False;2;FLOAT;0.0;False
Node;AmplifyShaderEditor.SamplerNode;10;-672,432;Float;True;Property;_Noise1;Noise1;4;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;280.499,854.7999;Float;False;0;COLOR;0.0;False;1;FLOAT4;0.0,0,0,0;False
Node;AmplifyShaderEditor.SimpleAddOpNode;27;-44.90295,495.0998;Float;False;0;COLOR;0.0;False;1;FLOAT4;0.0,0,0,0;False
Node;AmplifyShaderEditor.PannerNode;15;-930.8002,713.4999;Float;False;0.5;0;0;FLOAT2;0,0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.PannerNode;20;-928,816;Float;False;0.5;0.5;0;FLOAT2;0,0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.PannerNode;23;-928,928;Float;False;-0.5;-0.5;0;FLOAT2;0,0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.ColorNode;1;-828.5004,-7.899998;Float;False;Property;_SphereColor;SphereColor;0;0;0,0,0,0
WireConnection;7;0;1;0
WireConnection;7;1;2;0
WireConnection;2;0;6;0
WireConnection;2;1;3;0
WireConnection;2;2;5;0
WireConnection;2;3;4;0
WireConnection;0;0;25;0
WireConnection;0;2;27;0
WireConnection;16;1;20;0
WireConnection;18;0;10;0
WireConnection;18;1;16;0
WireConnection;18;2;21;0
WireConnection;21;0;17;4
WireConnection;22;1;23;0
WireConnection;24;0;22;0
WireConnection;24;1;18;0
WireConnection;24;2;21;0
WireConnection;10;1;15;0
WireConnection;25;0;1;0
WireConnection;25;1;24;0
WireConnection;27;0;7;0
WireConnection;27;1;25;0
WireConnection;15;0;13;0
WireConnection;20;0;13;0
WireConnection;23;0;13;0
ASEEND*/
//CHKSM=9F02FCE918E6A4FDF0C29B880A46F0D676096F6A