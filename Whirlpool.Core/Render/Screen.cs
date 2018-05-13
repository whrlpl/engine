using System;
using System.Collections.Generic;
using Whirlpool.Core.IO;
using Whirlpool.Core.UI;

namespace Whirlpool.Core.Render
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

        public void AddUIComponents(List<UIComponent> components)
        {
            foreach (UIComponent uic in components)
            {
                renderComponents.Add(uic);
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

        public UIComponent GetUIComponent(string name)
        {
            foreach (UIComponent uic in renderComponents)
            {
                if (uic.name == name)
                    return uic;
            }
            return null;
        }

        public void LoadFromFile(string file)
        {
            renderComponents = new List<RenderComponent>();
            AddUIComponents(UILoader.LoadFile(file));
            Init();
        }

        public virtual void Update()
        {
            try
            {
                foreach (RenderComponent rc in renderComponents)
                {
                    rc.Update();
                }
            }
            catch (Exception ex)
            {
                Logging.Write("Error updating screen: " + ex.ToString(), LogStatus.Error);
            }
        }

        public virtual void Init()
        {
            foreach (RenderComponent rc in renderComponents)
            {
                rc.parentScreen = this;
                rc.Init(this);
            }
        }
        
        public virtual void Render()
        {
            try
            {
                foreach (RenderComponent rc in renderComponents)
                {
                    rc.Render();
                }
            }
            catch (Exception ex)
            {
                Logging.Write("Error rendering screen: " + ex.ToString(), LogStatus.Error);
            }
}
    }
}
