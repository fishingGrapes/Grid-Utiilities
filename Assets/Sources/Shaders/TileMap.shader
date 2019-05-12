Shader "Custom/Tilemap Shader"
{

    Properties
    {
        _Tint ("Tint", Color) = (1, 1, 1, 1)
        _MainTex ("Texture", 2D) = "white" { }
    }

    SubShader
    {

        Pass
        {

            CGPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag 

            #include "UnityCG.cginc"

            float4 _Tint;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            //Tiling and Offset Variable

            //From Vertex to  Fragment Shader
            struct Interpolators
            {
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;

            };

            //Input injected into Vertex Shader
            struct VertexData
            {
                float4 position : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            Interpolators Vert(VertexData v)
            {
                Interpolators i;
                i.position = UnityObjectToClipPos(v.position);
                //UV * Scaling + Transformation 

                //i.uv = v.uv * _MainTex_ST.xy + _MainTex_ST.zw;
                //can be Replaced by
                i.uv = TRANSFORM_TEX(v.uv, _MainTex);
                i.color = v.color;
                return i;
            }

            float4 Frag(Interpolators i) : SV_TARGET
            {
                return tex2D(_MainTex, i.uv) * _Tint * i.color;
            }

            ENDCG
        }
    }
}