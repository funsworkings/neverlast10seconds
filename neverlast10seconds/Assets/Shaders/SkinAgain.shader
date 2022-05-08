// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SkinAgain"
{
	Properties
	{
		_Float0("Float 0", Float) = 5
		_Float1("Float 0", Float) = 0.11
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float3 worldPos;
		};

		uniform float4 _HandPosition;
		uniform float _Float0;
		uniform float _Float1;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float4 temp_output_5_0_g1 = ( ( float4( ase_worldPos , 0.0 ) - _HandPosition ) / _Float0 );
			float dotResult8_g1 = dot( temp_output_5_0_g1 , temp_output_5_0_g1 );
			float temp_output_16_0 = (3.0 + (( 1.0 - pow( saturate( dotResult8_g1 ) , _Float1 ) ) - 0.0) * (25.0 - 3.0) / (1.0 - 0.0));
			float mulTime11 = _Time.y * temp_output_16_0;
			float3 ase_vertexNormal = v.normal.xyz;
			float3 temp_output_13_0 = ( sin( ( ( ase_vertex3Pos.y * 5.0 ) + mulTime11 ) ) * ase_vertexNormal );
			v.vertex.xyz += ( temp_output_13_0 * 0.5 );
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18935
-1650;207.3333;1535.333;792.3334;1276.569;254.8398;1;True;False
Node;AmplifyShaderEditor.Vector4Node;1;-1658.392,355.8312;Inherit;False;Global;_HandPosition;_HandPosition;0;0;Create;True;0;0;0;False;0;False;0,0,0,0;-0.1151122,0.2728056,0.241446,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;6;-1482.261,487.1518;Inherit;False;Property;_Float0;Float 0;0;0;Create;True;0;0;0;False;0;False;5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-1477.261,579.1519;Inherit;False;Property;_Float1;Float 0;1;0;Create;True;0;0;0;False;0;False;0.11;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;2;-1284.09,394.523;Inherit;False;SphereMask;-1;;1;988803ee12caf5f4690caee3c8c4a5bb;0;3;15;FLOAT4;0,0,0,0;False;14;FLOAT;5;False;12;FLOAT;0.11;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;8;-1012.862,400.7519;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;16;-705.426,384.837;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;3;False;4;FLOAT;25;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;9;-1190.382,-130.3935;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;11;-1066.882,205.9065;Inherit;False;1;0;FLOAT;15;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-992.1182,-33.84161;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;10;-763.9823,21.80646;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;14;-653.4823,-149.7936;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SinOpNode;12;-604.0823,94.60651;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;19;-429.0673,174.5673;Inherit;False;Constant;_Float2;Float 2;2;0;Create;True;0;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-462.5825,61.50648;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-227.0673,39.56726;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldPosInputsNode;5;-1294.979,58.0294;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-208.5822,302.6065;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;40,0;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;SkinAgain;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;2;15;1;0
WireConnection;2;14;6;0
WireConnection;2;12;7;0
WireConnection;8;0;2;0
WireConnection;16;0;8;0
WireConnection;11;0;16;0
WireConnection;17;0;9;2
WireConnection;10;0;17;0
WireConnection;10;1;11;0
WireConnection;12;0;10;0
WireConnection;13;0;12;0
WireConnection;13;1;14;0
WireConnection;18;0;13;0
WireConnection;18;1;19;0
WireConnection;15;0;13;0
WireConnection;15;1;16;0
WireConnection;0;11;18;0
ASEEND*/
//CHKSM=0A7588A00A345BFE91DE6596F592206A8A8877FC