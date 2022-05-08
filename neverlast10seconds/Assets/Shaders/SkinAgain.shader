// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SkinAgain"
{
	Properties
	{
		_Rings("Rings", Float) = 5
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
			half filler;
		};

		uniform float _Rings;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float mulTime35 = _Time.y * 0.5;
			float temp_output_17_0 = ( ( ase_vertex3Pos.y + mulTime35 ) * _Rings );
			float temp_output_20_0 = frac( temp_output_17_0 );
			float temp_output_26_0 = abs( ( ( temp_output_20_0 * 2.0 ) - 1.0 ) );
			float smoothstepResult31 = smoothstep( 0.7 , 1.0 , temp_output_26_0);
			float3 ase_vertexNormal = v.normal.xyz;
			v.vertex.xyz += ( ( smoothstepResult31 * 2.0 ) * ase_vertexNormal );
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
40.8;142.4;1523.2;637.4;1965.749;318.358;1.9;True;False
Node;AmplifyShaderEditor.PosVertexDataNode;9;-1853.483,-244.3935;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;35;-1671.248,-50.45823;Inherit;False;1;0;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;34;-1395.748,-153.0582;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;21;-1290.303,-35.62396;Inherit;False;Property;_Rings;Rings;2;0;Create;True;0;0;0;False;0;False;5;2.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-1123.32,-146.8415;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.FractNode;20;-896.2021,-204.1241;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-720.4033,-222.5238;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;25;-576.903,-164.6239;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;26;-428.3031,-182.5239;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;31;-195.7005,-294.734;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0.7;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;10.25078,-134.058;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;32;-242.1016,406.0662;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;7;-1477.261,579.1519;Inherit;False;Property;_Float1;Float 0;1;0;Create;True;0;0;0;False;0;False;0.11;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;16;-705.426,384.837;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;3;False;4;FLOAT;25;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-1482.261,487.1518;Inherit;False;Property;_Float0;Float 0;0;0;Create;True;0;0;0;False;0;False;5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;19;-419.4673,204.9675;Inherit;False;Constant;_Float2;Float 2;2;0;Create;True;0;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;28;-233.8029,-98.73412;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-227.0673,39.56726;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TFHCRemapNode;29;-762.1006,-109.1337;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;2;-1284.09,394.523;Inherit;False;SphereMask;-1;;1;988803ee12caf5f4690caee3c8c4a5bb;0;3;15;FLOAT4;0,0,0,0;False;14;FLOAT;5;False;12;FLOAT;0.11;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;8;-1012.862,400.7519;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;12;-604.0823,94.60651;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector4Node;1;-1658.392,355.8312;Inherit;False;Global;_HandPosition;_HandPosition;0;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;11;-1066.882,205.9065;Inherit;False;1;0;FLOAT;15;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-462.5825,61.50648;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-208.5822,302.6065;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;10;-767.8823,56.90644;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;168.5973,51.66565;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NormalVertexDataNode;14;-663.7822,203.8065;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldPosInputsNode;5;-1808.979,81.0294;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;436.9,-129.4001;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;SkinAgain;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;34;0;9;2
WireConnection;34;1;35;0
WireConnection;17;0;34;0
WireConnection;17;1;21;0
WireConnection;20;0;17;0
WireConnection;24;0;20;0
WireConnection;25;0;24;0
WireConnection;26;0;25;0
WireConnection;31;0;26;0
WireConnection;36;0;31;0
WireConnection;16;0;8;0
WireConnection;28;0;26;0
WireConnection;18;0;13;0
WireConnection;18;1;19;0
WireConnection;29;0;20;0
WireConnection;2;15;1;0
WireConnection;2;14;6;0
WireConnection;2;12;7;0
WireConnection;8;0;2;0
WireConnection;12;0;10;0
WireConnection;11;0;16;0
WireConnection;13;0;12;0
WireConnection;13;1;14;0
WireConnection;15;0;13;0
WireConnection;15;1;16;0
WireConnection;10;0;17;0
WireConnection;10;1;11;0
WireConnection;33;0;36;0
WireConnection;33;1;32;0
WireConnection;0;11;33;0
ASEEND*/
//CHKSM=0D5588A1361C7782ACE42E2570C81928022082C6