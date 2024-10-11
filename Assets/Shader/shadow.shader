Shader "Custom/ToonShaderWithFresnel"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Base Color", Color) = (1, 1, 1, 1)
        _ShadowThreshold ("Shadow Threshold", Range(0, 1)) = 0.5
        _ShadowColor ("Shadow Color", Color) = (0, 0, 0, 1)
        _FresnelPower ("Fresnel Power", Range(0.1, 5)) = 1.5
        _FresnelColor ("Fresnel Color", Color) = (1, 1, 1, 1)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _Color;
            float _ShadowThreshold;
            float4 _ShadowColor;
            float _FresnelPower;
            float4 _FresnelColor;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
                float3 viewDir : TEXCOORD3;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldNormal = normalize(mul(v.normal, (float3x3)unity_WorldToObject));
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.viewDir = normalize(_WorldSpaceCameraPos - o.worldPos);
                return o;
            }

            // Fragment shader: applies toon shading and fresnel effect
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 texColor = tex2D(_MainTex, i.uv) * _Color;

                // Calculate light intensity (toon-style)
                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                float NdotL = dot(i.worldNormal, lightDir);
                float shadow = step(_ShadowThreshold, NdotL);
                fixed4 baseColor = lerp(_ShadowColor, texColor, shadow);

                // Fresnel effect for depth enhancement
                float fresnelTerm = pow(1.0 - saturate(dot(i.worldNormal, i.viewDir)), _FresnelPower);
                fixed4 fresnel = fresnelTerm * _FresnelColor;

                // Combine toon shading with fresnel
                fixed4 finalColor = baseColor + fresnel;
                return finalColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
