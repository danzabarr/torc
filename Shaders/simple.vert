#version 330 core
layout (location = 0) in vec3 pos;
layout (location = 1) in vec3 normal;
layout (location = 2) in vec4 color;
layout (location = 3) in vec2 uv;
layout (location = 4) in vec3 tangent;
layout (location = 5) in vec3 bitangent;

out vec4 vertexColor;
out vec3 vertexNormal;
out vec2 texCoord;
out vec3 fragPos;
out mat3 TBN;

uniform mat4 model;
uniform mat4 view;
uniform mat4 proj;

void main()
{
	gl_Position = proj * view * model * vec4(pos, 1.0);
	fragPos = vec3(model * vec4(pos, 1.0));
	vertexColor = color;
	vertexNormal = normalize(vec3(model * vec4(normal, 0)));

	vec3 T = normalize(vec3(model * vec4(tangent, 0)));
	vec3 B = normalize(vec3(model * vec4(bitangent, 0)));
	vec3 N = normalize(vec3(model * vec4(normal, 0)));
	TBN = mat3(T, B, N);

	//vertexNormal = normalize(mat3(transpose(inverse(model))) * normal);

	texCoord = uv;
}