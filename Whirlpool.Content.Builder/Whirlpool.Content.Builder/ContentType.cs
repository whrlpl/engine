using System;

namespace Whirlpool.Content.Builder
{
    public class ContentType
    {
        public string name;
        public IContentBuilderTypeCompiler buildClass;
        public string[] aliases;

        public ContentType(string name, Type buildClass, params string[] aliases)
        {
            this.name = name;
            this.buildClass = (IContentBuilderTypeCompiler)Activator.CreateInstance(buildClass);
            this.aliases = aliases;
        }

        public static ContentType Font = new ContentType("Font", typeof(FontBuilder), "ttf", "otf");
        public static ContentType Shader = new ContentType("Shader", typeof(ShaderBuilder), "glsl");
        public static ContentType Misc = new ContentType("Misc", typeof(MiscBuilder));
    }
}
