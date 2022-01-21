using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ImageEditor.Effects
{
    class CosineEffect : DynamicEffect
    {
        public CosineEffect(object[] config) : base(config) { }

        public override Bitmap ApplyEffect(Bitmap bitmap)
        {
            var newBitmap = new Bitmap(bitmap.Width, bitmap.Height);

            for (int h = 0; h < bitmap.Height; h++)
            {
                for (int w = 0; w < bitmap.Width; w++)
                {
                    var pixel = bitmap.GetPixel(w,h);

                    //Func<int, double, int> mul = (color, d) => (int)(color * d * Math.Abs(Math.Cos(ElapsedTime * 0.1)));

                    //var roundedPixel = Color.FromArgb(mul(pixel.R, 1), mul(pixel.G, 0.8), mul(pixel.B, 0.2));

                    var originX = bitmap.Width / 2;
                    var originY = bitmap.Height / 2;
                    var d = Math.Sqrt((w - originX) * (w - originX) + (h - originY) * (h - originY));

                    var radious = 10;
                    var a = d % 5 < 4 * Math.Sin(ElapsedTime * 0.1) ? 0 : 1;

                    var r = Math.Max(0, Math.Min(255, (int)(pixel.R * (1 / d) * radious)));
                    var g = Math.Max(0, Math.Min(255, (int)(pixel.G * (1 / d) * radious)));
                    var b = Math.Max(0, Math.Min(255, (int)(pixel.B * (1 / d) * radious)));

                    var roundedPixel = Color.FromArgb(a, r, g, b);

                    newBitmap.SetPixel(w, h, roundedPixel);
                }
            }

            return newBitmap;
        }
    }
}
