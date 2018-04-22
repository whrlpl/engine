#version 450
//----------------------------------------
layout(origin_upper_left) in vec4 gl_FragCoord;
//----------------------------------------
flat in vec4 outVertexPos;
in vec2 outTexCoord;
//----------------------------------------
layout(location = 0) out vec4 frag_color;
//----------------------------------------
uniform vec4 color1;
uniform vec4 color2;
//----------------------------------------

vec4 lerp(vec4 a, vec4 b, float pos) {
	// Find the difference between the two
	vec4 diff = vec4(a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);
	// Then divide diff by 100, then multiply that by pos
	return vec4((diff.x / 100) * pos, (diff.y / 100) * pos, (diff.z / 100) * pos, (diff.w / 100) * pos);
}

void main() {
	// Lerp between color1 and color2
	frag_color = lerp(color1, color2, outTexCoord.x);
}


