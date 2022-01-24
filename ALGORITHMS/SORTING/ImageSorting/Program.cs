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
        static int SortingSpeed = 1000;

        const int WIDTH = 700;
        const int HEIGHT = 500;

        public static Bitmap originalImage { get; set; }
        public static int[] indexes { get; set; }
        public static int[] originalIndexes { get; set; }
        public static int[] coloredIndexes { get; set; }
        public static int imgWidth { get; set; }
        public static int imgHeight { get; set; }
        public static int pixelRadious { get; set; }

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

        public static void MainLogic()
        {
            pixelRadious = 4;

            originalImage = (Bitmap)Image.FromFile(@"C:\Users\79\Desktop\MY_PROYECTS\IMAGE PROCESSING\PhotoShop\Images\buzz.jpg");

            imgWidth = originalImage.Width;
            imgHeight = originalImage.Height;

            var original = new List<int>();

            for (int h = 0; h < imgHeight; h += pixelRadious)
            {
                for (int w = 0; w < imgWidth; w += pixelRadious)
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

                    System.Threading.Thread.Sleep(1000);
                });
            }
        }

        private Bitmap GenerateImage()
        {
            var bitmap = new Bitmap(imgWidth, imgHeight);

            if(pixelRadious == 1)
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

                    for (int _h = 0; _h < pixelRadious && yo + _h < bitmap.Height && yn + _h < bitmap.Height; _h++)
                    {
                        for (int _w = 0; _w < pixelRadious && xo + _w < bitmap.Width && xn + _w < bitmap.Width; _w++)
                        {
                            var color = isSelected ? Color.Red : originalImage.GetPixel(xo + _w, yo + _h);

                            bitmap.SetPixel(xn + _w, yn + _h, color);
                        }
                    }
                }
            }
               
            return bitmap;
        }



        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;

            g.Clear(Color.Black);

            if (displayImage != null)
                g.DrawImage(displayImage, 0, 0, WIDTH, HEIGHT);

            base.OnPaint(e);
        }
    }
}
