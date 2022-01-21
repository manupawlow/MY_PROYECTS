using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageEditor.Effects
{
    public class BlurEffect : Effect
    {
        //public float[,] kernel { get; set; }
        public float[] kernel { get; set; }
        public int kernelSize { get; set; }

        public BlurEffect(object[] config) : base(config)
        {
            //kernel = new float[3, 3]{ { 1/16f, 1/8f, 1/16f},
            //                          { 1/8f,  1/4f, 1/8f },
            //                          { 1/16f, 1/8f, 1/16f}
            //}; kernelSize = 3;

            ////kernel = new float[5, 5]{ { 1/256f, 4/256f, 6/256f, 4/256f, 1/256f},
            ////                          { 4/256f, 16/256f, 24/256f, 16/256f, 4/256f},
            ////                          { 6/256f, 24/256f, 36/256f, 24/256f, 6/256f},
            ////                          { 4/256f, 16/256f, 24/256f, 16/256f, 4/256f},
            ////                          { 1/256f, 4/256f, 6/256f, 4/256f, 1/256f},
            ////}; kernelSize = 5;
            ///
            kernel = new float[13]
			{
				0.002216f, 0.008764f, 0.026995f, 0.064759f, 0.120985f, 0.176033f, 0.199471f, 0.176033f, 0.120985f, 0.064759f, 0.026995f, 0.008764f, 0.002216f,
			};
            kernelSize = 13;
        }

        public override Bitmap ApplyEffect(Bitmap bitmap)
        {
            var newBitmap = new Bitmap(bitmap.Width, bitmap.Height);

            int kernelRadius = 6;

            for (int h = 0; h < bitmap.Height; h++)
            {
                for (int w = 0; w < bitmap.Width; w++)
                {
                    float red = 0;
                    float green = 0;
                    float blue = 0;

                    for (int x = 0; x < kernelSize; ++x)
                        for (int y = 0; y < kernelSize; ++y)
                        {
                            int delta_w = x - kernelRadius;
                            int delta_h = y - kernelRadius;

                            int w_pos = w + delta_w < 0 || w + delta_w >= bitmap.Width ? w : w + delta_w;
                            int h_pos = h + delta_h < 0 || h + delta_h >= bitmap.Height ? h : h + delta_h;

                            Color pixel = bitmap.GetPixel(w_pos, h_pos);

                            red += pixel.R * kernel[x] * kernel[y];
                            green += pixel.G * kernel[x] * kernel[y];
                            blue += pixel.B * kernel[x] * kernel[y];
                        }

                    var finalColor = Color.FromArgb((int)red, (int)green, (int)blue);

                    newBitmap.SetPixel(w, h, finalColor);
                }

            }

            return newBitmap;
        }

    }
}
