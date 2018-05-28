using OpenTK;
using Whirlpool.Core.IO;
using Whirlpool.Core.Render;

namespace Whirlpool.Game.Logic
{
    public class LocalPlayer
    {
        public Camera camera;
        public Vector3 position;
        public Vector3 rotation;
        public float walkSpeed = 0.1f * 0.016f;
        
        public void Init()
        {
            position = rotation = Vector3.Zero;
            camera = new Camera();
        }

        public void Update()
        {
            InputStatus status = InputHandler.GetStatus();

            if (status.keyboardKeys[OpenTK.Input.Key.W])
            {
                MovePlayer(new Vector3(0.0f, 0.0f, -1.0f));
            }
            if (status.keyboardKeys[OpenTK.Input.Key.A])
            {
                MovePlayer(new Vector3(-1.0f, 0.0f, 0.0f));
            }
            if (status.keyboardKeys[OpenTK.Input.Key.S])
            {
                MovePlayer(new Vector3(0.0f, 0.0f, 1.0f));
            }
            if (status.keyboardKeys[OpenTK.Input.Key.D])
            {
                MovePlayer(new Vector3(1.0f, 0.0f, 0.0f));
            }
        }

        public void MovePlayer(Vector3 direction)
        {
            position += direction * walkSpeed * (float)Time.lastFrameTime;
        }
    }
}
