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
        public readonly string vertexPath;
        public readonly string fragmentPath;
        private Shader(uint id, string vertexPath, string fragmentPath)
        {
            this.id = id;
            this.vertexPath = vertexPath;
            this.fragmentPath = fragmentPath;
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

        public void UniformMatrix4fv(string location, mat4 matrix)
        {
            glUniformMatrix4fv(GetUniformLocation(location), 1, false, matrix.to_array());
        }

        public void UniformFloat(string location, float f)
        {
            glUniform1f(GetUniformLocation(location), f);
        }

        private static uint CreateShader(int type, string source)
        {
            var shader = glCreateShader(type);
            glShaderSource(shader, source);
            glCompileShader(shader);
            return shader;
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
            uint vertex = CreateShader(GL_VERTEX_SHADER, vertexText);
            uint fragment = CreateShader(GL_FRAGMENT_SHADER, fragmentText);

            uint id = glCreateProgram();
            glAttachShader(id, vertex);
            glAttachShader(id, fragment);

            glLinkProgram(id);

            glDeleteShader(vertex);
            glDeleteShader(fragment);

            return new Shader(id, vertexPath, fragmentPath);
        }
    }
}
