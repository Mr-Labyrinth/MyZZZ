Shader "UI/FlowingGradient"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _ColorA ("Color A (橙色)", Color) = (1, 0.55, 0, 1)
        _ColorB ("Color B (紫色)", Color) = (0.6, 0.2, 1, 1)
        _Speed ("流动速度", Float) = 0.5
        _Frequency ("频率", Float) = 1
        _Direction ("方向 (0=水平, 1=垂直)", Range(0,1)) = 1

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            sampler2D _MainTex;
            fixed4 _ColorA;
            fixed4 _ColorB;
            float _Speed;
            float _Frequency;
            float _Direction;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                o.color = v.color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // 根据方向选择横竖坐标
                float pos = lerp(i.texcoord.x, i.texcoord.y, _Direction);

                // 流动相位，随时间循环
                float flow = pos * _Frequency + _Time.y * _Speed;

                // sin 波，让颜色在两种颜色之间无缝来回过渡
                float blend = sin(flow * 6.2831853) * 0.5 + 0.5;

                // 混合颜色
                fixed4 col = lerp(_ColorA, _ColorB, blend);

                // 保留 Sprite 原贴图和顶点色（这样 Image 的 Alpha、Raycast 等都正常）
                fixed4 tex = tex2D(_MainTex, i.texcoord);
                col *= tex;
                col *= i.color;

                return col;
            }
            ENDCG
        }
    }
}