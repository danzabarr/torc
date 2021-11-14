#version 330 core
in vec4 vertexColor;
in vec3 vertexNormal;
in vec2 texCoord;
in vec3 fragPos;
out vec4 result;

uniform sampler2D texture0;
uniform vec3 color = vec3(1, 1, 1);
uniform vec3 lightDir = vec3(0, 0, 0);
uniform vec3 lightColor = vec3(1, 1, 1);
uniform float specularStrength = 0.5;
uniform vec3 viewPos;

void main()
{
	vec3 direction = normalize(-lightDir);
	vec3 normal = normalize(vertexNormal);
	float diffuse = max(dot(normal, lightDir), 0.0);
	
	vec3 viewDir = normalize(viewPos - fragPos);
	vec3 reflectDir = reflect(-direction, normal);

	float specular = specularStrength * pow(max(dot(viewDir, reflectDir), 0.0), 32);

	vec3 lighting = (diffuse + specular) * lightColor;

	result = texture(texture0, texCoord) * vec4(lighting, 1);
}
