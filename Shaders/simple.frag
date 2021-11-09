#version 330 core
in vec4 vertexColor;
out vec4 result;

uniform vec3 color = vec3(1, 1, 1);

void main()
{
	result = vec4(color, 1.0) * vertexColor;
}