using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OpenGL.GL;
//using SixLabors.ImageSharp;
//using SixLabors.ImageSharp.PixelFormats;
using System.Drawing;

namespace torc
{


    class Texture
    {
        enum Wrap
        {
            Repeat,
            Clamp
        }

        public static string directoryPath = "../../../Textures/";
        public readonly uint id;
        public readonly string path;
        public readonly int width, height;
        public byte[] data;

        public Texture(
            string path, 
            int wrapS = GL_REPEAT, 
            int wrapT = GL_REPEAT, 
            int minFilter = GL_NEAREST, 
            int magFilter = GL_NEAREST)
        {
            id = glGenTexture();
            glActiveTexture(GL_TEXTURE0);
            glBindTexture(GL_TEXTURE_2D, id);
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, wrapS);
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, wrapT);
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, minFilter);
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, magFilter);

            this.path = path;

            //Image<Rgba32> image = Image.Load<Rgba32>(directoryPath + path);

            Bitmap image = new Bitmap(directoryPath + path);

            width = image.Width;
            height = image.Height;

            Console.WriteLine(width);
            Console.WriteLine(height);


            /*
            byte[] data = new byte[width * height * 4];
            for (int y = 0; y < height; y++)
            {
                var row = image.GetPixelRowSpan(y);
                for (int x = 0; x < width; x++)
                {
                    data[y * width + x + 0] = row[x].R;
                    data[y * width + x + 1] = row[x].G;
                    data[y * width + x + 2] = row[x].B;
                    data[y * width + x + 3] = row[x].A;
                }
            }
            unsafe
            {
                fixed (byte* dataPtr = &data[0])
                {
                    glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, width, height, 0, GL_RGBA, GL_BYTE, dataPtr);
                }
            }
            */
            
            System.Drawing.Imaging.BitmapData data = image.LockBits
            (
                new Rectangle(0, 0, width, height), 
                System.Drawing.Imaging.ImageLockMode.ReadOnly, 
                System.Drawing.Imaging.PixelFormat.Format32bppArgb
            );
            
            glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, width, height, 0, GL_RGBA, GL_UNSIGNED_BYTE, data.Scan0);
        }
    }
}
