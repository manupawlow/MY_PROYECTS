using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ImageEditor.Effects
{
    class AsciiEffect : Effect
    {
        private readonly char[] ASCII_CHARACTERS = { '#', '@', '=', '+', '-', ' ' };

        /*
            private static string[] ascii_characters_low = { "█", "#", "=", ":", "-", " " };

            private static string[] ascii_characters_medium = { "█", "▓", "▒", "░", "#", "=", ":", "-", " " };

            private static string[] ascii_characters_high = { "█", "▓", "▒", "░", "#", "@", "±", "=", "+", ":", "-", "°", ".", " " };

            private static string[] ascii_characters_black_and_white = { "█", "▓", " ", " " };
        */

        public AsciiEffect(object[] config) : base(config)
        {
        }

        public override Bitmap ApplyEffect(Bitmap bitmap)
        {
            var charSize = (int)(bitmap.Width * 0.012);
            var fontName = "Arial";

            try
            {
                charSize = Convert.ToInt16(Configuration[0]);
                fontName = Convert.ToString(Configuration[1]);
            }
            catch { }

            var newBitmap = new PixelEffect(new object[] { charSize }).ApplyEffect(bitmap);

            var asciiBitmap = new StringBuilder();

            for (int h = 0; h < bitmap.Height; h += charSize)
            {
                for (int w = 0; w < bitmap.Width; w += charSize)
                {
                    var pixel = newBitmap.GetPixel(w, h);

                    var gray = pixel.R * .2126 + pixel.G * .7152 + pixel.B * .0722;

                    var index = (int)gray * ASCII_CHARACTERS.Length / 255;

                    asciiBitmap.Append(ASCII_CHARACTERS[index]);

                    //for (int k = 0; k < charSize; k++)
                    //{
                    //    asciiBitmap.Append(ASCII_CHARACTERS[index]);
                    //}
                }

                asciiBitmap.Append("\n");
            }

            return Convert_Text_to_Image(asciiBitmap.ToString(), bitmap.Width, bitmap.Height, charSize, fontName);
        }

        public static Bitmap Convert_Text_to_Image(string txt, int width, int height, int fontSize, string fontName)
        {
            Bitmap bmp = new Bitmap(width, height);

            Graphics graphics = Graphics.FromImage(bmp);

            Font font = new Font(FontFamily.GenericMonospace, fontSize);

            var stringSize = graphics.MeasureString(txt, font);

            bmp = new Bitmap(bmp, (int)stringSize.Width, (int)stringSize.Height);


            graphics = Graphics.FromImage(bmp);

            graphics.FillRectangle(Brushes.Black, 0, 0, bmp.Width, bmp.Height);

            graphics.DrawString(txt, font, Brushes.White, 0, 0);
            font.Dispose();
            graphics.Flush();
            graphics.Dispose();

            return bmp;
        }
    }
}
