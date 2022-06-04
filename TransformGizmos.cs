using static OpenGL.GL;
using GlmNet;
using System;

namespace torc
{
    public class TransformGizmos : Renderer
    {
        public override void Render(Camera camera)
        {
            vec3 pos = Object.Position;
            vec3 up = pos + Object.Up * 10;
            vec3 forward = pos + Object.Forward * 10;
            vec3 right = pos + Object.Right * 10;

            glLineWidth(2f);

            glBegin(GL_LINES);
            glColor4f(0, 1, 0, 1);
            glVertex3f(pos.x, pos.y, pos.z);
            glVertex3f(up.x, up.y, up.z);
            glEnd();

            glBegin(GL_LINES);
            glColor4f(0, 0, 1, 1);
            glVertex3f(pos.x, pos.y, pos.z);
            glVertex3f(forward.x, forward.y, forward.z);
            glEnd();
        
            glBegin(GL_LINES);
            glColor4f(1, 0, 0, 1);
            glVertex3f(pos.x, pos.y, pos.z);
            glVertex3f(right.x, right.y, right.z);
            glEnd();

            glBegin(GL_TRIANGLES);
            glColor3f(1.0f, 0.0f, 0.0f); glVertex2f(0.0f, 1.0f);
            glColor3f(0.0f, 1.0f, 0.0f); glVertex2f(0.87f, -0.5f);
            glColor3f(0.0f, 0.0f, 1.0f); glVertex2f(-0.87f, -0.5f);
            glEnd();
        }

        public override void Render(Shader shader)
        {
            throw new NotImplementedException();
        }
    }
}
