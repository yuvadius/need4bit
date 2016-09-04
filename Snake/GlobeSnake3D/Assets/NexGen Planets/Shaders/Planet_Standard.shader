// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

// Shader created with Shader Forge Beta 0.30 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.30;sub:START;pass:START;ps:flbk:NexGen/Planets/Sta,lico:0,lgpr:1,nrmq:0,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,enco:True,frtr:True,vitr:True,dbil:False,rmgx:True,hqsc:True,hqlp:False,blpr:0,bsrc:0,bdst:1,culm:0,dpts:2,wrdp:True,ufog:True,aust:False,igpj:True,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:1,fgcg:0,fgcb:0,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:30633,y:33669|diff-1616-OUT,diffpow-1616-OUT,spec-2531-OUT,gloss-1488-OUT,normal-1383-RGB,emission-1648-OUT;n:type:ShaderForge.SFN_Fresnel,id:10,x:32115,y:33257|NRM-1622-OUT;n:type:ShaderForge.SFN_Tex2d,id:1371,x:32115,y:33432,ptlb:Diffuse (RGB) Specular (A),ptin:_DiffuseRGBSpecularA,tex:926ab811b2a96c443846db02bff4d8e7,ntxv:0,isnm:False|UVIN-1739-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:1383,x:32115,y:33614,ptlb:Normal Map,ptin:_NormalMap,tex:d985cb5859ea9924ca2a6d60fe0199a5,ntxv:3,isnm:True|UVIN-1739-UVOUT;n:type:ShaderForge.SFN_Lerp,id:1394,x:31922,y:33316|A-1371-A,B-10-OUT,T-1395-OUT;n:type:ShaderForge.SFN_Vector1,id:1395,x:32115,y:33139,v1:0.7;n:type:ShaderForge.SFN_Multiply,id:1420,x:31736,y:33316|A-1371-A,B-1394-OUT;n:type:ShaderForge.SFN_Multiply,id:1426,x:31537,y:33267|A-1427-OUT,B-1420-OUT;n:type:ShaderForge.SFN_Slider,id:1427,x:31736,y:33169,ptlb:Specular Level,ptin:_SpecularLevel,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Slider,id:1488,x:31537,y:33469,ptlb:Glossiness,ptin:_Glossiness,min:0.2,cur:0.2,max:0.4;n:type:ShaderForge.SFN_Tex2d,id:1588,x:32115,y:33800,ptlb:Clouds,ptin:_Clouds,tex:23075e732097310458d9b8b09445673e,ntxv:0,isnm:False|UVIN-2181-UVOUT;n:type:ShaderForge.SFN_Add,id:1616,x:31373,y:33694|A-1371-RGB,B-2231-OUT;n:type:ShaderForge.SFN_LightVector,id:1622,x:32314,y:33257;n:type:ShaderForge.SFN_Tex2d,id:1638,x:32114,y:34108,ptlb:Nightlights,ptin:_Nightlights,tex:f9c06cf8bea79fe4c957866cb429e7b0,ntxv:0,isnm:False|UVIN-1739-UVOUT;n:type:ShaderForge.SFN_LightVector,id:1645,x:31923,y:33846;n:type:ShaderForge.SFN_Dot,id:1647,x:31694,y:33972,dt:0|A-1645-OUT,B-2400-OUT;n:type:ShaderForge.SFN_Multiply,id:1648,x:31419,y:34090|A-1647-OUT,B-1638-RGB;n:type:ShaderForge.SFN_Panner,id:1739,x:32584,y:33585,spu:1,spv:0|DIST-2101-OUT;n:type:ShaderForge.SFN_ValueProperty,id:2084,x:33223,y:33457,ptlb:Rotation,ptin:_Rotation,glob:False,v1:0;n:type:ShaderForge.SFN_Time,id:2100,x:33032,y:33698;n:type:ShaderForge.SFN_Multiply,id:2101,x:32757,y:33585|A-2114-OUT,B-2100-T;n:type:ShaderForge.SFN_Vector1,id:2113,x:33223,y:33574,v1:0.025;n:type:ShaderForge.SFN_Multiply,id:2114,x:33032,y:33508|A-2084-OUT,B-2113-OUT;n:type:ShaderForge.SFN_Panner,id:2181,x:32584,y:33793,spu:0.9,spv:0|DIST-2184-OUT;n:type:ShaderForge.SFN_Multiply,id:2184,x:32757,y:33792|A-2114-OUT,B-2100-T;n:type:ShaderForge.SFN_Multiply,id:2231,x:31602,y:33744|A-2232-OUT,B-1588-RGB;n:type:ShaderForge.SFN_Slider,id:2232,x:31795,y:33675,ptlb:Clouds Opacity,ptin:_CloudsOpacity,min:0,cur:0,max:3;n:type:ShaderForge.SFN_Vector3,id:2400,x:31913,y:34007,v1:0.5,v2:0.5,v3:0;n:type:ShaderForge.SFN_Slider,id:2436,x:34841,y:32885,ptlb:Rotation Speed_copy_copy,ptin:_RotationSpeed_copy_copy,min:0,cur:0.6466165,max:1;n:type:ShaderForge.SFN_Time,id:2438,x:34794,y:33078;n:type:ShaderForge.SFN_Multiply,id:2440,x:34410,y:32887|A-2442-OUT,B-2444-OUT;n:type:ShaderForge.SFN_Vector1,id:2442,x:34652,y:32811,v1:0.1;n:type:ShaderForge.SFN_Multiply,id:2444,x:34633,y:32947|A-2436-OUT,B-2438-TSL;n:type:ShaderForge.SFN_Multiply,id:2531,x:31136,y:33451|A-1426-OUT,B-2537-OUT;n:type:ShaderForge.SFN_OneMinus,id:2537,x:31278,y:33531|IN-2231-OUT;proporder:1371-1383-1427-1488-1588-1638-2084-2232;pass:END;sub:END;*/

