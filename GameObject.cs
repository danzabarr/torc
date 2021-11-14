using GlmNet;
using System;
using System.Collections;
using System.Collections.Generic;

namespace torc
{
    public sealed class GameObject : Component.Entity
    {
        public GameObject() : base() { }

        public override string ToString()
        {
            vec3 position = LocalPosition;
            vec3 eulers = EulerAngles;
            return $"GameObject: {position.x},{position.y},{position.z} ({eulers.x},{eulers.y},{eulers.z})";
        }

        public void Update()
        {
            foreach (Component c in EnumerateComponents())
                c.Update();
        }

        public void Render()
        {
            foreach (Renderer r in EnumerateComponents<Renderer>())
                r.Render();
        }
    }

    public abstract class Component
    {
        private Entity entity { get; set; }
        public GameObject Object => entity as GameObject;

        public virtual void Update() { }

        public abstract class Entity : IEnumerable<GameObject>
        {
            private mat4 m;
            private GameObject parent;
            private List<GameObject> children;
            private readonly List<Component> components;
            public mat4 Matrix => m;

            protected Entity()
            {
                m = mat4.identity();
                components = new();
                children = new();
            }

            public GameObject Parent
            {
                get => parent;
                set
                {
                    if (parent != null)
                    {
                        parent.children.Remove(this as GameObject);
                    }
                    parent = value;
                    parent.children.Add(this as GameObject);
                }
            }

            private mat4 WorldMatrix
            {
                get
                {
                    if (Parent == null)
                        return m;
                    else
                        return Parent.WorldMatrix * m;
                }
            }

            public vec3 LocalPosition
            {
                get => new(m[3, 0], m[3, 1], m[3, 2]);
                set
                {
                    m[3, 0] = value.x;
                    m[3, 1] = value.y;
                    m[3, 2] = value.z;
                }
            }

            public vec3 LocalScale
            {
                get => new(m[0, 0], m[1, 1], m[2, 2]);
                set
                {
                    m[0, 0] = value.x;
                    m[1, 1] = value.y;
                    m[2, 2] = value.z;
                }
            }

            public vec3 Position
            {
                get
                {
                    mat4 world = WorldMatrix;
                    return new(world[3, 0], world[3, 1], world[3, 2]);
                }
            }

            public vec3 EulerAngles
            {
                get
                {
                    if (m[0, 0] == 1.0f)
                        return new vec3(glm.degrees(glm.atan(m[0, 2], m[2, 3])), 0, 0);
                    else if (m[0, 0] == -1.0f)
                        return new vec3(glm.degrees(glm.atan(m[0, 2], m[2, 3])), 0, 0);
                    else
                        return new vec3(glm.degrees(glm.atan(-m[2, 0], m[0, 0])), glm.degrees(glm.asin(m[1, 0])), glm.degrees(glm.atan(-m[1, 2], m[1, 1])));
                }
                set
                {

                    vec3 position = LocalPosition;
                    vec3 scale = LocalScale;
                    vec3 rotation = value;

                    m = mat4.identity();
                    Translate(position);
                    //Scale(scale);
                    Rotate(rotation.x, new vec3(1, 0, 0));
                    Rotate(rotation.y, new vec3(0, 1, 0));
                    Rotate(rotation.z, new vec3(0, 0, 1));
                    return;
                    /*
                    float pitch = glm.radians(value.x);
                    float yaw = glm.radians(value.y);
                    float roll = glm.radians(value.z);

                    float cosPitch = glm.cos(pitch);
                    float sinPitch = glm.sin(pitch);

                    float cosYaw = glm.cos(yaw);
                    float sinYaw = glm.sin(yaw);

                    float cosRoll = glm.cos(roll);
                    float sinRoll = glm.sin(roll);

                    m[0, 0] = cosPitch * cosYaw;
                    m[0, 1] = cosPitch * sinYaw;
                    m[0, 2] = -sinPitch;
                    m[1, 0] = sinRoll * sinPitch * cosYaw - cosRoll * sinYaw;
                    m[1, 1] = sinRoll * sinPitch * sinYaw + cosRoll * cosPitch;
                    m[1, 2] = cosPitch * sinRoll;
                    m[2, 0] = cosRoll * sinPitch * cosYaw + sinRoll * sinYaw;
                    m[2, 1] = cosRoll * sinPitch * sinYaw - sinRoll * cosPitch;
                    m[2, 2] = cosPitch * cosRoll;
                    */
                }
            }

            public void Translate(float x, float y, float z)
            {
                m[3] = m[0] * x + m[1] * y + m[2] * z + m[3];
            }

            public void Translate(vec3 v)
            {
                m[3] = m[0] * v[0] + m[1] * v[1] + m[2] * v[2] + m[3];
            }

            public void Scale(float f)
            {
                m[0] *= f;
                m[1] *= f;
                m[2] *= f;
            }

            public void Scale(vec3 v)
            {
                m[0] *= v.x;
                m[1] *= v.y;
                m[2] *= v.z;
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

                vec4 v0 = m[0] * rotate[0][0] + m[1] * rotate[0][1] + m[2] * rotate[0][2];
                vec4 v1 = m[0] * rotate[1][0] + m[1] * rotate[1][1] + m[2] * rotate[1][2];
                vec4 v2 = m[0] * rotate[2][0] + m[1] * rotate[2][1] + m[2] * rotate[2][2];

                m[0] = v0;
                m[1] = v1;
                m[2] = v2;
            }

            public vec3 Forward
            {
                get
                {
                    vec4 forward = new vec4(0, 0, 1, 0);

                    forward = m * forward;
                    Console.WriteLine(forward);
                    return new vec3(forward.x, forward.y, forward.z);
                }
            }

            public GameObject this[int index] => children[index];
            public IEnumerator<GameObject> GetEnumerator() => children.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => children.GetEnumerator();

            public T GetComponentInParent<T>() where T : Component
            {
                foreach (Component c in components)
                {
                    if (c.GetType() == typeof(T))
                        return c as T;
                }

                return Parent?.GetComponentInParent<T>();
            }
            public int ComponentCount => components.Count;

            public Component GetComponent(int index) => components[index];

            public T GetComponent<T>() where T : Component
            {
                foreach (Component c in components)
                    if (c.GetType() == typeof(T))
                        return (T)c;
                return null;
            }

            public IEnumerable EnumerateComponents()
            {
                foreach (Component c in components)
                    yield return c;
            }

            public IEnumerable<T> EnumerateComponents<T>() where T : Component
            {
                foreach (Component c in components)
                {
                    if (typeof(T).IsAssignableFrom(c.GetType()))
                        yield return c as T;
                }
            }

            public Component[] GetComponents()
            {
                return components.ToArray();
            }

            public List<T> GetComponents<T>() where T : Component
            {
                List<T> list = new();
                foreach (T t in EnumerateComponents<T>())
                    list.Add(t);
                return list;
            }

            public T AddComponent<T>() where T : Component, new()
            {
                T component = new();
                component.entity = this;
                components.Add(component);
                return component;
            }

            public bool RemoveComponent(Component c)
            {
                if (c == null)
                    return false;

                return components.Remove(c);
            }

            public void RemoveComponentAt(int index)
            {
                components.RemoveAt(index);
            }

            public int RemoveComponents(Predicate<Component> match)
            {
                if (match == null)
                    return 0;

                return components.RemoveAll(match);
            }

            public int RemoveComponents<T>() where T : Component
            {
                return RemoveComponents((Component c) => { return c.GetType() == typeof(T); });
            }
        }
    }
}