using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace Whirlpool.Core.Render
{
    public class Render3D
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

        public static void DrawMesh(Mesh mesh, Vector3 position, Vector3 scale, Quaternion rotation, Quaternion localRotation, Texture texture = null, Material material = null)
        {
            var indexed = (mesh.EBO != -1);

            GL.BindVertexArray(mesh.VAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, mesh.VBO);
            if (indexed) GL.BindBuffer(BufferTarget.ElementArrayBuffer, mesh.EBO);
            if (material == null) material = defaultMaterial;

            material?.Use();
            texture?.Bind();

            Matrix4 model = Matrix4.CreateTranslation(position + sceneCamera.worldPosition); // lol??? TODO: maybe dont do this
            model.Transpose();
            model = Matrix4.CreateFromQuaternion(rotation) * model * Matrix4.CreateFromQuaternion(localRotation) * Matrix4.CreateScale(scale);
            model.Transpose();

            Matrix4 mvp = model * sceneCamera.view * sceneCamera.projection;
            
            material?.SetVariables(new Dictionary<string, Type.Any>(){
                { "AlbedoTexture", 0 },
                { "Model", model },
                { "View", sceneCamera.view },
                { "Projection", sceneCamera.projection },
                { "MVP", mvp }
            });

            GL.DrawArrays(PrimitiveType.Triangles, 0, mesh.vertexIndices.Count);
        }
    }
}
