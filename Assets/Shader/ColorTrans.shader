Shader "Unlit/ColorTrans"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Center ("Center", Range(0, 1)) = 0.5       
        _Radius ("Radius", Range(0, 1)) = 0.5        
        _FadeHardness ("Fade Hardness", Range(1, 10)) = 2  // �������ƽ���ǿ��
        _TargetColor ("Target Color", Color) = (1,0,0,0.5) 
    }
    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
        }

        Blend SrcAlpha OneMinusSrcAlpha  // Ҫ��͸���Ȼ��

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float2 screenPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Center;
            float _Radius;
            float _FadeHardness;
            fixed4 _TargetColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 center = float2(_Center, _Center);
                // ��ǰ���ص����ĵ�ľ���
                float dist = distance(i.screenPos, center);
                // ����ǿ��
                float falloff = 1 - smoothstep(_Radius * 0.8, _Radius, dist);
                falloff = pow(falloff, _FadeHardness);
                
                // �����ɫ Ҫ���û��ģʽ
                fixed4 col = tex2D(_MainTex, i.uv);
                return fixed4(lerp(col.rgb, _TargetColor.rgb, _TargetColor.a * falloff), 
                            col.a * (1 - falloff * _TargetColor.a));
            }
            ENDCG
        }
    }
}
