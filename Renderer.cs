namespace torc
{
    abstract class Renderer : Component
    {
        public Material material;

        public void UseMaterial()
        {
            UseMaterial(Program.activeScene.main);
        }

        public void UseMaterial(Camera camera)
        {
            if (material == null)
                return;
            material.Use();
            material.UniformAmbientLight();
            material.UniformLight(DirectionalLight.main);
            material.UniformMatrices(camera.Object.Position, Object.WorldMatrix, camera.ViewMatrix, camera.ProjectionMatrix);
        }

        public void Render()
        {
            Render(Program.activeScene.main);
        }

        public abstract void Render(Camera camera);
    }
}
