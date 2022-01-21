using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ImageEditor.Effects
{
    class NegativeEffect : Effect
    {
        public NegativeEffect(object[] config) : base(config)
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

                    var negative = Color.FromArgb(255 - pixel.R, 255 - pixel.G, 255 - pixel.B);

                    newBitmap.SetPixel(w, h, negative);
                }
            }

            return newBitmap;
        }
    }
}
