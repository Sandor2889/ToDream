Shader "Water"
{
    Properties
    {
        _BumpScale("Detail Wave Amount", Range(0, 2)) = 0.2 //fine detail multiplier
        _DitherPattern("Dithering Pattern", 2D) = "bump" {}
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent"}
        pass 
        {
            CGPROGRAM

            //#pragma vertex WaterVertex
            //#pragma fragment WaterFragment

            ///////////////////////////////////////////////////////////////////////////////
            //                  				Structs		                             //
            ///////////////////////////////////////////////////////////////////////////////

            struct WaterVertexInput // vert struct
            {
                float4	vertex 					: POSITION;		// vertex positions
                float2	texcoord 				: TEXCOORD0;	// local UVs
            };

            struct WaterVertexOutput // fragment struct
            {
                float4	uv 						: TEXCOORD0;	// Geometric UVs stored in xy, and world(pre-waves) in zw
                float3	posWS					: TEXCOORD1;	// world position of the vertices
                half3 	normal 					: NORMAL;		// vert normals
                float3 	viewDir 				: TEXCOORD2;	// view direction
                float3	preWaveSP 				: TEXCOORD3;	// screen position of the verticies before wave distortion
                half2 	fogFactorNoise          : TEXCOORD4;	// x: fogFactor, y: noise
                float4	additionalData			: TEXCOORD5;	// x = distance to surface, y = distance to surface, z = normalized wave height, w = horizontal movement
                half4	shadowCoord				: TEXCOORD6;	// for ssshadows

                float4	clipPos					: SV_POSITION;
            };


            ///////////////////////////////////////////////////////////////////////////////
            //          	   	      Water shading functions                            //
            ///////////////////////////////////////////////////////////////////////////////

            // Surface textures
            
            float2 uv_AbsorptionScatteringRamp;
            sampler2D _AbsorptionScatteringRamp;

            //TEXTURE2D(_AbsorptionScatteringRamp); SAMPLER(sampler__AbsorptionScatteringRamp);
            //TEXTURE2D(_FoamMap); SAMPLER(sampler_FoamMap);
            //TEXTURE2D(_DitherPattern); SAMPLER(sampler_DitherPattern);

            half3 Scattering(half depth)
            {
                return tex2d(_AbsorptionScatteringRamp, uv_AbsorptionScatteringRamp).rgb);
                //return SAMPLE_TEXTURE2D(_AbsorptionScatteringRamp, sampler_AbsorptionScatteringRamp, half2(depth, 0.375h)).rgb;
            }

            //half3 Absorption(half depth)
            //{
            //    //return SAMPLE_TEXTURE2D(_AbsorptionScatteringRamp, sampler_AbsorptionScatteringRamp, half2(depth, 0.0h)).rgb;
            //}

            ENDCG
        }

    }
        FallBack "Diffuse"
}