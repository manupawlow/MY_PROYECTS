using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageEditor.Effects
{
    class DitherEffect : Effect
    {
        public DitherEffect(object[] config) : base(config){ }

        public override Bitmap ApplyEffect(Bitmap bitmap)
        {
            var width = bitmap.Width;
            var height = bitmap.Height;

            var steps = 1.0;
            try
            {
                steps = Convert.ToDouble(Configuration[0]);
            }
            catch
            {
            }

            //var newBitmap = new Bitmap(bitmap.Width, bitmap.Height);

            for (int i = 0; i < height; i++)
            {
                for(int j = 0; j < width; j++)
                {
                    var oldColor = bitmap.GetPixel(j, i);

                    var newR = FindClosestPalletColor(oldColor.R, steps);
                    var newG = FindClosestPalletColor(oldColor.G, steps);
                    var newB = FindClosestPalletColor(oldColor.B, steps);

                    var newColor = Color.FromArgb(newR, newG, newB);
                    bitmap.SetPixel(j, i, newColor);

                    var quant_error = new int[3]
                    {
                        oldColor.R - newColor.R,
                        oldColor.G - newColor.G,
                        oldColor.B - newColor.B
                    };

                    DistributeError(bitmap, j, i, quant_error);
                }
            }

            return bitmap;
        }

        private void DistributeError(Bitmap newBitmap, int x, int y, int[] errors)
        {
            AddError(newBitmap, 7 / 16.0, x + 1, y + 0, errors);
            AddError(newBitmap, 3 / 16.0, x - 1, y + 1, errors);
            AddError(newBitmap, 5 / 16.0, x + 0, y + 1, errors);
            AddError(newBitmap, 1 / 16.0, x - 0, y + 1, errors);
        }

        private void AddError(Bitmap img, double factor, int x, int y, int[] errors)
        {
            if (IsBound(x, y, img.Width, img.Height))
                return;

            var color = img.GetPixel(x,y);

            var r = (byte)(color.R + errors[0] * factor);
            var g = (byte)(color.G + errors[1] * factor);
            var b = (byte)(color.B + errors[2] * factor);

            img.SetPixel(x, y, Color.FromArgb(r, g, b));
        }

        private int FindClosestPalletColor(int color, double steps = 1.0) 
            => (int)(Math.Round(steps * color / 255) * Math.Floor(255.0 / steps));

        public bool IsBound(int x, int y, int width, int height) => x < 0 || x >= width || y < 0 || y >= height;

    }
}
