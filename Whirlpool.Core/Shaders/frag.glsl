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
//----------------------------------------

void main() {
	//frag_color = texture(albedoTexture, vec2(gl_FragCoord.x / 1280, gl_FragCoord.y / 720));
	vec4 vertexCalc = outVertexPos * mvp;
	//vec4 fakeLighting = vec4((sin(vertexCalc.z-vertexCalc.x))/6);
	vec4 fakeLighting = vec4((sin(vertexCalc.z)));
	//vec2 temp = vec2(outVertexPos.x + outVertexPos.z / 2, outVertexPos.y + outVertexPos.z / 2) * mvp;
	vec2 calcTexCoord = vec2((outVertexPos.x + 1) / 2, (outVertexPos.y + 1) / 2);
	fakeLighting.w = 1.0;
	//frag_color = texture(albedoTexture, vec2(calcTexCoord.x * textureRepetitions, calcTexCoord.y * textureRepetitions)) + fakeLighting;
	//frag_color = vec4(calcTexCoord.x * textureRepetitions, calcTexCoord.y * textureRepetitions, 0, 1);
	//frag_color = vec4(outTexCoord.x, outTexCoord.y, 0, 1.0);
	//frag_color = (texture(albedoTexture, vec2(outTexCoord.x * textureRepetitions, outTexCoord.y * textureRepetitions)) + fakeLighting) * tint;
	//frag_color = texture(albedoTexture, vec2(outTexCoord.x * textureRepetitions, outTexCoord.y * textureRepetitions)) * tint;
	float ambientLightStrength = 0.1;
	vec3 ambientBase = ambientLightStrength * tint.xyz;
	vec3 normalizedNormal = normalize(outNormal);
	vec3 lightDirection = normalize(mainLightPos - outFragPos);

	float diffuseBase = max(dot(normalizedNormal, lightDirection), 0.0);
	vec3 diffuseLight = diffuseBase * tint.xyz;
	vec3 ambientLight = ambientBase * tint.xyz;

	vec3 result = (ambientBase + diffuseBase) * tint.xyz;
	frag_color = vec4(result, 1.0);
	frag_color = vec4(1, 1, 1, 1);
}

