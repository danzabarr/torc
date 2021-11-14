using static OpenGL.GL;
using System.Drawing;
using System.Drawing.Imaging;

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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
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

            Bitmap image = new(directoryPath + path);

            width = image.Width;
            height = image.Height;

            BitmapData data = image.LockBits
            (
                new Rectangle(0, 0, width, height), 
                ImageLockMode.ReadOnly,
                image.PixelFormat
            );

            glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, width, height, 0, image.PixelFormat switch
            {
                PixelFormat.Format32bppArgb => GL_RGBA,
                PixelFormat.Format24bppRgb => GL_RGB,
                _ => GL_RGBA,

            }, GL_UNSIGNED_BYTE, data.Scan0);
        }
    }
}
