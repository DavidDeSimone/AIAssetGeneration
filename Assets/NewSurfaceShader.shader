

Shader "Custom/OutlineSphere" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (1,0.5,0,1)
        _OutlineWidth ("Outline Width", Range (0.001, 0.03)) = 0.01
    }

    SubShader {
        Tags {"Queue"="Transparent" "RenderType"="Opaque"}

        Pass {
            Name "OUTLINE"
            Tags {"LightMode"="Always"}

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
            };

            float _OutlineWidth;
            float4 _OutlineColor;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            float4 frag (v2f i) : COLOR {
                float4 outputColor = _OutlineColor;
                
                outputColor.a = 1 - smoothstep(0, _OutlineWidth, fwidth(i.vertex.xyz));
                
                return outputColor;
            }
            ENDCG
        }

        Pass {
            Name "BASE"
            Tags { "LightMode" = "ForwardBase" }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG

        }
    }
    FallBack "Diffuse"
}