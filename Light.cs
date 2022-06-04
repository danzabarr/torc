using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlmNet;
using static OpenGL.GL;

namespace torc
{
    public abstract class Light : Component
    {
        public static vec4 AmbientLight = new(1, 1, 1, 0.0f);

        public vec3 color = new vec3(1, 1, 1);
        public float brightness = 1;
        public float specularStrength = 1f;
        public float specularPower = 128f;
    }

    public class DirectionalLight : Light
    {
        public static DirectionalLight main;

        public static int ShadowMapWidth = 1024;
        public static int ShadowMapHeight = 1024;

        public static Shader ShadowMap = Shader.Load("shadow_map.vert", "shadow_map.frag");

        private uint depthMapFBO;
        private uint depthMap;

        public mat4 LightSpaceMatrix { get; private set; }

        public uint DepthMap => depthMap;

        public DirectionalLight() : base()
        {
            if (main == null)
                main = this;
            GenerateShadowMap();
        }

        public unsafe void GenerateShadowMap()
        {
            depthMapFBO = glGenFramebuffer();

            depthMap = glGenTexture();
            glBindTexture(GL_TEXTURE_2D, depthMap);
            glTexImage2D(GL_TEXTURE_2D, 0, GL_DEPTH_COMPONENT, ShadowMapWidth, ShadowMapHeight, 0, GL_DEPTH_COMPONENT, GL_FLOAT, (void*)0);
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);

            glBindFramebuffer(GL_FRAMEBUFFER, depthMapFBO);
            glFramebufferTexture2D(GL_FRAMEBUFFER, GL_DEPTH_ATTACHMENT, GL_TEXTURE_2D, depthMap, 0);
            glDrawBuffer(GL_NONE);
            glReadBuffer(GL_NONE);
            glBindFramebuffer(GL_FRAMEBUFFER, 0);
        }

        private mat4 CalculateLightSpaceMatrix()
        {
            mat4 lightProjection, lightView;
            mat4 lightSpaceMatrix;
            float near_plane = 0.1f, far_plane = 100f;
            lightProjection = glm.ortho(-10.0f, 10.0f, -10.0f, 10.0f, near_plane, far_plane);
            lightView = glm.lookAt(Object.Position, Object.Position + Object.Forward, new vec3(0, 1, 0));
            lightSpaceMatrix = lightProjection * lightView;




            return lightSpaceMatrix;
        }

        public void RenderScene()
        {
            glBindFramebuffer(GL_FRAMEBUFFER, depthMapFBO);
            glFramebufferTexture2D(GL_FRAMEBUFFER, GL_DEPTH_ATTACHMENT, GL_TEXTURE_2D, depthMap, 0);
            glDrawBuffer(GL_NONE);
            glReadBuffer(GL_NONE);
            glBindFramebuffer(GL_FRAMEBUFFER, 0);

            LightSpaceMatrix = CalculateLightSpaceMatrix();
            // render scene from light's point of view
            ShadowMap.Use();
            ShadowMap.UniformMatrix4fv("lightSpaceMatrix", LightSpaceMatrix);

            glViewport(0, 0, ShadowMapWidth, ShadowMapHeight);
            glBindFramebuffer(GL_FRAMEBUFFER, depthMapFBO);
            glClear(GL_DEPTH_BUFFER_BIT);
            glActiveTexture(GL_TEXTURE0);
            Program.activeScene.Render(ShadowMap);
            glBindFramebuffer(GL_FRAMEBUFFER, 0);
        }
    }
}
