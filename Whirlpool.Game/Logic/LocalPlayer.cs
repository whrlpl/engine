using OpenTK;
using Whirlpool.Core.Render;

namespace Whirlpool.Game.Logic
{
    public class LocalPlayer
    {
        public Camera camera;
        public Vector3 position;
        public Vector3 rotation;
        
        public void Init()
        {
            position = rotation = Vector3.Zero;
            camera = new Camera();
        }

        public void Update()
        {
        }
    }
}
