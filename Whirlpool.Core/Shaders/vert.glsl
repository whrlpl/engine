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
//----------------------------------------
uniform vec2 position;
uniform vec2 size;
uniform float rotation;
uniform mat4 vp;
uniform mat4 model;
uniform float time;
//----------------------------------------

float random (vec2 st) {
    return fract(sin(dot(st.xy,
                         vec2(12.9898,78.233)))*
        43758.5453123);
}

float rnd(float n) {
	return noise1(n + inVertexPos.x + inVertexPos.y + inVertexPos.z) / 10;
	//return mod(n, 10) * (inVertexPos.x + inVertexPos.y) / 10;
}

void main() {
    outVertexPos = vec4(inVertexPos, 1.0); 
	outTexCoord = _inTexCoord;
	outNormal = inNormal;
	outFragPos = vec3(model * vec4(inVertexPos, 1.0));
	mat4 mvp = model * vp;

	gl_Position = vec4(inVertexPos.xyz + (vec3(random(vec2(outFragPos.x, outFragPos.y + outFragPos.z)) / 4)), 1.0) * mvp;
}