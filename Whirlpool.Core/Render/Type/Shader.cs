using OpenTK.Graphics.OpenGL4;
using System;
using System.IO;
using Whirlpool.Core.IO;

namespace Whirlpool.Core.Render.Type
{
    public class Shader
    {
        public ShaderType shaderType;
        public string fileName;
        public string shaderSource;
        public int glShader;

        /// <summary>
        /// Initializes a shader, reading & compiling from a file.
        /// </summary>
        /// <param name="file">The path to compile the shader from</param>
        /// <param name="type">The type of the shader</param>
        public Shader(string file, ShaderType type)
        {
            using (StreamReader sr = new StreamReader(file))
            {
                glShader = GL.CreateShader(type);
                shaderSource = sr.ReadToEnd();
                GL.ShaderSource(glShader, shaderSource);
                GL.CompileShader(glShader);
                fileName = file;
                GL.GetShader(glShader, ShaderParameter.CompileStatus, out var status);

                if (status == 0)
                    throw new Exception(GL.GetShaderInfoLog(glShader));
                else
                    Logging.Write("Shader " + file + " compiled");
            }
        }
    }
}
