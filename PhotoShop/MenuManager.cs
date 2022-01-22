using ImageEditor;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoShop
{
    public class MenuManager
    {
        IConfigurationRoot configuration;

        private struct CONSTANTS
        {
            public static readonly string DIRECTORY_PATH = Directory.GetCurrentDirectory();
            public static readonly string IMAGES_PATH = Path.Combine(DIRECTORY_PATH, "Images");
            public static readonly string EDITED_IMAGES_PATH = Path.Combine(DIRECTORY_PATH, "EditedImages");
            public static readonly string[] EXTENSIONS = { ".png", ".jpg", ".jpeg" };
        }

        private struct MENU_TITLES
        {
            public const string PROGRAM_TITLE = "**********************\n***  IMAGE EDITOR  ***\n**********************\n";
            public static string SELECTION_TITLE = "Select an image and apply any effect.\n" + ShowEffects();
            public static string STATUS_TITLE(string status) => "[STATUS] " + status;
            public static string LAST_MESSAGE_TITLE(string logLevel, string message) => $"[{logLevel}] {message}";
            public static string SELECT_IMAGE = "Image: ";
            public static string SELECT_EFFECT = "Effect: ";
        }


        public string CURR_IMAGE { get; set; }
        public string LAST_MESSAGE { get; set; }
        public List<string> EFFECTS_NAMES { get; set; }
        public Stack<Bitmap> PREV_IMAGES { get; set; }

        public MenuManager(Microsoft.Extensions.Configuration.IConfigurationRoot config)
        {
            configuration = config;

            CURR_IMAGE = string.Empty;
            LAST_MESSAGE = string.Empty;
            EFFECTS_NAMES = new List<string>();
            PREV_IMAGES = new Stack<Bitmap>();
        }

        public bool IsShowImages(string image) => image.ToUpper() == "SHOW";
        public bool IsFinishEffects(string effect) => effect.ToUpper() == "DONE";
        public bool IsResetEffect(string effect) => effect.ToUpper() == "RESET";

        public void SaveImage(Image img)
        {
            var filename = Path.Combine(configuration["EDITED_IMAGES_PATH"], CURR_IMAGE);

            foreach (var effect in EFFECTS_NAMES.Select(e => e.Split(':')[0]))
                filename += "[" + effect + "]";

            int counter = 0;
            while (File.Exists(filename + $"-{counter}.png"))
                counter++;

            filename += $"-{counter}.png";

            img.Save(filename, System.Drawing.Imaging.ImageFormat.Png);
            
            LAST_MESSAGE = MENU_TITLES.LAST_MESSAGE_TITLE("INFO", $"Image editing succesfully! Saved at {filename}");

            CURR_IMAGE = string.Empty;
            EFFECTS_NAMES.Clear();
            PREV_IMAGES.Clear();
        }

        public Bitmap ResetEffect()
        {
            if(PREV_IMAGES.Count() > 0)
            {
                var lastEffect = EFFECTS_NAMES.Last();
                EFFECTS_NAMES.Remove(lastEffect);

                LAST_MESSAGE = MENU_TITLES.LAST_MESSAGE_TITLE("INFO", "Reseted " + lastEffect.ToLower() + " effect");

                return PREV_IMAGES.Pop();
            }

            LAST_MESSAGE = MENU_TITLES.LAST_MESSAGE_TITLE("WARNING", "This is already the original image");

            return null;
        }

        public void AddEffect(Bitmap editedImage, Bitmap prevImage, string effectName)
        {
            EFFECTS_NAMES.Add(effectName);

            PREV_IMAGES.Push(prevImage);
            
            LAST_MESSAGE = MENU_TITLES.LAST_MESSAGE_TITLE("INFO", effectName.ToLower() + " applied!");
        }

        public void DisplayMenu()
        {
            var console = new StringBuilder();

            console.AppendLine(MENU_TITLES.PROGRAM_TITLE);

            console.AppendLine(MENU_TITLES.SELECTION_TITLE);

            console.AppendLine(MENU_TITLES.STATUS_TITLE(GetStatus()));

            console.AppendLine(LAST_MESSAGE);

            Console.Write(console.ToString());
        }

        public string GetImageName()
        {
            Console.Write(MENU_TITLES.SELECT_IMAGE);

            var selection = Path.GetFileNameWithoutExtension(Console.ReadLine());

            LAST_MESSAGE = MENU_TITLES.LAST_MESSAGE_TITLE("INFO", "Selected image: " + selection.ToLower());

            CURR_IMAGE = selection;

            return selection;
        }

        public string GetEffectName()
        {
            Console.Write(MENU_TITLES.SELECT_EFFECT);

            var selection = Console.ReadLine();

            LAST_MESSAGE = MENU_TITLES.LAST_MESSAGE_TITLE("INFO", "Selected effect: " + selection.ToLower());

            return selection;
        }

        public void ShowFiles()
        {
            var files = Directory.GetFiles(CONSTANTS.IMAGES_PATH);

            var fileNames = new StringBuilder();

            foreach (string file in files)
                fileNames.AppendLine(" - " + Path.GetFileNameWithoutExtension(file));

            LAST_MESSAGE = MENU_TITLES.LAST_MESSAGE_TITLE("INFO", "Showing images to select\n" + fileNames.ToString());
        }

        public Image GetImage(string fileName)
        {
            var extension = CONSTANTS.EXTENSIONS.First(e => File.Exists(Path.Combine(CONSTANTS.IMAGES_PATH, fileName + e)));

            var path = Path.Combine(CONSTANTS.IMAGES_PATH, fileName + extension);

            return Image.FromFile(path);
        }

        private string GetStatus()
        {
            if (string.IsNullOrEmpty(CURR_IMAGE))
                return "No status";

            var status = new StringBuilder(CURR_IMAGE.ToLower());

            EFFECTS_NAMES.ForEach(e => status.Append(" + " + e));

            return status.ToString();
        }

        public void EffectDoesntExists(string effectName)
        {
            LAST_MESSAGE = MENU_TITLES.LAST_MESSAGE_TITLE("ERROR", effectName.ToLower() + " doesnt exits. Try again");
        }

        public void UnknownError(Exception e)
        {
            LAST_MESSAGE = MENU_TITLES.LAST_MESSAGE_TITLE("ERROR", $"Something ocurred while trying to edit {CURR_IMAGE}. Error: {e.Message}");
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
