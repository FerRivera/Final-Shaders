// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Seminario/WorldDissolveWEmission"
{
	Properties
	{
		_MaskClipValue( "Mask Clip Value", Float ) = 0.5
		[HideInInspector] __dirty( "", Int ) = 1
		_Albedo("Albedo", 2D) = "white" {}
		_Normals("Normals", 2D) = "bump" {}
		_AO("AO", 2D) = "white" {}
		_MetallicValue("MetallicValue", Range( 0 , 1)) = 0
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		_Metallic("Metallic", 2D) = "white" {}
		_CenterPos("CenterPos", Vector) = (0,0,0,0)
		_Emission("Emission", 2D) = "white" {}
		_Distance("Distance", Float) = 0
		_Interpolation("Interpolation", Float) = 0
		_EmissionColor("EmissionColor", Color) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
		};

		uniform sampler2D _Normals;
		uniform float4 _Normals_ST;
		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform sampler2D _Emission;
		uniform float4 _Emission_ST;
		uniform float4 _EmissionColor;
		uniform sampler2D _Metallic;
		uniform float4 _Metallic_ST;
		uniform float _Smoothness;
		uniform float _MetallicValue;
		uniform sampler2D _AO;
		uniform float4 _AO_ST;
		uniform float _Distance;
		uniform float3 _CenterPos;
		uniform float _Interpolation;
		uniform float _MaskClipValue = 0.5;


		float3 mod289( float3 x ) { return x - floor( x / 289.0 ) * 289.0; }

		float4 mod289( float4 x ) { return x - floor( x / 289.0 ) * 289.0; }

		float4 permute( float4 x ) { return mod289( ( x * 34.0 + 1.0 ) * x ); }

		float4 taylorInvSqrt( float4 r ) { return 1.79284291400159 - r * 0.85373472095314; }

		float snoise( float3 v )
		{
			const float2 C = float2( 1.0 / 6.0, 1.0 / 3.0 );
			float3 i = floor( v + dot( v, C.yyy ) );
			float3 x0 = v - i + dot( i, C.xxx );
			float3 g = step( x0.yzx, x0.xyz );
			float3 l = 1.0 - g;
			float3 i1 = min( g.xyz, l.zxy );
			float3 i2 = max( g.xyz, l.zxy );
			float3 x1 = x0 - i1 + C.xxx;
			float3 x2 = x0 - i2 + C.yyy;
			float3 x3 = x0 - 0.5;
			i = mod289( i);
			float4 p = permute( permute( permute( i.z + float4( 0.0, i1.z, i2.z, 1.0 ) ) + i.y + float4( 0.0, i1.y, i2.y, 1.0 ) ) + i.x + float4( 0.0, i1.x, i2.x, 1.0 ) );
			float4 j = p - 49.0 * floor( p / 49.0 );  // mod(p,7*7)
			float4 x_ = floor( j / 7.0 );
			float4 y_ = floor( j - 7.0 * x_ );  // mod(j,N)
			float4 x = ( x_ * 2.0 + 0.5 ) / 7.0 - 1.0;
			float4 y = ( y_ * 2.0 + 0.5 ) / 7.0 - 1.0;
			float4 h = 1.0 - abs( x ) - abs( y );
			float4 b0 = float4( x.xy, y.xy );
			float4 b1 = float4( x.zw, y.zw );
			float4 s0 = floor( b0 ) * 2.0 + 1.0;
			float4 s1 = floor( b1 ) * 2.0 + 1.0;
			float4 sh = -step( h, 0.0 );
			float4 a0 = b0.xzyw + s0.xzyw * sh.xxyy;
			float4 a1 = b1.xzyw + s1.xzyw * sh.zzww;
			float3 g0 = float3( a0.xy, h.x );
			float3 g1 = float3( a0.zw, h.y );
			float3 g2 = float3( a1.xy, h.z );
			float3 g3 = float3( a1.zw, h.w );
			float4 norm = taylorInvSqrt( float4( dot( g0, g0 ), dot( g1, g1 ), dot( g2, g2 ), dot( g3, g3 ) ) );
			g0 *= norm.x;
			g1 *= norm.y;
			g2 *= norm.z;
			g3 *= norm.w;
			float4 m = max( 0.6 - float4( dot( x0, x0 ), dot( x1, x1 ), dot( x2, x2 ), dot( x3, x3 ) ), 0.0 );
			m = m* m;
			m = m* m;
			float4 px = float4( dot( x0, g0 ), dot( x1, g1 ), dot( x2, g2 ), dot( x3, g3 ) );
			return 42.0 * dot( m, px);
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Normals = i.uv_texcoord * _Normals_ST.xy + _Normals_ST.zw;
			o.Normal = UnpackNormal( tex2D( _Normals,uv_Normals) );
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			o.Albedo = tex2D( _Albedo,uv_Albedo).xyz;
			float2 uv_Emission = i.uv_texcoord * _Emission_ST.xy + _Emission_ST.zw;
			o.Emission = ( tex2D( _Emission,uv_Emission) * _EmissionColor ).rgb;
			float2 uv_Metallic = i.uv_texcoord * _Metallic_ST.xy + _Metallic_ST.zw;
			float4 tex2DNode125 = tex2D( _Metallic,uv_Metallic);
			o.Metallic = ( tex2DNode125.a * _Smoothness );
			o.Smoothness = ( tex2DNode125 * _MetallicValue ).x;
			float2 uv_AO = i.uv_texcoord * _AO_ST.xy + _AO_ST.zw;
			o.Occlusion = tex2D( _AO,uv_AO).x;
			o.Alpha = 1;
			float simplePerlin3D90 = snoise( i.worldPos );
			clip( ( 1.0 - ( _Distance - ( ( length( ( _CenterPos - i.worldPos ) ) + ( simplePerlin3D90 * _Interpolation ) ) - 0.5 ) ) ) - _MaskClipValue );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=5105
582;186;1266;765;992.8069;745.8179;1.66776;True;True
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;6.198883E-06,1.3;Float;False;True;2;Float;ASEMaterialInspector;Standard;Seminario/WorldDissolveWEmission;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Custom;0.5;True;True;0;True;TransparentCutout;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;13;OBJECT;0.0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False
Node;AmplifyShaderEditor.SamplerNode;122;-432,-640;Float;True;Property;_Albedo;Albedo;4;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.SamplerNode;124;-432,-224;Float;True;Property;_AO;AO;6;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.SamplerNode;125;-64,-432;Float;True;Property;_Metallic;Metallic;7;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.SimpleSubtractOpNode;141;-523.8879,48.02098;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.WorldPosInputsNode;136;-1019.153,588.5395;Float;True
Node;AmplifyShaderEditor.SimpleSubtractOpNode;137;-783.8517,541.1459;Float;False;0;FLOAT3;0.0,0,0;False;1;FLOAT3;0.0,0,0,0;False
Node;AmplifyShaderEditor.WorldPosInputsNode;2;-1026.393,790.2599;Float;True
Node;AmplifyShaderEditor.Vector3Node;135;-1009.565,175.8066;Float;False;Property;_CenterPos;CenterPos;8;0;0,0,0
Node;AmplifyShaderEditor.OneMinusNode;152;-199.547,162.7265;Float;False;0;FLOAT;0.0;False
Node;AmplifyShaderEditor.RangedFloatNode;154;210.8799,-99.41479;Float;False;Property;_Smoothness;Smoothness;7;0;0;0;1
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;155;533.3419,-214.3203;Float;False;0;FLOAT4;0.0;False;1;FLOAT;0.0,0,0,0;False
Node;AmplifyShaderEditor.RangedFloatNode;156;265.3105,-4.374427;Float;False;Property;_MetallicValue;MetallicValue;7;0;0;0;1
Node;AmplifyShaderEditor.SimpleSubtractOpNode;147;-174.6233,618.634;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.SimpleAddOpNode;142;-332.9597,676.1254;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.NoiseGeneratorNode;90;-805.5936,790.2599;Float;False;Simplex3D;0;FLOAT3;100,100,100;False
Node;AmplifyShaderEditor.LengthOpNode;134;-644.8787,490.5812;Float;False;0;FLOAT3;0,0,0,0;False
Node;AmplifyShaderEditor.RangedFloatNode;148;-174.2556,799.4915;Float;False;Constant;_Float0;Float 0;11;0;0.5;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;145;-587.7319,792.7291;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.RangedFloatNode;143;-781.7116,948.4363;Float;False;Property;_Interpolation;Interpolation;10;0;0;0;0
Node;AmplifyShaderEditor.RangedFloatNode;140;-799.3448,107.482;Float;False;Property;_Distance;Distance;9;0;0;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;153;323.9468,-243.6218;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.SamplerNode;123;-432,-432;Float;True;Property;_Normals;Normals;5;0;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.SamplerNode;157;-64,-656;Float;True;Property;_Emission;Emission;9;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;158;444.4669,-541.9349;Float;False;0;FLOAT4;0.0;False;1;COLOR;0.0,0,0,0;False
Node;AmplifyShaderEditor.ColorNode;159;316.5623,-435.6761;Float;False;Property;_EmissionColor;EmissionColor;10;0;0,0,0,0
WireConnection;0;0;122;0
WireConnection;0;1;123;0
WireConnection;0;2;158;0
WireConnection;0;3;153;0
WireConnection;0;4;155;0
WireConnection;0;5;124;0
WireConnection;0;10;152;0
WireConnection;141;0;140;0
WireConnection;141;1;147;0
WireConnection;137;0;135;0
WireConnection;137;1;136;0
WireConnection;152;0;141;0
WireConnection;155;0;125;0
WireConnection;155;1;156;0
WireConnection;147;0;142;0
WireConnection;147;1;148;0
WireConnection;142;0;134;0
WireConnection;142;1;145;0
WireConnection;90;0;2;0
WireConnection;134;0;137;0
WireConnection;145;0;90;0
WireConnection;145;1;143;0
WireConnection;153;0;125;4
WireConnection;153;1;154;0
WireConnection;158;0;157;0
WireConnection;158;1;159;0
ASEEND*/
//CHKSM=49C7D7A7C3F833E1E2F26A6415869807E6E4856F