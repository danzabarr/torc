using System;
using System.Collections.Generic;
using static OpenGL.GL;
using GlmNet;

namespace torc
{
    class MeshRenderer : Renderer
    {
        public uint vao { get; private set; }
        public uint vbo { get; private set; }
        public uint ebo { get; private set; }

        private Mesh mesh;

        public Mesh Mesh
        {
            get => mesh;
            set { SetMesh(value); }
        }

        public override void Render(Camera camera)
        {
            UseMaterial(camera);

            glBindVertexArray(vao);
            unsafe
            {
                glDrawElements(GL_TRIANGLES, mesh.indices.Length, GL_UNSIGNED_INT, (void*)0);
            }
            glBindVertexArray(0);
        }

        private unsafe void SetMesh(Mesh mesh)
        {
            this.mesh = mesh;

            vao = glGenVertexArray();
            vbo = glGenBuffer();
            ebo = glGenBuffer();

            glBindVertexArray(vao);

            //VERTEX BUFFER
            unsafe
            {

                glBindBuffer(GL_ARRAY_BUFFER, vbo);
                fixed (Vertex* v = &mesh.vertices[0])
                {
                    glBufferData(GL_ARRAY_BUFFER, sizeof(Vertex) * mesh.vertices.Length, v, GL_STATIC_DRAW);
                }

                //INDEX BUFFER
                glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, ebo);
                fixed (uint* i = &mesh.indices[0])
                {
                    glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(uint) * mesh.indices.Length, i, GL_STATIC_DRAW);
                }

                //POS
                glEnableVertexAttribArray(0);
                glVertexAttribPointer(0, 3, GL_FLOAT, false, sizeof(Vertex), (void*)0);

                //NORMAL
                glEnableVertexAttribArray(1);
                glVertexAttribPointer(1, 3, GL_FLOAT, false, sizeof(Vertex), new IntPtr(sizeof(vec3)));
                
                //COLOR
                glEnableVertexAttribArray(2);
                glVertexAttribPointer(2, 4, GL_FLOAT, false, sizeof(Vertex), new IntPtr(sizeof(vec3) + sizeof(vec3)));
                
                //UV
                glEnableVertexAttribArray(3);
                glVertexAttribPointer(3, 2, GL_FLOAT, false, sizeof(Vertex), new IntPtr(sizeof(vec3) + sizeof(vec3) + sizeof(vec4)));
            }

            glBindVertexArray(0);
        }
    }
}
