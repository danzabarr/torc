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
        Texture.CreateTextures();
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
    private static Material material2;

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

        camera.Object.Rotate(-20, new(1, 0, 0));
        camera.Object.Translate(0, 0, 10, Space.Self);

        DirectionalLight light = new GameObject().AddComponent<DirectionalLight>();
        light.color = new(1, 1, 1);
        light.brightness = 1;
        light.Object.Translate(1.5f, 1.5f, 0.5f);
        light.Object.Rotate(45, new(-.5f, -.8f, 0));

        GameObject cube = new();
        cube.AddComponent<TestRotator>();
        cube.Translate(1, 0, 0);

        GameObject cube2 = new();
        cube2.AddComponent<TestRotator>();
        cube2.Translate(-2, 0, 0);

        Texture yoda = Texture.Load("yoda.jpg");
        Texture brickNormal = Texture.Load("brick_normal_map.png");

        shader = Shader.Load("simple.vert", "simple.frag");
        material = new(shader);
        //material.mainTexture = yoda;
        material.normalMap = brickNormal;

        material2 = new(shader);
        //material2.mainTexture = yoda;

        MeshRenderer cubeRenderer = cube.AddComponent<MeshRenderer>();
        cubeRenderer.material = material;
        cubeRenderer.Mesh = Mesh.SmoothCube;

        MeshRenderer cubeRenderer2 = cube2.AddComponent<MeshRenderer>();
        cubeRenderer2.material = material;
        cubeRenderer2.Mesh = Mesh.Cube;

        GameObject floor = new();
        floor.Translate(0, -1, 0);
        floor.Scale(new vec3(10, .1f, 10));
        MeshRenderer floorRenderer = floor.AddComponent<MeshRenderer>();
        floorRenderer.material = material2;
        floorRenderer.Mesh = Mesh.Cube;

        GameObject missile = new();
        MeshRenderer missileRenderer = missile.AddComponent<MeshRenderer>();
        missile.Scale(.2f);
        missile.Rotate(90, new(-1, 0, 0));

        missileRenderer.Mesh = Mesh.Missile;
        missileRenderer.material = material;

        missile.Parent = light.Object;

        GameObject bigMissile = new();
        MeshRenderer bigMissileRenderer = bigMissile.AddComponent<MeshRenderer>();
        bigMissileRenderer.material = material;
        bigMissileRenderer.Mesh = missileRenderer.Mesh;
        bigMissile.Translate(5, 8, 0);
        bigMissile.Rotate(-150, new vec3(1, 1, 0));
        bigMissile.Scale(1);

        TestRotator missileRotator = bigMissile.AddComponent<TestRotator>();
        missileRotator.axis = new vec3(0, 1, 1);

        activeScene.Add(camera.Object);
        activeScene.Add(light.Object);
        activeScene.Add(missile);
        activeScene.Add(cube);
        activeScene.Add(cube2);
        activeScene.Add(floor);
        activeScene.Add(bigMissile);
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
        Input.Update();
    }
}
