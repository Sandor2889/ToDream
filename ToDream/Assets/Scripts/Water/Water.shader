Shader "Water"
{
    Properties
    {
        _BumpScale("Detail Wave Amount", Range(0, 2)) = 0.2 //fine detail multiplier
        _DitherPattern("Dithering Pattern", 2D) = "bump" {}
        [Toggle(_STATIC_SHADER)] _Static("Static", Float) = 0
        [KeywordEnum(Off, SSS, Refraction, Reflection, Normal, Fresnel, WaterEffects, Foam, WaterDepth)] _Debug("Debug mode", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent-100"}
        ZWrite On
        
        Pass
        {
            Name "WaterShading"
            Tags{"LightMode" = "ForwardBase"}

            HLSLPROGRAM

            ////////////////////INCLUDES//////////////////////
            #include "WaterCommon.hlsl"

            //non-tess
            #pragma vertex WaterVertex
            #pragma fragment WaterFragment

            ENDHLSL
        }
        
    }
    FallBack "Diffuse"
}
