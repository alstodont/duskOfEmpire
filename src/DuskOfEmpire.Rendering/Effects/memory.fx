// stub: Phase 3 replaces this pass with the memory filter
#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float4x4 MatrixTransform;

struct VSIn
{
    float4 Position : POSITION0;
    float4 Color : COLOR0;
    float2 Tex : TEXCOORD0;
};

struct VSOut
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float2 Tex : TEXCOORD0;
};

VSOut VS(VSIn input)
{
    VSOut output = (VSOut)0;
    output.Position = mul(input.Position, MatrixTransform);
    output.Color = input.Color;
    output.Tex = input.Tex;
    return output;
}

float4 PS(VSOut input) : COLOR
{
    return input.Color;
}

technique Stub
{
    pass Pass0
    {
        VertexShader = compile VS_SHADERMODEL VS();
        PixelShader = compile PS_SHADERMODEL PS();
    }
};
