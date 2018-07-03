﻿using OpenTK;
using OpenTK.Graphics;
using Whirlpool.Core.IO;

namespace Whirlpool.Core.Render
{
    public partial class Renderer
    {
        public static void RenderFramebuffer(Vector2 position, Vector2 size, Texture texture)
        {
            GetInstance()._RenderFramebuffer(position, size, texture);
        }

        public static void RenderQuad(Vector2 position, Vector2 size, Texture texture, Color4 tint, Material material = null, float textureRepetitions = 1, float rotation = 0, FlipMode flipMode = FlipMode.None)
        {
            GetInstance()._RenderQuad(position, size, texture, textureRepetitions, tint, rotation, flipMode, material);
        }

        public static void RenderQuad(Vector2 position, Vector2 size, string texture, Color4 tint, Material material = null, float textureRepetitions = 1, float rotation = 0, FlipMode flipMode = FlipMode.None)
        {
            GetInstance()._RenderQuad(position, size, GetTextureFromString(texture), textureRepetitions, tint, rotation, flipMode, material);
        }

        protected static Texture GetTextureFromString(string texture)
        {
            return FileBank.GetTexture(texture);
        }

        public static void RenderMesh(Mesh mesh, Vector3 position, Vector3 size, Vector3 rotation)
        {
            GetInstance()._RenderMesh(mesh, position, size, rotation);
        }
    }
}