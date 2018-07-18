using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whirlpool.Core.Render.Nova
{
    /// <summary>
    /// Legacy interface for use until everything is transitioned over to the Nova code.
    /// </summary>
    public class LegacyInterface
    {
        public static void RenderFramebuffer(Vector2 position, Vector2 size, Texture texture)
        {
            Render2D.DrawFramebuffer(position, size, texture);
        }

        public static void RenderQuad(Vector2 position, Vector2 size, Texture texture, Color4 tint, Material material = null, float textureRepetitions = 1, float rotation = 0, FlipMode flipMode = FlipMode.None, float opacity = 1.0f)
        {
            Render2D.DrawQuad(position, size, texture, material, rotation, flipMode);
        }

        public static void RenderQuad(Vector2 position, Vector2 size, string texture, Color4 tint, Material material = null, float textureRepetitions = 1, float rotation = 0, FlipMode flipMode = FlipMode.None, float opacity = 1.0f)
        {
            throw new Exception("Obsolete function");
        }

        public static void RenderAtlas(Vector2 position, Vector2 size, Vector2 texturePoint, Vector2 textureSize, Texture texture)
        {
            throw new Exception("Obsolete function");
        }

        public static void RenderMesh(Mesh mesh, Vector3 position, Vector3 size, Quaternion rotation, Texture texture, Material material = null)
        {
            Render3D.DrawMesh(mesh, position, size, rotation, texture, material);
        }

        public static void Init()
        {
            Render3D.Init();
            Render2D.Init();
        }
    }
}
