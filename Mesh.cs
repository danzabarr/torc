using System;
using System.IO;
using System.Collections.Generic;
using GlmNet;
using static OpenGL.GL;

namespace torc
{
    class Mesh
    {
        public static string directoryPath = "../../../Models/";

        public static Mesh Cube = Create
        (
            new uint[]
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

            new Vertex[]
            {
                //TOP
                new Vertex()
                {
                    position = new vec3(-1, +1, -1) * .5f,
                    normal = new vec3(0, +1, 0),
                    color = new vec4(1, 0, 0, 1),
                    uv = new vec2(1, 1)
                },
                new Vertex()
                {
                    position = new vec3(-1, +1, +1) * .5f,
                    normal = new vec3(0, +1, 0),
                    color = new vec4(0, 1, 0, 1),
                    uv = new vec2(1, 0)
                },
                new Vertex()
                {
                    position = new vec3(+1, +1, +1) * .5f,
                    normal = new vec3(0, +1, 0),
                    color = new vec4(0, 0, 1, 1),
                    uv = new vec2(0, 0)
                },
                new Vertex()
                {
                    position = new vec3(+1, +1, -1) * .5f,
                    normal = new vec3(0, +1, 0),
                    color = new vec4(1, 1, 1, 1),
                    uv = new vec2(0, 1)
                },

                //BOTTOM
                new Vertex()
                {
                    position = new vec3(-1, -1, +1) * .5f,
                    normal = new vec3(0, -1, 0),
                    color = new vec4(1, 1, 1, 1),
                    uv = new vec2(1, 1)
                },
                new Vertex()
                {
                    position = new vec3(-1, -1, -1) * .5f,
                    normal = new vec3(0, -1, 0),
                    color = new vec4(1, 1, 1, 1),
                    uv = new vec2(1, 0)
                },
                new Vertex()
                {
                    position = new vec3(+1, -1, -1) * .5f,
                    normal = new vec3(0, -1, 0),
                    color = new vec4(1, 1, 1, 1),
                    uv = new vec2(0, 0)
                },
                new Vertex()
                {
                    position = new vec3(+1, -1, +1) * .5f,
                    normal = new vec3(0, -1, 0),
                    color = new vec4(1, 1, 1, 1),
                    uv = new vec2(0, 1)
                },

                //FRONT
                new Vertex()
                {
                    position = new vec3(-1, -1, -1) * .5f,
                    normal = new vec3(0, 0, -1),
                    color = new vec4(1, 1, 1, 1),
                    uv = new vec2(1, 1)
                },
                new Vertex()
                {
                    position = new vec3(-1, +1, -1) * .5f,
                    normal = new vec3(0, 0, -1),
                    color = new vec4(1, 1, 1, 1),
                    uv = new vec2(1, 0)
                },
                new Vertex()
                {
                    position = new vec3(+1, +1, -1) * .5f,
                    normal = new vec3(0, 0, -1),
                    color = new vec4(1, 1, 1, 1),
                    uv = new vec2(0, 0)
                },
                new Vertex()
                {
                    position = new vec3(+1, -1, -1) * .5f,
                    normal = new vec3(0, 0, -1),
                    color = new vec4(1, 1, 1, 1),
                    uv = new vec2(0, 1)
                },

                //LEFT
                new Vertex()
                {
                    position = new vec3(-1, -1, +1) * .5f,
                    normal = new vec3(-1, 0, 0),
                    color = new vec4(1, 1, 1, 1),
                    uv = new vec2(1, 1)
                },
                new Vertex()
                {
                    position = new vec3(-1, +1, +1) * .5f,
                    normal = new vec3(-1, 0, 0),
                    color = new vec4(1, 1, 1, 1),
                    uv = new vec2(1, 0)
                },
                new Vertex()
                {
                    position = new vec3(-1, +1, -1) * .5f,
                    normal = new vec3(-1, 0, 0),
                    color = new vec4(1, 1, 1, 1),
                    uv = new vec2(0, 0)
                },
                new Vertex()
                {
                    position = new vec3(-1, -1, -1) * .5f,
                    normal = new vec3(-1, 0, 0),
                    color = new vec4(1, 1, 1, 1),
                    uv = new vec2(0, 1)
                },

                //BACK
                new Vertex()
                {
                    position = new vec3(+1, -1, +1) * .5f,
                    normal = new vec3(0, 0, +1),
                    color = new vec4(1, 1, 1, 1),
                    uv = new vec2(1, 1)
                },
                new Vertex()
                {
                    position = new vec3(+1, +1, +1) * .5f,
                    normal = new vec3(0, 0, +1),
                    color = new vec4(1, 1, 1, 1),
                    uv = new vec2(1, 0)
                },
                new Vertex()
                {
                    position = new vec3(-1, +1, +1) * .5f,
                    normal = new vec3(0, 0, +1),
                    color = new vec4(1, 1, 1, 1),
                    uv = new vec2(0, 0)
                },
                new Vertex()
                {
                    position = new vec3(-1, -1, +1) * .5f,
                    normal = new vec3(0, 0, +1),
                    color = new vec4(1, 1, 1, 1),
                    uv = new vec2(0, 1)
                },

                //RIGHT
                new Vertex()
                {
                    position = new vec3(+1, -1, -1) * .5f,
                    normal = new vec3(+1, 0, 0),
                    color = new vec4(1, 1, 1, 1),
                    uv = new vec2(1, 1)
                },
                new Vertex()
                {
                    position = new vec3(+1, +1, -1) * .5f,
                    normal = new vec3(+1, 0, 0),
                    color = new vec4(1, 1, 1, 1),
                    uv = new vec2(1, 0)
                },
                new Vertex()
                {
                    position = new vec3(+1, +1, +1) * .5f,
                    normal = new vec3(+1, 0, 0),
                    color = new vec4(1, 1, 1, 1),
                    uv = new vec2(0, 0)
                },
                new Vertex()
                {
                    position = new vec3(+1, -1, +1) * .5f,
                    normal = new vec3(+1, 0, 0),
                    color = new vec4(1, 1, 1, 1),
                    uv = new vec2(0, 1)
                },
            }
        );

