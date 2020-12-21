Shader "Hidden/DiffuseShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite On ZTest Always

        Pass
        {
            Tags
            {
                "RenderType" = "Opaque" 
                "LightMode"  = "ForwardBase"
                "PassFlags"  = "OnlyDirectional"
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal: NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 normal: NORMAL;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.normal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            sampler2D _MainTex;
            fixed4 _Color;

            fixed4 frag (v2f i) : SV_Target
            {
                float4 sample = tex2D(_MainTex, i.uv);
                float3 normal = normalize(i.normal);
                float NdotL = dot(_WorldSpaceLightPos0, normal); 
                return _Color * sample * NdotL;
            }
            ENDCG
        }
    }
}
