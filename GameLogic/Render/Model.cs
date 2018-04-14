using OpenTK;
using OpenTK.Graphics;
using OpenTKTest.Core.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKTest.Game.Render
{
    class Model : RenderComponent
    {
        public Vector3 position = Vector3.Zero;
        public Vector3 size = Vector3.Zero;
        public Vector3 rotation = Vector3.Zero;
        public string objName;

        private OpenTKTest.Core.IO.Object obj;

        public override void Init()
        {
            // Load model
            obj = OpenTKTest.Core.IO.ObjLoader.Load(objName);
        }

        public override void Render()
        {
            BaseRenderer.RenderModel(obj, position, size, rotation);
        }

        public override void Update()
        {
            // ????
            rotation = new Vector3(1.0f, 1.0f, 0.0f) * OpenTKTest.Core.IO.Time.currentTime;
        }
    }
}
