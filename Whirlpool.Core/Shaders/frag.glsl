#version 450
//----------------------------------------
layout(origin_upper_left) in vec4 gl_FragCoord;
//----------------------------------------
flat in vec4 outVertexPos;
in vec3 outNormal;
in vec3 outFragPos;
in vec2 outTexCoord;
//----------------------------------------
layout(location = 0) out vec4 frag_color;
//----------------------------------------
uniform float textureRepetitions;
uniform sampler2D albedoTexture;
uniform vec4 tint;
uniform mat4 mvp;
uniform vec3 mainLightPos;
uniform vec4 mainLightTint;
//----------------------------------------

void main() {
	float ambientLightStrength = 1/3;
	vec3 ambientBase = ambientLightStrength * mainLightTint.xyz;
	vec4 normalizedNormal = vec4(normalize(outNormal), 1);
	vec4 lightDirection = normalize(vec4(mainLightPos, 1) - (mvp * vec4(outFragPos, 1)));

	float diffuseBase = max(dot(normalizedNormal, lightDirection), 0.0);
	vec3 diffuseLight = diffuseBase * tint.xyz;
	vec3 ambientLight = ambientBase * tint.xyz;

	vec3 result = (ambientBase + diffuseBase) * texture(albedoTexture, outTexCoord).xyz;
	frag_color = vec4(result, 1.0);
	//frag_color = texture(albedoTexture, outTexCoord);
}

