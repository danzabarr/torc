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
            if (Mesh == null)
                return;

            if (material == null)
                return;

            material.Use();
            material.UniformAmbientLight(Light.AmbientLight);
            material.UniformLight(DirectionalLight.main);
            material.UniformMatrices(camera.Object.Position, Object.WorldMatrix, camera.ViewMatrix, camera.ProjectionMatrix);

            Mesh.Render();
        }

        public override void Render(Shader shader)
        {
            if (Mesh == null)
                return;

            if (shader == null)
                return;

            shader.Use();
            shader.UniformMatrix4fv("model", Object.WorldMatrix);
            Mesh.Render();
        }
    }
}
