#version 450
//----------------------------------------
in vec4 outVertexPos;
in vec2 outTexCoord;
//----------------------------------------
layout(location = 0) out vec4 frag_color;
//----------------------------------------
uniform float textureRepetitions;
uniform sampler2D albedoTexture;
uniform vec4 tint;
uniform bool flipX;
uniform bool flipY;
//----------------------------------------

void main() {
	vec4 color = texture(albedoTexture, vec2(((flipX) ? -outTexCoord.x : outTexCoord.x) * textureRepetitions, 
			((flipY) ? -outTexCoord.y : outTexCoord.y) * textureRepetitions)) * tint;
	frag_color = color;
}