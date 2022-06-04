namespace torc
{
    public abstract class Renderer : Component
    {
        public Material material;

        public void Render()
        {
            Render(Program.activeScene.main);
        }

        public abstract void Render(Camera camera);
        public abstract void Render(Shader shader);
    }
}
