using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpFont;
using static OpenGL.GL;
using GlmNet;
using System.IO;

namespace torc
{
    public struct Character
    {
        public uint TextureID;
        public vec2 Size;
        public vec2 Bearing;
        public int Advance;
    }

    class Font
    {
        public Face face;
        public Character[] characters;
    }

    class Fonts
    {
        private static Library library;

        private static uint _vbo, _vao;

        private static Shader shader;

        public static Font DDIN { get; private set; }

        public static void Init()
        {
            CreateQuad();
            shader = CreateShader();

            library = new Library();

            DDIN = LoadFont("../../../Fonts/D-DIN.otf");

        }

        private static Shader CreateShader()
        {
            string vertexText = @"
#version 460
layout(location = 0) in vec2 in_pos;
layout(location = 1) in vec2 in_uv;

out vec2 vUV;

layout(location = 0) uniform mat4 model;
layout(location = 1) uniform mat4 projection;

void main()
{
    vUV = in_uv.xy;
    gl_Position = projection * model * vec4(in_pos.xy, 0.0, 1.0);
}
";

            string fragmentText = @"
#version 460

in vec2 vUV;

layout (binding=0) uniform sampler2D u_texture;

layout (location = 2) uniform vec3 textColor;

out vec4 fragColor;

void main()
{
    vec2 uv = vUV.xy;
    //float text = texture(u_texture, uv).r;
    //fragColor = vec4(textColor.rgb*text, text);
    fragColor = vec4(1, 0, 0, 1);
}";

            return Shader.Create(vertexText, fragmentText);

        }

        public static unsafe void CreateQuad()
        {
            // bind default texture
            glBindTexture(GL_TEXTURE_2D, 0);

            // set default (4 byte) pixel alignment 
            glPixelStoref(GL_UNPACK_ALIGNMENT, 4);

            float[] vquad =
            {
                // x      y      u     v    
                0.0f, -1.0f,   0.0f, 0.0f,
                0.0f,  0.0f,   0.0f, 1.0f,
                1.0f,  0.0f,   1.0f, 1.0f,
                0.0f, -1.0f,   0.0f, 0.0f,
                1.0f,  0.0f,   1.0f, 1.0f,
                1.0f, -1.0f,   1.0f, 0.0f
            };

            // Create [Vertex Buffer Object](https://www.khronos.org/opengl/wiki/Vertex_Specification#Vertex_Buffer_Object)
            _vbo = glGenBuffer();
            glBindBuffer(GL_ARRAY_BUFFER, _vbo);

            fixed (float* ptr = &vquad[0])
            {
                glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(float) * vquad.Length, ptr, GL_STATIC_DRAW);
            }

            // [Vertex Array Object](https://www.khronos.org/opengl/wiki/Vertex_Specification#Vertex_Array_Object)
            _vao = glGenVertexArray();
            glBindVertexArray(_vao);
            glEnableVertexAttribArray(0);
            glVertexAttribPointer(0, 2, GL_FLOAT, false, sizeof(float) * 2, (void*)0);
            glEnableVertexAttribArray(1);
            glVertexAttribPointer(1, 2, GL_FLOAT, false, sizeof(float) * 2, new IntPtr(sizeof(float) * 2));
        }

        public static void DrawQuad()
        {
            shader.Use();

            mat4 model = glm.scale(mat4.identity(), new vec3(10, 10, 10));
            mat4 projection = glm.ortho(0, Screen.Width, Screen.Height, 0, 0, 100);

            glUniformMatrix4fv(shader.GetUniformLocation("model"), 1, false, model.to_array());
            glUniformMatrix4fv(shader.GetUniformLocation("projection"), 1, false, projection.to_array());

            glBindVertexArray(_vao);
            glDrawArrays(GL_TRIANGLES, 0, 6);
            
            glBindVertexArray(0);
        }

        public static void RenderText(string text, float x, float y, float scale, vec2 dir, Font font)
        {
            glActiveTexture(GL_TEXTURE0);
            glBindVertexArray(_vao);

            float angle_rad = (float)Math.Atan2(dir.y, dir.x);
            mat4 rotateM = glm.rotate(angle_rad, new vec3(0, 0, 1));
            mat4 transOriginM = glm.translate(mat4.identity(), new vec3(x, y, 0));

            // Iterate through all characters
            float char_x = 0.0f;
            foreach (var c in text)
            {
                Character ch = font.characters[c];

                float w = ch.Size.x * scale;
                float h = ch.Size.y * scale;
                float xrel = char_x + ch.Bearing.x * scale;
                float yrel = (ch.Size.y - ch.Bearing.y) * scale;

                // Now advance cursors for next glyph (note that advance is number of 1/64 pixels)
                char_x += (ch.Advance >> 6) * scale; // Bitshift by 6 to get value in pixels (2^6 = 64 (divide amount of 1/64th pixels by 64 to get amount of pixels))

                mat4 scaleM = glm.scale(mat4.identity(), new vec3(w, h, 1.0f));
                mat4 transRelM = glm.translate(mat4.identity(), new vec3(xrel, yrel, 0.0f));

                mat4 modelM = scaleM * transRelM * rotateM * transOriginM; // OpenTK `*`-operator is reversed
                glUniformMatrix4fv(shader.GetUniformLocation("model"), 1, false, modelM.to_array());

                // Render glyph texture over quad
                glBindTexture(GL_TEXTURE_2D, ch.TextureID);

                // Render quad
                glDrawArrays(GL_TRIANGLES, 0, 6);
            }

            glBindVertexArray(0);
            glBindTexture(GL_TEXTURE_2D, 0);
        }

        private static Font LoadFont(string path)
        {
            Face face = new(library, path);
            face.SetPixelSizes(0, 32);

            // set 1 byte pixel alignment 
            glPixelStoref(GL_UNPACK_ALIGNMENT, 1);

            Character[] characters = new Character[128];

            // Load first 128 characters of ASCII set
            for (uint c = 0; c < 128; c++)
            {
                try
                {
                    // load glyph
                    //face.LoadGlyph(c, LoadFlags.Render, LoadTarget.Normal);
                    face.LoadChar(c, LoadFlags.Render, LoadTarget.Normal);
                    GlyphSlot glyph = face.Glyph;
                    FTBitmap bitmap = glyph.Bitmap;

                    // create glyph texture
                    uint texObj = glGenTexture();
                    glBindTexture(GL_TEXTURE_2D, texObj);
                    glTexImage2D(GL_TEXTURE_2D, 0,
                                GL_R8, bitmap.Width, bitmap.Rows, 0,
                                GL_RED, GL_UNSIGNED_BYTE, bitmap.Buffer);

                    // set texture parameters
                    glTexParameterf(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
                    glTexParameterf(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
                    glTexParameterf(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_EDGE);
                    glTexParameterf(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_EDGE);

                    // add character
                    characters[c] = new()
                    {
                        TextureID = texObj,
                        Size = new vec2(bitmap.Width, bitmap.Rows),
                        Bearing = new vec2(glyph.BitmapLeft, glyph.BitmapTop),
                        Advance = glyph.Advance.X
                    };
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            return new Font()
            {
                face = face,
                characters = characters
            };
        }
    }
}
