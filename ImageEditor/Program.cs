using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace ImageEditor
{
    public class Program : Form
    {
        const int FRAME_RATE = 225;

        const int INIT_WIDTH = 700;
        const int INIT_HEIGHT = 500;

        private static bool _di_mutex { get; set; } = false; //display image mutex

        static void Main(string[] args)
        {
            var graphicsTask = new Task(() => Application.Run(new Program()));
            var logicTask = new Task(() => NewMainMenu());

            graphicsTask.Start();
            logicTask.Start();

            var tasks = new List<Task>() { graphicsTask, logicTask };

            Task.WaitAll(tasks.ToArray());
        }

        Timer timer = new Timer();

        public static Image displayImage = null;
        public static Bitmap stuntImage = null;

        public Program()
        {
            this.Size = new Size(INIT_WIDTH, INIT_HEIGHT);
            this.StartPosition = FormStartPosition.CenterScreen;

            timer.Enabled = true;
            timer.Interval = FRAME_RATE;
            timer.Tick += new EventHandler((s, e) => { this.Invalidate(); });
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

            if (displayImage is null)
            {
                g.FillRectangle(Brushes.Gray, 0, 0, Width, Height);
            }
            else
            {
                SetDisplayImage((Bitmap)Utils.FixedSize(displayImage, Width, Height));

                g.DrawImage(Utils.FixedSize(displayImage, Width, Height), 0, 0);
            }

            base.OnPaint(e);
        }

        private static List<DynamicEffect> DynamicEffects = new List<DynamicEffect>();

        //private static void MainLogic()
        //{
        //    var dynamicTask = new Task(() => 
        //    {
        //        while (true)
        //        {
        //            if(displayImage != null && !_di_mutex)
        //            {
        //                var aux = Copy(displayImage);

        //                DynamicEffects.ForEach(e => 
        //                {
        //                    e.ElapsedTime += FRAME_RATE;

        //                    aux = e.ApplyEffect(aux);
        //                });

        //                _di_mutex = true;

        //                displayImage = aux;

        //                _di_mutex = false;
        //            }

        //            Thread.Sleep(FRAME_RATE);
        //        }
        //    });

        //    dynamicTask.Start();

        //    Image selectedImage = null;
        //    Bitmap editedImage = null;
        //    //Bitmap prevImage = null;
        //    string imageName = string.Empty;
        //    var prevImages = new Stack<Bitmap>();
        //    var prevEffectsName = new Stack<string>();
        //    string status = string.Empty;

        //    string lastMessage = string.Empty;

        //    while (true)
        //    {
        //        ImageSelection:

        //        Console.Clear();
        //        Console.WriteLine(PROGRAM_TITLE + "\n" + MENU);
        //        if(!string.IsNullOrEmpty(lastMessage)) Console.WriteLine(lastMessage);

        //        try
        //        {
        //            Console.Write("\nImage: ");
        //            imageName = Path.GetFileNameWithoutExtension(Console.ReadLine());

        //            if (imageName.ToUpper() == "SHOW")
        //            {
        //                var files = Directory.GetFiles(IMAGES_PATH);

        //                lastMessage = "[INFO] Showing images to select\n";

        //                foreach (string file in files)
        //                    lastMessage += " - " + Path.GetFileNameWithoutExtension(file) + "\n";

        //                goto ImageSelection;
        //            }

        //            selectedImage = Image.FromFile(GetImagePath(imageName));

        //            displayImage = Copy(selectedImage);

        //            var effectNames = new List<string>();
        //            var effectName = string.Empty;

        //            //prevImage = new Bitmap(selectedImage, selectedImage.Width, selectedImage.Height);

        //            editedImage = (Bitmap)selectedImage;

        //            while (effectName.ToUpper() != "DONE")
        //            {
        //                try
        //                {
        //                    Console.Clear();
        //                    Console.WriteLine(PROGRAM_TITLE + "\n" + MENU);

        //                    status = imageName.ToLower();
        //                    effectNames.ForEach(e => status += $" + {e}");

        //                    Console.WriteLine($"[STATUS] {status}");
        //                    if (!string.IsNullOrEmpty(lastMessage)) Console.WriteLine(lastMessage);

        //                    Console.Write("Effect: ");
        //                    effectName = Console.ReadLine().Trim();

        //                    if(effectName.ToUpper() == "RESET")
        //                    {
        //                        if(prevImages.Count() > 0)
        //                        {
        //                            editedImage = prevImages.Pop();
        //                            effectNames.Remove(effectNames.Last());
        //                            displayImage = new Bitmap(editedImage, editedImage.Width, editedImage.Height);

        //                            lastMessage = "[INFO] Reseted " + prevEffectsName.Pop().ToLower() + " effect";
        //                        }
        //                        else
        //                        {
        //                            lastMessage = "[WARNING] This is already the original image";
        //                        }
                                
        //                        continue;
        //                    }

        //                    prevEffectsName.Push(effectName);
        //                    prevImages.Push(editedImage);
        //                    //prevImage = new Bitmap(editedImage, editedImage.Width, editedImage.Height);

        //                    var effect = Effect.GetEffect(effectName);

        //                    if (effect.IsDynamic)
        //                        DynamicEffects.Add((DynamicEffect)effect);

        //                    while (_di_mutex)
        //                        Thread.Sleep(50);

        //                    editedImage = effect.ApplyEffect(editedImage);

        //                    effectNames.Add(effectName);

        //                    lastMessage = $"[INFO] {effectName.ToLower()} applied!";

        //                    _di_mutex = true;
        //                    displayImage = Copy(editedImage);
        //                    _di_mutex = false;
        //                }
        //                catch(Exception e)
        //                {
        //                    lastMessage = $"[ERROR] {effectName.ToLower()} doesnt exists. Try again!";
        //                }
        //            }

        //            var savePath = GetEditedImagePath(imageName, effectNames);
        //            editedImage.Save(savePath, ImageFormat.Png);

        //            lastMessage = $"Image editing succesfully! Saved at {savePath}\n\n";

        //            displayImage = null;
        //        }
        //        catch (Exception e)
        //        {
        //            lastMessage = $"\n[ERROR] Something ocurred while trying to edit {imageName}. Error: {e.Message}\n" + "Try again:";
        //        }
        //    }
        //}

        public static void SetDisplayImage(Bitmap image)
        {
            while (_di_mutex)
                Thread.Sleep(50);
            
            _di_mutex = true;

            displayImage = Copy(image);

            _di_mutex = false;
        }

        public static void NewMainMenu()
        {
            var dynamicTask = new Task(() =>
            {
                while (true)
                {
                    if (stuntImage != null && !_di_mutex && DynamicEffects.Count() > 0)
                    {
                        var aux = Copy(stuntImage);

                        DynamicEffects.ForEach(e =>
                        {
                            e.ElapsedTime += FRAME_RATE;

                            aux = e.ApplyEffect(aux);
                        });

                        SetDisplayImage(aux);
                    }

                    Thread.Sleep(FRAME_RATE);
                }
            });

            dynamicTask.Start();

            var mm = new MenuManager();

            while (true)
            {
                Console.Clear();

                mm.DisplayMenu();

                try
                {
                    var imageName = mm.GetImageName();

                    Console.Clear(); mm.DisplayMenu();

                    if (mm.IsShowImages(imageName))
                    {
                        mm.ShowFiles();
                        continue;
                    }

                    stuntImage = (Bitmap)mm.GetImage(imageName);

                    displayImage = Copy(stuntImage);

                    var effectName = string.Empty;

                    do
                    {
                        Console.Clear(); mm.DisplayMenu();

                        effectName = mm.GetEffectName();

                        if (mm.IsResetEffect(effectName))
                        {
                            stuntImage = mm.ResetEffect() ?? stuntImage;

                            SetDisplayImage(Copy(stuntImage));

                            continue;
                        }

                        Effect effect;
                        try
                        {
                            effect = Effect.GetEffect(effectName);
                        }
                        catch (Exception e)
                        {
                            mm.EffectDoesntExists(effectName);
                            continue;
                        }

                        if (effect.IsDynamic)
                        {
                            DynamicEffects.Add((DynamicEffect)effect);
                        }
                        else
                        {
                            var prevImage = stuntImage;
                            stuntImage = effect.ApplyEffect(stuntImage);

                            mm.AddEffect(Copy(stuntImage), prevImage, effectName);

                            SetDisplayImage(stuntImage);
                        }
                    }
                    while (!mm.IsFinishEffects(effectName));

                    mm.SaveImage();

                    displayImage = null;
                }
                catch (Exception e)
                {
                    mm.UnknownError(e);
                }
            }

        }

        public static Bitmap Copy(Image img) => new Bitmap(img, img.Width, img.Height);
    }
}