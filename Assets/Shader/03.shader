Shader "Unlit/03"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            //6������Բ�ĺ���
            float circle(float2 uv, float2 pos, float r, float side){
                return smoothstep(r - side, r + side, length(uv - pos));
            }

            //7�����ƾ��εĺ���
            float rect(float2 uv, float xmin, float xmax, float ymin, float ymax, float xside, float yside){
                float r = smoothstep(xmin - xside, xmin + xside, uv.x);
                r *= smoothstep(xmax + xside, xmax - xside, uv.x);
                r *= smoothstep(ymin - yside, ymin + yside, uv.y);
                r *= smoothstep(ymax + yside, ymax - yside, uv.y);
                return r;
            }

            v2f vert (appdata v)
            {
                v2f o;

                //9���������������Ч��
                v.vertex.y += sin(v.vertex.x + _Time.y * 3);

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 red = fixed4(1,0,0,1);
                fixed4 blue = fixed4(0,0,1,1);
                
                ////4��������Ļ������
                //float size = 100;
                //i.uv = floor(i.uv * size)/size;
                
                fixed4 col = tex2D(_MainTex,i.uv);
                ////1����ɫ�������ɫ
                //col = red + blue;

                ////2���������ȱ��
                //col *= 1.5;

                ////3�������㱥�Ͷ�
                ////������ɫ���ȣ���RGB*��Ӧɫ������0.2��0.7��0.1֮��
                //float lumi = col.r * 0.2 + col.g * 0.7 +col.b * 0.1;
                ////���Ͷȼ������Ⱦ��룬��ʱΪ0
                //col = fixed4(lumi, lumi, lumi, 1);

                ////5����step��smoothstep���if-else
                //float isLeft = step(0.5,i.uv.x);
                //float isLeft = smoothstep(0.4,0.6,i.uv.x);
                //col *= lerp(red,blue,isLeft);

                ////6����״������circle
                ////Բ����������
                //float2 uv = i.uv * 2 - 1;
                ////Բ��λ��
                //float2 pos = float2(.5, .5);
                ////�뾶Ϊ0.5����Ե�黯0.1
                //float c = circle(uv, pos, 0.5, 0.1);
                //col *= c;

                ////7����״�����ƾ���
                ////�ݱ�Ϊ0.1��0.5�����Ϊ0.1��0.6���黯Ϊ0.02��0.05
                //float r = rect(i.uv, 0.1, 0.5, 0.1, 0.6, 0.02, 0.05);
                //col *= r;

                ////8����������ɫ�仯��uv����������
                ////col = fixed4(abs(sin(_Time.y) * 2), 0, 0, 1);
                i.uv = i.uv * 2 - 1;
                ////UV����Ч��
                //i.uv.y += sin(i.uv.x * 25 +_Time.y) * 0.1;
                //��������
                col *= smoothstep(0.4, 0.6, sin(length(i.uv) * 30 + _Time.y * 3));
                ////��ɫ����
                col *= lerp(fixed4(1,0,0,1), fixed4(0,0,1,1), sin(_Time.y) / 2 + 0.5);
                
                //�����С�ᱻ������0-1
                return col;
            }
            ENDCG
        }
    }
}
