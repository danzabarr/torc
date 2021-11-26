#version 330 core
in vec4 vertexColor;
in vec3 vertexNormal;
in vec2 texCoord;
in vec3 fragPos;
in mat3 TBN;
out vec4 result;

uniform vec4 ambient = vec4(0, 0, 0, 1);
uniform vec3 color = vec3(1, 1, 1);
uniform vec3 lightDir = vec3(0, 0, 0);
uniform vec3 lightColor = vec3(1, 1, 1);
uniform float specularStrength = 0.5;
uniform float specularPower = 32;
uniform vec3 viewPos;

layout(binding = 0) uniform sampler2D texture0;
layout(binding = 1) uniform sampler2D normalMap;

void main()
{
	vec3 direction = normalize(-lightDir);
	vec3 normal = vertexNormal;

	normal = texture(normalMap, texCoord).rgb;
	normal = normal * 2.0 - 1.0;   
	normal = normalize(TBN * normal); 

	float diffuse = max(dot(normal, -lightDir), 0.0);
	
	vec3 viewDir = normalize(viewPos - fragPos);
	vec3 reflectDir = reflect(-direction, normal);

	float specular = specularStrength * pow(max(dot(viewDir, reflectDir), 0.0), specularPower);

	vec3 lighting = (diffuse + specular) * lightColor;

	lighting = max(lighting, ambient.xyz * ambient.w);

	result = texture(texture0, texCoord) * vec4(lighting, 1);
}
