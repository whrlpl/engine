#version 450
//----------------------------------------
layout (location = 0) in vec3 inVertexPos;
layout (location = 1) in vec2 _inTexCoord;
layout (location = 2) in vec3 inNormal;
//----------------------------------------
out vec4 outVertexPos;
out vec3 outNormal;
out vec2 outTexCoord;
out vec3 outFragPos;
out vec4 debugCol;
//----------------------------------------
uniform vec2 position;
uniform vec2 size;
uniform float rotation;
uniform mat4 mvp;
uniform mat4 model;
uniform float time;
//----------------------------------------

float random (vec2 st) {
    return fract(sin(dot(st.xy,
                         vec2(12.9898,78.233)))*
        43758.5453123);
}

void main() {
    outVertexPos = vec4(inVertexPos, 1.0); 
	outTexCoord = _inTexCoord;
	outNormal = inNormal;
	vec4 calcVertPos = mvp * vec4(inVertexPos, 1.0);

	outFragPos = vec3(model * vec4(inVertexPos, 1.0));

	gl_Position = mvp * vec4(inVertexPos + vec3(random(calcVertPos.xy) / 8), 1.0);

	//gl_Position = mvp * (vec4(vec3(random(outFragPos.xy)), 1.0) + vec4(inVertexPos.xyz, 1.0));
	//gl_Position = mvp * vec4(inVertexPos.xyz, 1.0);
	debugCol = vec4(gl_Position.xyz, 1.0);
}