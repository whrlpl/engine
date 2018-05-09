using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using OpenTK;
using OpenTK.Graphics;
using Whirlpool.Core.UI;

namespace Whirlpool.Core.IO
{
    public class UILoader
    {
        public static List<UIComponent> LoadFile(string path)
        {
            List<UIComponent> components = new List<UIComponent>();
            XElement loaded = XElement.Load(path);
            foreach (var element in loaded.Elements())
            {
                UIComponent component = null;
                Vector2 componentSize = Vector2.Zero;
                Vector2 componentPosition = Vector2.Zero;
                Logging.Write(element.Name + ": " + element.Value, LogStatus.General);

                switch (element.Name.ToString())
                {
                    case "Button":
                        component = new Button();
                        break;
                    case "Checkbox":
                        component = new Checkbox();
                        break;
                    case "Label":
                        component = new Label();
                        break;
                    case "Notification":
                        component = new Notification();
                        break;
                    case "SlicedSprite":
                        component = new SlicedSprite();
                        break;
                    case "Textbox":
                        component = new Textbox();
                        break;
                    default:
                        Logging.Write("Unknown element '" + element.Name + "'.");
                        break;
                }
                Logging.Write("Children:");

                foreach (var child in element.Elements())
                {
                    Logging.Write("\t" + child.Name + ": " + child.Value, LogStatus.General);
                    switch (child.Name.ToString())
                    {
                        case "Text":
                            component.text = child.Value;
                            break;
                        case "HorizontalAnchor":
                            switch (child.Value)
                            {
                                case "Left":
                                    component.horizontalAnchor = HorizontalAnchorPoint.Left;
                                    break;
                                case "Centre":
                                case "Center":
                                case "Middle":
                                    component.horizontalAnchor = HorizontalAnchorPoint.Centre;
                                    break;
                                case "Right":
                                    component.horizontalAnchor = HorizontalAnchorPoint.Right;
                                    break;
                            };
                            break;
                        case "VerticalAnchor":
                            switch (child.Value)
                            {
                                case "Top":
                                    component.verticalAnchor = VerticalAnchorPoint.Top;
                                    break;
                                case "Centre":
                                case "Center":
                                case "Middle":
                                    component.verticalAnchor = VerticalAnchorPoint.Centre;
                                    break;
                                case "Bottom":
                                    component.verticalAnchor = VerticalAnchorPoint.Bottom;
                                    break;
                            };
                            break;
                        case "Placeholder":
                            if (element.Name == "Textbox")
                                ((Textbox)component).placeholder = child.Value;
                            break;
                        case "Password":
                            if (element.Name == "Textbox")
                                ((Textbox)component).isPassword = (child.Value == "True" ? true : false);
                            break;
                        case "Font":
                            int size = 16;
                            string name = "";
                            int kerning = 0;
                            foreach (var subChild in child.Elements())
                            {
                                switch (subChild.Name.ToString())
                                {
                                    case "Size":
                                        size = int.Parse(subChild.Value);
                                        break;
                                    case "Name":
                                        name = subChild.Value;
                                        break;
                                    case "Kerning":
                                        kerning = int.Parse(subChild.Value);
                                        break;
                                }
                            }
                            component.font = new Font(name, Color4.White, size, kerning);
                            break;
                        case "Tint":
                            Color4 temp = Color4.White;
                            foreach (var subChild in child.Elements())
                            {
                                switch (subChild.Name.ToString())
                                {
                                    case "R":
                                        temp.R = int.Parse(subChild.Value);
                                        break;
                                    case "G":
                                        temp.G = int.Parse(subChild.Value);
                                        break;
                                    case "B":
                                        temp.B = int.Parse(subChild.Value);
                                        break;
                                    case "A":
                                        temp.A = int.Parse(subChild.Value);
                                        break;
                                }
                            }
                            component.tint = temp;
                            break;
                        case "X":
                            componentPosition.X = int.Parse(child.Value.ToString());
                            break;
                        case "Y":
                            componentPosition.Y = int.Parse(child.Value.ToString());
                            break;
                        case "Width":
                            componentSize.X = int.Parse(child.Value.ToString());
                            break;
                        case "Height":
                            componentSize.Y = int.Parse(child.Value.ToString());
                            break;
                        case "Centered":
                            component.centered = (child.Value == "True" ? true : false);
                            break;
                        case "Focused":
                            component.focused = (child.Value == "True" ? true : false);
                            break;
                        default:
                            Logging.Write("Unknown property '" + child.Name + "' in '" + element.Name + "'.");
                            break;
                    }
                }
                component.size = componentSize;
                component.position = componentPosition;
                components.Add(component);
            }
            return components;
        }
    }
}
