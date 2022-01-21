using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsEngine.Graphics
{
    public class EngineColor
    {
        public EngineColor(int r, int g, int b, int a = 255)
        {
            r = Math.Max(r, 0);
            g = Math.Max(g, 0);
            b = Math.Max(b, 0);

            r = Math.Min(r, 255);
            g = Math.Min(g, 255);
            b = Math.Min(b, 255);

            R = r;
            G = g;
            B = b;
            A = a;
        }

        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }
        public int A { get; set; }

        public int Gray => (int)(0.299 * R + 0.587 * G + 0.114 * B);

        public static EngineColor operator +(EngineColor c1, EngineColor c2) => new EngineColor(c1.R + c2.R, c1.G + c2.G, c1.B + c2.B);
        public static EngineColor operator -(EngineColor c) => new EngineColor(-c.R, -c.G, -c.B);
        public static EngineColor operator -(EngineColor c1, EngineColor c2) => c1 + (-c2);
        public static EngineColor operator *(EngineColor c, double n) => new EngineColor((int)(c.R * n), (int)(c.G * n), (int)(c.B * n));
        public static EngineColor operator *(double n, EngineColor c) => c * n;

        public static EngineColor White => new EngineColor(255, 255, 255);
        public static EngineColor Black => new EngineColor(0, 0, 0);

        //PRIMARY
        public static EngineColor Red => new EngineColor(255, 0, 0);
        public static EngineColor Green => new EngineColor(0, 255, 0);
        public static EngineColor Blue => new EngineColor(0, 0, 255);
        //SECONDARY
        public static EngineColor Yellow => new EngineColor(255, 255, 0);
        public static EngineColor Purple => new EngineColor(128, 0, 128);
        //OTHERS
        public static EngineColor Pink => new EngineColor(255, 192, 203);

        public EngineColor Copy() => new EngineColor(R, G, B, A);
    }
}
