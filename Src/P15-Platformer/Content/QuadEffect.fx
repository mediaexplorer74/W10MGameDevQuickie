sampler TexSampler : register(s0)
{
	Texture = <Texture>;
	AddressU = clamp;
	AddressV = clamp;
	MinFilter = linear;
	MagFilter = linear;	
};

float4x4 MatrixTransform; 

struct VSOutput 
{
	float4 position : SV_Position;
	float4 color : COLOR0;
	float2 texCoord : TEXCOORD0;
};

VSOutput SpriteVertexShader(float4 position : SV_Position, float4 color : COLOR0, float2 texCoord : TEXCOORD0)
{
	VSOutput output; 	
	output.position = mul(position, MatrixTransform);
	output.color = color;
	output.texCoord = texCoord;
	return output;
}

float4 SpritePixelShader(VSOutput input) : SV_Target0
{
	return tex2D(TexSampler, input.texCoord) * input.color;
}


technique QuadBatch {
	pass 
	{
		VertexShader = compile vs_3_0 SpriteVertexShader();
		PixelShader = compile ps_3_0 SpritePixelShader();
	}
};