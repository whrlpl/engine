#version 450
//----------------------------------------
layout(origin_upper_left) in vec4 gl_FragCoord;
//----------------------------------------
flat in vec4 outVertexPos;
in vec2 outTexCoord;
//----------------------------------------
out vec4 frag_color;
//----------------------------------------
uniform sampler2D renderedTexture;
//----------------------------------------

// modified from https://github.com/Jam3/glsl-fast-gaussian-blur/blob/master/5.glsl
vec4 blur(sampler2D image, vec2 uv, vec2 resolution, int strength) {
	vec4 avg = vec4(0.0);
	for (int x = -strength; x <= strength; x++)
	{
		for (int y = -strength; y <= strength; y++)
		{
			vec4 color = vec4(0.0);
			vec2 off1 = vec2(1.3333333333333333) * vec2(x, y);
			color += texture2D(image, uv) * 0.29411764705882354;
			color += texture2D(image, uv + (off1 / resolution)) * 0.35294117647058826;
			color += texture2D(image, uv - (off1 / resolution)) * 0.35294117647058826;
			avg += color;
		}
	}
	return avg / 176;
}

void main() {
	//if (outTexCoord.x < 0.5)
	if (false)
	{
		frag_color = blur(renderedTexture, outTexCoord.xy, vec2(1280, 720), 5);
	}
	else
	{
		frag_color = texture(renderedTexture, vec2(outTexCoord.x, outTexCoord.y));
	}
}

