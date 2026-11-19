
Shader "Paro222/UnderwaterEffects"
{
    Properties
    {
        _BlitTexture ("Camera Texture", 2D) = "white" {}
        _NormalMap ("Normal Map", 2D) = "bump" {}
        _color ("Water Color", Color) = (0, 0.4, 0.7, 1)
        _FogDensity ("Fog Density", Float) = 1
        _alpha ("Alpha", Range(0, 1)) = 0
        _refraction ("Refraction", Float) = 0.1
        _normalUV ("Normal UV", Vector) = (1, 1, 0.2, 0.1)
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
            "RenderPipeline" = "UniversalPipeline"
        }

        Pass
        {
            Name "UnderwaterEffect"

            ZTest Always
            ZWrite Off
            Cull Off

            HLSLPROGRAM

            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Packing.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            TEXTURE2D(_NormalMap);
            SAMPLER(sampler_NormalMap);

            float4 _normalUV;
            half4 _color;
            float _FogDensity;
            float _alpha;
            float _refraction;

            half4 Frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                float2 uv = input.texcoord;

                // Animated normal map
                half4 normalSample = SAMPLE_TEXTURE2D(
                    _NormalMap,
                    sampler_NormalMap,
                    uv * _normalUV.xy + _normalUV.zw * _Time.y
                );

                half3 normal = UnpackNormal(normalSample);

                // Refraction
                float2 refractedUV =
                    uv + normal.xy * _refraction * 0.01;

                // Camera depth
                float depthTexture = SampleSceneDepth(refractedUV);

                float depth = Linear01Depth(
                    depthTexture,
                    _ZBufferParams
                );

                depth *= depth;

                // Underwater fog
                float fogAmount =
                    1.0 - exp(-_FogDensity * depth);

                // Camera color
                half4 col = SAMPLE_TEXTURE2D_X(
                    _BlitTexture,
                    sampler_LinearClamp,
                    refractedUV
                );

                // Final underwater color
                return lerp(
                    col,
                    _color,
                    saturate(fogAmount * 1000 + _alpha)
                );
            }

            ENDHLSL
        }
    }
}