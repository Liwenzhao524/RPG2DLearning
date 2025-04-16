Shader "Unlit/02"
{
    //��inspector�ڿɼ�
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MyColor ("MyColor", COLOR) = (1,1,1,1)
        //shader��������"������ʾ��"�� �������ͣ�= ��ֵ
    }
    //����ɫ����������Բ�ͬ�Կ��ֱ����
    SubShader
    {
        //һ��Pass��ʾһ����Ⱦ���̣���Shader����
        //һ��SubShader�����ж��Pass������ִ��
        Pass
        {
            //shader���������CGPROGRAM��ENDCG��
            CGPROGRAM
            //ָ������vertΪ����Shader������fragΪƬԪShader
            #pragma vertex vert   
            #pragma fragment frag
            
            //���������ļ�
            #include "UnityCG.cginc"

            //CPU�򶥵�Shader�ṩ��ģ������
            struct appdata
            {
                //���壬������������
                float4 vertex : POSITION;  //ģ�Ͷ�������
                float2 uv : TEXCOORD0;     //UV�����4��

                float4 color : COLOR;      //������ɫ
                float3 normal : NORMAL;    //���㷨��
                float4 tangent : TANGENT;  //��������
            };

            //����Shader����ƬԪShader������
            struct v2f
            {
                float2 uv : TEXCOORD0;  //����uv
                float4 vertex : SV_POSITION;  //ģ�Ͷ�������
            };

            //����������Properties�еı�������֮��������ʹ��
            float4 _MyColor;
            
            sampler2D _MainTex;
            float4 _MainTex_ST;

            //����Shader
            v2f vert (appdata v)
            {
                v2f o;
                //v.vertex *=2;
                o.vertex = UnityObjectToClipPos(v.vertex);  //����任
                
                //o.uv = v.uv;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            //ƬԪShader
            fixed4 frag (v2f i) : SV_Target //����ֵ������Ϊ������ɫ
            {
                //fixed4 col = fixed4(1,0,0,1);
                fixed4 col = fixed4(i.uv.x,0,0,1);
                //fixed4 col = tex2D(_MainTex, i.uv);  //������ɫ
                return col;
            }
            ENDCG
        }
    }
}
