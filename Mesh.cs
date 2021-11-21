using System;
using System.IO;
using System.Collections.Generic;
using GlmNet;

namespace torc
{
    class Mesh
    {
        public static string directoryPath = "../../../Models/";

        public uint[] indices;
        public Vertex[] vertices;

        public Mesh() { }

        public static Mesh LoadObjFile(string objFilePath)
        {
            if (!File.Exists(directoryPath + objFilePath))
            {
                Console.WriteLine($"Error: Fragment shader not found at path {directoryPath + objFilePath}");
                return null;
            }

            Console.WriteLine($"Loaded model from .obj file {objFilePath}");

            List<Vertex> vertices = new();
            List<vec3> positions = new();
            List<vec2> uvs = new();
            List<vec3> normals = new();
            List<uint> indices = new();

            foreach (string line in File.ReadAllLines(directoryPath + objFilePath))
            {
                if (line.StartsWith("v "))
                {
                    string[] split = line.Split(" ");
                    float x = float.Parse(split[1]);
                    float y = float.Parse(split[2]);
                    float z = float.Parse(split[3]);
                    positions.Add(new(x, y, z));
                }

                else if (line.StartsWith("vt "))
                {
                    string[] split = line.Split(" ");
                    float u = float.Parse(split[1]);
                    float v = float.Parse(split[2]);
                    uvs.Add(new(u, v));
                }

                else if (line.StartsWith("vn "))
                {
                    string[] split = line.Split(" ");
                    float x = float.Parse(split[1]);
                    float y = float.Parse(split[2]);
                    float z = float.Parse(split[3]);
                    normals.Add(new(x, y, z));
                }

                else if (line.StartsWith("f "))
                {
                    string[] split = line.Split(" ");

                    int faceVertexCount = split.Length - 1;

                    (int, int, int) Vert(string v)
                    {
                        string[] div = v.Split("/");
                        int positionIndex = int.Parse(div[0]) - 1;
                        int uvIndex = int.Parse(div[1]) - 1;
                        int normalIndex = int.Parse(div[2]) - 1;
                        return (positionIndex, uvIndex, normalIndex);
                    }

                    void AddVertex(string t)
                    {
                        (int, int, int) v = Vert(t);
                        indices.Add((uint)vertices.Count);
                        vertices.Add(new()
                        {
                            position = positions[v.Item1],
                            uv = uvs[v.Item2],
                            normal = normals[v.Item3]
                        });
                    }

                    void AddTriangle(string t0, string t1, string t2)
                    {
                        AddVertex(t0);
                        AddVertex(t1);
                        AddVertex(t2);
                    }

                    for (int i = 1; i < split.Length - 2; i++)
                    {
                        AddTriangle(split[1], split[i + 1], split[i + 2]);
                    }
                }
            }

            return new()
            {
                vertices = vertices.ToArray(),
                indices = indices.ToArray()
            };
        }

