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
        private readonly Dictionary<string, object> properties = new();

        public Material(Shader shader)
        {
            this.shader = shader;
        }

        public void Use()
        {
            shader.Use();
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

                Console.WriteLine(key);
                Console.WriteLine(value);
                Console.WriteLine(location);

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
        
        public void UniformMatrices(Camera camera, mat4 model)
        {
            UniformMatrices(model, camera.ViewMatrix, camera.ProjectionMatrix);   
        }

        public void UniformMatrices(mat4 model, mat4 view, mat4 proj)
        {
            glUniformMatrix4fv(glGetUniformLocation(shader.id, "model"), 1, false, model.to_array());
            glUniformMatrix4fv(glGetUniformLocation(shader.id, "view"), 1, false, view.to_array());
            glUniformMatrix4fv(glGetUniformLocation(shader.id, "proj"), 1, false, proj.to_array());
        }
    }
}
