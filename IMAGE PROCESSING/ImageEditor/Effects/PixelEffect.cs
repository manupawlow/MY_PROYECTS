using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ImageEditor.Effects
{
    public class PixelEffect : Effect
    {
        public PixelEffect(object[] config) : base(config)
        {
        }

        public override Bitmap ApplyEffect(Bitmap bitmap)
        {
            int PixelRadious = 10;

            try
            {
                PixelRadious = Convert.ToInt16(Configuration[0]);
            }
            catch
            {
            }

            var newBitmap = new Bitmap(bitmap, bitmap.Width, bitmap.Height);

            for (int h = 0; h < bitmap.Height; h += PixelRadious)
            {
                for (int w = 0; w < bitmap.Width; w += PixelRadious)
                {
                    var h0 = (int)(h / PixelRadious) * PixelRadious;
                    var w0 = (int)(w / PixelRadious) * PixelRadious;

                    var R_color_sum = 0d;
                    var G_color_sum = 0d;
                    var B_color_sum = 0d;

                    for (int _h = 0; _h < PixelRadious && h0 + _h < bitmap.Height; _h++)
                        for (int _w = 0; _w < PixelRadious && w0 + _w < bitmap.Width; _w++)
                        {
                            Color pixel = bitmap.GetPixel(w0 + _w, h0 + _h);
                            R_color_sum += pixel.R;
                            G_color_sum += pixel.G;
                            B_color_sum += pixel.B;
                        }

                    var red = (int)R_color_sum / (PixelRadious * PixelRadious);
                    var green = (int)G_color_sum / (PixelRadious * PixelRadious);
                    var blue = (int)B_color_sum / (PixelRadious * PixelRadious);

                    var color = Color.FromArgb(red, green, blue);

                    for (int _h = 0; _h < PixelRadious && h0 + _h < bitmap.Height; _h++)
                        for (int _w = 0; _w < PixelRadious && w0 + _w < bitmap.Width; _w++)
                        {
                            newBitmap.SetPixel(w0 + _w, h0 + _h, color);
                        }

                    //w = w + PixelRadious >= bitmap.Width ? w - (PixelRadious - bitmap.Width + PixelRadious) - 1 : w;
                }

                //h = h + PixelRadious >= bitmap.Height ? h - (PixelRadious - bitmap.Height + PixelRadious) - 1 : h;
            }

            return newBitmap;
        }

    }
}
