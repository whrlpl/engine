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
uniform mat4 Projection;
uniform mat4 View;
uniform mat4 Model;
uniform mat4 MVP;
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

	vec4 calcVertPos = MVP * vec4(inVertexPos, 1.0);
	outFragPos = vec3(Model * vec4(inVertexPos, 1.0));

	gl_Position = MVP * vec4(inVertexPos, 1.0);
	debugCol = vec4(gl_Position.xyz, 1.0);
}