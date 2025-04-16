Shader "Unlit/Normal"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _BumpMap ("Normal Map", 2D) = "bump"{}
        _NormalSlider ("NormalSlider", Range(0, 1)) = 0
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
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
                float3 worldNormal : TEXCOORD1;
                float3 worldTangent : TEXCOORD2;
                float3 worldBinormal : TEXCOORD3;
            };

            sampler2D _MainTex;
            sampler2D _BumpMap;
            float4 _MainTex_ST;
            fixed4 _Color;
            float _NormalSlider;

            v2f vert (appdata v)
            {
                v2f o;
                
                ////ͨ��MainTex����ɫ��Ϣ�ı䶥��߶�
                //v.vertex.y += tex2Dlod(_MainTex, float4(v.uv, 0, 0)).r;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                
                //ת������ռ�
                float3 worldNormal = UnityObjectToWorldNormal(v.normal);
                float3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz);
                float3 worldBinormal = cross(worldNormal, worldTangent);

                o.worldNormal = worldNormal;
                o.worldTangent = worldTangent;
                o.worldBinormal = worldBinormal;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 normalTex = UnpackNormal(tex2D(_BumpMap, i.uv));  //��ֵת��
                
                //����TBN����
                float3x3 TBN = float3x3(normalize(i.worldTangent), normalize(i.worldBinormal), normalize(i.worldNormal));
                //normalTexλ�����߿ռ䣬ת������������
                float3 normal = mul(normalTex, TBN);  //����mul���壬����TBN����
                
                float3 orinormal = i.normal;
                i.normal = lerp(normal, orinormal, _NormalSlider);
                //Lambert
                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                float diff = max(dot(i.normal, lightDir), 0.0);

                fixed4 col = diff * _Color;
                return col;
            }
            ENDCG
        }
    }
}
