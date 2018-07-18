using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whirlpool.Core.Type;

namespace Whirlpool.Core.Render
{
    public class Mesh
    {
        public List<Vector3> vertices = new List<Vector3>();
        public List<Vector2> texCoords = new List<Vector2>();
        public List<Vector3> normals = new List<Vector3>();

        public List<uint> vertexIndices = new List<uint>();
        public List<uint> normalIndices = new List<uint>();
        public List<uint> textureIndices = new List<uint>();

        public int VAO, VBO, EBO;

        public void GenerateBuffers()
        {
            // Generate VAO, VBO
            GL.GenVertexArrays(1, out VAO);
            GL.GenBuffers(1, out VBO);
            GL.GenBuffers(1, out EBO);

            // Buffer data
            uint[] vertexIndices_ = vertexIndices.ToArray();
            float[] vertices_ = new float[vertexIndices_.Length * 8];
            //for (int i = 0; i < vertices.Count; ++i)
            for (int i = vertexIndices.Count - 1; i >= 0; --i)
            {
                vertices_[i * 8 + 7] = normals[(int)normalIndices[i]].X;
                vertices_[i * 8 + 6] = normals[(int)normalIndices[i]].Y;
                vertices_[i * 8 + 5] = normals[(int)normalIndices[i]].Z;

                vertices_[i * 8 + 4] = texCoords[(int)textureIndices[i]].Y;
                vertices_[i * 8 + 3] = texCoords[(int)textureIndices[i]].X;

                //vertices_[i * 8 + 4] = texCoords[i].X;
                //vertices_[i * 8 + 3] = texCoords[i].Y;

                vertices_[i * 8 + 2] = vertices[(int)vertexIndices[i]].X;
                vertices_[i * 8 + 1] = vertices[(int)vertexIndices[i]].Y;
                vertices_[i * 8] = vertices[(int)vertexIndices[i]].Z;
            }

            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices_.Length * sizeof(float), vertices_, BufferUsageHint.StaticDraw);
            //GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            //GL.BufferData(BufferTarget.ElementArrayBuffer, vertexIndices_.Length * sizeof(uint), vertexIndices_, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0 * sizeof(float));
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 5 * sizeof(float));
            GL.EnableVertexAttribArray(2);
        }
    }
}
