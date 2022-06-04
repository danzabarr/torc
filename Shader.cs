using System;
using System.IO;
using System.Collections.Generic;
using static OpenGL.GL;
using GlmNet;

namespace torc
{
    public class Shader
    {
        public static string directoryPath = "../../../Shaders/";

        private static readonly Dictionary<uint, Shader> library = new();

        public static bool TryGet(uint id, out Shader shader)
            => library.TryGetValue(id, out shader);

        public readonly uint id;
        private Shader(uint id)
        {
            this.id = id;
            library[id] = this;
        }

        public void Use()
        {
            glUseProgram(id);
        }

        public int GetUniformLocation(string name)
        {
            return glGetUniformLocation(id, name);
        }

        public void UniformMatrix4fv(string name, mat4 matrix)
        {
            glUniformMatrix4fv(GetUniformLocation(name), 1, false, matrix.to_array());
        }

        public void Uniform1f(string name, float f)
        {
            glUniform1f(GetUniformLocation(name), f);
        }

        public void Uniform4f(string name, vec4 v)
        {
            glUniform4f(GetUniformLocation(name), v.x, v.y, v.z, v.w);
        }

        private static uint CreateShader(int type, string source)
        {
            var shader = glCreateShader(type);
            glShaderSource(shader, source);
            glCompileShader(shader);
            return shader;
        }

        public static Shader Create(string vertexText, string fragmentText)
        {
            uint vertex = CreateShader(GL_VERTEX_SHADER, vertexText);
            uint fragment = CreateShader(GL_FRAGMENT_SHADER, fragmentText);


            uint id = glCreateProgram();
            glAttachShader(id, vertex);
            glAttachShader(id, fragment);

            glLinkProgram(id);

            glDeleteShader(vertex);
            glDeleteShader(fragment);

            return new Shader(id);
        }

        public static Shader Load(string vertexShader, string fragmentShader)
        {
            string vertexPath = directoryPath + vertexShader;
            string fragmentPath = directoryPath + fragmentShader;

            if (!File.Exists(fragmentPath))
            {
                Console.WriteLine($"Error: Fragment shader not found at path {fragmentPath}");
                return null;
            }

            if (!File.Exists(vertexPath))
            {
                Console.WriteLine($"Error: Vertex shader not found at path {vertexPath}");
                return null;
            }

            Console.WriteLine($"Loaded shader {vertexShader} and {fragmentShader}");

            string fragmentText = File.ReadAllText(fragmentPath);
            string vertexText = File.ReadAllText(vertexPath);

            return Create(vertexText, fragmentText);

        }

        public void UniformAmbientLight(vec4 color)
        {
            Uniform4f("ambient", color);
        }

        public void UniformLight(Light light)
        {
            UniformLight(light.Object.Forward, light.color, light.brightness, light.specularStrength, light.specularPower);
        }

        public void UniformLight(vec3 direction, vec3 color, float brightness, float specularStrength, float specularPower)
        {
            glUniform3f(glGetUniformLocation(id, "lightDir"), direction.x, direction.y, direction.z);
            glUniform3f(glGetUniformLocation(id, "lightColor"), color.x * brightness, color.y * brightness, color.z * brightness);
            glUniform1f(glGetUniformLocation(id, "specularStrength"), specularStrength);
            glUniform1f(glGetUniformLocation(id, "specularPower"), specularPower);
        }

        public void UniformMatrices(Camera camera, mat4 model)
        {
            UniformMatrices(camera.Object.Position, model, camera.ViewMatrix, camera.ProjectionMatrix);
        }

        public void UniformMatrices(vec3 viewPos, mat4 model, mat4 view, mat4 proj)
        {
            glUniform3f(glGetUniformLocation(id, "viewPos"), viewPos.x, viewPos.y, viewPos.z);
            glUniformMatrix4fv(glGetUniformLocation(id, "model"), 1, false, model.to_array());
            glUniformMatrix4fv(glGetUniformLocation(id, "view"), 1, false, view.to_array());
            glUniformMatrix4fv(glGetUniformLocation(id, "proj"), 1, false, proj.to_array());
        }
    }
}
