using System.Collections.Generic;
using GlmNet;
using static OpenGL.GL;
using System.IO;
using System;

namespace torc
{
    class Material
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
        
        public void UniformAmbientLight()
        {
            shader.Uniform4f("ambient", Light.AmbientLight);
        }

        public void UniformLight(Light light)
        {
            UniformLight(light.Object.Forward, light.color, light.brightness, light.specularStrength, light.specularPower);
        }

        public void UniformLight(vec3 direction, vec3 color, float brightness, float specularStrength, float specularPower)
        {
            glUniform3f(glGetUniformLocation(shader.id, "lightDir"), direction.x, direction.y, direction.z);
            glUniform3f(glGetUniformLocation(shader.id, "lightColor"), color.x * brightness, color.y * brightness, color.z * brightness);
            glUniform1f(glGetUniformLocation(shader.id, "specularStrength"), specularStrength);
            glUniform1f(glGetUniformLocation(shader.id, "specularPower"), specularPower);
        }

        public void UniformMatrices(Camera camera, mat4 model)
        {
            UniformMatrices(camera.Object.Position, model, camera.ViewMatrix, camera.ProjectionMatrix);   
        }

        public void UniformMatrices(vec3 viewPos, mat4 model, mat4 view, mat4 proj)
        {
            glUniform3f(glGetUniformLocation(shader.id, "viewPos"), viewPos.x, viewPos.y, viewPos.z); 
            glUniformMatrix4fv(glGetUniformLocation(shader.id, "model"), 1, false, model.to_array());
            glUniformMatrix4fv(glGetUniformLocation(shader.id, "view"), 1, false, view.to_array());
            glUniformMatrix4fv(glGetUniformLocation(shader.id, "proj"), 1, false, proj.to_array());
        }
    }
}
