using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using OpenTK;
using OpenTK.Graphics;
using Whirlpool.Core.UI;

namespace Whirlpool.Core.IO
{
    public class UILoader
    {
        public static List<UIComponent> LoadStream(MemoryStream stream)
        {            
            List<UIComponent> components = new List<UIComponent>();

            Dictionary<string, Font> declaredFonts = new Dictionary<string, Font>();
            
            XElement loaded = XElement.Load(stream);
            foreach (var element in loaded.Elements())
            {
                UIComponent component = null;
                Vector2 componentSize = Vector2.Zero;
                Vector2 componentPosition = Vector2.Zero;

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
                    case "SlicedSprite":
                        component = new SlicedSprite();
                        break;
                    case "Image":
                        component = new Image();
                        break;
                    case "Textbox":
                        component = new Textbox();
                        break;
                    case "DialogBox":
                        component = new DialogBox();
                        break;
                    default:
                        Logging.Write("Unknown element '" + element.Name + "'.", LogStatus.Error);
                        break;
                }

                foreach (var child in element.Elements())
                {
                    switch (child.Name.ToString())
                    {
                        case "Tooltip":
                            component.tooltipText = child.Value;
                            break;
                        case "Name":
                            component.name = child.Value;
                            break;
                        case "Text":
                            component.text = child.Value;
                            break;
                        case "Sprite":
                            if (element.Name == "SlicedSprite")
                                ((SlicedSprite)component).spriteLoc = child.Value;
                            break;
                        case "Image":
                            if (element.Name == "Image")
                                ((Image)component).imageLoc = child.Value;
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
                        case "FormatColor":
                            if (element.Name == "Label")
                                ((Label)component).formatColor = (child.Value == "True" ? true : false);
                            break;
                        case "LineSpacing":
                            if (element.Name == "LineSpacing")
                                ((Label)component).lineSpacing = int.Parse(child.Value);
                            break;
                        case "Password":
                            if (element.Name == "Textbox")
                                ((Textbox)component).isPassword = (child.Value == "True" ? true : false);
                            break;
                        case "Visible":
                            component.visible = (child.Value == "True" ? true : false);
                            break;
                        case "OnClick":
                            component.onClickEvent = child.Value;
                            break;
                        case "Font":
                            string declaredName = "";
                            int size = 16;
                            string loc = "";
                            int kerning = 0;
                            Color4 tint = Color4.White;
                            if (!child.HasElements && declaredFonts.ContainsKey(child.Value))
                            {
                                component.font = declaredFonts[child.Value];
                                break;
                            }
                            foreach (var subChild in child.Elements())
                            {
                                switch (subChild.Name.ToString())
                                {
                                    case "Name":
                                        declaredName = subChild.Value;
                                        break;
                                    case "Size":
                                        size = int.Parse(subChild.Value);
                                        break;
                                    case "Location":
                                        loc = subChild.Value;
                                        break;
                                    case "Tint":
                                        foreach (var subSubChild in subChild.Elements())
                                        {
                                            switch (subSubChild.Name.ToString())
                                            {
                                                case "R":
                                                    tint.R = int.Parse(subSubChild.Value) / 255.0f;
                                                    break;
                                                case "G":
                                                    tint.G = int.Parse(subSubChild.Value) / 255.0f;
                                                    break;
                                                case "B":
                                                    tint.B = int.Parse(subSubChild.Value) / 255.0f;
                                                    break;
                                                case "A":
                                                    tint.A = int.Parse(subSubChild.Value) / 255.0f;
                                                    break;
                                            }
                                        }
                                        break;
                                    case "Kerning":
                                        kerning = int.Parse(subChild.Value);
                                        break;
                                }
                            }
                            component.font = new Font(loc, tint, size, kerning);
                            if (declaredName != string.Empty)
                                declaredFonts.Add(declaredName, component.font);
                            break;
                        case "Tint":
                            Color4 temp = Color4.Black;
                            foreach (var subChild in child.Elements())
                            {
                                switch (subChild.Name.ToString())
                                {
                                    case "R":
                                        temp.R = int.Parse(subChild.Value) / 255.0f;
                                        break;
                                    case "G":
                                        temp.G = int.Parse(subChild.Value) / 255.0f;
                                        break;
                                    case "B":
                                        temp.B = int.Parse(subChild.Value) / 255.0f;
                                        break;
                                    case "A":
                                        temp.A = int.Parse(subChild.Value) / 255.0f;
                                        break;
                                }
                            }
                            component.tint = temp;
                            break;
                        case "X":
                            if (child.Value.EndsWith("%"))
                                componentPosition.X = BaseGame.Size.Height / 100 * int.Parse(child.Value.ToString().Remove(child.Value.Length - 1));
                            else
                                componentPosition.X = int.Parse(child.Value.ToString());
                            break;
                        case "Y":
                            if (child.Value.EndsWith("%"))
                                componentPosition.Y = BaseGame.Size.Height / 100 * int.Parse(child.Value.ToString().Remove(child.Value.Length - 1));
                            else
                                componentPosition.Y = int.Parse(child.Value.ToString());
                            break;
                        case "Width":
                            if (child.Value.EndsWith("%"))
                                componentSize.X = BaseGame.Size.Width / 100 * int.Parse(child.Value.ToString().Remove(child.Value.Length - 1));
                            else
                                componentSize.X = int.Parse(child.Value.ToString());
                            break;
                        case "Height":
                            if (child.Value.EndsWith("%"))
                                componentSize.Y = BaseGame.Size.Height / 100 * int.Parse(child.Value.ToString().Remove(child.Value.Length - 1));
                            else
                                componentSize.Y = int.Parse(child.Value.ToString());
                            break;
                        case "Centered":
                            component.centered = (child.Value == "True" ? true : false);
                            break;
                        case "Focused":
                            component.focused = (child.Value == "True" ? true : false);
                            break;
                        default:
                            Logging.Write("Unknown property '" + child.Name + "' in '" + element.Name + "'.", LogStatus.Error);
                            break;
                    }
                }
                component.Init(null);
                component.size = componentSize;
                component.position = componentPosition;
                components.Add(component);
            }
            return components;
        }


        public static List<UIComponent> LoadFile(string path)
        {
            for (int i = 0; i < 3; ++i)
            {
                try
                {
                    var memoryStream = new MemoryStream();
                    var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
                    fileStream.CopyTo(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    var val = LoadStream(memoryStream);

                    memoryStream.Close();
                    fileStream.Close();
                    return val;
                }
                catch (IOException ex)
                {
                    if (i < 3)
                    {
                        Thread.Sleep(50); // bad practice but we only really load 1 screen
                    }
                    else
                    {
                        Logging.Write("Failed to read the file " + path + " after 150ms (" + ex.ToString() + ")", LogStatus.Error);
                    }
                }
            }
            return new List<UIComponent>();
        }
    }
}
