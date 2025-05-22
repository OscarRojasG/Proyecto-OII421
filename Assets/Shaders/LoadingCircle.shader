Shader "Custom/RotatingSpinner"
{
    Properties
    {
        _Color ("Color", Color) = (1, 1, 1, 1)
        _BackgroundColor ("Background Color", Color) = (0, 0, 0, 0)
        _ArcAngle ("Arc Angle (Degrees)", Range(0,360)) = 120
        _Thickness ("Ring Thickness", Range(0,1)) = 0.3
        _Rotation ("Rotation (Degrees)", Range(0,360)) = 0
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float _ArcAngle;
            float _Thickness;
            float _Rotation;
            fixed4 _Color;
            fixed4 _BackgroundColor;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv * 2.0 - 1.0; // [-1,1] space
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;

                float r = length(uv);
                if (r > 1.0) discard;

                float angle = degrees(atan2(uv.y, uv.x));
                angle = fmod(angle + 360.0, 360.0); // [0,360)

                float arcStart = fmod(_Rotation, 360.0);
                float arcEnd = fmod(_Rotation + _ArcAngle, 360.0);

                bool insideArc = arcStart < arcEnd ?
                    (angle >= arcStart && angle <= arcEnd) :
                    (angle >= arcStart || angle <= arcEnd);

                float edgeSoft = 0.1;

                float inner = 1.0 - _Thickness;
                float aa = smoothstep(inner, inner + edgeSoft, r) *
                           (1.0 - smoothstep(1.0 - edgeSoft, 1.0, r));

                return insideArc ? _Color * aa : _BackgroundColor;
            }
            ENDCG
        }
    }
}
