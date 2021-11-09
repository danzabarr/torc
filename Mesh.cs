using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlmNet;

namespace torc
{
    class Mesh
    {
        public bool IsDirty { get; private set; }

        public uint[] indices;
        public Vertex[] vertices;

        public Vertex[] Vertices
        {
            set
            {
                vertices = value;
                IsDirty = true;
            }
        }

        public uint[] Indices
        {
            set
            {
                indices = value;
                IsDirty = true;
            }
        }


        public Mesh() { }

        public static Mesh Quad = new()
        {

        };

        public static Mesh Cube = new()
        {
            indices = new uint[]
            {
                // front
		        0, 1, 2,
                2, 3, 0,
		        // right
		        1, 5, 6,
                6, 2, 1,
		        // back
		        7, 6, 5,
                5, 4, 7,
		        // left
		        4, 0, 3,
                3, 7, 4,
		        // bottom
		        4, 5, 1,
                1, 0, 4,
		        // top
		        3, 2, 6,
                6, 7, 3

            },

            vertices = new Vertex[]
            {
                new Vertex(new vec3(-.5f, -.5f, +.5f), new vec3(), new vec4(0, 0, 0, 1), new vec2()),
                new Vertex(new vec3(+.5f, -.5f, +.5f), new vec3(), new vec4(0, 0, 0, 1), new vec2()),
                new Vertex(+.5f, +.5f, +.5f),
                new Vertex(-.5f, +.5f, +.5f),

                new Vertex(-.5f, -.5f, -.5f),
                new Vertex(+.5f, -.5f, -.5f),
                new Vertex(+.5f, +.5f, -.5f),
                new Vertex(-.5f, +.5f, -.5f),
            }
        };
    }

    struct Vertex 
    {
        public vec3 position;
        public vec3 normal;
        public vec4 color;
        public vec2 uv;
    
        public Vertex(float x, float y, float z)
        {
            position = new vec3(x, y, z);
            normal = new vec3();
            color = new vec4(1, 1, 1, 1);
            uv = new vec2();
        }
        public Vertex(vec3 position, vec3 normal, vec4 color, vec2 uv)
        {
            this.position = position;
            this.normal = normal;
            this.color = color;
            this.uv = uv;
        }

        public Vertex(vec3 position, vec2 uv)
        {
            this.position = position;
            normal = new vec3();
            color = new vec4(1, 1, 1, 1);
            this.uv = uv;
        }
    }

}
