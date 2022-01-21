using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageEditor
{
    class Utils
    {
        public static Image FitImageInBoundaries(Image img, int maxWidth, int maxHeight)
        {
            // height = factor * width

            var scaleFactor = img.Height * 1.0 / img.Width;

            var minHeight = Math.Min(img.Height, maxHeight);
            var minWidth = Math.Min(img.Width, maxWidth);

            int newHeight = img.Height;
            var newWidth = img.Width;

            if(img.Width > maxWidth)
            {
                minWidth = maxWidth;
                newHeight = (int)(maxWidth * scaleFactor);
            }

            if (img.Height > maxHeight)
            {
                minHeight = maxHeight;
                newWidth = (int)(minHeight / scaleFactor);
            }


            return new Bitmap(img, newWidth, newHeight);
        }

        public static Image FixedSize(Image imgPhoto, int Width, int Height)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)Width / (float)sourceWidth);
            nPercentH = ((float)Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = System.Convert.ToInt16((Width -
                              (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = System.Convert.ToInt16((Height -
                              (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(imgPhoto, Width, Height);

            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            return bmPhoto;
        }

        public static string ShowEffects()
        {
            var effects = Effect.GetAllEffects();

            var listedEffects = new StringBuilder();

            listedEffects.Append("EFFECTS:\n");

            for (int i = 0; i < effects.Count; i++)
            {
                listedEffects.Append($"\t{(i + 1).ToString().PadLeft(2, ' ')}) {effects[i].Name.Replace("Effect", "")}\n");
            }

            listedEffects.Append("TYPE DONE TO FINISH EDITING");

            return listedEffects.ToString();
        }
    }
}
