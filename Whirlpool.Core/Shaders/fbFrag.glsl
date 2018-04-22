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

void main() {
	vec4 calculatedBlur = vec4(0, 0, 0, 0);
	vec2 pos = vec2(outTexCoord.x, outTexCoord.y);
	int size = 4;
	int strength = 16;

	for (int x = -size; x <= size; ++x)
	{
		for (int y = -size; y <= size; ++y)
		{
			calculatedBlur += texture(renderedTexture, vec2(pos.x + x * strength, pos.y + y * strength)) / pow(size * 2, 2);
		}
	}

	frag_color = vec4(calculatedBlur.xyz, 1.0);
	//frag_color = texture(renderedTexture, vec2(outTexCoord.x, outTexCoord.y)) * vec4(0.5, 1, 2.0, 1);
}