        public static Mesh SmoothCube = LoadObjFile("smooth_cube.obj");
        public static Mesh Missile = LoadObjFile("missile.obj");

        public uint VAO { get; private set; }
        public uint VBO { get; private set; }
        public uint EBO { get; private set; }

        public uint[] indices;
        public Vertex[] vertices;

        public Mesh() { }

        public static Mesh Create(uint[] indices, Vertex[] vertices)
        {
            Mesh mesh = new()
            {
                indices = indices,
                vertices = vertices
            };

            mesh.CalculateTangents();
            mesh.GenerateBuffers();

            Console.WriteLine($"Created model with {indices.Length / 3} triangles");

            return mesh;
        }

        public static Mesh LoadObjFile(string objFilePath)
        {
            if (!File.Exists(directoryPath + objFilePath))
            {
                Console.WriteLine($"Error: Fragment shader not found at path {directoryPath + objFilePath}");
                return null;
            }


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
                    normals.Add(new vec3(x, y, z).Normalized());
                }

                else if (line.StartsWith("f "))
                {
                    string[] split = line.Substring(2).Split(" ");

                    void AddVertex(string t)
                    {
                        string[] div = t.Split("/");
                        int positionIndex = int.Parse(div[0]) - 1;
                        int uvIndex = int.Parse(div[1]) - 1;
                        int normalIndex = int.Parse(div[2]) - 1;

                        indices.Add((uint)vertices.Count);
                        vertices.Add(new()
                        {
                            position = positions[positionIndex],
                            uv = uvs[uvIndex],
                            normal = normals[normalIndex]
                        });
                    }

                    for (int i = 0; i < split.Length - 2; i++)
                    {
                        AddVertex(split[i + 2]);
                        AddVertex(split[0]);
                        AddVertex(split[i + 1]);
                    }
                }
            }

            Mesh mesh = new()
            {
                vertices = vertices.ToArray(),
                indices = indices.ToArray()
            };

            mesh.CalculateTangents();
            mesh.GenerateBuffers();

            Console.WriteLine($"Loaded model from .obj file {objFilePath} with {indices.Count / 3} triangles");

