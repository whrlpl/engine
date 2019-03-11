using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.IO;
using Whirlpool.Core.IO;
using Whirlpool.Shared;

namespace Whirlpool.Core.Render.Type
{
    public class Mesh : IAsset
    {
        byte[] data;
        byte[] IAsset.data { get => data; set => data = value; }
        string fileName;
        string IAsset.fileName { get { return fileName; } set { fileName = value; } }

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
            //GL.GenBuffers(1, out EBO);

            // Buffer data
            uint[] vertexIndices_ = vertexIndices.ToArray();
            float[] vertices_ = new float[vertexIndices_.Length * 8];
            for (int i = 0; i < vertexIndices.Count; ++i)
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

        public void LoadAsset(PackageFile file)
        {
            LoadDataAsset(file.fileName, file.fileData);
        }

        private static int CountInstancesOfCharInString(string s, char c)
        {
            var i = 0;
            foreach (char c_ in s)
            {
                if (c_ == c)
                {
                    ++i;
                }
            }
            return i;
        }

        public override string ToString()
        {
            return this.fileName;
        }

        public void LoadDataAsset(string fileName, byte[] data)
        {
            this.fileName = fileName;
            using (var memStream = new MemoryStream(data))
            using (var sr = new StreamReader(memStream))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    if (line.Length < 1 || line[0] == '#') // Comment
                        continue;

                    // Get element id
                    var elementId = line.Remove(line.IndexOf(' '));
                    var parameterCount = CountInstancesOfCharInString(line, ' ');
                    switch (elementId)
                    {
                        case "v": // Vertex position (xyz[w])
                            // Check whether the optional parameter is present or not
                            if (parameterCount == 3)
                            {
                                // Only xyz
                                var baseLine = line.Remove(0, line.IndexOf(' ') + 1);
                                var x = baseLine.Remove(baseLine.IndexOf(' '));
                                var y = baseLine.Remove(0, baseLine.IndexOf(' ') + 1);
                                y = y.Remove(y.LastIndexOf(' ') - 1);
                                var z = baseLine.Remove(0, baseLine.LastIndexOf(' ') + 1);
                                vertices.Add(new Vector3(float.Parse(x), float.Parse(y), float.Parse(z)));
                            }
                            else if (parameterCount == 4)
                            {
                                // xyzw
                                throw new Exception("Optional parameter not implemented yet.");
                            }
                            else
                            {
                                throw new Exception("obj file is not valid.");
                            }
                            break;
                        case "vt": // Texture coordinate (uv[w])
                            if (parameterCount == 2)
                            {
                                // Only uv
                                var baseLine = line.Remove(0, line.IndexOf(' ') + 1);
                                var u = baseLine.Remove(baseLine.IndexOf(' '));
                                var v = baseLine.Remove(0, baseLine.LastIndexOf(' ') + 1);
                                texCoords.Add(new Vector2(float.Parse(u), float.Parse(v)));
                            }
                            else if (parameterCount == 3)
                            {
                                Logging.Write("UVW used");
                                // uvw
                                var baseLine = line.Remove(0, line.IndexOf(' ') + 1);
                                var u = baseLine.Remove(baseLine.IndexOf(' '));
                                var v = baseLine.Remove(0, baseLine.IndexOf(' ') + 1);
                                v = v.Remove(v.LastIndexOf(' ') - 1);
                                var w = baseLine.Remove(0, baseLine.LastIndexOf(' ') + 1);
                                //normals.Add(new Vector3(float.Parse(u), float.Parse(v), float.Parse(w)));
                                texCoords.Add(new Vector2(float.Parse(u), float.Parse(v)));
                            }
                            else
                            {
                                throw new Exception("obj file is not valid.");
                            }
                            break;
                        case "vn": // Vertex normal (xyz)
                            // Check whether the optional parameter is present or not
                            if (parameterCount == 3)
                            {
                                // Only xyz
                                var baseLine = line.Remove(0, line.IndexOf(' ') + 1);
                                var x = baseLine.Remove(baseLine.IndexOf(' '));
                                var y = baseLine.Remove(0, baseLine.IndexOf(' ') + 1);
                                y = y.Remove(y.LastIndexOf(' ') - 1);
                                var z = baseLine.Remove(0, baseLine.LastIndexOf(' ') + 1);
                                normals.Add(new Vector3(float.Parse(x), float.Parse(y), float.Parse(z)));
                            }
                            else
                            {
                                throw new Exception("obj file is not valid.");
                            }
                            break;
                        case "vp": // Parameter space vertex (u[vw])
                            Logging.Write("Parameter space vertices are not supported by this mesh loader.", LogStatus.Error);
                            break;
                        case "f": // Face
                            // Indices
                            if (parameterCount == 3)
                            {
                                var baseLine = line.Remove(0, line.IndexOf(' ') + 1);
                                var tmp = baseLine.Split('/');
                                var parameters = new List<string>();
                                bool[] nVal = new bool[9];
                                int i = 0;
                                foreach (string p in tmp)
                                {
                                    foreach (string s in p.Split(' '))
                                    {
                                        parameters.Add(s);
                                        if (string.IsNullOrEmpty(s))
                                        {
                                            nVal[i] = true;
                                            Logging.Write("Parameter had no value (" + fileName + ")", LogStatus.Error);
                                        }
                                        i++;
                                    }
                                }
                                vertexIndices.Add((nVal[0] ? 0 : uint.Parse(parameters[0]) - 1)); // v1
                                textureIndices.Add((nVal[1] ? 0 : uint.Parse(parameters[1]) - 1)); // vt1
                                normalIndices.Add((nVal[2] ? 0 : uint.Parse(parameters[2]) - 1)); // vn1

                                vertexIndices.Add((nVal[3] ? 0 : uint.Parse(parameters[3]) - 1)); // v2
                                textureIndices.Add((nVal[4] ? 0 : uint.Parse(parameters[4]) - 1)); // vt2
                                normalIndices.Add((nVal[5] ? 0 : uint.Parse(parameters[5]) - 1)); // vn2

                                vertexIndices.Add((nVal[6] ? 0 : uint.Parse(parameters[6]) - 1)); // v3
                                textureIndices.Add((nVal[7] ? 0 : uint.Parse(parameters[7]) - 1)); // vt3
                                normalIndices.Add((nVal[8] ? 0 : uint.Parse(parameters[8]) - 1)); // vn3
                            }
                            else
                            {
                                Logging.Write("Faces must be triangulated.", LogStatus.Error); // Enable the fucking export option!
                            }
                            break;
                        case "l": // Line
                            Logging.Write("Polylines are not supported by this mesh loader.", LogStatus.Error);
                            break;
                        case "mtllib": // Define material
                            Logging.Write("Materials are not implemented yet.", LogStatus.Warning);
                            break;
                        case "usemtl": // Use material
                            Logging.Write("Materials are not implemented yet.", LogStatus.Warning);
                            break;
                        case "o": // Object
                            break;
                        case "g": // Polygon group
                            break;
                        case "s": // Smooth shading
                            Logging.Write("Smooth shading is not supported by this mesh loader.", LogStatus.Error);
                            break;
                        default:
                            Logging.Write("OBJ file is not valid!", LogStatus.Error);
                            break;
                    }
                }
            }
            GenerateBuffers();
        }
    }
}

