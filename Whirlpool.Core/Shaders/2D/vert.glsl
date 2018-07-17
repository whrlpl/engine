#version 450
//----------------------------------------
layout (location = 0) in vec3 inVertexPos;
layout (location = 1) in vec2 _inTexCoord;
//----------------------------------------
out vec4 outVertexPos;
out vec2 outTexCoord;
//----------------------------------------
uniform vec2 Position;
uniform vec2 Scale;
uniform float Rotation;
//----------------------------------------

void main() {
    outVertexPos = vec4(inVertexPos, 1.0); 
	outTexCoord = _inTexCoord;
	vec4 _inVertexPos = vec4(inVertexPos, 1.0);
	_inVertexPos = vec4(_inVertexPos.x * Scale.x, _inVertexPos.y * Scale.y, _inVertexPos.z, 1.0);
    _inVertexPos = vec4(((_inVertexPos.x * cos(Rotation)) - (_inVertexPos.y * sin(Rotation))), 
		((_inVertexPos.x * sin(Rotation)) + (_inVertexPos.y * cos(Rotation))), _inVertexPos.z, 1.0);
	_inVertexPos = vec4(_inVertexPos.x + Scale.x, _inVertexPos.y - Scale.y, _inVertexPos.z, 1.0);
	_inVertexPos = vec4(_inVertexPos.x - Position.x, _inVertexPos.y - Position.y, _inVertexPos.z, 1.0);
	gl_Position = _inVertexPos;
}