            return mesh;
        }

        public void Print()
        {
            foreach (Vertex v in vertices)
                Console.WriteLine($"P{v.position} U{v.uv} N{v.normal} T{v.tangent} B{v.bitangent}");
        }

        private void GenerateBuffers()
        {
            VAO = glGenVertexArray();
            VBO = glGenBuffer();
            EBO = glGenBuffer();

            glBindVertexArray(VAO);

            //VERTEX BUFFER
            unsafe
            {

                glBindBuffer(GL_ARRAY_BUFFER, VBO);
                fixed (Vertex* v = &vertices[0])
                {
                    glBufferData(GL_ARRAY_BUFFER, sizeof(Vertex) * vertices.Length, v, GL_STATIC_DRAW);
                }

                //INDEX BUFFER
                glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, EBO);
                fixed (uint* i = &indices[0])
                {
                    glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(uint) * indices.Length, i, GL_STATIC_DRAW);
                }

                int offset = 0;

                //POS
                glEnableVertexAttribArray(0);
                glVertexAttribPointer(0, 3, GL_FLOAT, false, sizeof(Vertex), new IntPtr(offset));
                offset += sizeof(vec3);

                //NORMAL
                glEnableVertexAttribArray(1);
                glVertexAttribPointer(1, 3, GL_FLOAT, false, sizeof(Vertex), new IntPtr(offset));
                offset += sizeof(vec3);

                //COLOR
                glEnableVertexAttribArray(2);
                glVertexAttribPointer(2, 4, GL_FLOAT, false, sizeof(Vertex), new IntPtr(offset));
                offset += sizeof(vec4);

                //UV
                glEnableVertexAttribArray(3);
                glVertexAttribPointer(3, 2, GL_FLOAT, false, sizeof(Vertex), new IntPtr(offset));
                offset += sizeof(vec2);

                //TANGENT
                glEnableVertexAttribArray(4);
                glVertexAttribPointer(4, 3, GL_FLOAT, false, sizeof(Vertex), new IntPtr(offset));
                offset += sizeof(vec3);

                //BITANGENT
                glEnableVertexAttribArray(5);
                glVertexAttribPointer(5, 3, GL_FLOAT, false, sizeof(Vertex), new IntPtr(offset));
                offset += sizeof(vec3);

            }

            glBindVertexArray(0);
        }

        public void CalculateTangents()
        {
            for (int i = 0; i < indices.Length - 2; i += 3)
            {
                uint i0 = indices[i + 0];
                uint i1 = indices[i + 1];
                uint i2 = indices[i + 2];

                Vertex v0 = vertices[i0];
                Vertex v1 = vertices[i1];
                Vertex v2 = vertices[i2];

                vec3 edge1 = v1.position - v0.position;
                vec3 edge2 = v2.position - v0.position;
                vec2 deltaUV1 = v1.uv - v0.uv;
                vec2 deltaUV2 = v2.uv - v0.uv;
                deltaUV1.y = 1 - deltaUV1.y;
                deltaUV2.y = 1 - deltaUV2.y;

                float f = (deltaUV1.x * deltaUV2.y - deltaUV1.y * deltaUV2.x);

                f = f == 0 ? 1 : 1 / f;

                vec3 tangent = ((edge1 * deltaUV2.y - edge2 * deltaUV1.y) * f).Normalized();
                vec3 bitangent = ((edge2 * deltaUV1.x - edge1 * deltaUV2.x) * f).Normalized();

                v0.tangent = v1.tangent = v2.tangent = tangent;
                v0.bitangent = v1.bitangent = v2.bitangent = bitangent;

                vertices[i0] = v0;
                vertices[i1] = v1;
                vertices[i2] = v2;
            }
        }

        private static vec3 Cross(vec3 a, vec3 b)
        {
            return new vec3
            (
                a.y * b.z - a.z * b.y,
                a.z * b.x - a.x * b.z,
                a.x * b.y - a.y * b.x
            );
        }

        public void Render()
        {
            glBindVertexArray(VAO);
            unsafe
            {
                glDrawElements(GL_TRIANGLES, indices.Length, GL_UNSIGNED_INT, (void*)0);
            }
            glBindVertexArray(0);
        }

        
    }

    struct Vertex 
    {
        public vec3 position;
        public vec3 normal;
        public vec4 color;
        public vec2 uv;
        public vec3 tangent;
        public vec3 bitangent;
    
        public Vertex(float x, float y, float z)
        {
            position = new vec3(x, y, z);
            normal = new vec3();
            color = new vec4(1, 1, 1, 1);
            uv = new vec2();

            tangent = new vec3();
            bitangent = new vec3();
        }

        public Vertex(vec3 position, vec3 normal, vec4 color, vec2 uv)
        {
            this.position = position;
            this.normal = normal;
            this.color = color;
            this.uv = uv;

            tangent = new vec3();
            bitangent = new vec3();
        }

        public Vertex(vec3 position, vec2 uv)
        {
            this.position = position;
            normal = new vec3();
            color = new vec4(1, 1, 1, 1);
            this.uv = uv;


            tangent = new vec3();
            bitangent = new vec3();
        }
    }
}
