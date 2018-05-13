using OpenTK;
using OpenTK.Graphics;
using Whirlpool.Core.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whirlpool.Game.Render
{
    class Model : RenderComponent
    {
        public Vector3 position = Vector3.Zero;
        public Vector3 size = Vector3.Zero;
        public Vector3 rotation = Vector3.Zero;
        public string objName = string.Empty;

        private Whirlpool.Core.IO.Object obj;

        public override void Init(Screen screen)
        {
            // Load model
            obj = Whirlpool.Core.IO.ObjLoader.Load(objName);
        }

        public override void Render()
        {
            BaseRenderer.RenderModel(obj, position, size, rotation);
        }

        public override void Update()
        {
            // ????
            rotation = new Vector3(1.0f, 1.0f, 0.0f) * Whirlpool.Core.IO.Time.currentTime;
        }
    }
}
