﻿using OpenTK;
using OpenTK.Graphics;
using Whirlpool.Core.IO;
using Whirlpool.Core.Render.Nova;

namespace Whirlpool.Core.Render
{
    public partial class Renderer
    {
        public static void RenderFramebuffer(Vector2 position, Vector2 size, Texture texture)
        {
            LegacyInterface.RenderFramebuffer(position, size, texture);
        }

        public static void RenderQuad(Vector2 position, Vector2 size, Texture texture, Color4 tint, Material material = null, float textureRepetitions = 1, float rotation = 0, FlipMode flipMode = FlipMode.None, float opacity = 1.0f)
        {
            LegacyInterface.RenderQuad(position, size, texture, tint, material, textureRepetitions, rotation, flipMode, opacity);
        }

        public static void RenderQuad(Vector2 position, Vector2 size, string texture, Color4 tint, Material material = null, float textureRepetitions = 1, float rotation = 0, FlipMode flipMode = FlipMode.None, float opacity = 1.0f)
        {
            LegacyInterface.RenderQuad(position, size, GetTextureFromString(texture), tint, material, textureRepetitions, rotation, flipMode, opacity);
        }

        public static void RenderAtlas(Vector2 position, Vector2 size, Vector2 texturePoint, Vector2 textureSize, Texture texture)
        {
            LegacyInterface.RenderAtlas(position, size, texturePoint, textureSize, texture);
        }

        protected static Texture GetTextureFromString(string texture)
        {
            return FileBank.GetTexture(texture);
        }

        public static void RenderMesh(Mesh mesh, Vector3 position, Vector3 size, Quaternion rotation, Texture texture, Material material = null)
        {
            LegacyInterface.RenderMesh(mesh, position, size, rotation, texture, material);
        }

        public static void Init()
        {
            LegacyInterface.Init();
        }
    }
}
