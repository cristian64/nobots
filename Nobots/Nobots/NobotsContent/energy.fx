sampler ColorMapSampler : register(s0);

struct VertexShaderOutput
{
    float2 Tex : TEXCOORD0;
};

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	// Get the source pixel color
	float4 Color = tex2D(ColorMapSampler, input.Tex);
	Color[0] = Color[3];
	Color[1] = Color[3];
	Color[2] = Color[3];

	return Color;
}

technique PostOutline
{
    pass Pass0
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
