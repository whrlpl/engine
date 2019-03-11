using OpenTK.Graphics;
using System;

namespace Whirlpool.Core.Render.Utils
{
    public static class Color4Ext
    {
        private static float HexToFloat(string hex)
        {
            int ret = 0;
            hex = hex.ToUpper();

            for (int i = hex.Length - 1; i >= 0; --i)
            {
                var o = (hex.Length - 1) - i;
                if (hex[i] > 64 && hex[i] < 71)
                    ret += (hex[i] - 55) * (1 + 15 * o);
                else if (char.IsDigit(hex[i]))
                    ret += (hex[i] - 48) * (1 + 15 * o);
                else
                    throw new Exception("Invalid hex string '" + hex + "'");
            }
            return ret / 255.0f;
        }

        public static Color4 ColorFromHex(string hex_)
        {
            string hex = hex_;
            if (hex.StartsWith("#"))
                hex = hex.Substring(1);
            if (hex.Length != 6 && hex.Length != 8)
            {
                throw new Exception("Invalid hex value '" + hex + "'");
            }

            Color4 col = new Color4(
                HexToFloat(hex.Substring(0, 2)),
                HexToFloat(hex.Substring(2, 2)),
                HexToFloat(hex.Substring(4, 2)),
                (hex.Length == 6) ? 1.0f : HexToFloat(hex.Substring(6, 2))
            );
            return col;
        }
    }
}
