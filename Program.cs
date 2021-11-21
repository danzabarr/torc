using torc;
using System;
using GLFW;
using GlmNet;
using System.Timers;
using static OpenGL.GL;
using SharpFont;

class Program
{
    private static float targetFPS = 30;
    private static long interval = (long)((1f / targetFPS) * 1000000000L);
    private static long counter;

    private static long fpsCounter;
    private static float fps;

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
        //glClearColor(1, 1, 1, 1);
    }

    private static Shader shader;
    private static Material material;

    private static void LoadScene()
    {
        Camera camera = new GameObject().AddComponent<Camera>();
        camera.Object.AddComponent<CameraControls>();

        //camera.Mode = CameraMode.Orthographic;
        //camera.Left = 0;
        //camera.Right = Screen.Width;
        //camera.Bottom = Screen.Height;
        //camera.Top = 0;
        //camera.ZNear = 0;

        camera.Mode = CameraMode.Perspective;
        camera.Aspect = (float)Screen.Width / (float)Screen.Height;
        Console.WriteLine($"r {camera.Object.Right}");
        Console.WriteLine($"u {camera.Object.Up}");
        Console.WriteLine($"f {camera.Object.Forward}");

        camera.Object.Rotate(-20, new(1, 0, 0));
        camera.Object.Translate(0, 0, 10, Space.Self);

        Console.WriteLine($"e {camera.Object.EulerAngles}");

        Console.WriteLine($"r {camera.Object.Right}");
        Console.WriteLine($"u {camera.Object.Up}");
        Console.WriteLine($"f {camera.Object.Forward}");

        DirectionalLight light = new GameObject().AddComponent<DirectionalLight>();
        light.color = new(1, 1, 1);
        light.brightness = 1;
        light.Object.Translate(1.5f, 1.5f, 0.5f);
        light.Object.Rotate(45, new(-.5f, -.8f, 0));

        GameObject cube = new();
        cube.AddComponent<TestRotator>();
        cube.Translate(1, 0, 0);
        cube.Rotate(45, new vec3(0, 1, 0));
        cube.AddComponent<TransformGizmos>();
        
        shader = Shader.Load("simple.vert", "simple.frag");

        material = new(shader);
        material.mainTexture = new Texture("yoda.jpg");
        material.Use();
        material.UploadProperties();
        material.UniformLight(light);

        MeshRenderer cubeRenderer = cube.AddComponent<MeshRenderer>();
        cubeRenderer.material = material;
        cubeRenderer.Mesh = Mesh.Cube;

        GameObject floor = new();
        floor.Translate(0, -1, 0);
        floor.Scale(new vec3(10, .1f, 10));
        MeshRenderer floorRenderer = floor.AddComponent<MeshRenderer>();
        floorRenderer.material = material;
        floorRenderer.Mesh = Mesh.Cube;

        GameObject missile = new();
        MeshRenderer missileRenderer = missile.AddComponent<MeshRenderer>();
        missile.Scale(.2f);
        missile.Rotate(90, new(-1, 0, 0));

        missileRenderer.Mesh = Mesh.LoadObjFile("missile.obj");
        missileRenderer.material = material;

        missile.Parent = light.Object;

        activeScene.Add(camera.Object);
        activeScene.Add(light.Object);
        activeScene.Add(missile);
        activeScene.Add(cube);
        activeScene.Add(floor);
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
            fpsCounter += deltaTime;
            lastTime = currentTime;

            while (counter >= interval)
            {
                counter -= interval;
                Update();
            }

            while (fpsCounter >= 1000000000L)
            {
                fpsCounter -= 1000000000L;
                //Console.WriteLine($"{fps} FPS");
                fps = 0;
            }

            fps++;
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
        Screen.Update();
    }
}
