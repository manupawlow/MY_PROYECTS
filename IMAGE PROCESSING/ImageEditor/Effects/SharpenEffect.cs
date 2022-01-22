using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace ImageEditor.Effects
{
    class SharpenEffect : Effect
    {
        private float[][] _kernel { get; set; }
        private int _kernelSize { get; set; }
        private int _kernelRadious { get; set; }

        public SharpenEffect(object[] config) : base(config)
        {

            _kernelSize = 3;
            _kernelRadious = 1;

            _kernel = new float[][]
            {
                new float[] {  0, -1,  0 },
                new float[] { -1,  5, -1 },
                new float[] {  0, -1,  0 }
            };

        }

        public override Bitmap ApplyEffect(Bitmap bitmap)
        {
            var newBitmap = new Bitmap(bitmap.Width, bitmap.Height);

            for (int h = 0; h < bitmap.Height; h++)
            {
                for (int w = 0; w < bitmap.Width; w++)
                {
                    var red = 0.0;
                    var green = 0.0;
                    var blue = 0.0;

                    for (int x = 0; x < _kernelSize; ++x)
                    {
                        for (int y = 0; y < _kernelSize; ++y)
                        {
                            int delta_w = x - _kernelRadious;
                            int delta_h = y - _kernelRadious;

                            int w_pos = w + delta_w < 0 || w + delta_w >= bitmap.Width ? w : w + delta_w;
                            int h_pos = h + delta_h < 0 || h + delta_h >= bitmap.Height ? h : h + delta_h;

                            var pixel = bitmap.GetPixel(w_pos, h_pos);

                            red += pixel.R * _kernel[y][x];
                            green += pixel.G * _kernel[y][x];
                            blue += pixel.B * _kernel[y][x];
                        }
                    }


                    red = Math.Max(red, 0);
                    green = Math.Max(green, 0);
                    blue = Math.Max(blue, 0);

                    red = Math.Min(red, 255);
                    green = Math.Min(green, 255);
                    blue = Math.Min(blue, 255);

                    var color = Color.FromArgb((int)red, (int)green, (int)blue);

                    newBitmap.SetPixel(w, h, color);
                }
            }

            return newBitmap;
        }

    }
}
