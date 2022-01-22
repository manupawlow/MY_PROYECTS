using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ImageEditor.Effects
{
    class GrayEffect : Effect
    {
        public GrayEffect(object[] config) : base(config)
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

                    var gray_color = (int)(0.2126 * pixel.R + 0.7152 * pixel.G + 0.0722 * pixel.B);

                    newBitmap.SetPixel(w, h, Color.FromArgb(gray_color, gray_color, gray_color));
                }
            }

            return newBitmap;
        }
    }
}
