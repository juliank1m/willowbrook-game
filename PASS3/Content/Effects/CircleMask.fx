sampler2D TextureSampler : register(s0);

float2 CircleCenter; // Normalized coordinates (0 to 1)
float CircleRadius;  // In pixels
float2 ScreenDimensions;

float4 MainPS(float2 texCoord : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(TextureSampler, texCoord);

    // Convert texCoord to screen space coordinates
    float2 screenPos = texCoord * ScreenDimensions;
    float2 circlePos = CircleCenter * ScreenDimensions;

    // Calculate distance from the center of the circle
    float dist = distance(screenPos, circlePos);

    // If outside the circle, darken the color
    if (dist > CircleRadius)
    {
        color.rgb *= 0.2; // Darken factor, adjust as needed
    }

    return color;
}

technique CircleMask
{
    pass P0
    {
        PixelShader = compile ps_2_0 MainPS();
    }
}
