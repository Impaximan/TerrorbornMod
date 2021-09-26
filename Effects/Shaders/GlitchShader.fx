sampler uImage0 : register(s0);
sampler uImage1 : register(s1); // Automatically Images/Misc/Perlin via Force Shader testing option
sampler uImage2 : register(s2); // Automatically Images/Misc/noise via Force Shader testing option
sampler uImage3 : register(s3);
float3 uColor;
float3 uSecondaryColor;
float2 uScreenResolution;
float2 uScreenPosition;
float2 uTargetPosition;
float2 uDirection;
float uOpacity;
float uTime;
float uIntensity;
float uProgress;
float2 uImageSize1;
float2 uImageSize2;
float2 uImageSize3;
float2 uImageOffset;
float uSaturation;
float4 uSourceRect;
float2 uZoom;

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    float4 color2 = tex2D(uImage0, float2(coords.x - uIntensity, coords.y));
    
    if (distance(coords.y, uTargetPosition.y) < 0.05)
    {
        if (uColor.r == 1)
        {
            return float4(color.r + color2.r, color.g, color.b, color.a);
        }
        else if (uColor.g == 1)
        {
            return float4(color.r, color.g + color2.g, color.b, color.a);
        }
        else
        {
            return float4(color.r, color.g, color.b + color2.b, color.a);
        }
    }
    else
    {
        return float4(color.r, color.g, color.b, color.a);
    }
}

technique Technique1
{
    pass Glitch
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}