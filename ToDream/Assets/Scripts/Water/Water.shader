Shader "Water"
{
	Properties
	{
		_Color("Color", color) = (1, 1, 1, 1)
	}
	SubShader
	{
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent-100" }
		ZWrite On

		Pass
		{
			Name "WaterShading"
			Tags{"LightMode" = "UniversalForward"}

			HLSLPROGRAM
		////////////////////INCLUDES//////////////////////
		//#include "WaterCommon.hlsl"

		//non-tess
		//#pragma vertex WaterVertex
		//#pragma fragment WaterFragment

		ENDHLSL
		}
	}
	FallBack "Diffuse"
}
