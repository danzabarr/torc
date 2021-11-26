using System;
using System.IO;
using System.Collections.Generic;
using static OpenGL.GL;
using GlmNet;

namespace torc
{
    class Shader
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
    }
}
