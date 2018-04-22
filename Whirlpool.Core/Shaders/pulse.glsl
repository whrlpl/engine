#version 450
//----------------------------------------
in vec3 outVertexPos;
in vec2 outTexCoord;
//----------------------------------------
layout(location = 0) out vec4 frag_color;
//----------------------------------------
uniform float textureRepetitions;
uniform sampler2D albedoTexture;
uniform vec4 tint;
uniform float time;
//----------------------------------------

void main() {
	vec4 color = texture(albedoTexture, vec2(outTexCoord.x * textureRepetitions, outTexCoord.y * textureRepetitions)) * ((sin(time * 8) + 4) / 8);
	// Normalize values
	//vec4 value = ((color.x + color.y + color.z) / 3) * tint;
	//float alpha = (outTexCoord.x - outTexCoord.y) / 2;
	vec4 value = color * tint;
	//color.w = 1;
	frag_color = color;
	//frag_color = value;
}