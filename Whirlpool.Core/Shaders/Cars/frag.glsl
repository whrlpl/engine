#version 450
//----------------------------------------
layout(origin_upper_left) in vec4 gl_FragCoord;
//----------------------------------------
flat in vec4 outVertexPos;
in vec3 outNormal;
in vec3 outFragPos;
in vec2 outTexCoord;
in vec4 debugCol;
//----------------------------------------
layout(location = 0) out vec4 frag_color;
//----------------------------------------
uniform float textureRepetitions;
uniform sampler2D albedoTexture;
uniform sampler2D decalTexture;
uniform sampler2D reflectionTexture;
uniform vec4 tint;
uniform mat4 mvp;
uniform mat4 model;
uniform mat4 vp;
uniform vec3 mainLightPos;
uniform vec4 mainLightTint;
uniform float time;
uniform vec3 position;
//----------------------------------------

void main() {
	float ambientLightStrength = 1/2;
	vec3 ambientBase = ambientLightStrength * mainLightTint.xyz;
	vec4 normalizedNormal = vec4(normalize(outNormal), 1);
	vec4 lightDirection = normalize(vec4(mainLightPos, 1) - vec4(outFragPos.xyz, 1));

	float diffuseBase = max(dot(normalizedNormal, lightDirection), 0.0);
	vec3 diffuseLight = diffuseBase * tint.xyz;
	vec3 ambientLight = ambientBase * tint.xyz;

	vec3 result = texture(albedoTexture, outTexCoord).xyz;
	
	vec4 decalCol = texture(decalTexture, outTexCoord);
	if (decalCol.w > 0)
		result = mix(result.xyz, decalCol.xyz, decalCol.w);

	result = max((ambientBase + diffuseBase), 0.2) * result;

	vec4 mvpTex = mvp * vec4(outTexCoord, 1.0, 1.0);

	frag_color = vec4(mix(result, texture(reflectionTexture, vec2(mvpTex.x + (position.z / 500), mvpTex.y + (-position.x / 500) + 5) * 5).xyz, 0.1), 1.0);
}

