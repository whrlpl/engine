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
uniform mat4 Model;
uniform mat4 View;
uniform mat4 Projection;
uniform mat4 MVP;
uniform vec3 MainLightPos;
uniform vec4 MainLightTint;
uniform float AmbientLightStrength;
uniform vec3 Position;
//----------------------------------------

void main() {
	vec3 ambientBase = AmbientLightStrength * MainLightTint.xyz;
	vec4 normalizedNormal = vec4(normalize(outNormal), 1);
	vec4 lightDirection = normalize(vec4(MainLightPos, 1) - vec4(outFragPos.xyz, 1));

	float diffuseBase = max(dot(normalizedNormal, lightDirection), 0.0);
	vec3 diffuseLight = diffuseBase * MainLightTint.xyz;
	vec3 ambientLight = ambientBase * MainLightTint.xyz;

	vec3 result = max((ambientBase + diffuseBase), AmbientLightStrength) * texture(AlbedoTexture, outTexCoord).xyz;

	vec4 mvpTex = MVP * vec4(outTexCoord, 1.0, 1.0);

	frag_color = vec4(result, 1.0);
}

