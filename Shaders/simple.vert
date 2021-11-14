﻿#version 330 core
layout (location = 0) in vec3 pos;
layout (location = 1) in vec3 normal;
layout (location = 2) in vec4 color;
layout (location = 3) in vec2 uv;

out vec4 vertexColor;
out vec3 vertexNormal;
out vec2 texCoord;
out vec3 fragPos;

uniform mat4 model;
uniform mat4 view;
uniform mat4 proj;

void main()
{
	gl_Position = proj * view * model * vec4(pos, 1.0);
	fragPos = vec3(model * vec4(pos, 1.0));
	vertexColor = color;
	vertexNormal = vec3(proj * view * model * vec4(normal, 1.0));
	texCoord = uv;
}