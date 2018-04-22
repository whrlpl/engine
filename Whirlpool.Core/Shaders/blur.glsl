#version 450
//----------------------------------------
layout(origin_upper_left) in vec4 gl_FragCoord;
//----------------------------------------
layout(location = 0) out vec4 frag_color;
//----------------------------------------
uniform sampler2D screenTexture;
//uniform sampler2D blurTexture;
uniform vec2 screenSize;
//----------------------------------------

void main() {
	//vec4 calculatedBlur = vec4(0, 0, 0, 0);
	//vec2 pos = vec2(gl_FragCoord.x / screenSize.x, gl_FragCoord.y / screenSize.y)
	//int strength = 6;

	//for (int x = -strength; x <= strength; ++x)
	//{
	//	for (int y = -strength; y <= strength; ++y)
	//	{
	//		calculatedBlur += texture(screenTexture, vec2(pos.x + x, pos.y + y));
	//	}
	//}

	//frag_color = calculatedBlur / pow(strength, 2);
	frag_color = texture(screenTexture, vec2(gl_FragCoord.x / 1280, gl_FragCoord.y / 720));
}

