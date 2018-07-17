// Upgrade NOTE: replaced 'UNITY_INSTANCE_ID' with 'UNITY_VERTEX_INPUT_INSTANCE_ID'

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Seminario/Sphere Skill Room"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_MaskClipValue( "Mask Clip Value", Float ) = 0.5
		_Noise("Noise", 2D) = "white" {}
		_Noise2("Noise2", 2D) = "white" {}
		_Noise3("Noise3", 2D) = "white" {}
		_Hardness("Hardness", Range( 0 , 1)) = 0.31
		_Radius("Radius", Range( 0 , 1)) = 0.31
		_SphereColor("SphereColor", Color) = (0,0,0,0)
		_Bias("Bias", Float) = -0.07
		_Power("Power", Float) = 0
		_Scale("Scale", Float) = 0
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
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
			float2 texcoord_1;
		};

		uniform float _Radius;
		uniform float4 _SphereColor;
		uniform float _Bias;
		uniform float _Scale;
		uniform float _Power;
		uniform float _Hardness;
		uniform sampler2D _Noise;
		uniform sampler2D _Noise2;
		uniform sampler2D _Noise3;
		uniform float _MaskClipValue = 0.5;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			o.texcoord_0.xy = v.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
			o.texcoord_1.xy = v.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
			float temp_output_7_0 = distance( o.texcoord_1 , float2( 0.5,0.5 ) );
			float3 temp_cast_0 = ( 1.0 - ( ( temp_output_7_0 - 0.3273872 ) / 0.1982458 ) );
			v.normal = temp_cast_0;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float temp_output_7_0 = distance( i.texcoord_0 , float2( 0.5,0.5 ) );
			float temp_output_25_0 = ( temp_output_7_0 - _Radius );
			float3 temp_cast_0 = temp_output_25_0;
			o.Normal = temp_cast_0;
			float3 worldViewDir = normalize( UnityWorldSpaceViewDir( i.worldPos ) );
			float3 worldNormal = WorldNormalVector( i, float3(0,0,1) );
			float3 vertexNormal = mul( unity_WorldToObject, float4( worldNormal, 0 ) );
			float fresnelFinalVal90 = (_Bias + _Scale*pow( 1.0 - dot( vertexNormal, worldViewDir ) , _Power));
			float temp_output_63_0 = clamp( ( 1.0 - ( temp_output_25_0 / ( 1.0 - _Hardness ) ) ) , 0.0 , 1.0 );
			float cos9 = cos( _Time[1] );
			float sin9 = sin( _Time[1] );
			float2 rotator9 = mul(i.texcoord_0 - float2( 0.5,0.5 ), float2x2(cos9,-sin9,sin9,cos9)) + float2( 0.5,0.5 );
			float cos13 = cos( _Time[1] );
			float sin13 = sin( _Time[1] );
			float2 rotator13 = mul(i.texcoord_0 - float2( 0,0 ), float2x2(cos13,-sin13,sin13,cos13)) + float2( 0,0 );
			float4 temp_cast_2 = clamp( ( ( temp_output_7_0 - 0.3273872 ) / 0.1100354 ) , 0.0 , 1.0 );
			o.Emission = ( ( _SphereColor * fresnelFinalVal90 ) * ( temp_output_63_0 * ( ( temp_output_63_0 * lerp( lerp( tex2D( _Noise,rotator9) , tex2D( _Noise2,rotator13) , _SinTime.z ) , tex2D( _Noise3,rotator9) , _SinTime.z ) ) + temp_cast_2 ) ) ).xyz;
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
532;124;1266;765;688.7189;126.9235;1.9;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;3;-1120,112;Float;False;0;-1;2;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False
Node;AmplifyShaderEditor.RotatorNode;9;-864,32;Float;False;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;0.0;False
Node;AmplifyShaderEditor.SamplerNode;1;-656,-48;Float;True;Property;_Noise;Noise;0;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.Vector2Node;6;-1136,304;Float;False;Constant;_Vector0;Vector 0;1;0;0.5,0.5
Node;AmplifyShaderEditor.RotatorNode;13;-864,192;Float;False;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;-1.0;False
Node;AmplifyShaderEditor.SamplerNode;11;-656,161.3;Float;True;Property;_Noise2;Noise2;1;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.SamplerNode;14;-656,368;Float;True;Property;_Noise3;Noise3;2;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.LerpOp;10;-348.5986,180.6005;Float;False;0;FLOAT4;0.0;False;1;FLOAT4;0.0,0,0,0;False;2;FLOAT;0.0;False
Node;AmplifyShaderEditor.LerpOp;15;-320,304;Float;True;0;FLOAT4;0.0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0.0;False
Node;AmplifyShaderEditor.DistanceOpNode;7;-939,423;Float;True;0;FLOAT2;0,0;False;1;FLOAT2;0.0;False
Node;AmplifyShaderEditor.SinTimeNode;12;-470,620;Float;False
Node;AmplifyShaderEditor.SimpleSubtractOpNode;25;-675.8009,713.4004;Float;True;0;FLOAT;0.0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.RangedFloatNode;21;-1008,720;Float;False;Property;_Radius;Radius;3;0;0.31;0;1
Node;AmplifyShaderEditor.OneMinusNode;28;-672,960;Float;False;0;FLOAT;0.0;False
Node;AmplifyShaderEditor.RangedFloatNode;27;-1009.7,800;Float;False;Property;_Hardness;Hardness;3;0;0.31;0;1
Node;AmplifyShaderEditor.OneMinusNode;60;-264.1165,858.6002;Float;True;0;FLOAT;0.0;False
Node;AmplifyShaderEditor.SimpleDivideOpNode;26;-463.9999,865.6008;Float;True;0;FLOAT;0.0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.SimpleDivideOpNode;41;-427.6999,1264;Float;True;0;FLOAT;0.0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.OneMinusNode;67;-182.5145,1256.4;Float;True;0;FLOAT;0.0;False
Node;AmplifyShaderEditor.ClampOpNode;63;-103.9162,864.8;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False
Node;AmplifyShaderEditor.RangedFloatNode;33;-1019.005,1074.402;Float;False;Constant;_RadiusFixed;RadiusFixed;3;0;0.3273872;0;1
Node;AmplifyShaderEditor.SimpleSubtractOpNode;32;-688,1072;Float;True;0;FLOAT;0.0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.RangedFloatNode;72;-1040,1440;Float;False;Constant;_Float3;Float 3;3;0;0.1100354;0;1
Node;AmplifyShaderEditor.SimpleSubtractOpNode;71;-688,1328;Float;True;0;FLOAT;0.0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.SimpleDivideOpNode;73;-416,1520;Float;True;0;FLOAT;0.0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.RangedFloatNode;42;-1017.008,1163.499;Float;False;Constant;_HardNessFixed;HardNessFixed;3;0;0.1982458;0;1
Node;AmplifyShaderEditor.ClampOpNode;82;66.28584,1504.997;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False
Node;AmplifyShaderEditor.RangedFloatNode;70;-1040,1360;Float;False;Constant;_Float1;Float 1;3;0;0.3273872;0;1
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;Standard;Seminario/Sphere Skill Room;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Custom;0.5;True;True;0;True;TransparentCutout;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;13;OBJECT;0.0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;79;82.33309,958.2613;Float;True;0;FLOAT;0.0;False;1;FLOAT4;0;False
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;86;428.14,1053.614;Float;True;0;FLOAT;0.0;False;1;FLOAT4;0.0;False
Node;AmplifyShaderEditor.SimpleAddOpNode;85;176,1264;Float;True;0;FLOAT4;0.0;False;1;FLOAT;0.0,0,0,0;False
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;87;599.9525,813.6993;Float;True;0;COLOR;0.0,0,0,0;False;1;FLOAT4;0.0;False
Node;AmplifyShaderEditor.FresnelNode;90;1120,912;Float;False;0;FLOAT3;0,0,0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;5.0;False
Node;AmplifyShaderEditor.NormalVertexDataNode;94;928,752;Float;False
Node;AmplifyShaderEditor.RangedFloatNode;93;928,1072;Float;False;Property;_Power;Power;6;0;0;0;0
Node;AmplifyShaderEditor.RangedFloatNode;92;926.4,992;Float;False;Property;_Scale;Scale;6;0;0;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;89;1324.08,701.5771;Float;True;0;COLOR;0.0;False;1;FLOAT;0.0,0,0,0;False
Node;AmplifyShaderEditor.RangedFloatNode;91;928,912;Float;False;Property;_Bias;Bias;6;0;-0.07;0;0
Node;AmplifyShaderEditor.ColorNode;88;349.3967,638.1895;Float;False;Property;_SphereColor;SphereColor;5;0;0,0,0,0
WireConnection;9;0;3;0
WireConnection;1;1;9;0
WireConnection;13;0;3;0
WireConnection;11;1;13;0
WireConnection;14;1;9;0
WireConnection;10;0;1;0
WireConnection;10;1;11;0
WireConnection;10;2;12;3
WireConnection;15;0;10;0
WireConnection;15;1;14;0
WireConnection;15;2;12;3
WireConnection;7;0;3;0
WireConnection;7;1;6;0
WireConnection;25;0;7;0
WireConnection;25;1;21;0
WireConnection;28;0;27;0
WireConnection;60;0;26;0
WireConnection;26;0;25;0
WireConnection;26;1;28;0
WireConnection;41;0;32;0
WireConnection;41;1;42;0
WireConnection;67;0;41;0
WireConnection;63;0;60;0
WireConnection;32;0;7;0
WireConnection;32;1;33;0
WireConnection;71;0;7;0
WireConnection;71;1;70;0
WireConnection;73;0;71;0
WireConnection;73;1;72;0
WireConnection;82;0;73;0
WireConnection;0;1;25;0
WireConnection;0;2;87;0
WireConnection;0;12;67;0
WireConnection;79;0;63;0
WireConnection;79;1;15;0
WireConnection;86;0;63;0
WireConnection;86;1;85;0
WireConnection;85;0;79;0
WireConnection;85;1;82;0
WireConnection;87;0;89;0
WireConnection;87;1;86;0
WireConnection;90;0;94;0
WireConnection;90;1;91;0
WireConnection;90;2;92;0
WireConnection;90;3;93;0
WireConnection;89;0;88;0
WireConnection;89;1;90;0
ASEEND*/
//CHKSM=06B9A162839436F43D311C8A80634D8EF4DABBAE