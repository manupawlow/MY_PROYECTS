using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ImageEditor.Effects
{
    class RoundEffect : Effect
    {
        public RoundEffect(object[] config) : base(config)
        {
        }

        public override Bitmap ApplyEffect(Bitmap bitmap)
        {
            var newBitmap = new Bitmap(bitmap.Width, bitmap.Height);

            var factor = 1;
            Func<int, int> round = (x) => (int)(Math.Round(x * factor / 255.0) * 255 / factor);

            for (int h = 0; h < bitmap.Height; h++)
            {
                for (int w = 0; w < bitmap.Width; w++)
                {
                    var pixel = bitmap.GetPixel(w,h);

                    var roundedPixel = Color.FromArgb(round(pixel.R), round(pixel.G), round(pixel.B));

                    newBitmap.SetPixel(w, h, roundedPixel);
                }
            }

            return newBitmap;
        }
    }
}
