using OpenTK;
using System;
using System.Collections.Generic;
using System.IO;
using Whirlpool.Core.Render;

namespace Whirlpool.Core.IO.Assets
{
    public class MeshLoader
    {
        public static Mesh LoadAsset(string fileName)
        {
            Mesh temp = new Mesh();
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
                                temp.vertices.Add(new Vector3(float.Parse(x), float.Parse(y), float.Parse(z)));
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
                                temp.texCoords.Add(new Vector2(float.Parse(u), float.Parse(v)));
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
                                //temp.normals.Add(new Vector3(float.Parse(u), float.Parse(v), float.Parse(w)));
                                temp.texCoords.Add(new Vector2(float.Parse(u), float.Parse(v)));
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
                                temp.normals.Add(new Vector3(float.Parse(x), float.Parse(y), float.Parse(z)));
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
                                foreach (string p in tmp)
                                {
                                    foreach (string s in p.Split(' '))
                                    {
                                        parameters.Add(s);
                                        if (string.IsNullOrEmpty(s))
                                        {
                                            throw new Exception("Parameter had no value (" + fileName + ")");
                                        }
                                    }
                                }                                    
                                temp.vertexIndices.Add(uint.Parse(parameters[0]) - 1); // v1
                                temp.textureIndices.Add(uint.Parse(parameters[1]) - 1); // vt1
                                temp.normalIndices.Add(uint.Parse(parameters[2]) - 1); // vn1

                                temp.vertexIndices.Add(uint.Parse(parameters[3]) - 1); // v2
                                temp.textureIndices.Add(uint.Parse(parameters[4]) - 1); // vt2
                                temp.normalIndices.Add(uint.Parse(parameters[5]) - 1); // vn2

                                temp.vertexIndices.Add(uint.Parse(parameters[6]) - 1); // v3
                                temp.textureIndices.Add(uint.Parse(parameters[7]) - 1); // vt3
                                temp.normalIndices.Add(uint.Parse(parameters[8]) - 1); // vn3
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
            temp.GenerateBuffers();
            return temp;
        }


        public static int CountInstancesOfCharInString(string s, char c)
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
    }
}
