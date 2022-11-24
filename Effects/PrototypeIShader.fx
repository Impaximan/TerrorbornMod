sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
sampler uImage2 : register(s2);
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
    float distanceMultiplier = (distance(coords, float2(0.5, 0.5)) - 0.2) * 0.1;
    float distanceMultiplierBlue = 1 - distance(coords, float2(0.5, 0.5)) * 2;
    
    if (distanceMultiplier < 0)
    {
        return float4(color.r, color.g, color.b * (1 + abs(distanceMultiplierBlue) * uOpacity), color.a);
    }
    else
    {
        return float4(color.r, color.g + abs(distanceMultiplier) * uOpacity, color.b * (1 + abs(distanceMultiplierBlue) * uOpacity), color.a);
    }
}

technique Technique1
{
    pass PrototypeI
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}