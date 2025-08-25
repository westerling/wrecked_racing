Shader "Custom/MoebiusPostEffect"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _EdgeColor ("Edge Color", Color) = (0,0,0,1)
        _Thickness ("Edge Thickness", Float) = 1.0
        _PosterizeSteps ("Posterization Steps", Range(2,16)) = 4
        _HatchTex ("Hatching Texture", 2D) = "white" {}
        _HatchStrength ("Hatch Strength", Range(0,1)) = 0.5
        _PaperTex ("Paper Texture", 2D) = "white" {}
        _PaperStrength ("Paper Strength", Range(0,1)) = 0.3
    }

   SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            ZTest Always Cull Off ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _CameraDepthNormalsTexture;
            sampler2D _HatchTex;
            sampler2D _PaperTex;

            float4 _MainTex_TexelSize;
            float4 _EdgeColor;

            float _Thickness;
            int _PosterizeSteps;
            float _HatchStrength;
            float _PaperStrength;

            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata_img v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                return o;
            }

            // Sample depth+normals edge difference
            float sampleEdge(sampler2D tex, float2 uv, float2 offset) {
                float3 s1 = tex2D(tex, uv).rgb;
                float3 s2 = tex2D(tex, uv + offset).rgb;
                return distance(s1, s2);
            }

            // Posterize function
            float3 posterize(float3 col, int steps) {
                return floor(col * steps) / steps;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float4 dn = tex2D(_CameraDepthNormalsTexture, i.uv);
                // Depth is encoded in dn.a, decode to linear depth
                float depth = dn.a;
                if (depth >= 1.0) {
                    // Skip outlines on skybox
                 return tex2D(_MainTex, i.uv);
            }

                float2 texel = _MainTex_TexelSize.xy * _Thickness;

                // Edge detection (using depth+normals)
                float edge = 0.0;
                edge += sampleEdge(_CameraDepthNormalsTexture, i.uv, float2(texel.x, 0));
                edge += sampleEdge(_CameraDepthNormalsTexture, i.uv, float2(0, texel.y));
                edge += sampleEdge(_CameraDepthNormalsTexture, i.uv, float2(-texel.x, 0));
                edge += sampleEdge(_CameraDepthNormalsTexture, i.uv, float2(0, -texel.y));

                float4 baseColor = tex2D(_MainTex, i.uv);
                float edgeStrength = saturate(edge * 10.0);

                // Posterization
                float3 posterized = posterize(baseColor.rgb, _PosterizeSteps);

                // Hatching based on brightness (darker = more hatching)
                float brightness = dot(posterized, float3(0.299, 0.587, 0.114));
                float2 hatchUV = i.uv * 8.0; // tile the hatching texture
                float hatchSample = tex2D(_HatchTex, hatchUV).r;
                float hatchMask = saturate((1.0 - brightness) * _HatchStrength);
                posterized = lerp(posterized, posterized - (1.0 - hatchSample) * _HatchStrength, hatchMask);

                // Paper overlay (subtle texture)
                float paperSample = tex2D(_PaperTex, i.uv * 2.0).r;
                posterized = lerp(posterized, posterized + (paperSample - 0.5) * 0.2, _PaperStrength);

                // Blend edges on top
                float3 finalColor = lerp(posterized, _EdgeColor.rgb, edgeStrength);

                return float4(finalColor, 1.0);
            }
            ENDCG
        }
    }
}