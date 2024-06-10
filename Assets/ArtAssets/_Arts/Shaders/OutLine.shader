
Shader "Custom/OutLine"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        //次级纹理
        _secondary("secondaryTexture", 2D) = "black" {}
        //描边宽度
        outline_width("outline_width", float) = 1
        //纹理尺寸（在OutLine.cs中获取）
        _MainTex_TexelSize ("Texel Size", Vector) = (0,0,0,0)
        //描边颜色
        outline_color("outline Color", Color) = (1, 0, 0, 1) // 默认为红色  
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        LOD 100 
        Pass
        {
            //混合模式定义,有下面这行才能将alpha传到片段着色器
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
                //uv值是0-1之间，_MainTex_TexelSize在OutLine.cs中是1/图片像素宽度,所以_MainTex_TexelSize.y是单位像素在uv上的偏移量，该值*描边宽度即让原先uv往上偏移描边宽度。
                float2 uv_up = Uv + float2(0,_MainTex_TexelSize.y) * outline_width;
                float2 uv_down = Uv + float2(0,-_MainTex_TexelSize.y) * outline_width;
                float2 uv_left = Uv + float2(_MainTex_TexelSize.x,0) * outline_width;
                float2 uv_right = Uv + float2(-_MainTex_TexelSize.x,0) * outline_width;
                //上面4行表示原纹理uv向四周偏移outline_width
                //使用偏移uv映射主纹理颜色
                fixed4 color_up = tex2D(_MainTex, uv_up);
                fixed4 color_down = tex2D(_MainTex, uv_down);
                fixed4 color_left = tex2D(_MainTex, uv_left);
                fixed4 color_right = tex2D(_MainTex, uv_right);
                //将四个纹理颜色叠加
                fixed4 outline = color_up + color_down + color_left + color_right;
                //fixed4 outlinetest = tex2D(_MainTex, i.uv);
                //获得原图颜色
                fixed4 origin_color = tex2D(_MainTex, Uv);
                //颜色等于描边颜色
                outline.rgb = outline_color.rgb;
                //只有叠加部分显示origin_color，偏移部分显示outline
                //lerp(x,y,a)，a为0-1，当其为1时，返回y，0时，返回x，origin_color.a表示透明度，原图不透明时即显示原图颜色，否则是描边颜色
                fixed4 mixedColor = lerp(outline, origin_color, origin_color.a);
                fixed4 secondaryColor = tex2D(_secondary,Uv);
                //次级纹理与主纹理混合（不能混合a透明度）
                mixedColor.rgb = mixedColor.rgb + secondaryColor.rgb;
                return mixedColor;
            }
            ENDCG
        }
    }
}
