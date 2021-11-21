using System;
using GlmNet;

namespace torc
{
    class TestRotator : Component
    {
        public float speed = 2.5f;
        public vec3 axis = vec3.Up;

        public override void Update()
        {
            Object.Rotate(speed, axis);
        }
    }
}
