// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Scope Pro/Scope Effect"
{
	Properties
	{
		_Reticle("Reticle", 2D) = "white" {}
		[HDR]_ReticleColor("Reticle Color", Color) = (1,1,1,1)
		_ReticleSize("Reticle Size", Range( 0.1 , 2)) = 0.8
		_ScopeDepth("Scope Depth", Range( 0 , 5)) = 0.9411765
		_ScopeApperture("Scope Apperture", Range( -1 , 0.5)) = 0
		_ScopeFade("Scope Fade", Range( 0.001 , 1.2)) = 0.5
		_RenderSize("Render Size", Range( 0 , 2)) = 1
		[Toggle]_AppertureBasedDistance("Apperture Based Distance", Float) = 0
		[Toggle]_ReticleSizeBDistance("Reticle Size B Distance", Float) = 0
		[SingleLineTexture]_RenderTexture("RenderTexture", 2D) = "white" {}

	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Opaque" }
	LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend Off
		AlphaToMask Off
		Cull Back
		ColorMask RGBA
		ZWrite On
		ZTest LEqual
		Offset 0 , 0
		
		
		
		Pass
		{
			Name "Unlit"
			Tags { "LightMode"="ForwardBase" }
			CGPROGRAM

			

			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			//only defining to not throw compilation error over Unity 5.5
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			#define ASE_NEEDS_FRAG_WORLD_POSITION


			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_tangent : TANGENT;
				float3 ase_normal : NORMAL;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 worldPos : TEXCOORD0;
				#endif
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_texcoord4 : TEXCOORD4;
				float4 ase_texcoord5 : TEXCOORD5;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform float _ScopeApperture;
			uniform float _ScopeFade;
			uniform float _AppertureBasedDistance;
			uniform half _ScopeDepth;
			uniform sampler2D _RenderTexture;
			uniform float _RenderSize;
			uniform sampler2D _Reticle;
			uniform float _ReticleSizeBDistance;
			uniform half _ReticleSize;
			uniform float4 _ReticleColor;

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				float3 ase_worldTangent = UnityObjectToWorldDir(v.ase_tangent);
				o.ase_texcoord2.xyz = ase_worldTangent;
				float3 ase_worldNormal = UnityObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord3.xyz = ase_worldNormal;
				float ase_vertexTangentSign = v.ase_tangent.w * unity_WorldTransformParams.w;
				float3 ase_worldBitangent = cross( ase_worldNormal, ase_worldTangent ) * ase_vertexTangentSign;
				o.ase_texcoord4.xyz = ase_worldBitangent;
				
				o.ase_texcoord1.xy = v.ase_texcoord.xy;
				o.ase_texcoord5 = v.vertex;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.zw = 0;
				o.ase_texcoord2.w = 0;
				o.ase_texcoord3.w = 0;
				o.ase_texcoord4.w = 0;
				float3 vertexValue = float3(0, 0, 0);
				#if ASE_ABSOLUTE_VERTEX_POS
				vertexValue = v.vertex.xyz;
				#endif
				vertexValue = vertexValue;
				#if ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);

				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				#endif
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed4 finalColor;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 WorldPosition = i.worldPos;
				#endif
				float3 ase_worldTangent = i.ase_texcoord2.xyz;
				float3 ase_worldNormal = i.ase_texcoord3.xyz;
				float3 ase_worldBitangent = i.ase_texcoord4.xyz;
				float3 tanToWorld0 = float3( ase_worldTangent.x, ase_worldBitangent.x, ase_worldNormal.x );
				float3 tanToWorld1 = float3( ase_worldTangent.y, ase_worldBitangent.y, ase_worldNormal.y );
				float3 tanToWorld2 = float3( ase_worldTangent.z, ase_worldBitangent.z, ase_worldNormal.z );
				float3 ase_worldViewDir = UnityWorldSpaceViewDir(WorldPosition);
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 ase_tanViewDir =  tanToWorld0 * ase_worldViewDir.x + tanToWorld1 * ase_worldViewDir.y  + tanToWorld2 * ase_worldViewDir.z;
				ase_tanViewDir = normalize(ase_tanViewDir);
				float3 normalizeResult8 = normalize( ase_tanViewDir );
				float2 texCoord72 = i.ase_texcoord1.xy * float2( 1,1 ) + -normalizeResult8.xy;
				float2 temp_cast_1 = (0.5).xx;
				float3 objectSpaceViewDir115 = ObjSpaceViewDir( float4( i.ase_texcoord5.xyz , 0.0 ) );
				float clampResult121 = clamp( ( length( objectSpaceViewDir115 ) / 20.0 ) , 0.0 , 1.0 );
				float lerpResult118 = lerp( 0.1 , 5.0 , clampResult121);
				float smoothstepResult42 = smoothstep( _ScopeApperture , _ScopeFade , ( 1.0 - length( ( ( texCoord72 - temp_cast_1 ) * (( _AppertureBasedDistance )?( ( _ScopeDepth * lerpResult118 ) ):( _ScopeDepth )) ) ) ));
				float2 temp_cast_3 = (0.5).xx;
				float2 temp_cast_4 = (0.5).xx;
				float4 tex2DNode76 = tex2D( _Reticle, ( ( ( texCoord72 - temp_cast_4 ) * (( _ReticleSizeBDistance )?( ( _ReticleSize * lerpResult118 ) ):( _ReticleSize )) ) + 0.5 ) );
				float4 lerpResult82 = lerp( ( smoothstepResult42 * tex2D( _RenderTexture, ( ( ( texCoord72 - temp_cast_3 ) * _RenderSize ) + 0.5 ) ) ) , ( tex2DNode76 * _ReticleColor ) , tex2DNode76.a);
				
				
				finalColor = lerpResult82;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18935
2079;117;1477;768;5102.493;956.7504;4.450804;True;False
Node;AmplifyShaderEditor.CommentaryNode;123;-3327.863,538.5874;Inherit;False;1061.242;254.828;Apperture based distance;6;117;115;119;121;118;116;;1,1,1,1;0;0
Node;AmplifyShaderEditor.PosVertexDataNode;117;-3277.863,610.4156;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ObjSpaceViewDirHlpNode;115;-3102.863,607.4156;Inherit;False;1;0;FLOAT4;0,0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.CommentaryNode;136;-2118.674,367.6206;Inherit;False;1238.288;543.5219;Camera Render;10;7;8;9;14;12;135;134;133;132;72;;1,1,1,1;0;0
Node;AmplifyShaderEditor.LengthOpNode;116;-2872.263,622.0157;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;7;-2044.789,486.2586;Inherit;False;Tangent;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleDivideOpNode;119;-2736.833,600.3448;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;20;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;121;-2603.371,588.5876;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;8;-1739.026,497.4744;Inherit;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;125;-2355.15,-462.906;Inherit;False;1156.183;767.0647;Scope Effect;11;66;68;65;42;41;63;59;60;126;122;13;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-2293.831,48.80448;Half;False;Property;_ScopeDepth;Scope Depth;3;0;Create;True;0;0;0;False;0;False;0.9411765;1.41;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;124;-2319.212,1072.537;Inherit;False;1380.423;472.9729;Reticle;11;84;83;86;88;75;90;94;76;99;128;129;;1,1,1,1;0;0
Node;AmplifyShaderEditor.NegateNode;9;-1569.095,490.3684;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;118;-2448.619,592.9803;Inherit;False;3;0;FLOAT;0.1;False;1;FLOAT;5;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;72;-2056.157,672.2359;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;60;-2212.01,-79.31667;Inherit;False;Constant;_Float0;Float 0;3;0;Create;True;0;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;83;-2306.213,1462.51;Half;False;Property;_ReticleSize;Reticle Size;2;0;Create;True;0;0;0;False;0;False;0.8;0.541;0.1;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;122;-2243.334,184.0698;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;126;-2036.542,153.3107;Inherit;False;Property;_AppertureBasedDistance;Apperture Based Distance;7;0;Create;True;0;0;0;False;0;False;0;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;84;-2261.469,1356.623;Half;False;Constant;_MidPoint;MidPoint;3;0;Create;True;0;0;0;False;0;False;0.5;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;59;-2028.001,-65.42656;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;128;-2031.374,1376.992;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;129;-1910.838,1440.944;Inherit;False;Property;_ReticleSizeBDistance;Reticle Size B Distance;8;0;Create;True;0;0;0;False;0;False;0;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;133;-1770.01,642.7938;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;132;-1807.58,797.2457;Inherit;False;Property;_RenderSize;Render Size;6;0;Create;True;0;0;0;False;0;False;1;0.5;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;63;-1842.529,14.89279;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;86;-2041.657,1278.214;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LengthOpNode;41;-1674.782,17.96948;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;88;-1895.53,1271.164;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;134;-1589.28,670.6305;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;66;-1786.726,-412.9059;Inherit;False;Property;_ScopeApperture;Scope Apperture;4;0;Create;True;0;0;0;False;0;False;0;-0.354;-1;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;14;-1450.393,411.9823;Inherit;True;Property;_RenderTexture;RenderTexture;9;1;[SingleLineTexture];Create;True;0;0;0;False;0;False;84a125eed79b10d4080cef5f7473edb9;84a125eed79b10d4080cef5f7473edb9;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.OneMinusNode;65;-1709.885,-239.1549;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;75;-1744.355,1122.537;Inherit;True;Property;_Reticle;Reticle;0;0;Create;True;0;0;0;False;0;False;81808dfc347e4ca4db61579ccf80fbf9;81808dfc347e4ca4db61579ccf80fbf9;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SimpleAddOpNode;135;-1414.434,663.6187;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;90;-1738.193,1351.064;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;68;-1782.826,-338.8068;Float;False;Property;_ScopeFade;Scope Fade;5;0;Create;True;0;0;0;False;0;False;0.5;0.593;0.001;1.2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;12;-1217.576,596.5668;Inherit;True;Property;_ScopeRender;ScopeRender;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;42;-1452.971,-223.976;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;76;-1512.507,1128.579;Inherit;True;Property;_TextureSample0;Texture Sample 0;5;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;94;-1419.133,1318.958;Inherit;False;Property;_ReticleColor;Reticle Color;1;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,1;1.419608,2,0.007843138,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;64;-1086.092,39.75613;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;99;-1100.797,1240.249;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;82;-664.8409,513.0424;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;137;-319.7729,421.5189;Float;False;True;-1;2;ASEMaterialInspector;100;1;Scope Pro/Scope Effect;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;False;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque=RenderType;True;2;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;0;1;True;False;;False;0
WireConnection;115;0;117;0
WireConnection;116;0;115;0
WireConnection;119;0;116;0
WireConnection;121;0;119;0
WireConnection;8;0;7;0
WireConnection;9;0;8;0
WireConnection;118;2;121;0
WireConnection;72;1;9;0
WireConnection;122;0;13;0
WireConnection;122;1;118;0
WireConnection;126;0;13;0
WireConnection;126;1;122;0
WireConnection;59;0;72;0
WireConnection;59;1;60;0
WireConnection;128;0;83;0
WireConnection;128;1;118;0
WireConnection;129;0;83;0
WireConnection;129;1;128;0
WireConnection;133;0;72;0
WireConnection;133;1;84;0
WireConnection;63;0;59;0
WireConnection;63;1;126;0
WireConnection;86;0;72;0
WireConnection;86;1;84;0
WireConnection;41;0;63;0
WireConnection;88;0;86;0
WireConnection;88;1;129;0
WireConnection;134;0;133;0
WireConnection;134;1;132;0
WireConnection;65;0;41;0
WireConnection;135;0;134;0
WireConnection;135;1;84;0
WireConnection;90;0;88;0
WireConnection;90;1;84;0
WireConnection;12;0;14;0
WireConnection;12;1;135;0
WireConnection;42;0;65;0
WireConnection;42;1;66;0
WireConnection;42;2;68;0
WireConnection;76;0;75;0
WireConnection;76;1;90;0
WireConnection;64;0;42;0
WireConnection;64;1;12;0
WireConnection;99;0;76;0
WireConnection;99;1;94;0
WireConnection;82;0;64;0
WireConnection;82;1;99;0
WireConnection;82;2;76;4
WireConnection;137;0;82;0
ASEEND*/
//CHKSM=009E78350C04AC944447F56BF255D2F033765A34