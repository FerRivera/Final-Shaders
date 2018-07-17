// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Explotion Shader"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_MaskClipValue( "Mask Clip Value", Float ) = 0.5
		_LavaTexture("Lava Texture", 2D) = "white" {}
		_Dissolve("Dissolve", Range( 0 , 0.8)) = 0
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float2 texcoord_0;
		};

		uniform sampler2D _LavaTexture;
		uniform float _Dissolve;
		uniform float _MaskClipValue = 0.5;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			o.texcoord_0.xy = v.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 tex2DNode7 = tex2D( _LavaTexture,(abs( i.texcoord_0+_Time[1] * float2(0,-0.2 ))));
			o.Emission = tex2DNode7.xyz;
			o.Alpha = 1;
			float4 temp_cast_1 = (-0.5 + (( 1.0 - _Dissolve ) - 0.0) * (0.5 - -0.5) / (1.0 - 0.0));
			clip( ( temp_cast_1 + tex2DNode7 ) - ( _MaskClipValue ).xxxx );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=5105
232;539;835;640;1327.58;180.4982;2.009091;True;False
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;168.9429,188.7749;Float;False;True;2;Float;ASEMaterialInspector;Standard;Explotion Shader;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Masked;0.5;True;True;0;False;TransparentCutout;AlphaTest;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;13;OBJECT;0.0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False
Node;AmplifyShaderEditor.SimpleAddOpNode;49;-78.36917,455.8089;Float;False;0;FLOAT;0.0,0,0,0;False;1;FLOAT4;0.0;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;83;-1014.853,512.4286;Float;False;0;-1;2;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False
Node;AmplifyShaderEditor.PannerNode;84;-723.9066,514.7375;Float;False;0;-0.2;0;FLOAT2;0,0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.SamplerNode;7;-488.8481,466.6011;Float;True;Property;_LavaTexture;Lava Texture;0;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.TFHCRemap;58;-272.7485,206.0911;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;-0.5;False;4;FLOAT;0.5;False
Node;AmplifyShaderEditor.OneMinusNode;85;-464.0617,180.82;Float;False;0;FLOAT;0.0;False
Node;AmplifyShaderEditor.RangedFloatNode;8;-802.8251,141.6043;Float;False;Property;_Dissolve;Dissolve;1;0;0;0;0.8
WireConnection;0;2;7;0
WireConnection;0;10;49;0
WireConnection;49;0;58;0
WireConnection;49;1;7;0
WireConnection;84;0;83;0
WireConnection;7;1;84;0
WireConnection;58;0;85;0
WireConnection;85;0;8;0
ASEEND*/
//CHKSM=C6B02E4F097147587FC77E8F36F950B4D8DD114C