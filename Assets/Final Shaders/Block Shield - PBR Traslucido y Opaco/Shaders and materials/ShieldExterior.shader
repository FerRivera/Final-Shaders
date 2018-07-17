// Upgrade NOTE: replaced 'UNITY_INSTANCE_ID' with 'UNITY_VERTEX_INPUT_INSTANCE_ID'

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Shield exterior"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_Noise1("Noise 1", 2D) = "white" {}
		_Noise2("Noise 2", 2D) = "white" {}
		_Exteriorcolor("Exterior color", Color) = (0,0.9586205,1,1)
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
		};

		uniform sampler2D _Noise2;
		uniform float4 _Noise2_ST;
		uniform sampler2D _Noise1;
		uniform float4 _Noise1_ST;
		uniform float4 _Exteriorcolor;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Noise2 = i.uv_texcoord * _Noise2_ST.xy + _Noise2_ST.zw;
			float2 uv_Noise1 = i.uv_texcoord * _Noise1_ST.xy + _Noise1_ST.zw;
			float4 temp_output_42_0 = ( 1.0 - lerp( tex2D( _Noise2,uv_Noise2) , tex2D( _Noise1,uv_Noise1) , (-0.09 + (_SinTime.w - 0.0) * (0.4 - -0.09) / (1.0 - 0.0)) ) );
			o.Emission = ( temp_output_42_0 * _Exteriorcolor ).rgb;
			o.Alpha = temp_output_42_0.x;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha 

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
22;149;778;625;2072.53;291.916;1.6;True;False
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-478.3187,73.50012;Float;False;True;2;Float;ASEMaterialInspector;Standard;Shield exterior;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;0;False;0;0;Transparent;0.5;True;True;0;False;Transparent;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;13;OBJECT;0.0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False
Node;AmplifyShaderEditor.SinTimeNode;45;-1884.032,340.0839;Float;False
Node;AmplifyShaderEditor.TFHCRemap;48;-1634.73,379.6844;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;-0.09;False;4;FLOAT;0.4;False
Node;AmplifyShaderEditor.OneMinusNode;42;-1169.832,83.18361;Float;False;0;FLOAT4;0.0;False
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;-878.6738,275.2389;Float;True;0;FLOAT4;0.0,0,0,0;False;1;COLOR;0;False
Node;AmplifyShaderEditor.LerpOp;43;-1434.132,66.98372;Float;False;0;FLOAT4;0.0,0,0,0;False;1;FLOAT4;0.0,0,0,0;False;2;FLOAT;0.5;False
Node;AmplifyShaderEditor.SamplerNode;36;-2012.628,-133.395;Float;True;Property;_Noise1;Noise 1;2;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.SamplerNode;44;-1971.632,99.48367;Float;True;Property;_Noise2;Noise 2;2;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.ColorNode;38;-1271.025,246.982;Float;False;Property;_Exteriorcolor;Exterior color;3;0;0,0.9586205,1,1
WireConnection;0;2;39;0
WireConnection;0;9;42;0
WireConnection;48;0;45;4
WireConnection;42;0;43;0
WireConnection;39;0;42;0
WireConnection;39;1;38;0
WireConnection;43;0;44;0
WireConnection;43;1;36;0
WireConnection;43;2;48;0
ASEEND*/
//CHKSM=FA101D9818158B52ACF8188CE47325BA272F1D6E