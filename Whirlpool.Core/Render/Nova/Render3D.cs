using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace Whirlpool.Core.Render.Nova
{
    public class Render3D
    {
        public static Camera sceneCamera;
        private static Material defaultUnlitMaterial;

        public static void Init()
        {
            PostProcessing.GetInstance().Init(new Vector2(BaseGame.Size.Width, BaseGame.Size.Height));
            sceneCamera = new Camera();
            sceneCamera.viewportSize = new Vector2(BaseGame.Size.Width, BaseGame.Size.Height);
            defaultUnlitMaterial = new MaterialBuilder()
                .Build()
                .Attach(new Shader("Shaders\\vert.glsl", ShaderType.VertexShader))
                .Attach(new Shader("Shaders\\frag.glsl", ShaderType.FragmentShader))
                .Link()
                .GetMaterial();
        }

        public static void DrawMesh(Mesh mesh, Vector3 position, Vector3 scale, Quaternion rotation, Texture texture = null, Material material = null)
        {
            var indexed = (mesh.EBO != -1);

            GL.BindVertexArray(mesh.VAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, mesh.VBO);
            if (indexed) GL.BindBuffer(BufferTarget.ElementArrayBuffer, mesh.EBO);
            if (material == null) material = defaultUnlitMaterial;

            material?.Use();
            texture?.Bind();

            Matrix4 model = Matrix4.CreateTranslation(position);
            model.Transpose();
            model *= Matrix4.CreateScale(scale) * Matrix4.CreateFromQuaternion(rotation);
            model.Transpose();

            Matrix4 mvp = model * sceneCamera.view * sceneCamera.projection;
            
            material?.SetVariables(new List<Tuple<string, Type.Any>>{
                new Tuple<string, Type.Any>("AlbedoTexture", 0),
                new Tuple<string, Type.Any>("Model", model),
                new Tuple<string, Type.Any>("View", sceneCamera.view),
                new Tuple<string, Type.Any>("Projection", sceneCamera.projection),
                new Tuple<string, Type.Any>("MVP", mvp)
            });

            GL.DrawArrays(PrimitiveType.Triangles, 0, mesh.vertexIndices.Count);
        }
    }
}
