using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKTest.Core.Render
{
    // Base screen & render functions
    // TODO: bytecode API for screens & screen switching
    public class Screen
    {
        private List<RenderComponent> renderComponents = new List<RenderComponent>();
        
        public void AddComponents(List<RenderComponent> components)
        {
            foreach (RenderComponent rc in components)
            {
                renderComponents.Add(rc);
            }
        }

        public void RemoveComponent(RenderComponent component)
        {
            renderComponents.Remove(component);
        }

        public void RemoveComponent(int index)
        {
            renderComponents.RemoveAt(index);
        }

        public void AddComponent(RenderComponent component)
        {
            renderComponents.Add(component);
        }

        public virtual void Update()
        {
            foreach (RenderComponent rc in renderComponents)
            {
                rc.Update();
            }
        }

        public virtual void Init()
        {
            foreach (RenderComponent rc in renderComponents)
            {
                rc.Init();
            }
        }

        public virtual void Render()
        {
            foreach (RenderComponent rc in renderComponents)
            {
                rc.Render();
            }
        }
    }
}
