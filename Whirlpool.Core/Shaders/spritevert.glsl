#version 450
//----------------------------------------
layout (location = 0) in vec3 inVertexPos;
layout (location = 1) in vec2 _inTexCoord;
//----------------------------------------
out vec4 outVertexPos;
out vec2 outTexCoord;
//----------------------------------------
uniform vec2 position;
uniform vec2 size;
uniform float rotation;
uniform float time;
//----------------------------------------

void main() {
    outVertexPos = vec4(inVertexPos, 1.0); 
	outTexCoord = _inTexCoord;
	vec4 _inVertexPos = vec4(inVertexPos, 1.0);
	_inVertexPos = vec4(_inVertexPos.x * size.x, _inVertexPos.y * size.y, _inVertexPos.z, 1.0);
    _inVertexPos = vec4(((_inVertexPos.x * cos(rotation)) - (_inVertexPos.y * sin(rotation))), 
		((_inVertexPos.x * sin(rotation)) + (_inVertexPos.y * cos(rotation))), _inVertexPos.z, 1.0);
	_inVertexPos = vec4(_inVertexPos.x + size.x, _inVertexPos.y - size.y, _inVertexPos.z, 1.0);
	_inVertexPos = vec4(_inVertexPos.x - position.x, _inVertexPos.y - position.y, _inVertexPos.z, 1.0);
	gl_Position = _inVertexPos;
}