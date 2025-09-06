Shader "Unlit/TesselationShaderUnlit"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "black" {}
        _UniformTess ("Uniform Tessellation", Range(1,100)) = 1
        _TessTex ("Tessellation Map", 2D) = "black" {}
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma target 4.6
            #pragma vertex Vertex
            #pragma multi_compile_fog
            #pragma vertex TessellationVertexProgram
            #pragma fragment Fragments
            #pragma hull HullProgram
            #pragma domain DomainProgram
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

            struct ControlPoint
            {
                float4 vertex : INTERNALTESSPOS;
                float2 uv : TEXCOORD0;
            };

            struct TessellationFactors
            {
                float edge[3] : SV_TessFactor;
                float inside : SV_InsideTessFactor;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _UniformTess;
            sampler2D _TessTex;

            [UNITY_domain("tri")]
            [UNITY_outputcontrolpoints(3)]
            [UNITY_outputtopology("triangle_cw")]
            [UNITY_partitioning("integer")]
            [UNITY_patchconstantfunc("PatchConstantFunction")]
            ControlPoint HullProgram(InputPatch<ControlPoint, 3> patch, uint id : SV_OutputControlPointID)
            {
                return patch[id];
            }

            ControlPoint TessellationVertexProgram(appdata v)
            {
                ControlPoint cp;
                cp.vertex = v.vertex;
                cp.uv = v.uv;
                return cp;
            }

            TessellationFactors PatchConstantFunction(InputPatch<ControlPoint, 3> patch)
            {
                float p0factor = tex2Dlod(_TessTex, float4(patch[0].uv.x, patch[0].uv.y, 0, 0)).r;
                float p1factor = tex2Dlod(_TessTex, float4(patch[1].uv.x, patch[1].uv.y, 0, 0)).r;
                float p2factor = tex2Dlod(_TessTex, float4(patch[2].uv.x, patch[2].uv.y, 0, 0)).r;
                float factor = (p0factor + p1factor + p2factor);
                TessellationFactors f;
                f.edge[0] = factor > 0.0 ? _UniformTess : 1.0;
                f.edge[1] = factor > 0.0 ? _UniformTess : 1.0;
                f.edge[2] = factor > 0.0 ? _UniformTess : 1.0;
                f.inside = factor > 0.0 ? _UniformTess : 1.0;
                return f;
            }

            v2f Vertex(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }

            [UNITY_domain("tri")]
            v2f DomainProgram(TessellationFactors factors,
                  OutputPatch<ControlPoint, 3> patch,
                  float3 barycentricCoordinates : SV_DomainLocation)
            {
                appdata data;
                
                data.vertex = patch[0].vertex * barycentricCoordinates.x +
                    patch[1].vertex * barycentricCoordinates.y +
                    patch[2].vertex * barycentricCoordinates.z;
                data.uv = patch[0].uv * barycentricCoordinates.x +
                    patch[1].uv * barycentricCoordinates.y +
                    patch[2].uv * barycentricCoordinates.z;
            
                return Vertex(data);
            }

            fixed4 Fragments(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}