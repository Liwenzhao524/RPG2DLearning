Shader "Unlit/02"
{
    //在inspector内可见
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MyColor ("MyColor", COLOR) = (1,1,1,1)
        //shader变量名（"引擎显示名"， 变量类型）= 初值
    }
    //子着色器，可以针对不同显卡分别设计
    SubShader
    {
        //一个Pass表示一次渲染过程，是Shader主体
        //一个SubShader可以有多个Pass，依序执行
        Pass
        {
            //shader代码包裹在CGPROGRAM和ENDCG内
            CGPROGRAM
            //指定函数vert为顶点Shader，函数frag为片元Shader
            #pragma vertex vert   
            #pragma fragment frag
            
            //引用内置文件
            #include "UnityCG.cginc"

            //CPU向顶点Shader提供的模型数据
            struct appdata
            {
                //语义，表明变量含义
                float4 vertex : POSITION;  //模型顶点坐标
                float2 uv : TEXCOORD0;     //UV，最多4套

                float4 color : COLOR;      //顶点颜色
                float3 normal : NORMAL;    //顶点法线
                float4 tangent : TANGENT;  //顶点切线
            };

            //顶点Shader流向片元Shader的数据
            struct v2f
            {
                float2 uv : TEXCOORD0;  //顶点uv
                float4 vertex : SV_POSITION;  //模型顶点坐标
            };

            //变量声明，Properties中的变量须与之重名才能使用
            float4 _MyColor;
            
            sampler2D _MainTex;
            float4 _MainTex_ST;

            //顶点Shader
            v2f vert (appdata v)
            {
                v2f o;
                //v.vertex *=2;
                o.vertex = UnityObjectToClipPos(v.vertex);  //坐标变换
                
                //o.uv = v.uv;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            //片元Shader
            fixed4 frag (v2f i) : SV_Target //返回值，语义为像素颜色
            {
                //fixed4 col = fixed4(1,0,0,1);
                fixed4 col = fixed4(i.uv.x,0,0,1);
                //fixed4 col = tex2D(_MainTex, i.uv);  //像素颜色
                return col;
            }
            ENDCG
        }
    }
}
