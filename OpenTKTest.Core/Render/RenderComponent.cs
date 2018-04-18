using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKTest.Core.Render
{
    public abstract class RenderComponent
    {
        public abstract void Init();
        public abstract void Update();
        public abstract void Render();
    }
}
