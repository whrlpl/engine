#version 450
//----------------------------------------
in vec4 outVertexPos;
in vec2 outTexCoord;
//----------------------------------------
layout(location = 0) out vec4 frag_color;
//----------------------------------------
uniform float textureRepetitions;
uniform sampler2D albedoTexture;
uniform float opacity;
uniform vec4 tint;
uniform bool flipX;
uniform bool flipY;
uniform bool atlas;
uniform vec2 atlasPoint;
uniform vec2 atlasSize;
//----------------------------------------

void main() {
	vec2 point = vec2(((flipX) ? 1 - outTexCoord.x : outTexCoord.x) * textureRepetitions, 
			((flipY) ? 1 - outTexCoord.y : outTexCoord.y) * textureRepetitions);

	float opacity_ = round(opacity * 32) / 32;

	if (atlas)
	{
		frag_color = texture(albedoTexture, (atlasPoint + point) * atlasSize) * tint;
		frag_color.w = opacity;
	}
	else
	{
		frag_color = texture(albedoTexture, point) * tint;
		frag_color.w = opacity;
	}
}