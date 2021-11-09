using System;
using GlmNet;

namespace torc
{
    class TestRotator : Component
    {
        public float speed = 1f;
        public vec3 axis = new(0, 1, 0);

        public override void Update()
        {
            Object.Rotate(speed, axis);
        }
    }
}