Shader "NexGen/Planets/Standard" {
    Properties {
        _DiffuseRGBSpecularA ("Diffuse (RGB) Specular (A)", 2D) = "white" {}
        _NormalMap ("Normal Map", 2D) = "bump" {}
        _SpecularLevel ("Specular Level", Range(0, 1)) = 1
        _Glossiness ("Glossiness", Range(0.2, 0.4)) = 0.2
        _Clouds ("Clouds", 2D) = "white" {}
        _Nightlights ("Nightlights", 2D) = "white" {}
        _Rotation ("Rotation", Float ) = 0
        _CloudsOpacity ("Clouds Opacity", Range(0, 3)) = 0
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "RenderType"="Opaque"
        }
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers xbox360 ps3 flash 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform float4 _TimeEditor;
            uniform sampler2D _DiffuseRGBSpecularA; uniform float4 _DiffuseRGBSpecularA_ST;
            uniform sampler2D _NormalMap; uniform float4 _NormalMap_ST;
            uniform float _SpecularLevel;
            uniform float _Glossiness;
            uniform sampler2D _Clouds; uniform float4 _Clouds_ST;
            uniform sampler2D _Nightlights; uniform float4 _Nightlights_ST;
            uniform float _Rotation;
            uniform float _CloudsOpacity;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float4 uv0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 binormalDir : TEXCOORD4;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.uv0;
                o.normalDir = mul(float4(v.normal,0), unity_WorldToObject).xyz;
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.binormalDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                float3x3 tangentTransform = float3x3( i.tangentDir, i.binormalDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
/////// Normals:
                float node_2114 = (0.025);
                float4 node_2100 = _Time + _TimeEditor;
                float2 node_3215 = i.uv0;
                float2 node_1739 = (node_3215.rg+(node_2114*node_2100.g)*float2(1,0));
                float3 normalLocal = UnpackNormal(tex2D(_NormalMap,TRANSFORM_TEX(node_1739, _NormalMap))).rgb;
                float3 normalDirection =  normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = 1;
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
                float NdotL = dot( normalDirection, lightDirection );
                float4 node_1371 = tex2D(_DiffuseRGBSpecularA,TRANSFORM_TEX(node_1739, _DiffuseRGBSpecularA));
                float2 node_2181 = (node_3215.rg+(node_2114*node_2100.g)*float2(0.9,0));
                float3 node_2231 = (_CloudsOpacity*tex2D(_Clouds,TRANSFORM_TEX(node_2181, _Clouds)).rgb);
                float3 node_1616 = (node_1371.rgb+node_2231);
                float3 diffuse = pow(max( 0.0, NdotL), node_1616)*InvPi * attenColor + UNITY_LIGHTMODEL_AMBIENT.xyz;
////// Emissive:
                float3 emissive = (dot(lightDirection,float3(0.5,0.5,0))*tex2D(_Nightlights,TRANSFORM_TEX(node_1739, _Nightlights)).rgb);
///////// Gloss:
                float gloss = exp2(_Glossiness*10.0+1.0);
////// Specular:
                NdotL = max(0.0, NdotL);
                float3 specularColor = ((_SpecularLevel*(node_1371.a*lerp(node_1371.a,(1.0-max(0,dot(lightDirection, viewDirection))),0.7)))*(1.0 - node_2231));
                float specularMonochrome = dot(specularColor,float3(0.3,0.59,0.11));
                float normTerm = (gloss + 8.0 ) / (8.0 * Pi);
                float3 specular = (floor(attenuation) * _LightColor0.xyz) * pow(max(0,dot(halfDirection,normalDirection)),gloss) * specularColor*normTerm;
                float3 finalColor = 0;
                float3 diffuseLight = diffuse;
                diffuseLight *= 1-specularMonochrome;
                finalColor += diffuseLight * node_1616;
                finalColor += specular;
                finalColor += emissive;
/// Final Color:
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "NexGen/Planets/Sta"
    CustomEditor "ShaderForgeMaterialInspector"
}
