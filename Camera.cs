using GlmNet;

namespace torc
{
    public enum CameraMode
    {
        Perspective,
        Orthographic
    }

    class Camera : Component
    {
        private mat4 proj;
        private CameraMode mode = CameraMode.Perspective;
        private float fovy = glm.radians(30);
        private float aspect = 16 / 9;
        private float zNear = 0.01f;
        private float zFar = 1000f;
        private float left, right, bottom, top;

        public Camera()
        {
            UpdateProjection();
        }

        public mat4 ProjectionMatrix => proj;
        public mat4 ViewMatrix => glm.inverse(Object.Matrix);

        public CameraMode Mode
        {
            get => mode;
            set { mode = value; UpdateProjection(); }
        }
        public float Fovy
        {
            get => fovy;
            set { fovy = value; UpdateProjection(); }
        }
        public float Aspect
        {
            get => aspect;
            set { aspect = value; UpdateProjection(); }
        }

        public float ZNear
        {
            get => zNear;
            set { zNear = value; UpdateProjection(); }
        }

        public float ZFar
        {
            get => zFar;
            set { zFar = value; UpdateProjection(); }
        }

        public float Left
        {
            get => left;
            set { left = value; UpdateProjection(); }
        }

        public float Right
        {
            get => right;
            set { right = value; UpdateProjection(); }
        }

        public float Bottom
        {
            get => bottom;
            set { bottom = value; UpdateProjection(); }
        }

        public float Top
        {
            get => top;
            set { top = value; UpdateProjection(); }
        }

        private void UpdateProjection()
        {
            proj = mode switch
            {                
                CameraMode.Perspective => glm.perspective(fovy, aspect, zNear, zFar),
                CameraMode.Orthographic => glm.ortho(left, right, bottom, top, zNear, zFar),
                _ => mat4.identity()
            };
        }
    }
}
