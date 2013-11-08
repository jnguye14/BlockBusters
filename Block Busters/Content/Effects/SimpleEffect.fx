float4x4 World;
float4x4 View;
float4x4 Projection;

float3 LightPosition;
float3 CameraPosition;
float Shininess;

// TODO: add effect parameters here.

struct VertexShaderInput
{
    float4 Position : POSITION0;
	float3 Normal : NORMAL0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
	float4 Color : COLOR0;
	float3 WorldPosition : TEXCOORD0;
	float3 Normal : TEXCOORD1;
};

VertexShaderOutput GouraudVertex(VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);
	output.WorldPosition = worldPosition;
	output.Normal = input.Normal;

    // TODO: add your vertex shader code here.
	float3 L = normalize(LightPosition - worldPosition);
	float3 V = normalize(CameraPosition - worldPosition);
	float3 N = normalize(input.Normal);
	float3 R = -reflect(L,N);

	output.Color = max(dot(N,L),0) + pow(max(dot(R,V),0),Shininess);

    return output;
}

float4 GouraudPixel(VertexShaderOutput input) : COLOR0
{
    return float4(0.2,0.2,0.2,1) + input.Color;
}

VertexShaderOutput PhongVertex(VertexShaderInput input)
{
    VertexShaderOutput output;
    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);
	output.WorldPosition = worldPosition;
	output.Color = float4(0,0,0,1);
	output.Normal = input.Normal;
    // TODO: add your vertex shader code here.
    return output;
}
float4 PhongPixel(VertexShaderOutput input) : COLOR0
{
	float3 L = normalize(LightPosition - input.WorldPosition);
	float3 V = normalize(CameraPosition - input.WorldPosition);
	float3 N = normalize(input.Normal);
	float3 R = -reflect(L,N);
	return float4(0.2,0.2,0.2,1) + 
			max(dot(N,L),0) + pow(max(dot(R,V),0), 20);
}
float4 BlinnPixel(VertexShaderOutput input) : COLOR0
{
	float3 L = normalize(LightPosition - input.WorldPosition);
	float3 V = normalize(CameraPosition - input.WorldPosition);
	float3 N = normalize(input.Normal);
	return float4(0.2,0.2,0.2,1) + 
			max(dot(N,L),0) + pow(dot(normalize(L + V),V), 20);
}
float4 SchlickPixel(VertexShaderOutput input) : COLOR0
{
	float3 L = normalize(LightPosition - input.WorldPosition);
	float3 V = normalize(CameraPosition - input.WorldPosition);
	float3 N = normalize(input.Normal);
	float3 R = -reflect(L,N);
	float t = dot(R,V);
	return float4(0.2,0.2,0.2,1) + 
			max(dot(N,L),0) + t / (t + Shininess - t*Shininess);
}

technique Gouraud
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 GouraudVertex();
        PixelShader = compile ps_2_0 GouraudPixel();
    }
}
technique Phong
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 PhongVertex();
        PixelShader = compile ps_2_0 PhongPixel();
    }
}

technique Blinn
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 PhongVertex();
        PixelShader = compile ps_2_0 BlinnPixel();
    }
}
technique Schlick
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 PhongVertex();
        PixelShader = compile ps_2_0 SchlickPixel();
    }
}