#version 330 core
layout (location = 0) in vec3 pos;
layout (location = 1) in vec3 normal;
layout (location = 2) in vec4 color;
layout (location = 3) in vec2 uv;

smooth out vec4 vertexColor;

uniform mat4 model;
uniform mat4 view;
uniform mat4 proj;

void main()
{
	gl_Position = proj * view * model * vec4(pos, 1.0);
	vertexColor = color;
}