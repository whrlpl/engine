namespace Whirlpool.Core.Render
{
    /// <summary>
    /// The base component that supplies initialization, update and render functions.
    /// </summary>
    public abstract class RenderComponent
    {
        public abstract void Init();
        public abstract void Update();
        public abstract void Render();
    }
}