        public static Mesh Cube = new()
        {
            indices = new uint[]
            {
                0, 1, 2,
                0, 2, 3,

                4, 5, 6,
                4, 6, 7,

                8, 9, 10,
                8, 10, 11,

                12, 13, 14,
                12, 14, 15,

                16, 17, 18,
                16, 18, 19,

                20, 21, 22,
                20, 22, 23
            },

            vertices = new Vertex[]
            {
                //TOP
                new Vertex()
                {
                    position = new vec3(-1, +1, -1) * .5f,
                    normal = new vec3(0, +1, 0),
                    color = new vec4(1, 0, 0, 1),
                    uv = new vec2(0, 1)
                },
                new Vertex()
                {
                    position = new vec3(-1, +1, +1) * .5f,
                    normal = new vec3(0, +1, 0),
                    color = new vec4(0, 1, 0, 1),
                    uv = new vec2(0, 0)
                },
                new Vertex()
                {
                    position = new vec3(+1, +1, +1) * .5f,
                    normal = new vec3(0, +1, 0),
                    color = new vec4(0, 0, 1, 1),
                    uv = new vec2(1, 0)
                },
                new Vertex()
                {
                    position = new vec3(+1, +1, -1) * .5f,
                    normal = new vec3(0, +1, 0),
                    color = new vec4(1, 1, 1, 1),
                    uv = new vec2(1, 1)
                },

                //BOTTOM
                new Vertex()
                {
                    position = new vec3(-1, -1, +1) * .5f,
                    normal = new vec3(0, -1, 0),
                    color = new vec4(1, 1, 1, 1),
                    uv = new vec2(0, 1)
                },
                new Vertex()
                {
                    position = new vec3(-1, -1, -1) * .5f,
                    normal = new vec3(0, -1, 0),
                    color = new vec4(1, 1, 1, 1),
                    uv = new vec2(0, 0)
                },
                new Vertex()
                {
                    position = new vec3(+1, -1, -1) * .5f,
                    normal = new vec3(0, -1, 0),
                    color = new vec4(1, 1, 1, 1),
                    uv = new vec2(1, 0)
                },
                new Vertex()
                {
                    position = new vec3(+1, -1, +1) * .5f,
                    normal = new vec3(0, -1, 0),
                    color = new vec4(1, 1, 1, 1),
                    uv = new vec2(1, 1)
                },

                //FRONT
                new Vertex()
                {
                    position = new vec3(-1, -1, -1) * .5f,
                    normal = new vec3(0, 0, -1),
                    color = new vec4(1, 1, 1, 1),
                    uv = new vec2(0, 1)
                },
                new Vertex()
                {
                    position = new vec3(-1, +1, -1) * .5f,
                    normal = new vec3(0, 0, -1),
                    color = new vec4(1, 1, 1, 1),
                    uv = new vec2(0, 0)
                },
                new Vertex()
                {
                    position = new vec3(+1, +1, -1) * .5f,
                    normal = new vec3(0, 0, -1),
                    color = new vec4(1, 1, 1, 1),
                    uv = new vec2(1, 0)
                },
                new Vertex()
                {
                    position = new vec3(+1, -1, -1) * .5f,
                    normal = new vec3(0, 0, -1),
                    color = new vec4(1, 1, 1, 1),
                    uv = new vec2(1, 1)
                },

                //LEFT
                new Vertex()
                {
                    position = new vec3(-1, -1, +1) * .5f,
                    normal = new vec3(-1, 0, 0),
                    color = new vec4(1, 1, 1, 1),
                    uv = new vec2(0, 1)
                },
                new Vertex()
                {
                    position = new vec3(-1, +1, +1) * .5f,
                    normal = new vec3(-1, 0, 0),
                    color = new vec4(1, 1, 1, 1),
                    uv = new vec2(0, 0)
                },
                new Vertex()
                {
                    position = new vec3(-1, +1, -1) * .5f,
                    normal = new vec3(-1, 0, 0),
                    color = new vec4(1, 1, 1, 1),
                    uv = new vec2(1, 0)
                },
                new Vertex()
                {
                    position = new vec3(-1, -1, -1) * .5f,
                    normal = new vec3(-1, 0, 0),
                    color = new vec4(1, 1, 1, 1),
                    uv = new vec2(1, 1)
                },

                //BACK
                new Vertex()
                {
                    position = new vec3(+1, -1, +1) * .5f,
                    normal = new vec3(0, 0, +1),
                    color = new vec4(1, 1, 1, 1),
                    uv = new vec2(0, 1)
                },
                new Vertex()
                {
                    position = new vec3(+1, +1, +1) * .5f,
                    normal = new vec3(0, 0, +1),
                    color = new vec4(1, 1, 1, 1),
                    uv = new vec2(0, 0)
                },
                new Vertex()
                {
                    position = new vec3(-1, +1, +1) * .5f,
                    normal = new vec3(0, 0, +1),
                    color = new vec4(1, 1, 1, 1),
                    uv = new vec2(1, 0)
                },
                new Vertex()
                {
                    position = new vec3(-1, -1, +1) * .5f,
                    normal = new vec3(0, 0, +1),
                    color = new vec4(1, 1, 1, 1),
                    uv = new vec2(1, 1)
                },

                //RIGHT
                new Vertex()
                {
                    position = new vec3(+1, -1, -1) * .5f,
                    normal = new vec3(+1, 0, 0),
                    color = new vec4(1, 1, 1, 1),
                    uv = new vec2(0, 1)
                },
                new Vertex()
                {
                    position = new vec3(+1, +1, -1) * .5f,
                    normal = new vec3(+1, 0, 0),
                    color = new vec4(1, 1, 1, 1),
                    uv = new vec2(0, 0)
                },
                new Vertex()
                {
                    position = new vec3(+1, +1, +1) * .5f,
                    normal = new vec3(+1, 0, 0),
                    color = new vec4(1, 1, 1, 1),
                    uv = new vec2(1, 0)
                },
                new Vertex()
                {
                    position = new vec3(+1, -1, +1) * .5f,
                    normal = new vec3(+1, 0, 0),
                    color = new vec4(1, 1, 1, 1),
                    uv = new vec2(1, 1)
                },
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
