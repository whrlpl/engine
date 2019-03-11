using Whirlpool.Shared;

namespace Whirlpool.Content.Builder
{
    public class MiscBuilder : IContentBuilderTypeCompiler
    {
        public void Build(ref Package package, string fileName, string internalName)
        {
            package.AddFile(internalName, fileName);
        }
    }
}
