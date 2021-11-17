using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GLFW;
using static OpenGL.GL;
using GlmNet;

namespace torc
{
    class Screen
    {

        public static readonly int MAX_KEYS = 349;
        public static readonly int MAX_BUTTONS = 8;

        private static bool[] keys = new bool[MAX_KEYS];
        private static bool[] buttons = new bool[MAX_BUTTONS];

        private static vec2 mousePosition, lastMousePosition;

        public static Window Window { get; private set; }

        public static int Width { get; private set; }
        public static int Height { get; private set; }

        public static void Init()
        {
            PrepareContext();
            Window = CreateWindow("torc", 1024, 576);
            BindCallbacks();  
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
            glViewport(0, 0, width, height);
            Width = width;
            Height = height;

            return window;
        }
        
        public static bool IsKeyDown(Keys key)
        {
            if ((int)key < 0)
                return false;

            if ((int)key >= MAX_KEYS)
                return false;
            return keys[(int)key];
        }

        public static vec2 MousePosition()
        {
            return mousePosition;
        }

        public static vec2 MouseMovement()
        {
            return mousePosition - lastMousePosition;
        }

        private static ErrorCallback errorCallback;
        private static PositionCallback windowPositionCallback;
        private static SizeCallback windowSizeCallback, framebufferSizeCallback;
        private static FocusCallback windowFocusCallback;
        private static WindowCallback closeCallback, windowRefreshCallback;
        private static FileDropCallback dropCallback;
        private static MouseCallback cursorPositionCallback, scrollCallback;
        private static MouseEnterCallback cursorEnterCallback;
        private static MouseButtonCallback mouseButtonCallback;
        private static CharModsCallback charModsCallback;
        private static KeyCallback keyCallback;
        private static WindowMaximizedCallback windowMaximizeCallback;
        private static WindowContentsScaleCallback windowContentScaleCallback;

        private static void OnError(ErrorCode code, IntPtr message)
        {

        }

        private static void OnPositionChanged(double x, double y)
        {

        }

        private static void OnSizeChanged(int w, int h)
        {

        }

        private static void OnFocusChanged(bool focusing)
        {

        }

        private static void OnClosing()
        {

        }

        private static void OnFileDrop(int count, IntPtr arrayPtr)
        {

        }

        private static void OnMouseMove(double x, double y)
        {
            lastMousePosition = mousePosition;
            mousePosition = new vec2((float)x, (float)y);
        }

        private static void OnMouseEnter(bool entering)
        {

        }

        private static void OnMouseButton(MouseButton button, InputState state, ModifierKeys mod)
        {
            buttons[(int)button] = state != InputState.Release;
        }

        private static void OnMouseScroll(double x, double y)
        {

        }

        private static void OnCharacterInput(uint cp, ModifierKeys mod)
        {

        }

        private static void OnFramebufferSizeChanged(int width, int height)
        {
            Width = width;
            Height = height;
            glViewport(0, 0, width, height);
            Program.activeScene.main.Aspect = (float)width / height;
        }

        private static void Refreshed()
        {

        }

        private static void OnKey(Keys key, int code, InputState state, ModifierKeys mod)
        {
            keys[(int)key] = state != InputState.Release;
        }

        private static void OnMaximizeChanged(bool maximized)
        {

        }

        private static void OnContentScaleChanged(double x, double y)
        {

        }

        private static void BindCallbacks()
        {
            errorCallback = (code, message) => OnError(code, message);
            windowPositionCallback = (_, x, y) => OnPositionChanged(x, y);
            windowSizeCallback = (_, w, h) => OnSizeChanged(w, h);
            windowFocusCallback = (_, focusing) => OnFocusChanged(focusing);
            closeCallback = _ => OnClosing();
            dropCallback = (_, count, arrayPtr) => OnFileDrop(count, arrayPtr);
            cursorPositionCallback = (_, x, y) => OnMouseMove(x, y);
            cursorEnterCallback = (_, entering) => OnMouseEnter(entering);
            mouseButtonCallback = (_, button, state, mod) => OnMouseButton(button, state, mod);
            scrollCallback = (_, x, y) => OnMouseScroll(x, y);
            charModsCallback = (_, cp, mods) => OnCharacterInput(cp, mods);
            framebufferSizeCallback = (_, w, h) => OnFramebufferSizeChanged(w, h);
            windowRefreshCallback = _ => Refreshed();
            keyCallback = (_, key, code, state, mods) => OnKey(key, code, state, mods);
            windowMaximizeCallback = (_, maximized) => OnMaximizeChanged(maximized);
            windowContentScaleCallback = (_, x, y) => OnContentScaleChanged(x, y);

            Glfw.SetWindowPositionCallback(Window, windowPositionCallback);
            Glfw.SetWindowSizeCallback(Window, windowSizeCallback);
            Glfw.SetWindowFocusCallback(Window, windowFocusCallback);
            Glfw.SetCloseCallback(Window, closeCallback);
            Glfw.SetDropCallback(Window, dropCallback);
            Glfw.SetCursorPositionCallback(Window, cursorPositionCallback);
            Glfw.SetCursorEnterCallback(Window, cursorEnterCallback);
            Glfw.SetMouseButtonCallback(Window, mouseButtonCallback);
            Glfw.SetScrollCallback(Window, scrollCallback);
            Glfw.SetCharModsCallback(Window, charModsCallback);
            Glfw.SetFramebufferSizeCallback(Window, framebufferSizeCallback);
            Glfw.SetWindowRefreshCallback(Window, windowRefreshCallback);
            Glfw.SetKeyCallback(Window, keyCallback);
            Glfw.SetWindowMaximizeCallback(Window, windowMaximizeCallback);
            Glfw.SetWindowContentScaleCallback(Window, windowContentScaleCallback);
        }
    }
}
