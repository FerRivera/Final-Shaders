// Upgrade NOTE: replaced 'UNITY_INSTANCE_ID' with 'UNITY_VERTEX_INPUT_INSTANCE_ID'

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "HallowedGround"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_MaskClipValue( "Mask Clip Value", Float ) = 0.5
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_Node2D("Node 2D", Range( 0 , 10)) = 1.3
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Geometry+0" }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 texcoord_0;
			float2 texcoord_1;
		};

		uniform sampler2D _TextureSample1;
		uniform float _Node2D;
		uniform float _MaskClipValue = 0.5;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			o.texcoord_0.xy = v.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
			o.texcoord_1.xy = v.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float temp_output_14_0 = ( 1.0 - ( ( distance( i.texcoord_0 , float2( 0.5,0.5 ) ) * 2.0 ) * 1.2 ) );
			float cos28 = cos( _Time[1] );
			float sin28 = sin( _Time[1] );
			float2 rotator28 = mul(i.texcoord_1 - ( ( clamp( ( 1.0 - 0.0 ) , 0.0 , 1.0 ) * sin( _Time.w ) ) * _Node2D ), float2x2(cos28,-sin28,sin28,cos28)) + ( ( clamp( ( 1.0 - 0.0 ) , 0.0 , 1.0 ) * sin( _Time.w ) ) * _Node2D );
			float4 appendResult16 = float4( ( temp_output_14_0 * rotator28.x ) , ( temp_output_14_0 * rotator28.y ) , 0 , 0 );
			float4 tex2DNode2 = tex2D( _TextureSample1,appendResult16.xy);
			o.Albedo = tex2DNode2.xyz;
			o.Alpha = ( tex2DNode2 * 5.9 ).x;
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
465;95;935;389;1526.055;148.2299;2.569229;True;True
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;324.8,71.2;Float;False;True;2;Float;ASEMaterialInspector;Standard;HallowedGround;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Custom;0.5;True;True;0;True;Transparent;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;13;OBJECT;0.0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;139.1979,305.5008;Float;False;0;FLOAT4;0.0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.SamplerNode;2;-275.0994,66.59997;Float;True;Property;_TextureSample1;Texture Sample 1;1;0;Assets/Art/Shaders/Skills/Hallowed Ground/ws_Blue_Space_Points_Stars_1280x720 (1).jpg;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;3;-2416.011,-117.2154;Float;False;0;-1;2;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False
Node;AmplifyShaderEditor.Vector2Node;5;-2404.01,95.7848;Float;False;Constant;_Vector0;Vector 0;2;0;0.5,0.5
Node;AmplifyShaderEditor.DistanceOpNode;6;-2130.909,-14.21527;Float;True;0;FLOAT2;0.0;False;1;FLOAT2;0.0,0;False
Node;AmplifyShaderEditor.RangedFloatNode;11;-2110.211,252.1849;Float;False;Constant;_Float0;Float 0;2;0;2;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-1842.713,57.38465;Float;True;0;FLOAT;0,0,0,0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.RangedFloatNode;13;-1775.211,342.1849;Float;False;Constant;_Float1;Float 1;2;0;1.2;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-1542.71,179.6849;Float;True;0;FLOAT;0.0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.OneMinusNode;14;-1285.209,232.1849;Float;True;0;FLOAT;0.0;False
Node;AmplifyShaderEditor.TimeNode;19;-2375.811,707.0848;Float;False
Node;AmplifyShaderEditor.SinOpNode;21;-2055.812,746.0847;Float;False;0;FLOAT;0.0;False
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-1815.012,726.3848;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.OneMinusNode;23;-2355.912,514.085;Float;False;0;FLOAT;0.0;False
Node;AmplifyShaderEditor.ClampOpNode;24;-2107.912,482.085;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-1595.111,802.3844;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;27;-1762.312,567.1853;Float;False;0;-1;2;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False
Node;AmplifyShaderEditor.RotatorNode;28;-1398.311,659.5852;Float;False;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False
Node;AmplifyShaderEditor.BreakToComponentsNode;33;-1161.111,640.5851;Float;False;FLOAT2;0;FLOAT2;0.0;False
Node;AmplifyShaderEditor.RangedFloatNode;26;-1908.712,987.184;Float;False;Property;_Node2D;Node 2D;2;0;1.3;0;10
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-844.2087,301.5696;Float;True;0;FLOAT;0.0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.AppendNode;16;-524.2088,384.0698;Float;False;FLOAT4;0;0;0;0;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-677.8091,601.7701;Float;True;0;FLOAT;0.0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.RangedFloatNode;35;-164.1329,421.4006;Float;False;Constant;_Opacity;Opacity;3;0;5.9;0;10
WireConnection;0;0;2;0
WireConnection;0;9;34;0
WireConnection;34;0;2;0
WireConnection;34;1;35;0
WireConnection;2;1;16;0
WireConnection;6;0;3;0
WireConnection;6;1;5;0
WireConnection;8;0;6;0
WireConnection;8;1;11;0
WireConnection;12;0;8;0
WireConnection;12;1;13;0
WireConnection;14;0;12;0
WireConnection;21;0;19;4
WireConnection;22;0;24;0
WireConnection;22;1;21;0
WireConnection;24;0;23;0
WireConnection;25;0;22;0
WireConnection;25;1;26;0
WireConnection;28;0;27;0
WireConnection;28;1;25;0
WireConnection;33;0;28;0
WireConnection;15;0;14;0
WireConnection;15;1;33;0
WireConnection;16;0;15;0
WireConnection;16;1;17;0
WireConnection;17;0;14;0
WireConnection;17;1;33;1
ASEEND*/
//CHKSM=F45A585545070AEF1E24F65C18F578F5AB41C5A6