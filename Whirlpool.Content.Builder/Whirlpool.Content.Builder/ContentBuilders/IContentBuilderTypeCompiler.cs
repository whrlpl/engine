using Whirlpool.Shared;

namespace Whirlpool.Content.Builder
{
    public interface IContentBuilderTypeCompiler
    {
        void Build(ref Package package, string fileName, string internalName);
    }

}
