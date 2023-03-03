Shader "Unlit/Scarf"
{
    Properties
    {
        _Color("Scarf Color", Color) = (1,1,1,1)
        _Color2("Scarf Shade", Color) = (1,1,1,1)

        _CurveAmp("CurveAmp", Range(0,10)) = 0
        _WindAmp("Wind Amplitude", Range(0,0.5)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            #define TAU 6.283185307179586


            float4 _Color;
            float4 _Color2;

            float _WindAmp;
            float _CurveAmp;

            struct MeshData
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Interpolate
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Interpolate vert (MeshData v)
            {
                Interpolate o;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

                return o;
            }

            fixed4 frag (Interpolate i) : SV_Target
            {
                float t = cos((i.uv.x + _Time.y * 0.1) * TAU * _CurveAmp) * 0.5 + 0.5;

                float4 _outColor = lerp(_Color, _Color2, t);

                return _outColor;
            }
            ENDCG
        }
    }
}
