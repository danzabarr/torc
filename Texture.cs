using GLFW;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OpenGL.GL;
using System.Drawing;

namespace torc
{


    class Texture
    {
        public static string directoryPath = "../../../Textures/";
        public readonly uint id;
        public readonly string path;
        public readonly int width, height;
        public byte[] data;

        public static Texture Load(string path, int width, int height)
        {
            return new Texture(directoryPath + path, width, height);
        }

        private Texture(string path, int width, int height)
        {
            id = glGenTexture();
            this.path = path;
            this.width = width;
            this.height = height;

            unsafe
            {
                data = File.ReadAllBytes(path);
                fixed (byte* ptr = &data[0])
                {
                    glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB, width, height, 0, GL_RGB, GL_BYTE, ptr);
                }
            }
        }
    }
}
