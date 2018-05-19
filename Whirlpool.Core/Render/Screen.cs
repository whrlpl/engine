using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using Whirlpool.Core.IO;
using Whirlpool.Core.Type;
using Whirlpool.Core.UI;

namespace Whirlpool.Core.Render
{
    // Base screen & render functions
    // TODO: bytecode API for screens & screen switching
    public class Screen
    {
        public string name = "";
        private List<RenderComponent> renderComponents = new List<RenderComponent>();
        private Tooltip currentTooltip = new Tooltip()
        {
            font = new Font("Content\\Fonts\\Montserrat-Regular.ttf", Color4.White, 16)
        };

        public Screen(string file)
        {
            LoadFromFile(file);
        }

        public void LoadFromFile(string file)
        {
            renderComponents = new List<RenderComponent>();
            AddUIComponents(UILoader.LoadFile(file));
            name = file;
            Init();
        }

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

        public virtual void Update()
        {
            try
            {
                bool tooltipShown = false;
                foreach (RenderComponent rc in renderComponents)
                {
                    rc.Update();
                    if (rc.GetType().BaseType == typeof(UIComponent))
                    {
                        var uic = (UIComponent)rc;
                        var status = InputHandler.GetStatus();
                        if (uic.tooltipText != "" && new Rectangle(uic.position.X, uic.position.Y, uic.size.X, uic.size.Y).Contains(status.mousePosition))
                        {
                            currentTooltip.text = uic.tooltipText;
                            currentTooltip.position = status.mousePosition;
                            tooltipShown = true;
                        }
                    }
                }
                if (!tooltipShown) currentTooltip.text = "";
            }
            catch (Exception ex)
            {
                Logging.Write("Error updating screen: " + ex.ToString(), LogStatus.Error);
            }
        }

        public virtual void Init()
        {
            currentTooltip.Init(this);
            foreach (RenderComponent rc in renderComponents)
            {
                rc.parentScreen = this;
                rc.Init(this);
            }
            ScreenEvents.GetEvent("OnLoad")?.Invoke(this);
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
            if (currentTooltip.text != "") currentTooltip.Render();
}
    }
}
