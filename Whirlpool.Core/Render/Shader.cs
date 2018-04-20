using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whirlpool.Core.Render
{
    public class Shader
    {
        public ShaderType shaderType;
        public string fileName;
        public int glShader;

        public Shader(string file, ShaderType type)
        {
            using (StreamReader sr = new StreamReader(file))
            {
                glShader = GL.CreateShader(type);
                GL.ShaderSource(glShader, sr.ReadToEnd());
                GL.CompileShader(glShader);
                fileName = file;
                GL.GetShader(glShader, ShaderParameter.CompileStatus, out var status);

                if (status == 0)
                    throw new Exception(GL.GetShaderInfoLog(glShader));
                else
                    Console.WriteLine("Shader " + file + " compiled");
            }
        }
    }
}
