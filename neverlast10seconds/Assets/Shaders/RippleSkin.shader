// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "RippleSkin"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 4.6
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			half filler;
		};

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 color75 = IsGammaSpace() ? float4(0.5,0.5,0.5,0) : float4(0.2140411,0.2140411,0.2140411,0);
			o.Albedo = color75.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18935
-1637.333;72;1535.333;792.3334;-694.994;816.8877;1;True;False
Node;AmplifyShaderEditor.Vector3Node;69;-735.6852,383.0453;Inherit;False;Property;_Vector2;Vector 2;11;0;Create;True;0;0;0;False;0;False;0,0,0;0,105.3,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldPosInputsNode;9;-719.3661,587.722;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.NormalVertexDataNode;2;43.74664,90.81535;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;3;46.88493,271.442;Inherit;False;Property;_VertexOffsetStrength;VertexOffsetStrength;0;0;Create;True;0;0;0;False;0;False;0;0.02;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;70;-362.1854,556.1454;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;289.8849,150.442;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SmoothstepOpNode;74;195.0483,658.079;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;25;False;2;FLOAT;45;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;35;-251.038,-775.7194;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;1,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;65;801.5072,-1318.559;Inherit;True;Property;_TextureSample5;Texture Sample 5;8;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RoundOpNode;73;254.8481,868.6776;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;32;-873.8108,-664.9594;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;100;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;28;513.7639,-510.5177;Inherit;False;Property;_Float0;Float 0;6;0;Create;True;0;0;0;False;0;False;1.1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;38;-630.8928,-509.8039;Inherit;False;Property;_HoleMorphStrength;Hole Morph Strength;8;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;24;95.01565,-972.8315;Inherit;True;Property;_Texture0;Texture 0;5;0;Create;True;0;0;0;False;0;False;None;4591b642618b9ca44be1260b40f33b1e;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;-367.1854,-574.2925;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;18;292.9815,-376.6725;Inherit;True;Normal From Height;-1;;17;1942fe2c5f1a1f94881a33d532e4afeb;0;2;20;FLOAT;0;False;110;FLOAT;15;False;2;FLOAT3;40;FLOAT3;0
Node;AmplifyShaderEditor.PosVertexDataNode;11;-761.0508,744.0504;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;21;2001.545,-1182.185;Inherit;True;SphereMask;-1;;15;988803ee12caf5f4690caee3c8c4a5bb;0;3;15;FLOAT3;0,0,0;False;14;FLOAT;0;False;12;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;22;-437.8641,-238.6156;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;17;406.9815,-102.6725;Inherit;True;NormalCreate;1;;16;e12f7ae19d416b942820e3932b56220f;0;4;1;SAMPLER2D;;False;2;FLOAT2;0,0;False;3;FLOAT;0.5;False;4;FLOAT;2;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;19;-483.6648,-106.6798;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;51;-67.50496,-482.1454;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;34;-459.7595,-832.3589;Inherit;False;Property;_HoleUV;Hole UV;7;0;Create;True;0;0;0;False;0;False;1,1;100,100;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.OneMinusNode;67;1706.044,-1081.727;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SinOpNode;6;253.4096,390.2543;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;14;-673.847,-200.0824;Inherit;False;Constant;_Color0;Color 0;3;0;Create;True;0;0;0;False;0;False;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexCoordVertexDataNode;33;-399.8447,-980.617;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;1104.336,-267.5237;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;1,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;36;-1015.389,-912.115;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;54;815.0968,-955.0549;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;100,100;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;604.2516,252.6449;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ParallaxMappingNode;52;1182.702,-850.4247;Inherit;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;1;False;2;FLOAT;0.5;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;61;1194.112,-514.9077;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;1,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;44;350.32,-1280.554;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;48;364.8304,-1117.023;Inherit;True;2;0;FLOAT;0.5;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;41;-124.7227,-1171.442;Inherit;True;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;63;448.5225,509.0911;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;8;101.2656,463.5593;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;64;555.3925,-1321.497;Inherit;True;Property;_Texture2;Texture 2;10;0;Create;True;0;0;0;False;0;False;None;0000000000000000f000000000000000;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TextureCoordinatesNode;66;559.2925,-1082.298;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;100,100;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-93.92133,603.8498;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;50;-341.0271,-476.1203;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;5;-194.9046,408.3386;Inherit;False;1;0;FLOAT;4;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;55;1389.805,-1101.131;Inherit;True;Property;_TextureSample4;Texture Sample 4;8;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;60;647.8474,-456.3889;Inherit;False;Constant;_Float1;Float 1;10;0;Create;True;0;0;0;False;0;False;-0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;23;-676.7183,-298.0031;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ScaleAndOffsetNode;26;291.1337,-528.432;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;58;886.643,-415.6856;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;49;146.8388,-764.6559;Inherit;True;3;0;FLOAT2;0,0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;75;1580.994,-713.8877;Inherit;False;Constant;_Color1;Color 1;11;0;Create;True;0;0;0;False;0;False;0.5,0.5,0.5,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;10;-380.8514,406.5186;Inherit;False;Property;_VertexSpeed;VertexSpeed;3;0;Create;True;0;0;0;False;0;False;0;4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;154.7674,-149.4028;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT2;1,1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.AbsOpNode;71;366.7953,372.3543;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-318.2521,742.6815;Inherit;False;Property;_SinWaveScale;Sin Wave Scale;4;0;Create;True;0;0;0;False;0;False;0;1.7;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;53;949.1339,-744.2408;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.OneMinusNode;68;1949.22,-828.258;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.Vector3Node;1;-328.9011,221.1411;Inherit;False;Constant;_Vector0;Vector 0;0;0;Create;True;0;0;0;False;0;False;0,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;25;487.3656,-721.908;Inherit;True;Property;_TextureSample3;Texture Sample 3;5;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;62;1388.964,-107.2532;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TexturePropertyNode;57;1155.186,-1224.179;Inherit;True;Property;_Texture1;Texture 1;9;0;Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.ClampOpNode;72;40.1951,881.2538;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;3;False;1;FLOAT;0
Node;AmplifyShaderEditor.VoronoiNode;15;-43.12753,-353.2401;Inherit;True;0;0;1;2;1;False;1;False;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;50;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.Vector2Node;42;-307.7741,-1126.386;Inherit;False;Constant;_Vector1;Vector 1;8;0;Create;True;0;0;0;False;0;False;0.5,0.5;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TexCoordVertexDataNode;40;-330.757,-1245.114;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;59;894.6765,-539.1442;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;39;1622.228,-888.6625;Inherit;False;Normal From Height;-1;;13;1942fe2c5f1a1f94881a33d532e4afeb;0;2;20;FLOAT;0;False;110;FLOAT;10;False;2;FLOAT3;40;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1917.462,-702.9444;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;RippleSkin;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;5.5;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;70;0;69;0
WireConnection;70;1;9;0
WireConnection;4;0;2;0
WireConnection;4;1;3;0
WireConnection;74;0;70;0
WireConnection;35;0;36;0
WireConnection;35;1;34;0
WireConnection;35;2;37;0
WireConnection;65;0;64;0
WireConnection;65;1;66;0
WireConnection;73;0;70;0
WireConnection;32;0;36;0
WireConnection;37;0;32;0
WireConnection;37;1;38;0
WireConnection;18;20;15;0
WireConnection;22;0;23;0
WireConnection;17;2;16;0
WireConnection;51;0;50;0
WireConnection;67;0;55;0
WireConnection;6;0;8;0
WireConnection;27;0;58;0
WireConnection;27;1;7;0
WireConnection;7;0;4;0
WireConnection;7;1;74;0
WireConnection;52;0;54;0
WireConnection;52;1;65;0
WireConnection;52;3;53;0
WireConnection;61;0;59;0
WireConnection;61;1;7;0
WireConnection;44;0;41;0
WireConnection;48;0;44;0
WireConnection;41;0;40;0
WireConnection;41;1;42;0
WireConnection;63;0;6;0
WireConnection;8;0;5;0
WireConnection;8;1;12;0
WireConnection;12;0;70;0
WireConnection;12;1;13;0
WireConnection;50;0;36;0
WireConnection;5;0;10;0
WireConnection;55;0;57;0
WireConnection;55;1;52;0
WireConnection;58;0;25;0
WireConnection;49;0;35;0
WireConnection;49;2;51;0
WireConnection;16;0;15;2
WireConnection;71;0;6;0
WireConnection;68;0;39;0
WireConnection;25;0;24;0
WireConnection;25;1;49;0
WireConnection;62;0;27;0
WireConnection;62;1;61;0
WireConnection;72;0;70;0
WireConnection;15;0;19;0
WireConnection;59;0;25;0
WireConnection;59;1;60;0
WireConnection;39;20;55;0
WireConnection;0;0;75;0
ASEEND*/
//CHKSM=55FF18BD2D01162F2CD4DB9428CFAB9B32334466