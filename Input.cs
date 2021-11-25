using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GLFW;
using GlmNet;

namespace torc
{
    class Input
    {
        public static readonly int MAX_KEYS = 349;
        public static readonly int MAX_BUTTONS = 8;

        private static bool[] keys = new bool[MAX_KEYS];
        private static bool[] buttons = new bool[MAX_BUTTONS];
        private static ModifierKeys mod;

        private static vec2 lastMousePosition;
        public static vec2 MousePosition { get; private set; }
        public static vec2 MouseDelta => MousePosition - lastMousePosition;
        public static vec2 DragStart { get; private set; }
        public static vec2 DragEnd { get; private set; }
        public static vec2 DragDelta => DragEnd - DragStart;

        public static void Update()
        {
            lastMousePosition = DragEnd = MousePosition;

            if (!IsButtonDown(MouseButton.Button1))
                DragStart = MousePosition;
        }

        public static bool IsButtonDown(MouseButton button)
        {
            int index = (int)button;

            if (index < 0 || index >= MAX_BUTTONS)
                return false;

            return buttons[index];
        }

        public static bool IsKeyDown(Keys key)
        {
            int index = (int)key;

            if (index < 0 || index >= MAX_KEYS)
                return false;

            return keys[index];
        }

        public static bool IsKeyDown(Keys keys, ModifierKeys mod)
        {
            return IsKeyDown(keys) && IsModifierDown(mod);
        }

        public static bool IsModifierDown(ModifierKeys mod)
        {
            return Input.mod.HasFlag(mod);
        }

        internal static void OnMouseMove(double x, double y)
        {
            MousePosition = new vec2((float)x, (float)y);
        }

        internal static void OnMouseEnter(bool entering)
        {

        }

        internal static void OnMouseButton(MouseButton button, InputState state, ModifierKeys mod)
        {
            buttons[(int)button] = state != InputState.Release;
            Input.mod = mod;
        }

        internal static void OnKey(Keys key, int code, InputState state, ModifierKeys mod)
        {
            keys[(int)key] = state != InputState.Release;
            Input.mod = mod;

            switch (key)
            {
                case Keys.Escape:
                    if (state == InputState.Press)
                    {
                        int cursorState = Glfw.GetInputMode(Screen.Window, InputMode.Cursor);

                        if (cursorState == Glfw.GLFW_CURSOR_DISABLED)
                            Glfw.SetInputMode(Screen.Window, InputMode.Cursor, Glfw.GLFW_CURSOR_NORMAL);
                        else
                            Glfw.SetInputMode(Screen.Window, InputMode.Cursor, Glfw.GLFW_CURSOR_DISABLED);
                    
                    }
                    break;
            }

        }

        internal static void OnMouseScroll(double x, double y)
        {

        }

        internal static void OnCharacterInput(uint cp, ModifierKeys mod)
        {
            Input.mod = mod;
        }
    }
}
