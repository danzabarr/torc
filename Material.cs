using System.Collections.Generic;
using GlmNet;
using static OpenGL.GL;
using System.IO;
using System;

namespace torc
{
    public class Material
    {
        public Shader shader;
        public Texture[] textures;
        private readonly Dictionary<string, object> properties = new();

        public Texture mainTexture;
        public Texture normalMap;


        public Material(Shader shader)
        {
            this.shader = shader;
        }

        public void Use()
        {
            shader.Use();

            glActiveTexture(GL_TEXTURE0);
            if (mainTexture != null)
            {
                glBindTexture(GL_TEXTURE_2D, mainTexture.id);
            }
            else
            {
                glBindTexture(GL_TEXTURE_2D, Texture.White.id);
            }

            glActiveTexture(GL_TEXTURE1);
            if (normalMap != null)
            {
                glBindTexture(GL_TEXTURE_2D, normalMap.id);
            }
            else
            {
                glBindTexture(GL_TEXTURE_2D, Texture.BlankNormal.id);
            }

            glActiveTexture(GL_TEXTURE2);
            glBindTexture(GL_TEXTURE_2D, DirectionalLight.main.DepthMap);

            glActiveTexture(GL_TEXTURE0);
            //if (textures != null) for (int i = 0; i < textures.Length; i++)
            //{
            //    glActiveTexture(i);
            //    glBindTexture(GL_TEXTURE_2D, textures[i].id);
            //}
        }

        public int GetUniformLocation(string name)
        {
            return shader.GetUniformLocation(name);
        }

        public void SetProperty(string name, object value)
        {
            properties[name] = value;
        }

        public void RemoveProperty(string name)
        {
            properties.Remove(name);
        }

        public void UploadProperties()
        {
            foreach (KeyValuePair<string, object> p in properties)
            {
                string key = p.Key;
                object value = p.Value;

                int location = glGetUniformLocation(shader.id, key);

                switch (value)
                {
                    case float:
                        float fValue = (float) value;
                        glUniform1f(location, fValue);
                        break;

                    case vec3:
                        vec3 v3Value = (vec3) value;
                        glUniform3f(location, v3Value.x, v3Value.y, v3Value.z);
                        break;

                    case vec4:
                        vec4 v4Value = (vec4)value;
                        glUniform4f(location, v4Value.x, v4Value.y, v4Value.z, v4Value.w);
                        break;

                    case mat4:
                        mat4 m4Value = (mat4) value;
                        glUniformMatrix4fv(location, 1, false, m4Value.to_array());
                        break;

                    default:
                        Console.WriteLine("Unsupported uniform type: " + value.GetType().ToString());
                        break;
                }
            }
        }
        
        public void UniformAmbientLight(vec4 color)
        {
            shader.UniformAmbientLight(color);
        }

        public void UniformLight(Light light)
        {
            shader.UniformLight(light);
        }

        public void UniformLight(vec3 direction, vec3 color, float brightness, float specularStrength, float specularPower)
        {
            shader.UniformLight(direction, color, brightness, specularStrength, specularPower);

            shader.UniformMatrix4fv("lightSpaceMatrix", DirectionalLight.main.LightSpaceMatrix);
        }

        public void UniformMatrices(Camera camera, mat4 model)
        {
            shader.UniformMatrices(camera, model);
        }

        public void UniformMatrices(vec3 viewPos, mat4 model, mat4 view, mat4 proj)
        {
            shader.UniformMatrices(viewPos, model, view, proj);
        }
    }
}
