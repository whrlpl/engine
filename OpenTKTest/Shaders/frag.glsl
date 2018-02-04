#version 450
//----------------------------------------
layout(origin_upper_left) in vec4 gl_FragCoord;
//----------------------------------------
in vec4 outVertexPos;
in vec2 outTexCoord;
//----------------------------------------
out vec4 frag_color;
//----------------------------------------
uniform float textureRepetitions;
uniform sampler2D albedoTexture;
uniform vec4 tint;
//----------------------------------------

void main() {
	//frag_color = texture(albedoTexture, vec2(gl_FragCoord.x / 1280, gl_FragCoord.y / 720));
	vec4 fakeLighting = vec4((sin(outVertexPos.z))/3);
	fakeLighting.w = 1.0;
	frag_color = texture(albedoTexture, vec2(outTexCoord.x * textureRepetitions, outTexCoord.y * textureRepetitions)) + fakeLighting;
}