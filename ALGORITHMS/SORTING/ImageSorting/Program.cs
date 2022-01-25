using SortingAlgorithm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageSorting
{
    class Program : Form
    {
        static int SortingSpeed = 1;

        const int WIDTH = 700;
        const int HEIGHT = 500;

        public static Bitmap originalImage { get; set; }
        public static int[] indexes { get; set; }
        public static int[] originalIndexes { get; set; }
        public static int[] coloredIndexes { get; set; }
        public static int imgWidth { get; set; }
        public static int imgHeight { get; set; }
        public static int pixelRadiousX { get; set; }
        public static int pixelRadiousY { get; set; }

        public Bitmap displayImage { get; set; }

        Timer timer = new Timer();

        public Program()
        {
            this.Size = new Size(WIDTH, HEIGHT);
            this.StartPosition = FormStartPosition.CenterScreen;

            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, true);

            timer.Enabled = true;
            timer.Interval = 1000 / SortingSpeed;
            timer.Tick += new EventHandler((s, e) => { displayImage = GenerateImage(); this.Invalidate(); });
        }

        static void Main(string[] args)
        {
            var graphicsTask = new Task(() => Application.Run(new Program()));
            var logicTask = new Task(() => MainLogic());

            graphicsTask.Start();
            logicTask.Start();

            var tasks = new List<Task>() { graphicsTask, logicTask };

            Task.WaitAll(tasks.ToArray());
        }

        public static bool done = false;

        public static void MainLogic()
        {
            originalImage = (Bitmap)Image.FromFile(@"C:\Users\79\Desktop\MY_PROYECTS\IMAGE PROCESSING\PhotoShop\Images\buzz.jpg");

            imgWidth = originalImage.Width;
            imgHeight = originalImage.Height;

            var original = new List<int>();

            pixelRadiousX = imgWidth / 5;
            pixelRadiousY = imgHeight / 5;

            for (int h = 0; h < imgHeight; h += pixelRadiousY)
            {
                for (int w = 0; w < imgWidth; w += pixelRadiousX)
                {
                    original.Add(h * imgWidth + w);
                }
            }

            originalIndexes = original.ToArray();
            indexes = new int[originalIndexes.Length];

            var sortingAlgorithms = new List<Type>()
            {
                //typeof(BogoSort),
                typeof(QuickSort),
                typeof(HeapSort),
                typeof(MergeSort),
                typeof(SelectionSort),
                typeof(InsertionSort),
                typeof(BubbleSort),
            };

            //indexes = SortingAlgorithm.SortingAlgorithm.RandomizeArr(indexes);

            coloredIndexes = new int[2];

            while (true)
            {
                sortingAlgorithms.ForEach(s =>
                {
                    indexes = SortingAlgorithm.SortingAlgorithm.RandomizeArr(originalIndexes);

                    var instance = (SortingAlgorithm.SortingAlgorithm)Activator.CreateInstance(s, 1000 / (SortingSpeed * 1));

                    instance.Sort(indexes, coloredIndexes);

                    done = true;
                    System.Threading.Thread.Sleep(1000);
                    done = false;
                });
            }
        }

        private Bitmap GenerateImage()
        {
            var bitmap = new Bitmap(imgWidth, imgHeight);

            if(pixelRadiousX == 1 && pixelRadiousY == 1)
            {
                if (indexes != null)
                {
                    for (int i = 0; i < imgHeight; i++)
                    {
                        for (int j = 0; j < imgWidth; j++)
                        {
                            var index = indexes[i * imgWidth + j];

                            var x = index % imgWidth;
                            var y = index / imgWidth;
                        
                            bitmap.SetPixel(j, i, originalImage.GetPixel(x, y));
                        }
                    }
                }
            }
            else
            {
                int indexN, indexO;
                for (int i = 0; i < indexes.Length; i++)
                {
                    indexO = originalIndexes[i];
                    indexN = indexes[i];

                    var xo = indexO % imgWidth;
                    var yo = indexO / imgWidth;

                    var xn = indexN % imgWidth;
                    var yn = indexN / imgWidth;

                    var isSelected = coloredIndexes[0] == i;

                    for (int _h = 0; _h < pixelRadiousY && yo + _h < bitmap.Height && yn + _h < bitmap.Height; _h++)
                    {
                        for (int _w = 0; _w < pixelRadiousX && xo + _w < bitmap.Width && xn + _w < bitmap.Width; _w++)
                        {
                            var originalColor = originalImage.GetPixel(xo + _w, yo + _h);

                            var color = /*isSelected && !done ? BrightColor(originalColor) : */originalColor;

                            bitmap.SetPixel(xn + _w, yn + _h, color);
                        }
                    }
                }
            }
               
            return bitmap;
        }

        private Color BrightColor(Color color)
        {
            var r = (int)Math.Min(color.R * 1.5, 255);
            var g = (int)Math.Min(color.G * 1.5, 255);
            var b = (int)Math.Min(color.B * 1.5, 255);

            return Color.FromArgb(r, g, b);
        }

        Pen selectedPen = new Pen(Color.Red, 1);

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;

            g.Clear(Color.Black);

            if (displayImage != null)
            {
                g.DrawImage(displayImage, 0, 0, WIDTH, HEIGHT);

                var x = coloredIndexes[0] % imgWidth;
                var y = coloredIndexes[0] / imgWidth;

                g.DrawRectangle(selectedPen, x, y, pixelRadiousX, pixelRadiousY);
            }

            base.OnPaint(e);
        }
    }
}
