using OpenTK;
using Whirlpool.Core.IO.Assets;
using Whirlpool.Core.Render;

namespace Whirlpool.Game.Render
{
    class Model : RenderComponent
    {
        public Vector3 position = Vector3.Zero;
        public Vector3 size = Vector3.One;
        public Vector3 rotation = Vector3.Zero;
        public string objName = string.Empty;
        public Mesh mesh;
        public Mesh collisionMesh;

        public override void Init(Screen screen)
        {
            // Load model
            mesh = MeshLoader.LoadAsset(objName);
            collisionMesh = MeshLoader.LoadAsset(objName);
        }

        public override void Render()
        {
            Renderer.RenderMesh(mesh, position, size, rotation);
        }

        public override void Update()
        {
            // ????
            //rotation = new Vector3(0.0f, 90.0f, 0.0f) * Whirlpool.Core.IO.Time.currentTime;
        }
    }
}
