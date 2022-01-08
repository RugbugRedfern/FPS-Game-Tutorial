// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SyntyStudios/DamageZone"
{
	Properties
	{
		_Gradient_Opacity("Gradient_Opacity", Range( 0 , 1)) = 1
		_Gradient_Height("Gradient_Height", Float) = 1
		_Gradient_Offset("Gradient_Offset", Range( 0 , 1.2)) = 1
		_Gradient_Smoothness("Gradient_Smoothness", Range( 0.001 , 100)) = 0.001
		_Zone_Rim("Zone_Rim", Range( 0 , 1)) = 0.2
		_Zone_Fresnel("Zone_Fresnel", Range( 0 , 10)) = 1
		_Zone_Power("Zone_Power", Range( 0 , 10)) = 1
		_Zone_Color("Zone_Color", Color) = (1,0,0,1)
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma surface surf Standard alpha:fade keepalpha noshadow nolightmap  
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			float4 screenPos;
		};

		uniform float _Zone_Fresnel;
		uniform float4 _Zone_Color;
		uniform sampler2D _CameraDepthTexture;
		uniform float _Zone_Rim;
		uniform float _Zone_Power;
		uniform float _Gradient_Opacity;
		uniform float _Gradient_Offset;
		uniform float _Gradient_Height;
		uniform float _Gradient_Smoothness;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNDotV1 = dot( normalize( ase_worldNormal ), ase_worldViewDir );
			float fresnelNode1 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNDotV1, (10.0 + (_Zone_Fresnel - 0.0) * (0.0 - 10.0) / (10.0 - 0.0)) ) );
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth80 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD(ase_screenPos))));
			float distanceDepth80 = abs( ( screenDepth80 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _Zone_Rim ) );
			float clampResult85 = clamp( distanceDepth80 , 0.0 , 1.0 );
			float4 lerpResult84 = lerp( float4(1,1,1,0) , _Zone_Color , clampResult85);
			o.Emission = ( fresnelNode1 + ( lerpResult84 * _Zone_Power ) ).rgb;
			float clampResult33 = clamp( ( ( _Gradient_Offset + ase_worldPos.y ) / _Gradient_Height ) , 0.0 , 1.0 );
			float4 lerpResult41 = lerp( ( float4(1,1,1,0) * _Gradient_Opacity ) , float4(0,0,0,0) , saturate( pow( clampResult33 , _Gradient_Smoothness ) ));
			o.Alpha = lerpResult41.r;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15301
2619;84;2473;1498;469.3959;776.149;1.17;True;True
Node;AmplifyShaderEditor.CommentaryNode;39;299.5315,-694.728;Float;False;1345.422;739.1632;;14;41;45;42;37;43;40;34;33;32;31;30;29;27;28;Opacity;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldPosInputsNode;27;315.6243,-268.5481;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;28;315.3853,-633.4678;Float;False;Property;_Gradient_Offset;Gradient_Offset;2;0;Create;True;0;0;False;0;1;0;0;1.2;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;87;302.7008,76.74004;Float;False;1353.168;688.8281;;12;81;80;85;86;83;3;66;84;2;1;62;65;Edge;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;29;474.9975,-110.4142;Float;False;Property;_Gradient_Height;Gradient_Height;1;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;30;518.3856,-359.468;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;81;326.1506,500.1287;Float;False;Property;_Zone_Rim;Zone_Rim;4;0;Create;True;0;0;False;0;0.2;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;31;671.3677,-365.9912;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;80;638.7314,493.1287;Float;False;True;1;0;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;33;823.0023,-449.7661;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;3;329.7429,142.0841;Float;False;Property;_Zone_Fresnel;Zone_Fresnel;5;0;Create;True;0;0;False;0;1;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;86;791.8801,593.0781;Float;False;Property;_Zone_Color;Zone_Color;7;0;Create;True;0;0;False;0;1,0,0,1;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;83;833.1508,273.6884;Float;False;Constant;_Color1;Color 1;9;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;32;798.9572,-310.697;Float;False;Property;_Gradient_Smoothness;Gradient_Smoothness;3;0;Create;True;0;0;False;0;0.001;0.001;0.001;100;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;85;854.2103,450.3981;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;43;754.3047,-47.57648;Float;False;Property;_Gradient_Opacity;Gradient_Opacity;0;0;Create;True;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;34;1022.602,-471.4712;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;66;1066.636,626.5038;Float;False;Property;_Zone_Power;Zone_Power;6;0;Create;True;0;0;False;0;1;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;40;775.9142,-221.5765;Float;False;Constant;_White;White;1;0;Create;True;0;0;False;0;1,1,1,0;0,1,0.7517242,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;2;624.1556,133.3648;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;10;False;3;FLOAT;10;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;84;1184.5,313.6682;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;42;1117.404,-138.2066;Float;False;Constant;_Black;Black;3;0;Create;True;0;0;False;0;0,0,0,0;0,1,0.7517242,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;62;1371.352,363.162;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.FresnelNode;1;839.0558,124.42;Float;False;World;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;45;1139.314,-305.3268;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;37;1210.317,-476.2766;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;41;1488.045,-268.2264;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;65;1495.53,126.4142;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1766.78,-4.75515;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;SyntyStudios/DamageZone;False;False;False;False;False;False;True;False;False;False;False;False;False;False;True;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;0;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;0;0;0;False;-1;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;30;0;28;0
WireConnection;30;1;27;2
WireConnection;31;0;30;0
WireConnection;31;1;29;0
WireConnection;80;0;81;0
WireConnection;33;0;31;0
WireConnection;85;0;80;0
WireConnection;34;0;33;0
WireConnection;34;1;32;0
WireConnection;2;0;3;0
WireConnection;84;0;83;0
WireConnection;84;1;86;0
WireConnection;84;2;85;0
WireConnection;62;0;84;0
WireConnection;62;1;66;0
WireConnection;1;3;2;0
WireConnection;45;0;40;0
WireConnection;45;1;43;0
WireConnection;37;0;34;0
WireConnection;41;0;45;0
WireConnection;41;1;42;0
WireConnection;41;2;37;0
WireConnection;65;0;1;0
WireConnection;65;1;62;0
WireConnection;0;2;65;0
WireConnection;0;9;41;0
ASEEND*/
//CHKSM=7F3666C75BFE307995417A1654549166B8CEC159