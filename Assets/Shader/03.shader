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

            //6、绘制圆的函数
            float circle(float2 uv, float2 pos, float r, float side){
                return smoothstep(r - side, r + side, length(uv - pos));
            }

            //7、绘制矩形的函数
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

                //9、动画：波浪起伏效果
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
                
                ////4、后处理：屏幕马赛克
                //float size = 100;
                //i.uv = floor(i.uv * size)/size;
                
                fixed4 col = tex2D(_MainTex,i.uv);
                ////1、颜色：混合颜色
                //col = red + blue;

                ////2、后处理：明度变高
                //col *= 1.5;

                ////3、后处理：零饱和度
                ////计算颜色亮度，即RGB*对应色的明度0.2，0.7，0.1之和
                //float lumi = col.r * 0.2 + col.g * 0.7 +col.b * 0.1;
                ////饱和度即与亮度距离，此时为0
                //col = fixed4(lumi, lumi, lumi, 1);

                ////5、用step和smoothstep替代if-else
                //float isLeft = step(0.5,i.uv.x);
                //float isLeft = smoothstep(0.4,0.6,i.uv.x);
                //col *= lerp(red,blue,isLeft);

                ////6、形状：绘制circle
                ////圆心移至中央
                //float2 uv = i.uv * 2 - 1;
                ////圆心位移
                //float2 pos = float2(.5, .5);
                ////半径为0.5，边缘虚化0.1
                //float c = circle(uv, pos, 0.5, 0.1);
                //col *= c;

                ////7、形状：绘制矩形
                ////纵边为0.1，0.5，横边为0.1，0.6，虚化为0.02，0.05
                //float r = rect(i.uv, 0.1, 0.5, 0.1, 0.6, 0.02, 0.05);
                //col *= r;

                ////8、动画：颜色变化，uv，涟漪流动
                ////col = fixed4(abs(sin(_Time.y) * 2), 0, 0, 1);
                i.uv = i.uv * 2 - 1;
                ////UV流动效果
                //i.uv.y += sin(i.uv.x * 25 +_Time.y) * 0.1;
                //涟漪收缩
                col *= smoothstep(0.4, 0.6, sin(length(i.uv) * 30 + _Time.y * 3));
                ////颜色渐变
                col *= lerp(fixed4(1,0,0,1), fixed4(0,0,1,1), sin(_Time.y) / 2 + 0.5);
                
                //输出大小会被限制在0-1
                return col;
            }
            ENDCG
        }
    }
}
