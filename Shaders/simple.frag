#version 330 core
in vec4 vertexColor;
in vec3 vertexNormal;
in vec2 texCoord;
in vec3 fragPos;
in mat3 TBN;
in vec4 FragPosLightSpace;
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
layout(binding = 2) uniform sampler2D shadowMap;

float ShadowCalculation(vec4 fragPosLightSpace)
{
    // perform perspective divide
    vec3 projCoords = fragPosLightSpace.xyz / fragPosLightSpace.w;
    // transform to [0,1] range
    projCoords = projCoords * 0.5 + 0.5;
    // get closest depth value from light's perspective (using [0,1] range fragPosLight as coords)
    float closestDepth = texture(shadowMap, projCoords.xy).r; 
    // get depth of current fragment from light's perspective
    float currentDepth = projCoords.z;
    // check whether current frag pos is in shadow
    float shadow = currentDepth > closestDepth  ? 1.0 : 0.0;

    return shadow;
}  

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

	float shadow = ShadowCalculation(FragPosLightSpace);       
	vec3 lighting = (ambient.rgb * ambient.w + (1.0 - shadow) * (diffuse + specular)) * lightColor;    

	result = texture(texture0, texCoord) * vec4(lighting, 1);
}
