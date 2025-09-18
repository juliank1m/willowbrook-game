sampler2D TextureSampler : register(s0);
float blendFactor; // This value will be passed from the application

float4 MainPS(float2 texCoord : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(TextureSampler, texCoord);
    float gray = dot(color.rgb, float3(0.3, 0.59, 0.11));
    float4 grayscale = float4(gray, gray, gray, color.a);
    return lerp(grayscale, color, blendFactor);
}

technique Grayscale
{
    pass P0
    {
        PixelShader = compile ps_2_0 MainPS();
    }
}
