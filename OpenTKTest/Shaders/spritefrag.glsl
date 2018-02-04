#version 450
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
	frag_color = texture(albedoTexture, vec2(outTexCoord.x * textureRepetitions, outTexCoord.y * textureRepetitions));
}