﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlmNet;

namespace torc
{
    class CameraControls : Component
    {
        public vec2 rotationSensitivity { get; set; } = new vec2(.1f, .075f);

        public float speed = .8f;

        public override void Update()
        {
            if (Screen.IsKeyDown(GLFW.Keys.W))
            {
                Object.Translate(vec3.Forward * speed, Space.Self);
                //Object.Translate(Object.Forward * speed, Space.World);
            }

            if (Screen.IsKeyDown(GLFW.Keys.S))
            {
                Object.Translate(vec3.Back * speed, Space.Self);
            }

            if (Screen.IsKeyDown(GLFW.Keys.A))
            {
                Object.Translate(vec3.Left * speed, Space.Self);
            }

            if (Screen.IsKeyDown(GLFW.Keys.D))
            {
                Object.Translate(vec3.Right * speed, Space.Self);
            }

            if (Screen.IsKeyDown(GLFW.Keys.Space))
            {
                Object.Translate(vec3.Up * speed);
            }

            if (Screen.IsKeyDown(GLFW.Keys.X))
            {
                Object.Translate(vec3.Down * speed);
            }

            Object.Rotate(-Screen.MouseMovement.x * rotationSensitivity.x, vec3.Up, Space.World);
            Object.Rotate(-Screen.MouseMovement.y * rotationSensitivity.y, vec3.Right, Space.Self);
        }
    }
}
