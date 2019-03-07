using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using Whirlpool.Core.Type;

namespace Whirlpool.Core.Render
{
    public class Renderer3D
    {
        public static Camera sceneCamera;
        public static Material defaultMaterial;

        public static void Init()
        {
            PostProcessing.GetInstance().Init(new Vector2(BaseGame.Size.Width, BaseGame.Size.Height), GlobalSettings.Default.renderResolutionX, GlobalSettings.Default.renderResolutionY);
            sceneCamera = new Camera();
            sceneCamera.viewportSize = new Vector2(GlobalSettings.Default.renderResolutionX, GlobalSettings.Default.renderResolutionY);
            defaultMaterial = new MaterialBuilder()
                .Build()
                .SetName("Default Material")
                .Attach(new Shader("Shaders\\3D\\vert.glsl", ShaderType.VertexShader))
                .Attach(new Shader("Shaders\\3D\\frag.glsl", ShaderType.FragmentShader))
                .Link()
                .GetMaterial();
        }

        public static void TestOptimizedDraw(int indexCount, Vector3 position, Vector3 scale, Quaternion rotation, Quaternion localRotation, Texture texture, Material material)
        {
            Matrix4 model = Matrix4.CreateTranslation(position);
            model.Transpose();
            model = Matrix4.CreateFromQuaternion(rotation) * model * Matrix4.CreateFromQuaternion(localRotation) * Matrix4.CreateScale(scale);
            model.Transpose();
            material.SetVariable("Model", model);

            GL.DrawArrays(PrimitiveType.Triangles, 0, indexCount);
        }

        public static void DrawBatchedMesh(int indexCount, Vector3 position, Vector3 scale, Quaternion rotation, Texture texture = null, Material material = null, Dictionary<string, Any> materialParameters = null)
        {
            if (material == null) material = defaultMaterial;

            Matrix4 model = Matrix4.CreateTranslation(position);
            model.Transpose();
            model = model * Matrix4.CreateFromQuaternion(rotation) * Matrix4.CreateScale(scale);
            model.Transpose();

            material.Use();

            if (texture != null)
            {
                texture.Bind();
                //material.SetVariable("AlbedoTexture", 0);
            }

            material.SetVariables(new Dictionary<string, Type.Any>(){
                { "Model", model },
                { "View", sceneCamera.view },
                { "Projection", sceneCamera.projection },
            });

            if (materialParameters != null) material?.SetVariables(materialParameters);

            GL.DrawArrays(PrimitiveType.Triangles, 0, indexCount);
        }

        public static void DrawMesh(Mesh mesh, Vector3 position, Vector3 scale, Quaternion rotation, Texture texture = null, Material material = null, Dictionary<string, Any> materialParameters = null)
        {
            GL.BindVertexArray(mesh.VAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, mesh.VBO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, mesh.EBO);
            DrawBatchedMesh(mesh.vertexIndices.Count, position, scale, rotation, texture, material, materialParameters);
        }
    }
}
