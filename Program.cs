using torc;
using System;
using GLFW;
using GlmNet;
using System.Timers;
using static OpenGL.GL;

class Program
{
    public static Window window;
    public static int width = 1024;
    public static int height = 576;
    public static float targetFPS = 30;
    public static long interval = (long)((1f / targetFPS) * 1000000000L);
    public static long counter;
    public static Scene activeScene = new();

    static void Main()
    {
        Setup();
        LoadScene();
        MainLoop();
    }

    private static void Setup()
    {
        PrepareContext();
        CreateWindow();
        InitGLCapabilities();
    }

    private static void CreateWindow()
    {
        window = CreateWindow("torc", width, height);
    }

    private static void InitGLCapabilities()
    {
        glEnable(GL_DEPTH_TEST);
        glEnable(GL_CULL_FACE);
    }

    private static void LoadScene()
    {
        Camera camera = new GameObject().AddComponent<Camera>();
        camera.Aspect *= (float)width / (float)height;
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
        glViewport(0, 0, width, height);
        glDepthMask(true);
        long lastTime = NanoTime();

        while (!Glfw.WindowShouldClose(window))
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
        Glfw.SwapBuffers(window);
        Glfw.PollEvents();

        // Clear the framebuffer to defined background color
        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

        activeScene.Render();
    }

    private static void Update()
    {
        activeScene.Update();
    }

    private static void PrepareContext()
    {
        // Set some common hints for the OpenGL profile creation
        Glfw.WindowHint(Hint.Resizable, true);
        Glfw.WindowHint(Hint.ClientApi, ClientApi.OpenGL);
        Glfw.WindowHint(Hint.ContextVersionMajor, 3);
        Glfw.WindowHint(Hint.ContextVersionMinor, 3);
        Glfw.WindowHint(Hint.OpenglProfile, Profile.Core);
        Glfw.WindowHint(Hint.Doublebuffer, true);
        Glfw.WindowHint(Hint.Decorated, true);
    }

    /// <summary>
    /// Creates and returns a handle to a GLFW window with a current OpenGL context.
    /// </summary>
    /// <param name="width">The width of the client area, in pixels.</param>
    /// <param name="height">The height of the client area, in pixels.</param>
    /// <returns>A handle to the created window.</returns>
    private static Window CreateWindow(string title, int width, int height)
    {
        // Create window, make the OpenGL context current on the thread, and import graphics functions
        var window = Glfw.CreateWindow(width, height, title, Monitor.None, Window.None);

        // Center window
        var screen = Glfw.PrimaryMonitor.WorkArea;
        var x = (screen.Width - width) / 2;
        var y = (screen.Height - height) / 2;
        Glfw.SetWindowPosition(window, x, y);

        Glfw.MakeContextCurrent(window);
        Import(Glfw.GetProcAddress);

        Glfw.SetFramebufferSizeCallback(window, (Window window, int width, int height) =>
        {
            glViewport(0, 0, width, height);
            activeScene.main.Aspect = (float) width / height;
        });

        return window;
    }
}
