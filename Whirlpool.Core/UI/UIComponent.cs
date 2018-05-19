using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using Whirlpool.Core.Render;

namespace Whirlpool.Core.UI
{
    public enum VerticalAnchorPoint
    {
        Top = 0,
        Center = 1,
        Middle = 1,
        Centre = 1,
        Bottom = 2
    }

    public enum HorizontalAnchorPoint
    {
        Left = 0,
        Center = 1,
        Middle = 1,
        Centre = 1,
        Right = 2
    }

    public abstract class UIComponent : RenderComponent
    {
        public Vector2 m_position;
        public Vector2 position
        {
            get
            {
                if (m_position != null)
                    return m_position;
                return Vector2.Zero;
            }
            set
            {
                m_position = CalculatePosition(value, BaseGame.Size);
            }
        }

        public string tooltipText { get; set; } = "";
        public string name { get; set; } = "";
        public string text { get; set; } = "";
        public string onClickEvent { get; set; } = "";
        public Font font { get; set; } = null;
        public Color4 tint { get; set; } = Color4.White;
        public Vector2 size { get; set; } = Vector2.Zero;
        public bool centered { get; set; } = false;
        public bool focused { get; set; } = false;
        public bool visible { get; set; } = true;
        public HorizontalAnchorPoint horizontalAnchor { get; set; } = HorizontalAnchorPoint.Left;
        public VerticalAnchorPoint verticalAnchor { get; set; } = VerticalAnchorPoint.Top;
        public bool initialized { get; set; } = false;

        private Vector2 CalculatePosition(Vector2 point, Size windowSize)
        {
            float x = point.X;
            float y = point.Y;
            switch (horizontalAnchor)
            {
                case HorizontalAnchorPoint.Centre:
                    x = (windowSize.Width / 2) + x;
                    break;
                case HorizontalAnchorPoint.Right:
                    x = windowSize.Width - x;
                    break;
            }
            switch (verticalAnchor)
            {
                case VerticalAnchorPoint.Centre:
                    y = (windowSize.Height / 2) + y;
                    break;
                case VerticalAnchorPoint.Bottom:
                    y = windowSize.Height - y;
                    break;
            }
            
            return CalculateCenterPos(new Vector2(x, y));
        }

        public virtual Vector2 CalculateCenterPos(Vector2 point)
        {
            if (centered) return (point - size / 2);
            return point;
        }
    }
}
