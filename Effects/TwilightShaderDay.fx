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
    float4 Color = 0.0f;
    float blurSizeX = (sin(uTime * 10) * 0.0025) * uOpacity * distance(coords, float2(0.5, 0.5));
    Color += tex2D(uImage0, float2(coords.x, coords.y - 3.0 * blurSizeX)) * 0.09f;
    Color += tex2D(uImage0, float2(coords.x, coords.y - 2.0 * blurSizeX)) * 0.11f;
    Color += tex2D(uImage0, float2(coords.x, coords.y - blurSizeX)) * 0.18f;
    Color += tex2D(uImage0, coords) * 0.24;
    Color += tex2D(uImage0, float2(coords.x, coords.y + blurSizeX)) * 0.18f;
    Color += tex2D(uImage0, float2(coords.x, coords.y + 2.0 * blurSizeX)) * 0.11f;
    Color += tex2D(uImage0, float2(coords.x, coords.y + 3.0 * blurSizeX)) * 0.09f;
    float distanceMultiplier = (distance(coords, float2(0.5, 0.5))) * 0.7;
    Color = float4(Color.r - abs(distanceMultiplier) * uOpacity, Color.g - abs(distanceMultiplier) * uOpacity, Color.b - abs(distanceMultiplier) * uOpacity, Color.a - abs(distanceMultiplier) * uOpacity);
    Color = lerp(Color, float4(0.56, 0.35, 0.61, 0.86), distance(coords, float2(0.5, 0.5)) * 1.1 * uOpacity);
    return Color;
}

technique Technique1
{
    pass TwilightShaderDay
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}