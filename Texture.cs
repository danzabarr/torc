using static OpenGL.GL;
using System.Drawing;
using System.Drawing.Imaging;
using System;

namespace torc
{
    public class Texture
    {
        enum Wrap
        {
            Repeat,
            Clamp
        }

        public static string directoryPath = "../../../Textures/";
        public uint id { get; private set; }
        public string path { get; private set; }
        public int width { get; private set; }
        public int height { get; private set; }

        public static Texture BlankNormal { get; private set; }
        public static Texture White { get; private set; }
        public static Texture Black { get; private set; }
        public static Texture Red { get; private set; }

        public static void CreateTextures()
        {
            BlankNormal = Create(1, 1, 128, 128, 255, 255);
            White = Create(1, 1, 255, 255, 255, 255);
            Black = Create(1, 1, 0, 0, 0, 255);
            Red = Create(1, 1, 255, 0, 0, 255);
        }

        public static Texture Create(int width, int height, byte r, byte g, byte b, byte a)
        {
            uint id = glGenTexture();
            glBindTexture(GL_TEXTURE_2D, id);
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);

            byte[] rawdata = new byte[width * height * 4];

            for (int i = 0; i < rawdata.Length; i += 4)
            {
                rawdata[i + 0] = r;
                rawdata[i + 1] = g;
                rawdata[i + 2] = b;
                rawdata[i + 3] = a;
            }
            unsafe
            {
                fixed (byte* bptr = rawdata)
                {
                    int* ptr = (int*)bptr;
                    glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, width, height, 0, GL_RGBA, GL_UNSIGNED_BYTE, ptr);
                }
            }

            glGenerateMipmap(GL_TEXTURE_2D);

            Console.WriteLine($"Texture #{id}: Created colored texture ({width}x{height}) R:{r} G:{g} B:{b} A:{a}");

            glBindTexture(GL_TEXTURE_2D, 0);


            return new Texture()
            {
                id = id,
                width = width,
                height = height
            };
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        public static Texture Load(
            string path,
            int wrapS = GL_TEXTURE_WRAP_S, 
            int wrapT = GL_TEXTURE_WRAP_T, 
            int minFilter = GL_LINEAR, 
            int magFilter = GL_LINEAR)
        {
            uint id = glGenTexture();
            glBindTexture(GL_TEXTURE_2D, id);
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, wrapS);
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, wrapT);
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, minFilter);
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, magFilter);

            Bitmap image = new Bitmap(directoryPath + path);

            int width = image.Width;
            int height = image.Height;

            BitmapData data = image.LockBits
            (
                new Rectangle(0, 0, width, height), 
                ImageLockMode.ReadOnly,
                image.PixelFormat
            );

            Console.WriteLine(image.PixelFormat);

            glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, width, height, 0, image.PixelFormat switch
            {
                PixelFormat.Format32bppArgb => GL_BGRA,
                PixelFormat.Format24bppRgb => GL_BGR,
                _ => GL_BGRA,

            }, GL_UNSIGNED_BYTE, data.Scan0);

            glGenerateMipmap(GL_TEXTURE_2D);

            Console.WriteLine($"Texture #{id}: Loaded texture from {path} ({width}x{height}) {image.PixelFormat}");

            glBindTexture(GL_TEXTURE_2D, 0);

            return new Texture()
            {
                id = id,
                path = path,
                width = width,
                height = height
            };
        }
    }
}
