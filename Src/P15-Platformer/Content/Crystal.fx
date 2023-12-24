sampler TexSamp : register(s0)     // what is being drawn
{
	Filter = Linear;
	AddressU = Clamp;
	AddressV = Clamp;
};

sampler ReflectSamp : register(s1) // sprites to reflect 
{
	Texture = (ReflectMap);
	Filter = Linear;
	AddressU = Clamp;
	AddressV = Clamp;
};

sampler BackSamp : register(s2)   // background to refract
{
	Texture = (BackMap);
	Filter = Linear;
	AddressU = Wrap;
	AddressV = Wrap;
};


float2 screen; 

static const float scale          = 0.79;	// 0.1 = big, 0.9 = small (1-0 instead of 0-1 because of extraction method)
static const float distort_amount = 0.007;  // maybe 0.001 to 0.04
static const float visibility     = 0.7;    // visibility of reflection



//--------------------------------------
// C R Y S T A L  P I X E L  S H A D E R
//--------------------------------------
float4 CrystalPS(float4 pos : SV_Position, float4 col : COLOR0, float2 uv : TEXCOORD0) : COLOR0
{
	float2 norm_uv = uv;
	norm_uv.x += 0.125; // 128 pixels to the right on tiles_image(which has 1024 total width) so 128/1024 = 0.125

	float2 reflect_uv;
	reflect_uv.x = pos.x / screen.x * scale + 0.05; 
	reflect_uv.y = pos.y / screen.y * scale + 0.10;
	
	// r=0-1, g=0-1 and say red = 0.4 and green = 0.7 then distort.x = -0.2 and distort.y = 0.4 (ranging -1 to +1) This is why: *2 - 1
	// multiply final result by magnitude to get it in terms of a small space of distortion in screen/uv coords (0-1) 
	float2 distort = (tex2D(TexSamp, norm_uv).xy*2.0 - 1.0)*distort_amount;

	float4 TexColor = tex2D(TexSamp, uv); // color of pixel to draw from crystals  	

	reflect_uv.xy += distort.xy;          // change reflection source by distortion amount

	float4 sparkle = tex2D(BackSamp, (reflect_uv + distort * 6)); // distort the background source (or an appropriate texture) a lot more to make surface sparkle 
    if (TexColor.a > 0.5) TexColor += sparkle;                    // if visible add-blend sparkle color 
	float4 Reflect = tex2D(ReflectSamp, reflect_uv);              // alien hero pixel to reflect
	if ((TexColor.a > 0.5) && (Reflect.a > 0.9)) {                // if both pixels should be visible (crystal + reflection): 
		TexColor = Reflect*visibility + (TexColor+sparkle)*(1 - visibility); // remake the color with reflection mixed in proportionally
	}

	return TexColor*col;
}



technique CrystalShader {
	pass 
	{		
		PixelShader = compile ps_3_0 CrystalPS();
	}
};