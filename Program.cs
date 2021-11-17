using torc;
using System;
using GLFW;
using GlmNet;
using System.Timers;
using static OpenGL.GL;

class Program
{
    public static float targetFPS = 30;
    public static long interval = (long)((1f / targetFPS) * 1000000000L);
    public static long counter;
    public static Scene activeScene = new();

    static void Main()
    {
        Screen.Init();
        InitGLCapabilities(); 
        LoadScene();
        MainLoop();
    }

    private static void InitGLCapabilities()
    {
        glEnable(GL_DEPTH_TEST);
        glEnable(GL_CULL_FACE);
    }

    private static void LoadScene()
    {
        Camera camera = new GameObject().AddComponent<Camera>();
        camera.Aspect = (float)Screen.Width / (float)Screen.Height;
        camera.Object.Rotate(-20, new(1, 0, 0));
        camera.Object.Translate(0, 0, 10);

        DirectionalLight light = new GameObject().AddComponent<DirectionalLight>();
        light.color = new(1, 1, 1);
        light.brightness = 1;
        light.Object.Translate(1.5f, 1.5f, 0.5f);
        light.Object.Rotate(45, new(-.5f, -.8f, 0));

        GameObject cube = new();
        cube.AddComponent<TestRotator>();

        Shader shader = Shader.Load("simple.vert", "simple.frag");

        Material material = new(shader);
        material.Use();
        material.UploadProperties();
        material.UniformLight(light);

        MeshRenderer cubeRenderer = cube.AddComponent<MeshRenderer>();
        cubeRenderer.material = material;
        cubeRenderer.Mesh = Mesh.Cube;

        GameObject missile = new GameObject();
        MeshRenderer missileRenderer = missile.AddComponent<MeshRenderer>();
        missile.Scale(.2f);
        missile.Rotate(90, new(-1, 0, 0));

        missileRenderer.Mesh = Mesh.LoadObjFile("missile.obj");
        missileRenderer.material = material;

        missile.Parent = light.Object;

        Texture texture = new("yoda.jpg");

        glActiveTexture(0);
        glBindTexture(GL_TEXTURE_2D, texture.id);

        activeScene.Add(camera.Object);
        activeScene.Add(light.Object);
        activeScene.Add(missile);
        activeScene.Add(cube);
    }

    private static void MainLoop()
    {
        //glViewport(0, 0, width, height);
        glDepthMask(true);
        long lastTime = NanoTime();

        while (!Glfw.WindowShouldClose(Screen.Window))
        {
            long currentTime = NanoTime();
            long deltaTime = currentTime - lastTime;
            counter += deltaTime;
            lastTime = currentTime;

            while (counter >= interval)
            {
                counter -= interval;
                Update();
            }

            Render();
        }

        Glfw.Terminate();
    }
    private static long NanoTime()
    {
        long nano = 10000L * System.Diagnostics.Stopwatch.GetTimestamp();
        nano /= TimeSpan.TicksPerMillisecond;
        nano *= 100L;
        return nano;
    }

    private static void Render()
    {
        // Swap fore/back framebuffers, and poll for operating system events.
        Glfw.SwapBuffers(Screen.Window);
        Glfw.PollEvents();

        // Clear the framebuffer to defined background color
        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

        activeScene.Render();
    }

    private static void Update()
    {
        activeScene.Update();

        if (Screen.IsKeyDown(Keys.A))
            Console.WriteLine("A");
    }
}
