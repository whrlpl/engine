using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKTest.IO
{
    public class Object
    {
        public List<Vector3> vertices = new List<Vector3>();
        public List<Vector3> texCoords = new List<Vector3>();
        public List<Vector3> normals = new List<Vector3>();
        public int VAO, VBO;

        public void GenerateBuffers()
        {
            // Generate VAO, VBO
            GL.GenVertexArrays(1, out VAO);
            GL.GenBuffers(1, out VBO);

            // Buffer data
            float[] vertices_ = new float[vertices.Count * 3];
            for (int i = 0; i < vertices.Count; ++i)
            {
                vertices_[i * 3] = vertices[i].X;
                vertices_[i * 3 + 1] = vertices[i].Y;
                vertices_[i * 3 + 2] = vertices[i].Z;
            }
            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices_.Length * sizeof(float), vertices_, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0 * sizeof(float));
            GL.EnableVertexAttribArray(0);
        }
    }

    public class ObjLoader
    {
        public static Object Load(string fileName)
        {
            Object temp = new Object();
            using (var sr = new StreamReader(fileName))
            {
                List<int> vertexIndices = new List<int>();
                List<Vector3> tempVertices = new List<Vector3>();
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
                                tempVertices.Add(new Vector3(float.Parse(x), float.Parse(y), float.Parse(z)));
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
                            Console.WriteLine("Texture coordinates are not implemented yet.");
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
                                temp.normals.Add(new Vector3(float.Parse(x), float.Parse(y), float.Parse(z)));
                            }
                            else
                            {
                                throw new Exception("obj file is not valid.");
                            }
                            break;
                        case "vp": // Parameter space vertex (u[vw])
                            Console.WriteLine("Parameter space vertices are not implemented yet.");
                            break;
                        case "f": // Face
                            // Indices
                            if (parameterCount == 3)
                            {
                                var baseLine = line.Remove(0, line.IndexOf(' ') + 1);
                                var tmp = baseLine.Split('/');
                                var parameters = new List<string>();
                                foreach (string p in tmp)
                                    foreach (string s in p.Split(' '))
                                        parameters.Add(s);
                                vertexIndices.Add(int.Parse(parameters[0]));
                                vertexIndices.Add(int.Parse(parameters[3]));
                                vertexIndices.Add(int.Parse(parameters[6]));
                            }
                            else
                            {
                                Console.WriteLine("TRIANGULATE FACES!!!"); // Enable the fucking export option!
                            }
                            break;
                        case "l": // Line
                            Console.WriteLine("Lines are not implemented yet.");
                            break;
                        case "mtllib": // Define material
                            Console.WriteLine("Materials are not implemented yet.");
                            break;
                        case "usemtl": // Use material
                            Console.WriteLine("Materials are not implemented yet.");
                            break;
                        case "o": // Object
                        case "g": // Polygon group
                            Console.WriteLine("Polygon grouping is not implemented yet.");
                            break;
                        case "s": // Smooth shading
                            Console.WriteLine("Smooth shading is not implemented yet.");
                            break;
                        default:
                            throw new Exception("obj file is not valid.");
                    }

                    for (int i = 0; i < vertexIndices.Count; ++i)
                    {
                        int vertexIndex = vertexIndices[i];
                        temp.vertices.Add(tempVertices[vertexIndex - 1]);
                    }
                }
            }
            return temp;
        }

        public static int CountInstancesOfCharInString(string s, char c) { var count = 0; foreach (char c_ in s) if (c_ == c) ++count; return count; }
    }
}
