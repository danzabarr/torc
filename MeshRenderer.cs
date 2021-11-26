using System;
using System.Collections.Generic;
using static OpenGL.GL;
using GlmNet;

namespace torc
{
    class MeshRenderer : Renderer
    {
        public Mesh Mesh;

        public override void Render(Camera camera)
        {
            UseMaterial(camera);

            if (Mesh != null)
                Mesh.Render();
        }
    }
}
