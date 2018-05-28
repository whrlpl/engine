using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whirlpool.Core.Render;

namespace Whirlpool.Game.Logic
{
    // ported from the C++ code at https://en.wikipedia.org/wiki/Perlin_noise
    class PerlinNoise
    {
        Vector3[] gradient = new Vector3[]
        {
            new Vector3(1,1,0), new Vector3(-1,1,0), new Vector3(1,-1,0), new Vector3(-1,-1,0),
            new Vector3(1,0,1), new Vector3(-1,0,1), new Vector3(1,0,-1), new Vector3(-1,0,-1),
            new Vector3(0,1,1), new Vector3(0,-1,1), new Vector3(0,1,-1), new Vector3(0,-1,-1)
        };

        float lerp (float a, float b, float w)
        {
            return (1.0f - w) * a + w * b;
        }

        float dotGridGradient(int ix, int iy, float x, float y)
        {
            float dx = x - ix;
            float dy = y - iy;
            return (dx * gradient[ix + iy * 80].X + dy * gradient[iy + ix].Y);
        }

        float perlin(float x, float y)
        {
            int x0 = (int)x;
            int x1 = x0 + 1;
            int y0 = (int)y;
            int y1 = y0 + 1;

            float sx = x - x0;
            float sy = y - y0;

            float n0, n1, ix0, ix1, value;
            n0 = dotGridGradient(x0, y0, x, y);
            n1 = dotGridGradient(x1, y0, x, y);
            ix0 = lerp(n0, n1, sx);

            n0 = dotGridGradient(x0, y1, x, y);
            n1 = dotGridGradient(x1, y1, x, y);
            ix1 = lerp(n0, n1, sx);
            value = lerp(ix0, ix1, sy);
            return value;
        }

        public Texture generateTexture()
        {
            int texSize = 1280;
            float scale = 5.0f;
            // gen gradients
            gradient = new Vector3[texSize * texSize];
            var rand = new Random(3756547);

            for (int y = 0; y < texSize; ++y)
            {
                for (int x = 0; x < texSize; ++x)
                {
                    gradient[x + y * texSize] = new Vector3(rand.Next(2), rand.Next(2), rand.Next(2));
                }
            }
            Color4[] data = new Color4[texSize * texSize];

            for (int x = 0; x < texSize; ++x)
            {
                for (int y = 0; y < texSize; ++y)
                {
                    var cl = perlin(x / (float)texSize * scale, y / (float)texSize * scale);
                    cl = (cl + 1) / 2;
                    data[y * texSize + x] = new Color4(cl, cl, cl, 1.0f);
                }
            }
            return Texture.FromData(data, texSize, texSize);
        }
    }
}
