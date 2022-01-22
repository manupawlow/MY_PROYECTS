using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace ImageEditor.Effects
{
    class EdgeEffect : Effect
    {
        private float[][] _kernelX { get; set; }
        private float[][] _kernelY { get; set; }
        private int _kernelSize { get; set; }
        private int _kernelRadious { get; set; }

        public EdgeEffect(object[] config) : base(config)
        {

            _kernelSize = 3;
            _kernelRadious = 1;

            _kernelX = new float[][]
            {
                new float[] { -1, 0, 1 },
                new float[] { -2, 0, 2 },
                new float[] { -1, 0, 1 }
            };

            _kernelY = new float[][]
            {
                new float[] { -1, -2, -1 },
                new float[] {  0,  0,  0 },
                new float[] {  1,  2,  1 }
            };
        }


        public override Bitmap ApplyEffect(Bitmap bitmap)
        {
            bool withOrientation = false;
            try
            {
                withOrientation = Convert.ToBoolean(Configuration[0]);
            }
            catch
            {
            }

            var newBitmap = new Bitmap(bitmap.Width, bitmap.Height);

            for (int h = 0; h < bitmap.Height; h++)
            {
                for (int w = 0; w < bitmap.Width; w++)
                {
                    var color = EdgePixel(bitmap, h, w);

                    newBitmap.SetPixel(w, h, color);
                    //newBitmap.SetPixel(w, h, GxColor);
                }
            }

            return newBitmap;
        }

        private Color EdgePixel(Bitmap bitmap, int h, int w)
        {
            var pixel = bitmap.GetPixel(w, h);

            //var gray_color = 0.2126 * pixel.R + 0.7152 * pixel.G + 0.0722 * pixel.B;

            var Gx = 0.0;
            var Gy = 0.0;

            var GxColor = Color.FromArgb(0, 0, 0);

            for (int x = 0; x < _kernelSize; ++x)
            {
                for (int y = 0; y < _kernelSize; ++y)
                {
                    int delta_w = x - _kernelRadious;
                    int delta_h = y - _kernelRadious;

                    int w_pos = w + delta_w < 0 || w + delta_w >= bitmap.Width ? w : w + delta_w;
                    int h_pos = h + delta_h < 0 || h + delta_h >= bitmap.Height ? h : h + delta_h;

                    Color n_pixel = bitmap.GetPixel(w_pos, h_pos);
                    var gray_color = 0.2126 * n_pixel.R + 0.7152 * n_pixel.G + 0.0722 * n_pixel.B;

                    Gx += gray_color * _kernelX[x][y];
                    Gy += gray_color * _kernelY[x][y];

                    var r = Math.Min(255, Math.Max((int)(GxColor.R + n_pixel.R * _kernelX[x][y]), 0));
                    var g = Math.Min(255, Math.Max((int)(GxColor.G + n_pixel.G * _kernelX[x][y]), 0));
                    var b = Math.Min(255, Math.Max((int)(GxColor.B + n_pixel.B * _kernelX[x][y]), 0));
                    GxColor = Color.FromArgb(r, g, b);
                }
            }

            var G = Math.Sqrt(Gx * Gx + Gy * Gy);

            var int_color = Math.Min(255, Math.Max((int)G, 0));

            return Color.FromArgb(int_color, int_color, int_color);
        }
    }
}
