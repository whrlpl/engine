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
uniform sampler2D AlbedoTexture;
uniform sampler2D DecalTexture;
uniform sampler2D ReflectionTexture;
uniform mat4 Model;
uniform mat4 View;
uniform mat4 Projection;
uniform mat4 MVP;
uniform vec3 MainLightPos;
uniform vec4 MainLightTint;
uniform vec3 Position;
//----------------------------------------

void main() {
	float ambientLightStrength = 1/2;
	vec3 ambientBase = ambientLightStrength * MainLightTint.xyz;
	vec4 normalizedNormal = vec4(normalize(outNormal), 1);
	vec4 lightDirection = normalize(vec4(MainLightPos, 1) - vec4(outFragPos.xyz, 1));

	float diffuseBase = max(dot(normalizedNormal, lightDirection), 0.0);
	vec3 diffuseLight = diffuseBase * MainLightTint.xyz;
	vec3 ambientLight = ambientBase * MainLightTint.xyz;

	vec3 result = texture(AlbedoTexture, outTexCoord).xyz;
	
	vec4 decalCol = texture(DecalTexture, outTexCoord);
	if (decalCol.w > 0)
		result = mix(result.xyz, decalCol.xyz, decalCol.w);

	result = max((ambientBase + diffuseBase), 0.2) * result;

	vec4 mvpTex = MVP * vec4(outTexCoord, 1.0, 1.0);

	frag_color = vec4(mix(result, texture(ReflectionTexture, vec2(mvpTex.x + (Position.z / 500), mvpTex.y + (-Position.x / 500) + 5) * 5).xyz, 0.1), 1.0);
	//frag_color = texture(AlbedoTexture, outTexCoord);
}

