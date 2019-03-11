using Whirlpool.Shared;

namespace Whirlpool.Content.Builder
{
    public class ShaderBuilder : IContentBuilderTypeCompiler
    {
        public void Build(ref Package package, string fileName, string internalName)
        {
            // TODO: compile shaders to SPIR-V?
            package.AddFile(internalName, fileName);
        }
    }
}
