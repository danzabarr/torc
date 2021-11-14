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
            material.Use();
            material.UniformMatrices(camera.Object.Position, Object.WorldMatrix, camera.ViewMatrix, camera.ProjectionMatrix);
        }

        public void Render()
        {
            Render(Program.activeScene.main);
        }

        public abstract void Render(Camera camera);
    }
}
