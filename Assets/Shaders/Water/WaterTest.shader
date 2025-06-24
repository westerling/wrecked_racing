Shader "Custom/Water"
{
     Properties
    {
        _ShallowColor ("Shallow Water Color", Color) = (0.0, 0.5, 1.0, 1.0)
        _DeepColor ("Deep Water Color", Color) = (0.0, 0.1, 0.3, 1.0)
        _DepthFactor ("Depth Factor", Range(0.1, 10)) = 3.0
        _WaveSpeed ("Wave Speed", Range(0, 10)) = 1.0
        _WaveStrength ("Wave Strength", Range(0, 0.1)) = 0.02
        _RippleStrength ("Ripple Strength", Range(0, 0.2)) = 0.05
        _RippleFrequency ("Ripple Frequency", Range(0, 10)) = 5.0
        _WaterHeight ("Water Surface Y Position", Float) = 0.0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        CGPROGRAM
        #pragma surface surf Standard alpha vertex:vert
        #pragma target 3.0
        #include "UnityCG.cginc"

        struct Input
        {
            float3 worldPos;
        };

        fixed4 _ShallowColor;
        fixed4 _DeepColor;
        float _DepthFactor;
        float _WaveSpeed;
        float _WaveStrength;
        float _RippleStrength;
        float _RippleFrequency;
        float _WaterHeight;

        void vert(inout appdata_full v)
        {
            float2 worldPosXZ = v.vertex.xz;
            
            // Wave movement
            float wave = sin((worldPosXZ.x + _Time.y * _WaveSpeed) * 2.0) * 
                         cos((worldPosXZ.y + _Time.y * _WaveSpeed) * 2.0) * 
                         _WaveStrength;

            // Ripple effect (higher frequency, smaller ripples)
            float ripple = sin((worldPosXZ.x + _Time.y * _WaveSpeed) * _RippleFrequency) * 
                           cos((worldPosXZ.y + _Time.y * _WaveSpeed) * _RippleFrequency) * 
                           _RippleStrength;

            v.vertex.y += wave + ripple; // Apply both wave and ripple to vertex
        }

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            // Calculate depth based on water height vs object position
            float sceneDepth = _WaterHeight - IN.worldPos.y;

            // Blend colors based on depth
            float depthFactor = saturate(sceneDepth / _DepthFactor);
            fixed4 waterColor = lerp(_ShallowColor, _DeepColor, depthFactor);

            o.Albedo = waterColor.rgb;
            o.Alpha = waterColor.a;
            o.Metallic = 0.1;
            o.Smoothness = 0.8;
        }
            ENDCG
    }
    FallBack "Diffuse"
}