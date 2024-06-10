
Shader "Custom/OutLine"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        //�μ�����
        _secondary("secondaryTexture", 2D) = "black" {}
        //��߿��
        outline_width("outline_width", float) = 1
        //����ߴ磨��OutLine.cs�л�ȡ��
        _MainTex_TexelSize ("Texel Size", Vector) = (0,0,0,0)
        //�����ɫ
        outline_color("outline Color", Color) = (1, 0, 0, 1) // Ĭ��Ϊ��ɫ  
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        LOD 100 
        Pass
        {
            //���ģʽ����,���������в��ܽ�alpha����Ƭ����ɫ��
            Blend SrcAlpha OneMinusSrcAlpha  
            ZWrite Off  
            Cull Off  
            Fog { Mode Off }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

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
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            uniform float outline_width = 1.0;
            uniform fixed4 outline_color : COLOR = fixed4(0,0,0,1);
            float4 _MainTex_TexelSize;
            sampler2D _secondary;

            //uniform fixed2 TexelSize = Texture_TexelSize;

            fixed4 frag (v2f i) : SV_Target
            {
                float2 Uv = i.uv;
                //uvֵ��0-1֮�䣬_MainTex_TexelSize��OutLine.cs����1/ͼƬ���ؿ��,����_MainTex_TexelSize.y�ǵ�λ������uv�ϵ�ƫ��������ֵ*��߿�ȼ���ԭ��uv����ƫ����߿�ȡ�
                float2 uv_up = Uv + float2(0,_MainTex_TexelSize.y) * outline_width;
                float2 uv_down = Uv + float2(0,-_MainTex_TexelSize.y) * outline_width;
                float2 uv_left = Uv + float2(_MainTex_TexelSize.x,0) * outline_width;
                float2 uv_right = Uv + float2(-_MainTex_TexelSize.x,0) * outline_width;
                //����4�б�ʾԭ����uv������ƫ��outline_width
                //ʹ��ƫ��uvӳ����������ɫ
                fixed4 color_up = tex2D(_MainTex, uv_up);
                fixed4 color_down = tex2D(_MainTex, uv_down);
                fixed4 color_left = tex2D(_MainTex, uv_left);
                fixed4 color_right = tex2D(_MainTex, uv_right);
                //���ĸ�������ɫ����
                fixed4 outline = color_up + color_down + color_left + color_right;
                //fixed4 outlinetest = tex2D(_MainTex, i.uv);
                //���ԭͼ��ɫ
                fixed4 origin_color = tex2D(_MainTex, Uv);
                //��ɫ���������ɫ
                outline.rgb = outline_color.rgb;
                //ֻ�е��Ӳ�����ʾorigin_color��ƫ�Ʋ�����ʾoutline
                //lerp(x,y,a)��aΪ0-1������Ϊ1ʱ������y��0ʱ������x��origin_color.a��ʾ͸���ȣ�ԭͼ��͸��ʱ����ʾԭͼ��ɫ�������������ɫ
                fixed4 mixedColor = lerp(outline, origin_color, origin_color.a);
                fixed4 secondaryColor = tex2D(_secondary,Uv);
                //�μ��������������ϣ����ܻ��a͸���ȣ�
                mixedColor.rgb = mixedColor.rgb + secondaryColor.rgb;
                return mixedColor;
            }
            ENDCG
        }
    }
}
