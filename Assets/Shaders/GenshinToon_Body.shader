Shader "GenshinToon/Body"
{
    Properties
    {
        [Header(Textures)]
        _BaseMap ("Base Map", 2D) = "white" {} // 基础纹理
        _LightMap ("Light Map", 2D) = "white" {} // 光照贴图
        [Toggle(_USE_LIGHTMAP_AO)] _UseLightMapAO ("Use LightMap AO", Range(0, 1)) = 1 // AO开关

        [Header(Ramp)]
        _RampTex ("Ramp Tex", 2D) = "white" {}
        [Toggle(_USE_RAMP_SHADOW)] _UseRampShadow ("Use RampShadow", Range(0, 1)) = 1 // 色阶阴影开关
        _ShadowRampWidth ("Shadow Ramp Width", Float) = 1 // 阴影边缘宽度
        _ShadowPosition ("Shadow Position", Float) = 0.55 // 阴影位置，比他更小的就是阴影
        _ShadowSoftness ("Shadow Softness", Float) = 0.5 // 阴影柔和度
        [Toggle] _UseRampShadow2 ("Use Ramp Shadow 2", Range(0, 1)) = 1 // 使用第2行阴影
        [Toggle] _UseRampShadow3 ("Use Ramp Shadow 3", Range(0, 1)) = 1 // 使用第3行阴影
        [Toggle] _UseRampShadow4 ("Use Ramp Shadow 4", Range(0, 1)) = 1 // 使用第4行阴影
        [Toggle] _UseRampShadow5 ("Use Ramp Shadow 5", Range(0, 1)) = 1 // 使用第5行阴影

        [Header(Lighting Options)]
        _DayOrNight ("Day Or Night", Range(0, 1)) = 0 // 日夜开关
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalRenderPipeline"
            "RenderType" = "Opaque"
        }

        HLSLINCLUDE
            // 预处理指令和头文件、常量定义、函数定义
            #pragma multi_compile _MAIN_LIGHT_SHADOWS // 主光源阴影
            #pragma multi_compile _MAIN_LIGHT_SHADOWS_CASCADE // 主光源阴影级联
            #pragma multi_compile _MAIN_LIGHT_SHADOWS_SCREEN // 主光源阴影屏幕空间

            #pragma multi_compile_fragment _LIGHT_LAYERS // 光照层
            #pragma multi_compile_fragment _LIGHT_COOKIES // 光照饼干
            #pragma multi_compile_fragment _SCREEN_SPACE_OCCLUSION // 屏幕空间遮挡
            #pragma multi_compile_fragment _ADDITIONAL_LIGHT_SHADOWS // 额外光源阴影
            #pragma multi_compile_fragment _SHADOWS_SOFT // 阴影软化

            #pragma shader_feature_local _USE_LIGHTMAP_AO // AO开关
            #pragma shader_feature_local _USE_RAMP_SHADOW // 色阶阴影开关

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl" // 核心库
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl" // 光照库

            CBUFFER_START(unityPerMaterial) // 常量缓冲区
                // Textures
                sampler2D _BaseMap; // 基础纹理
                sampler2D _LightMap; // 光照贴图

                // Ramp
                sampler2D _RampTex; // 色阶阴影贴图
                Float _ShadowRampWidth; // 阴影边缘宽度
                Float _ShadowPosition; // 阴影位置
                Float _ShadowSoftness; // 阴影柔和度
                Float _UseRampShadow2; // 第2行Ramp开关
                Float _UseRampShadow3; // 第3行Ramp开关
                Float _UseRampShadow4; // 第4行Ramp开关
                Float _UseRampShadow5; // 第5行Ramp开关
                
                // Lighting Options
                Float _DayOrNight; // 日夜开关

            CBUFFER_END

            // 官方版本的RampShadowID函数
            float RampShadowID(float input, 
                float useShadow2, float useShadow3, float useShadow4, float useShadow5, 
                float shadowValue1, float shadowValue2, float shadowValue3, float shadowValue4, float shadowValue5)
            {
                // 根据input值将模型分为5个区域
                float v1 = step(0.6, input) * step(input, 0.8); // 0.6-0.8区域
                float v2 = step(0.4, input) * step(input, 0.6); // 0.4-0.6区域
                float v3 = step(0.2, input) * step(input, 0.4); // 0.2-0.4区域
                float v4 = step(input, 0.2);                    // 0-0.2区域

                // 根据开关控制是否使用不同材质的值
                float blend12 = lerp(shadowValue1, shadowValue2, useShadow2);
                float blend15 = lerp(shadowValue1, shadowValue5, useShadow5);
                float blend13 = lerp(shadowValue1, shadowValue3, useShadow3);
                float blend14 = lerp(shadowValue1, shadowValue4, useShadow4);

                // 根据区域选择对应的材质值
                float result = blend12;                // 默认使用材质1或2
                result = lerp(result, blend15, v1);    // 0.6-0.8区域使用材质5
                result = lerp(result, blend13, v2);    // 0.4-0.6区域使用材质3
                result = lerp(result, blend14, v3);    // 0.2-0.4区域使用材质4
                result = lerp(result, shadowValue1, v4); // 0-0.2区域使用材质1

                return result;
            }

            struct UniversalAttributes // 顶点着色器输入参数
            {
                float4 positionOS : POSITION; // 物体坐标
                float2 uv0 : TEXCOORD0; // 纹理坐标
                float2 uv1 : TEXCOORD1; // 第二套纹理坐标
                float3 normalOS : NORMAL; // 物体法线坐标
                float4 color : COLOR0; // 顶点颜色
            };

            struct UniversalVaryings // 顶点着色器的输出，片元着色器的输入
            {
                float4 positionCS : SV_POSITION; // 裁剪空间坐标
                float2 uv0 : TEXCOORD0; // uv坐标
                float3 normalWS : TEXCOORD1; // 世界坐标法线
                float4 color : TEXCOORD2; // 顶点颜色
            };

            // 顶点着色器，返回其次裁剪空间坐标
            UniversalVaryings MainVS(UniversalAttributes input)
            {
                UniversalVaryings output; // 定义返回值

                // Position
                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz); // 坐标转换
                output.positionCS = vertexInput.positionCS; // 得到裁剪空间坐标

                // normal
                VertexNormalInputs vertexNormalInputs = GetVertexNormalInputs(input.normalOS);
                output.normalWS = vertexNormalInputs.normalWS; // 拿到世界空间法线

                // uv
                output.uv0 = input.uv0; // 得到纹理坐标

                // color
                output.color = input.color; // 传递顶点颜色

                return output;
            }

            // 片元着色器，返回颜色
            half4 MainFS(UniversalVaryings input) : SV_TARGET
            {
                Light light = GetMainLight(); // 获取主光源
                half4 vertexColor = input.color;

                // Normallize Vector
                half3 N = normalize(input.normalWS); // 法线方向
                half3 L = normalize(light.direction); // 主光源方向
                half NoL = dot(N, L); // 计算点积

                // Lambert
                half lambert = NoL;
                half halflambert = lambert * 0.5 + 0.5;
                halflambert = pow(halflambert, 3);
                half lambertstep = smoothstep(0.01, 0.4, halflambert); // 平滑插值
                half shadowFactor = lerp(0, halflambert, lambertstep); // 计算阴影因子

                // Textures Info
                half4 baseMap = tex2D(_BaseMap, input.uv0); // 采样纹理贴图
                half4 lightMap = tex2D(_LightMap, input.uv0); // 采样光照贴图

                // AO
                #if _USE_LIGHTMAP_AO
                    half ambient = lightMap.g;
                #else
                    half ambient = halflambert;
                #endif
                half shadow = (ambient + halflambert) * 0.5;
                shadow = lerp(shadow, 1, step(0.95, ambient));
                shadow = lerp(0, shadow, step(0.05, ambient));
                half isShadowArea = step(shadow, _ShadowPosition); // 是否处于阴影区域
                half shadowDepth = saturate((_ShadowPosition - shadow) / _ShadowPosition); // 阴影深度
                shadowDepth = pow(shadowDepth, _ShadowSoftness); // 根据柔和度调整阴影深度
                shadowDepth = min(shadowDepth, 1); // 限制阴影深度不超过1
                half rampWidthFactor = vertexColor.g * 2 * _ShadowRampWidth; // 使用顶点颜色G通道控制ramp宽度
                half shadowPositon = (_ShadowPosition - shadowFactor) / _ShadowPosition; // 代入阴影因子计算阴影

                // Ramp
                half rampU = 1 - saturate(shadowDepth / rampWidthFactor); // 计算Ramp坐标转换
                half rampID = RampShadowID(lightMap.a, _UseRampShadow2, _UseRampShadow3, _UseRampShadow4, _UseRampShadow5, 1, 2, 3, 4 , 5);
                half rampV = 0.45 - (rampID - 1) * 0.1; // 根据rampID计算V坐标
                half2 rampDayUV = half2(rampU, rampV + 0.5); // 构建白天UV坐标
                half3 rampDayColor = tex2D(_RampTex, rampDayUV).rgb; // 采样白天Ramp颜色
                half2 rampNightUV = half2(rampU, rampV); // 构建夜晚UV坐标
                half3 rampNightColor = tex2D(_RampTex, rampNightUV).rgb; // 采样夜晚Ramp颜色
                half3 rampColor = lerp(rampDayColor, rampNightColor, _DayOrNight); // 采样Ramp贴图的颜色

                // Merge Color
                #if _USE_RAMP_SHADOW
                    half3 finalColor = baseMap.rgb * rampColor * (isShadowArea ? 1 : 1.2);
                #else
                    half3 finalColor = baseMap.rgb * halflambert * (shadow + 0.3); // 最终颜色
                #endif

                return half4(finalColor.rgb, 1);     
            }
        ENDHLSL

        Pass // 渲染通道
        {
            Name "UniversalForward" //通道名称
            Tags
            {
                "LightMode" = "UniversalForward" // 光照模型，向前渲染
            }

            HLSLPROGRAM // 着色器程序
                #pragma vertex MainVS
                #pragma fragment MainFS
            ENDHLSL
        }

        Pass
        {
            Name "UniversalForward" //通道名称
            Tags
            {
                "LightMode" = "SRPDefaultUnlit" // 光照模型，向前渲染
                "Queue" = "Geometry + 1"
            }

            Cull Front

            HLSLPROGRAM // 着色器程序
                #pragma vertex BackMainVS
                #pragma fragment MainFS

                UniversalVaryings BackMainVS(UniversalAttributes input)
                {
                    UniversalVaryings output; // 定义返回值

                    output = MainVS(input);
                    output.uv0 = input.uv1;
                    output.normalWS = -output.normalWS;
                    return output;
                }
            ENDHLSL
        }

        Pass
        {
            Name "ShadowCaster"
            Tags
            {
                "LightMode" = "ShadowCaster" // 光照模式，阴影投射
            }

            ZWrite On // 开启深度写入
            ZTest LEqual // 深度测试，小于等于
            ColorMask 0 // 不写入颜色缓冲区
            Cull Off // 不裁剪

            HLSLPROGRAM // 着色器程序开始
                
                #pragma multi_compile_instancing // 启用GPU实例化编译
                #pragma multi_compile _ DOTS_INSTANCING_ON // 启用DOTS实例化编译
                #pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW // 启用点光源阴影

                #pragma vertex ShadowVS 
                #pragma fragment ShadowFS

                float3 _LightDirection; // 光源方向
                float3 _LightPosition; // 光源位置

                struct Attributes // 顶点着色器输入参数
                {
                    float4 positionOS : POSITION;
                    float3 normalOS : NORMAL;
                };

                struct Varyings // 顶点着色器的输出，片元着色器的输入
                {
                    float4 positionCS : SV_POSITION; // 裁剪空间坐标
                };

                // 将阴影的世界空间顶点位置转换为适合投影的裁剪空间位置
                float4 GetShadowPositionHClip(Attributes input)
                {
                    float3 positionWS = TransformObjectToWorld(input.positionOS.xyz); // 将本地空间顶点坐标转换为世界空间顶点坐标
                    float3 normalWS = TransformObjectToWorldNormal(input.normalOS); // 将本地空间法线转换为世界空间法线

                    #if _CASTING_PUNCTUAL_LIGHT_SHADOW // 点光源
                        float3 lightDirectionWS = normalize(_LightPosition - positionWS); // 计算光源方向
                    #else // 平行光
                        float3 lightDirectionWS = _LightDirection; // 使用预定义的光源方向
                    #endif

                    float4 positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, lightDirectionWS)); // 应用阴影偏移并转换到裁剪空间

                    // 根据平台的Z缓冲方向调整Z值
                    #if UNITY_REVERSED_Z // 反转Z缓冲区
                        positionCS.z = min(positionCS.z, UNITY_NEAR_CLIP_VALUE); // 限制Z值在近裁剪平面以下
                    #else // 正向Z缓冲区
                        positionCS.z = max(positionCS.z, UNITY_NEAR_CLIP_VALUE); // 限制Z值在远裁剪平面以上
                    #endif

                    return positionCS; // 返回裁剪空间顶点坐标
                }

                // 顶点着色器，返回其次裁剪空间坐标
                Varyings ShadowVS(Attributes input)
                {
                    Varyings output;
                    output.positionCS = GetShadowPositionHClip(input);
                    return output;
                }

                half4 ShadowFS(Varyings input) : SV_TARGET
                {
                    return 0;
                }
                
            ENDHLSL
        }
    }


}
