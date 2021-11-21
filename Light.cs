using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlmNet;

namespace torc
{
    public abstract class Light : Component
    {
        public vec3 color = new vec3(1, 1, 1);
        public float brightness = 1;
        public float specularStrength = 1f;
        public float specularPower = 128f;
    }

    public class DirectionalLight : Light
    {
        public static DirectionalLight main;

        public DirectionalLight() : base()
        {
            if (main == null)
                main = this;
        }
    }
}
