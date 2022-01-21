////using ImageEditor.Effects;
////using System;
////using System.Collections.Generic;
////using System.Drawing;
////using System.Drawing.Imaging;
////using System.IO;
////using System.Linq;
////using System.Text;
////using System.Threading.Tasks;

////namespace ImageEditor
////{
////    public class ImageEditor
////    {
////        public const string DirectoyPath = @"C:\Users\79\Desktop\MY PROYECTS\ImageEditor\";
////        public const string ImagesPath = DirectoyPath + @"Images\";
////        public const string EditedImagesPath = DirectoyPath + @"EditedImages\";
        
////        public List<Effect> Effects { get; }

////        public ImageEditor(string[] effects)
////        {
////            //FileName = filename;

////            Effects = new List<Effect>();

////            SetEffects(effects);
////        }

////        public void SetEffects(string[] effectsNames)
////        {
////            Effects.Clear();
            
////            foreach (var effect in effectsNames)
////                Effects.Add(Effect.GetEffect(effect));
////        }

////        public Bitmap EditImage(Bitmap image)
////        {
////            //var image = Image.FromFile(DirectoyPath + FileName + ".jpg");

////            var bitmap = new Bitmap(image, image.Width, image.Height);

////            Effects.ForEach(effect =>
////            {
////                bitmap = effect.ApplyEffect(bitmap);
////                Console.WriteLine(effect.GetType().Name.Replace("Effect", "") + " applied!");
////            });

////            return bitmap;

////            ////SAVING .PNG FILE
////            //var newFileName = FileName;
////            //foreach (var effect in Effects)
////            //    newFileName += "-" + effect.EffectName;
////            //newFileName += ".png";

////            //var newFile = ImagePath + newFileName;

////            //if (File.Exists(newFile))
////            //    File.Delete(newFile);

////            //bitmap.Save(newFile, ImageFormat.Png);
////        }

        
////        //public Image FromPathToImage(string path) => Image.FromFile(DirectoyPath + FileName + ".jpg");
////    }
////}
