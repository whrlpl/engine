namespace Whirlpool.Core.Render
{
    /// <summary>
    /// The base component that supplies initialization, update and render functions.
    /// </summary>
    public abstract class RenderComponent
    {
        public Screen parentScreen;
        public abstract void Init(Screen screen);
        public abstract void Update();
        public abstract void Render();
    }
}
