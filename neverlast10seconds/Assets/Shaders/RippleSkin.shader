// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "RippleSkin"
{
	Properties
	{
		_EdgeLength ( "Edge length", Range( 2, 50 ) ) = 5.5
		_VertexOffsetStrength("VertexOffsetStrength", Float) = 0
		_VertexSpeed("VertexSpeed", Float) = 0
		_SinWaveScale("Sin Wave Scale", Float) = 0
		_Texture0("Texture 0", 2D) = "white" {}
		_HoleUV("Hole UV", Vector) = (1,1,0,0)
		_HoleMorphStrength("Hole Morph Strength", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "Tessellation.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
			float2 uv_texcoord;
		};

		uniform sampler2D _Texture0;
		uniform float2 _HoleUV;
		uniform float _HoleMorphStrength;
		uniform float _VertexOffsetStrength;
		uniform float _VertexSpeed;
		uniform float _SinWaveScale;
		uniform float _EdgeLength;


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		float3 PerturbNormal107_g6( float3 surf_pos, float3 surf_norm, float height, float scale )
		{
			// "Bump Mapping Unparametrized Surfaces on the GPU" by Morten S. Mikkelsen
			float3 vSigmaS = ddx( surf_pos );
			float3 vSigmaT = ddy( surf_pos );
			float3 vN = surf_norm;
			float3 vR1 = cross( vSigmaT , vN );
			float3 vR2 = cross( vN , vSigmaS );
			float fDet = dot( vSigmaS , vR1 );
			float dBs = ddx( height );
			float dBt = ddy( height );
			float3 vSurfGrad = scale * 0.05 * sign( fDet ) * ( dBs * vR1 + dBt * vR2 );
			return normalize ( abs( fDet ) * vN - vSurfGrad );
		}


		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			return UnityEdgeLengthBasedTess (v0.vertex, v1.vertex, v2.vertex, _EdgeLength);
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float simplePerlin2D32 = snoise( v.texcoord.xy*100.0 );
			simplePerlin2D32 = simplePerlin2D32*0.5 + 0.5;
			float simplePerlin2D50 = snoise( v.texcoord.xy*5.0 );
			simplePerlin2D50 = simplePerlin2D50*0.5 + 0.5;
			float4 tex2DNode25 = tex2Dlod( _Texture0, float4( ((v.texcoord.xy*_HoleUV + ( simplePerlin2D32 * _HoleMorphStrength ))*1.0 + ( simplePerlin2D50 * 2.0 )), 0, 0.0) );
			float3 ase_vertexNormal = v.normal.xyz;
			float mulTime5 = _Time.y * _VertexSpeed;
			float3 ase_vertex3Pos = v.vertex.xyz;
			float3 temp_output_7_0 = ( ( ase_vertexNormal * _VertexOffsetStrength ) * sin( ( mulTime5 + ( ase_vertex3Pos.z * _SinWaveScale ) ) ) );
			v.vertex.xyz += ( ( ( 1.0 - tex2DNode25 ) * float4( temp_output_7_0 , 0.0 ) ) + ( ( tex2DNode25 * 0.5 ) * float4( temp_output_7_0 , 0.0 ) ) ).rgb;
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 surf_pos107_g6 = ase_worldPos;
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 surf_norm107_g6 = ase_worldNormal;
			float simplePerlin2D32 = snoise( i.uv_texcoord*100.0 );
			simplePerlin2D32 = simplePerlin2D32*0.5 + 0.5;
			float simplePerlin2D50 = snoise( i.uv_texcoord*5.0 );
			simplePerlin2D50 = simplePerlin2D50*0.5 + 0.5;
			float4 tex2DNode25 = tex2D( _Texture0, ((i.uv_texcoord*_HoleUV + ( simplePerlin2D32 * _HoleMorphStrength ))*1.0 + ( simplePerlin2D50 * 2.0 )) );
			float height107_g6 = tex2DNode25.r;
			float scale107_g6 = 10.0;
			float3 localPerturbNormal107_g6 = PerturbNormal107_g6( surf_pos107_g6 , surf_norm107_g6 , height107_g6 , scale107_g6 );
			o.Normal = localPerturbNormal107_g6;
			o.Albedo = tex2DNode25.rgb;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows vertex:vertexDataFunc tessellate:tessFunction 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 4.6
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
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
Version=18935
190.4;842.4;1229.6;742.2;162.8959;1327.936;2.6961;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;36;-1015.389,-912.115;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NoiseGeneratorNode;32;-873.8108,-664.9594;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;100;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;38;-630.8928,-509.8039;Inherit;False;Property;_HoleMorphStrength;Hole Morph Strength;13;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;34;-459.7595,-832.3589;Inherit;False;Property;_HoleUV;Hole UV;12;0;Create;True;0;0;0;False;0;False;1,1;100,100;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;-367.1854,-574.2925;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;50;-341.0271,-476.1203;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;11;-575.4473,443.3338;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;13;-469.8486,634.2651;Inherit;False;Property;_SinWaveScale;Sin Wave Scale;9;0;Create;True;0;0;0;False;0;False;0;30;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-540.2479,322.8018;Inherit;False;Property;_VertexSpeed;VertexSpeed;8;0;Create;True;0;0;0;False;0;False;0;20;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;5;-354.301,324.6218;Inherit;False;1;0;FLOAT;4;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-253.3177,520.1331;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;51;-67.50496,-482.1454;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;35;-251.038,-775.7194;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;1,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NormalVertexDataNode;2;-73.95779,-65.8625;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;8;-58.13086,379.8427;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-112.5115,187.7252;Inherit;False;Property;_VertexOffsetStrength;VertexOffsetStrength;5;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;24;95.01565,-972.8315;Inherit;True;Property;_Texture0;Texture 0;10;0;Create;True;0;0;0;False;0;False;None;4591b642618b9ca44be1260b40f33b1e;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.ScaleAndOffsetNode;49;57.83887,-727.5722;Inherit;True;3;0;FLOAT2;0,0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;60;606.8875,-393.4399;Inherit;False;Constant;_Float1;Float 1;10;0;Create;True;0;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;25;463.2615,-775.6791;Inherit;True;Property;_TextureSample3;Texture Sample 3;5;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;130.4885,66.7252;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SinOpNode;6;133.631,291.8222;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;59;899.4233,-591.3625;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;58;853.4132,-363.4673;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;358.631,164.8222;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;61;1147.443,-10.6634;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;1,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;1104.336,-267.5237;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;1,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;28;513.7639,-510.5177;Inherit;False;Property;_Float0;Float 0;11;0;Create;True;0;0;0;False;0;False;1.1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;21;1603.679,-1152.656;Inherit;True;SphereMask;-1;;4;988803ee12caf5f4690caee3c8c4a5bb;0;3;15;FLOAT3;0,0,0;False;14;FLOAT;0;False;12;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;55;1476.698,-923.8542;Inherit;True;Property;_TextureSample4;Texture Sample 4;8;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;19;-483.6648,-106.6798;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;17;406.9815,-102.6725;Inherit;True;NormalCreate;6;;5;e12f7ae19d416b942820e3932b56220f;0;4;1;SAMPLER2D;;False;2;FLOAT2;0,0;False;3;FLOAT;0.5;False;4;FLOAT;2;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;53;929.0593,-823.5356;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TexturePropertyNode;57;1181.009,-1133.171;Inherit;True;Property;_Texture1;Texture 1;14;0;Create;True;0;0;0;False;0;False;None;4591b642618b9ca44be1260b40f33b1e;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.ClampOpNode;44;350.32,-1280.554;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;18;292.9815,-376.6725;Inherit;True;Normal From Height;-1;;2;1942fe2c5f1a1f94881a33d532e4afeb;0;2;20;FLOAT;0;False;110;FLOAT;15;False;2;FLOAT3;40;FLOAT3;0
Node;AmplifyShaderEditor.WorldPosInputsNode;9;-502.4624,771.9051;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ParallaxMappingNode;52;1125.618,-873.7835;Inherit;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0.05;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FunctionNode;39;1104.018,-591.0666;Inherit;False;Normal From Height;-1;;6;1942fe2c5f1a1f94881a33d532e4afeb;0;2;20;FLOAT;0;False;110;FLOAT;10;False;2;FLOAT3;40;FLOAT3;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;33;-399.8447,-980.617;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;62;1355.735,-168.9656;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;14;-673.847,-200.0824;Inherit;False;Constant;_Color0;Color 0;3;0;Create;True;0;0;0;False;0;False;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScaleAndOffsetNode;26;291.1337,-528.432;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;1;-488.2975,137.4243;Inherit;False;Constant;_Vector0;Vector 0;0;0;Create;True;0;0;0;False;0;False;0,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.StepOpNode;48;364.8304,-1117.023;Inherit;True;2;0;FLOAT;0.5;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;54;863.597,-998.8335;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;100,100;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DistanceOpNode;41;-124.7227,-1171.442;Inherit;True;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;42;-307.7741,-1126.386;Inherit;False;Constant;_Vector1;Vector 1;8;0;Create;True;0;0;0;False;0;False;0.5,0.5;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.WorldPosInputsNode;23;-676.7183,-298.0031;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DistanceOpNode;22;-437.8641,-238.6156;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VoronoiNode;15;-43.12753,-353.2401;Inherit;True;0;0;1;2;1;False;1;False;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;50;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;154.7674,-149.4028;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT2;1,1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;40;-330.757,-1245.114;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1712.141,-568.5059;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;RippleSkin;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;5.5;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;0;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;32;0;36;0
WireConnection;37;0;32;0
WireConnection;37;1;38;0
WireConnection;50;0;36;0
WireConnection;5;0;10;0
WireConnection;12;0;11;3
WireConnection;12;1;13;0
WireConnection;51;0;50;0
WireConnection;35;0;36;0
WireConnection;35;1;34;0
WireConnection;35;2;37;0
WireConnection;8;0;5;0
WireConnection;8;1;12;0
WireConnection;49;0;35;0
WireConnection;49;2;51;0
WireConnection;25;0;24;0
WireConnection;25;1;49;0
WireConnection;4;0;2;0
WireConnection;4;1;3;0
WireConnection;6;0;8;0
WireConnection;59;0;25;0
WireConnection;59;1;60;0
WireConnection;58;0;25;0
WireConnection;7;0;4;0
WireConnection;7;1;6;0
WireConnection;61;0;59;0
WireConnection;61;1;7;0
WireConnection;27;0;58;0
WireConnection;27;1;7;0
WireConnection;55;0;57;0
WireConnection;55;1;52;0
WireConnection;17;2;16;0
WireConnection;44;0;41;0
WireConnection;18;20;15;0
WireConnection;52;0;54;0
WireConnection;52;1;25;1
WireConnection;52;3;53;0
WireConnection;39;20;25;0
WireConnection;62;0;27;0
WireConnection;62;1;61;0
WireConnection;48;0;44;0
WireConnection;41;0;40;0
WireConnection;41;1;42;0
WireConnection;22;0;23;0
WireConnection;15;0;19;0
WireConnection;16;0;15;2
WireConnection;0;0;25;0
WireConnection;0;1;39;0
WireConnection;0;11;62;0
ASEEND*/
//CHKSM=10933DD794691633EA15882EA1990B2C27F0D4BC