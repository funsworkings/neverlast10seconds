// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "RippleSkin"
{
	Properties
	{
		_Rings("Rings", Float) = 5
		_Texture0("Texture 0", 2D) = "white" {}
		_HoleUV("Hole UV", Vector) = (1,1,0,0)
		_HoleMorphStrength("Hole Morph Strength", Float) = 0
		_Color1("Color 1", Color) = (0.9056604,0.5416874,0.7765515,0)
		_Color2("Color 2", Color) = (0.5660378,0.253115,0.253115,0)
		_OffsetStrength("OffsetStrength", Float) = 0.01
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
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

		uniform float GlobalTime;
		uniform float _Rings;
		uniform float _OffsetStrength;
		uniform sampler2D _Texture0;
		uniform float2 _HoleUV;
		uniform float _HoleMorphStrength;
		uniform float GlobalTime1;
		uniform float4 _Color1;
		uniform float4 _Color2;


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


		float3 PerturbNormal107_g17( float3 surf_pos, float3 surf_norm, float height, float scale )
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


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float smoothstepResult80 = smoothstep( 0.7 , 1.0 , abs( ( ( frac( ( ( ase_vertex3Pos.y + ( GlobalTime * 0.5 ) ) * _Rings ) ) * 2.0 ) - 1.0 ) ));
			float3 ase_vertexNormal = v.normal.xyz;
			v.vertex.xyz += ( ( ( smoothstepResult80 * 2.0 ) * ase_vertexNormal ) * _OffsetStrength );
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 surf_pos107_g17 = ase_worldPos;
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 surf_norm107_g17 = ase_worldNormal;
			float simplePerlin2D32 = snoise( i.uv_texcoord*50.0 );
			simplePerlin2D32 = simplePerlin2D32*0.5 + 0.5;
			float simplePerlin2D50 = snoise( i.uv_texcoord*5.0 );
			simplePerlin2D50 = simplePerlin2D50*0.5 + 0.5;
			float2 temp_cast_0 = (( GlobalTime1 * 0.5 )).xx;
			float2 uv_TexCoord100 = i.uv_texcoord + temp_cast_0;
			float simplePerlin2D99 = snoise( uv_TexCoord100 );
			simplePerlin2D99 = simplePerlin2D99*0.5 + 0.5;
			float4 tex2DNode25 = tex2D( _Texture0, ( ((i.uv_texcoord*_HoleUV + ( simplePerlin2D32 * _HoleMorphStrength ))*1.0 + ( simplePerlin2D50 * 2.0 )) + simplePerlin2D99 ) );
			float height107_g17 = tex2DNode25.r;
			float scale107_g17 = 15.0;
			float3 localPerturbNormal107_g17 = PerturbNormal107_g17( surf_pos107_g17 , surf_norm107_g17 , height107_g17 , scale107_g17 );
			float3 temp_output_18_0 = localPerturbNormal107_g17;
			o.Normal = temp_output_18_0;
			float4 lerpResult91 = lerp( _Color1 , _Color2 , tex2DNode25);
			o.Albedo = lerpResult91.rgb;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float fresnelNdotV94 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode94 = ( 0.0 + 3.0 * pow( 1.0 - fresnelNdotV94, 3.0 ) );
			o.Emission = ( temp_output_18_0 * fresnelNode94 );
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows vertex:vertexDataFunc 

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
				vertexDataFunc( v, customInputData );
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
-4.8;264.8;1523.2;659;1996.512;243.569;1.709602;True;False
Node;AmplifyShaderEditor.RangedFloatNode;104;-928.7009,110.7847;Inherit;False;Global;GlobalTime;GlobalTime;8;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;76;-1033.118,-49.80371;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;107;-758.9412,193.3495;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;78;-627.8912,37.33164;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;79;-522.4462,154.7656;Inherit;False;Property;_Rings;Rings;0;0;Create;True;0;0;0;False;0;False;5;2.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;36;-1015.389,-912.115;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;88;-293.1774,17.12366;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;105;-344.4576,-227.3507;Inherit;False;Global;GlobalTime1;GlobalTime;8;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;38;-630.8928,-509.8039;Inherit;False;Property;_HoleMorphStrength;Hole Morph Strength;3;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;32;-922.5718,-650.4516;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;50;False;1;FLOAT;0
Node;AmplifyShaderEditor.FractNode;84;-54.73386,-55.25838;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;34;-485.3595,-787.9315;Inherit;False;Property;_HoleUV;Hole UV;2;0;Create;True;0;0;0;False;0;False;1,1;50,50;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;-367.1854,-574.2925;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;85;121.0647,-73.65829;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;106;-140.1412,-184.9504;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;50;-341.0271,-476.1203;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;35;-251.038,-775.7194;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;1,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;86;264.5651,-15.75839;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;51;-67.50496,-482.1454;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;100;122.8441,-450.27;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.AbsOpNode;87;413.1652,-33.65829;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;49;146.8388,-764.6559;Inherit;True;3;0;FLOAT2;0,0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;99;420.5109,-471.8296;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;80;572.1555,-104.3443;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0.7;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;102;500.2083,-692.6841;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;24;95.01565,-972.8315;Inherit;True;Property;_Texture0;Texture 0;1;0;Create;True;0;0;0;False;0;False;None;4591b642618b9ca44be1260b40f33b1e;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.NormalVertexDataNode;83;716.3896,254.8227;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;81;778.1066,56.33163;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;25;732.7651,-737.6687;Inherit;True;Property;_TextureSample3;Texture Sample 3;5;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;92;920.925,-1097.833;Inherit;False;Property;_Color1;Color 1;4;0;Create;True;0;0;0;False;0;False;0.9056604,0.5416874,0.7765515,0;0.9056604,0.5416874,0.7765515,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;93;920.9247,-929.1586;Inherit;False;Property;_Color2;Color 2;5;0;Create;True;0;0;0;False;0;False;0.5660378,0.253115,0.253115,0;0.5660378,0.253115,0.253115,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;90;941.9517,410.5368;Inherit;False;Property;_OffsetStrength;OffsetStrength;6;0;Create;True;0;0;0;False;0;False;0.01;0.01;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;82;936.4538,242.0556;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.FunctionNode;18;1188.244,-609.4536;Inherit;True;Normal From Height;-1;;17;1942fe2c5f1a1f94881a33d532e4afeb;0;2;20;FLOAT;0;False;110;FLOAT;15;False;2;FLOAT3;40;FLOAT3;0
Node;AmplifyShaderEditor.FresnelNode;94;1194.314,-304.4423;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;3;False;3;FLOAT;3;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;91;1236.078,-864.0568;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;95;1499.337,-419.6422;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;89;1116.255,274.5827;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1917.462,-702.9444;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;RippleSkin;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;5.5;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;107;0;104;0
WireConnection;78;0;76;2
WireConnection;78;1;107;0
WireConnection;88;0;78;0
WireConnection;88;1;79;0
WireConnection;32;0;36;0
WireConnection;84;0;88;0
WireConnection;37;0;32;0
WireConnection;37;1;38;0
WireConnection;85;0;84;0
WireConnection;106;0;105;0
WireConnection;50;0;36;0
WireConnection;35;0;36;0
WireConnection;35;1;34;0
WireConnection;35;2;37;0
WireConnection;86;0;85;0
WireConnection;51;0;50;0
WireConnection;100;1;106;0
WireConnection;87;0;86;0
WireConnection;49;0;35;0
WireConnection;49;2;51;0
WireConnection;99;0;100;0
WireConnection;80;0;87;0
WireConnection;102;0;49;0
WireConnection;102;1;99;0
WireConnection;81;0;80;0
WireConnection;25;0;24;0
WireConnection;25;1;102;0
WireConnection;82;0;81;0
WireConnection;82;1;83;0
WireConnection;18;20;25;0
WireConnection;91;0;92;0
WireConnection;91;1;93;0
WireConnection;91;2;25;0
WireConnection;95;0;18;0
WireConnection;95;1;94;0
WireConnection;89;0;82;0
WireConnection;89;1;90;0
WireConnection;0;0;91;0
WireConnection;0;1;18;0
WireConnection;0;2;95;0
WireConnection;0;11;89;0
ASEEND*/
//CHKSM=0D12EE2AC8FDFD5EE9AA54FA9801B7A8B8A32AEA