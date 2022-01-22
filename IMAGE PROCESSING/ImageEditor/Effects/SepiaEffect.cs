using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ImageEditor.Effects
{
    class SepiaEffect : Effect
    {
        public SepiaEffect(object[] config) : base(config)
        {
        }

        public override Bitmap ApplyEffect(Bitmap bitmap)
        {
            var newBitmap = new Bitmap(bitmap.Width, bitmap.Height);

            for (int h = 0; h < bitmap.Height; h++)
            {
                for (int w = 0; w < bitmap.Width; w++)
                {
                    var pixel = bitmap.GetPixel(w,h);

                    var r = (int)Math.Min(255, pixel.R * 0.399 + pixel.R * 0.769 + pixel.R * 0.189);
                    var g = (int)Math.Min(255, pixel.R * 0.349 + pixel.G * 0.686 + pixel.B * 0.168);
                    var b = (int)Math.Min(255, pixel.R * 0.272 + pixel.G * 0.534 + pixel.B * 0.131);

                    var negative = Color.FromArgb(r, g, b);

                    newBitmap.SetPixel(w, h, negative);
                }
            }

            return newBitmap;
        }
    }
}
