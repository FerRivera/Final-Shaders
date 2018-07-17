// Upgrade NOTE: replaced 'UNITY_INSTANCE_ID' with 'UNITY_VERTEX_INPUT_INSTANCE_ID'

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Skill obtained"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Opacity("Opacity", Range( 0 , 3)) = 0
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[Header(Translucency)]
		_Translucency("Strength", Range( 0 , 50)) = 1
		_TransNormalDistortion("Normal Distortion", Range( 0 , 1)) = 0.1
		_TransScattering("Scaterring Falloff", Range( 1 , 50)) = 2
		_TransDirect("Direct", Range( 0 , 1)) = 1
		_TransAmbient("Ambient", Range( 0 , 1)) = 0.2
		_TransShadow("Shadow", Range( 0 , 1)) = 0.9
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
		struct Input
		{
			float2 texcoord_0;
			float2 uv_texcoord;
		};

		struct SurfaceOutputStandardCustom
		{
			fixed3 Albedo;
			fixed3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			fixed Alpha;
			fixed3 Translucency;
		};

		uniform sampler2D _TextureSample0;
		uniform half _Translucency;
		uniform half _TransNormalDistortion;
		uniform half _TransScattering;
		uniform half _TransDirect;
		uniform half _TransAmbient;
		uniform half _TransShadow;
		uniform float _Opacity;
		uniform sampler2D _TextureSample1;
		uniform float4 _TextureSample1_ST;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			o.texcoord_0.xy = v.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
		}

		inline half4 LightingStandardCustom(SurfaceOutputStandardCustom s, half3 viewDir, UnityGI gi )
		{
			#if !DIRECTIONAL
			float3 lightAtten = gi.light.color;
			#else
			float3 lightAtten = lerp( _LightColor0, gi.light.color, _TransShadow );
			#endif
			half3 lightDir = gi.light.dir + s.Normal * _TransNormalDistortion;
			half transVdotL = pow( saturate( dot( viewDir, -lightDir ) ), _TransScattering );
			half3 translucency = lightAtten * (transVdotL * _TransDirect + gi.indirect.diffuse * _TransAmbient) * s.Translucency;
			half4 c = half4( s.Albedo * translucency * _Translucency, 0 );

			SurfaceOutputStandard r;
			r.Albedo = s.Albedo;
			r.Normal = s.Normal;
			r.Emission = s.Emission;
			r.Metallic = s.Metallic;
			r.Smoothness = s.Smoothness;
			r.Occlusion = s.Occlusion;
			r.Alpha = s.Alpha;
			return LightingStandard (r, viewDir, gi) + c;
		}

		inline void LightingStandardCustom_GI(SurfaceOutputStandardCustom s, UnityGIInput data, inout UnityGI gi )
		{
			UNITY_GI(gi, s, data);
		}

		void surf( Input i , inout SurfaceOutputStandardCustom o )
		{
			float4 tex2DNode1 = tex2D( _TextureSample0,(abs( i.texcoord_0+_Time[1] * float2(0,-2 ))));
			o.Emission = ( tex2DNode1 * float4(0,0.5066429,0.7573529,0) ).rgb;
			float2 uv_TextureSample1 = i.uv_texcoord * _TextureSample1_ST.xy + _TextureSample1_ST.zw;
			float4 temp_output_13_0 = ( ( tex2DNode1 * _Opacity ) * tex2D( _TextureSample1,uv_TextureSample1) );
			o.Translucency = temp_output_13_0.xyz;
			o.Alpha = temp_output_13_0.x;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardCustom alpha:fade keepalpha vertex:vertexDataFunc 

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
				SurfaceOutputStandardCustom o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandardCustom, o )
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
214;135;778;648;664.3952;182.91;1.3;True;False
Node;AmplifyShaderEditor.SamplerNode;1;-922.7999,-290.6;Float;True;Property;_TextureSample0;Texture Sample 0;0;0;Assets/Final Shaders/Skill obtained - PBR/cloud.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-551.9001,-85.09999;Float;False;0;FLOAT4;0.0;False;1;COLOR;0,0,0,0;False
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-414.4001,168.9;Float;False;0;FLOAT4;0.0;False;1;FLOAT;0,0,0,0;False
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-231.3005,237.7;Float;False;0;FLOAT4;0.0;False;1;FLOAT4;0,0,0,0;False
Node;AmplifyShaderEditor.SamplerNode;11;-610.8,371.1002;Float;True;Property;_TextureSample1;Texture Sample 1;2;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;6;Float;ASEMaterialInspector;Standard;Skill obtained;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;0;False;0;0;Transparent;0.5;True;True;0;False;Transparent;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;1;0;0;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;13;OBJECT;0.0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False
Node;AmplifyShaderEditor.SamplerNode;14;-2358.887,-139.4964;Float;True;Property;_TextureSample2;Texture Sample 2;3;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.SamplerNode;15;-2428.258,105.1282;Float;True;Property;_TextureSample3;Texture Sample 3;4;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.SamplerNode;16;-2355.236,316.8929;Float;True;Property;_TextureSample4;Texture Sample 4;5;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.SamplerNode;17;-2373.492,561.5178;Float;True;Property;_TextureSample5;Texture Sample 5;6;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.LerpOp;18;-1956.671,21.64376;Float;False;0;FLOAT4;0.0;False;1;FLOAT4;0.0,0,0,0;False;2;FLOAT;0.0;False
Node;AmplifyShaderEditor.SinTimeNode;20;-2730.948,240.5505;Float;False
Node;AmplifyShaderEditor.LerpOp;19;-1804.358,171.4218;Float;False;0;FLOAT4;0.0;False;1;FLOAT4;0.0,0,0,0;False;2;FLOAT;0.0;False
Node;AmplifyShaderEditor.LerpOp;21;-1693.509,391.3641;Float;True;0;FLOAT4;0.0;False;1;FLOAT4;0.0,0,0,0;False;2;FLOAT;0.0;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;22;-2907.294,461.191;Float;False;0;-1;2;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False
Node;AmplifyShaderEditor.RangedFloatNode;7;-740.8992,246.1;Float;False;Property;_Opacity;Opacity;1;0;0;0;3
Node;AmplifyShaderEditor.ColorNode;3;-924.9001,-27.2;Float;False;Constant;_Color0;Color 0;1;0;0,0.5066429,0.7573529,0
Node;AmplifyShaderEditor.TextureCoordinatesNode;6;-1531.5,-283.8001;Float;False;0;-1;2;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False
Node;AmplifyShaderEditor.PannerNode;5;-1185.3,-259;Float;False;0;-2;0;FLOAT2;0,0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.RangedFloatNode;24;-1395.194,-112.4093;Float;False;Property;_Float0;Float 0;7;0;0;0;8
WireConnection;1;1;5;0
WireConnection;4;0;1;0
WireConnection;4;1;3;0
WireConnection;8;0;1;0
WireConnection;8;1;7;0
WireConnection;13;0;8;0
WireConnection;13;1;11;0
WireConnection;0;2;4;0
WireConnection;0;7;13;0
WireConnection;0;9;13;0
WireConnection;14;1;22;0
WireConnection;15;1;22;0
WireConnection;16;1;22;0
WireConnection;17;1;22;0
WireConnection;18;0;14;0
WireConnection;18;1;15;0
WireConnection;18;2;20;3
WireConnection;19;0;18;0
WireConnection;19;1;16;0
WireConnection;19;2;20;3
WireConnection;21;0;19;0
WireConnection;21;1;17;0
WireConnection;21;2;20;3
WireConnection;5;0;6;0
ASEEND*/
//CHKSM=A09D2653CC676D419909DBC072FB1B3802F64FFC