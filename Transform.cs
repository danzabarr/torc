using GlmNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace torc
{
    class Transform : Component
    {
        private mat4 matrix;
        public Transform Parent { get; set; }
        private mat4 WorldMatrix
        {
            get
            {
                if (Parent == null)
                    return matrix;
                else
                    return Parent.WorldMatrix * matrix;
            }
        }
        
        public Transform(float x, float y, float z)
        {
            matrix = mat4.identity();
            matrix[3, 0] = x;
            matrix[3, 1] = y;
            matrix[3, 2] = z;
        }

        public Transform(vec3 position)
        {
            matrix = mat4.identity();
            LocalPosition = position;
        }

        public vec3 LocalPosition
        {
            get => new(matrix[3, 0], matrix[3, 1], matrix[3, 2]);
            set {
                matrix[3, 0] = value.x;
                matrix[3, 1] = value.y;
                matrix[3, 2] = value.z;
            }
        }

        public vec3 LocalScale
        {
            get => new(matrix[0, 0], matrix[1, 1], matrix[2, 2]);
            private set
            {
                matrix[0, 0] = value.x;
                matrix[1, 1] = value.y;
                matrix[2, 2] = value.z;
            }
        }

        public void Translate(vec3 v)
        {
            matrix[3] = matrix[0] * v[0] + matrix[1] * v[1] + matrix[2] * v[2] + matrix[3];
        }

        public void Scale(float f)
        {
            matrix[0] *= f;
            matrix[1] *= f;
            matrix[2] *= f;
        }

        public void Scale(vec3 v)
        {
            matrix[0] *= v.x;
            matrix[1] *= v.y;
            matrix[2] *= v.z;
        }

        public void Rotate(float degrees, vec3 v)
        {
            float radians = glm.radians(degrees);
            float c = glm.cos(radians);
            float s = glm.sin(radians);

            vec3 axis = glm.normalize(v);
            vec3 temp = (1.0f - c) * axis;

            mat4 rotate = mat4.identity();
            rotate[0, 0] = c + temp[0] * axis[0];
            rotate[0, 1] = 0 + temp[0] * axis[1] + s * axis[2];
            rotate[0, 2] = 0 + temp[0] * axis[2] - s * axis[1];

            rotate[1, 0] = 0 + temp[1] * axis[0] - s * axis[2];
            rotate[1, 1] = c + temp[1] * axis[1];
            rotate[1, 2] = 0 + temp[1] * axis[2] + s * axis[0];

            rotate[2, 0] = 0 + temp[2] * axis[0] + s * axis[1];
            rotate[2, 1] = 0 + temp[2] * axis[1] - s * axis[0];
            rotate[2, 2] = c + temp[2] * axis[2];

            matrix[0] = matrix[0] * rotate[0][0] + matrix[1] * rotate[0][1] + matrix[2] * rotate[0][2];
            matrix[1] = matrix[0] * rotate[1][0] + matrix[1] * rotate[1][1] + matrix[2] * rotate[1][2];
            matrix[2] = matrix[0] * rotate[2][0] + matrix[1] * rotate[2][1] + matrix[2] * rotate[2][2];
        }

        public vec3 EulerAngles
        {
            get
            {
                if (matrix[0, 0] == 1.0f)
                {
                    return new vec3
                    (
                        glm.degrees(glm.atan(matrix[0, 2], matrix[2, 3])),
                        0,
                        0
                    );

                }
                else if (matrix[0,0] == -1.0f)
                {
                    return new vec3
                    (
                        glm.degrees(glm.atan(matrix[0, 2], matrix[2, 3])),
                        0,
                        0
                    );
                }
                else
                {
                    return new vec3
                    (
                        glm.degrees(glm.atan(-matrix[2,0], matrix[0,0])),
                        glm.degrees(glm.asin(matrix[1, 0])),
                        glm.degrees(glm.atan(-matrix[1, 2], matrix[1,1]))
                    );
                }
            }
        }

        public override string ToString()
        {
            vec3 eulers = EulerAngles;
            return $"{matrix[3, 0]}, {matrix[3, 1]}, {matrix[3, 2]} ({eulers.x},{eulers.y},{eulers.z})";
        }
    }
}
