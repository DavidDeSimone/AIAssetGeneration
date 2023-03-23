

Shader "Custom/OrangeTint" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _TintColor ("Tint Color", Color) = (1, 0.5, 0, 1) // Orange
    }
 
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 100
 
        CGPROGRAM
        #pragma surface surf Lambert
 
        sampler2D _MainTex;
        fixed4 _TintColor;
 
        struct Input {
            float2 uv_MainTex;
        };
 
        void surf (Input IN, inout SurfaceOutput o) {
            fixed4 tex = tex2D (_MainTex, IN.uv_MainTex) * _TintColor;
            o.Albedo = tex.rgb;
            o.Alpha = tex.a;
        }
 
        ENDCG
    }
 
    FallBack "Diffuse"
}