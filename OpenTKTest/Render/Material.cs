#define CACHELOCATIONS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace OpenTKTest.Render
{
    public class Material
    {
        private int shaderProgram;
        private Dictionary<string, int> locations = new Dictionary<string, int>();

        public Material()
        {
            shaderProgram = GL.CreateProgram();
        }

        public void Attach(Shader shader)
        {
            GL.AttachShader(shaderProgram, shader.glShader);
        }

        public void Link()
        {
            GL.LinkProgram(shaderProgram);
            GL.GetProgram(shaderProgram, GetProgramParameterName.ActiveUniforms, out var uniformCount);
            for (int i = 0; i < uniformCount; ++i)
            {
                StringBuilder uniformName = new StringBuilder();
                GL.GetActiveUniform(shaderProgram, i, 2048, out var length, out var size, out var type, uniformName);
                locations.Add(uniformName.ToString(), i);
            }
        }

        public void Use()
        {
            GL.UseProgram(shaderProgram);
        }

        protected int GetVariableLocation(string variable)
        {
            if (locations.TryGetValue(variable, out var locationCached))
                return locationCached;
            return -1;
#if !CACHELOCATIONS
            var location = GL.GetUniformLocation(shaderProgram, variable);
            if (location == (int)ErrorCode.InvalidValue ||
                location == (int)ErrorCode.InvalidOperation ||
                location == -1)
            {
#if DEBUG
                throw new Exception("Error getting GLSL variable.");
#else
                Console.WriteLine("Error getting GLSL uniform '" + variable + "'.");
#endif
            }
            locations.Add(variable, location);
            return location;
#endif
        }

        public void SetVariable(string variable, bool value)
        {
            GL.ProgramUniform1(shaderProgram, GetVariableLocation(variable), (value) ? 1 : 0);
        }

        public void SetVariable(string variable, float value)
        {
            GL.ProgramUniform1(shaderProgram, GetVariableLocation(variable), value);
        }
        public void SetVariable(string variable, int value)
        {
            GL.ProgramUniform1(shaderProgram, GetVariableLocation(variable), value);
        }
        public void SetVariable(string variable, Vector2 value)
        {
            GL.ProgramUniform2(shaderProgram, GetVariableLocation(variable), value.X, value.Y);
        }
        public void SetVariable(string variable, Vector3 value)
        {
            GL.ProgramUniform3(shaderProgram, GetVariableLocation(variable), value.X, value.Y, value.Z);
        }
        public void SetVariable(string variable, Color4 value)
        {
            GL.ProgramUniform4(shaderProgram, GetVariableLocation(variable), value.R, value.G, value.B, value.A);
        }
        public void SetVariable(string variable, Matrix4 value)
        {
            GL.ProgramUniformMatrix4(shaderProgram, GetVariableLocation(variable), false, ref value);
        }
    }
}
