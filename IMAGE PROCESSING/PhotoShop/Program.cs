using ImageEditor;
using Microsoft.Extensions.Configuration;
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

namespace PhotoShop
{
    public class Program : Form
    {
        IConfigurationRoot configuration;

        const int FRAME_RATE = 225;

        const int INIT_WIDTH = 700;
        const int INIT_HEIGHT = 500;

        private static bool _di_mutex { get; set; } = false; //display image mutex

        static void Main(string[] args)
        {
            var program = new Program();

            var graphicsTask = new Task(() => Application.Run(program));
            var logicTask = new Task(() => program.MainMenu());

            graphicsTask.Start();
            logicTask.Start();

            var tasks = new List<Task>() { graphicsTask, logicTask };

            Task.WaitAll(tasks.ToArray());
        }

        public static Image displayImage = null;
        public static Bitmap stuntImage = null;

        public Program()
        {
            this.Size = new Size(INIT_WIDTH, INIT_HEIGHT);
            this.StartPosition = FormStartPosition.CenterScreen;
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
                g.DrawImage(displayImage, 0, 0);
            }

            base.OnPaint(e);
        }

        public void MainMenu()
        {
            configuration = new ConfigurationBuilder().AddJsonFile("config.json", optional: true).Build();

            var mm = new MenuManager(configuration);

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

                    Draw();

                    var effectName = string.Empty;

                    do
                    {
                        Console.Clear(); mm.DisplayMenu();

                        effectName = mm.GetEffectName();

                        if (mm.IsResetEffect(effectName))
                        {
                            stuntImage = mm.ResetEffect() ?? stuntImage;

                            Draw();

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

                        var prevImage = stuntImage;
                        stuntImage = effect.ApplyEffect(stuntImage);

                        mm.AddEffect(Copy(stuntImage), prevImage, effectName);

                        Draw();

                    } while (!mm.IsFinishEffects(effectName));

                    mm.SaveImage(displayImage);

                    stuntImage = null;

                    Draw();
                }
                catch (Exception e)
                {
                    mm.UnknownError(e);
                }
            }
        }

        private void Draw()
        {
            displayImage = stuntImage is null ? stuntImage : Utils.FixedSize(stuntImage, Width, Height);

            Invalidate();
        }

        public static Bitmap Copy(Image img) => new Bitmap(img, img.Width, img.Height);
    }
}