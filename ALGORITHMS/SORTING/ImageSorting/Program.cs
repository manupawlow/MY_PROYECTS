using SortingAlgorithm;
using System;
using System.Collections.Generic;
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

        public static Color[] pixels { get; set; }
        public static int[] indexes { get; set; }
        public static int arrSize { get; set; }
        public static int imgWidth { get; set; }
        public static int imgHeight { get; set; }

        Timer timer = new Timer();

        public Program()
        {
            this.Size = new Size(WIDTH, HEIGHT);
            this.StartPosition = FormStartPosition.CenterScreen;

            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, true);

            timer.Enabled = true;
            timer.Interval = 1000 / SortingSpeed;
            timer.Tick += new EventHandler((s, e) => { Console.WriteLine("Tick"); this.Invalidate(); });
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
        public static bool allDone = false;

        public static void MainLogic()
        {
            var image = (Bitmap)Image.FromFile(@"C:\Users\79\Desktop\logo.jpg");

            imgWidth = image.Width;
            imgHeight = image.Height;
            arrSize = imgWidth * imgHeight;

            pixels = new Color[arrSize];
            indexes = new int[arrSize];

            for (int i = 0; i < imgHeight; i++) 
            {
                for(int j = 0; j < imgWidth; j++)
                {
                    var index = i * imgWidth + j;
                    pixels[index] = image.GetPixel(j, i);
                    indexes[index] = index;
                }
            }

            var sortingAlgorithms = new List<Type>()
            {
                typeof(BogoSort),
                typeof(QuickSort),
                typeof(HeapSort),
                typeof(MergeSort),
                typeof(SelectionSort),
                typeof(InsertionSort),
                typeof(BubbleSort),
            };

            //indexes = SortingAlgorithm.SortingAlgorithm.RandomizeArr(indexes);

            while (true)
            {

                sortingAlgorithms.ForEach(s =>
                {
                    indexes = SortingAlgorithm.SortingAlgorithm.RandomizeArr(indexes);
                    
                    var instance = (SortingAlgorithm.SortingAlgorithm)Activator.CreateInstance(s,  1000 / (SortingSpeed * 1));

                    instance.Sort(indexes, new int[] { -1, -1 });
                });
            }
        }

        private Bitmap GenerateImage()
        {
            var bitmap = new Bitmap(imgWidth, imgHeight);

            for (int i = 0; i < imgHeight; i++)
            {
                for (int j = 0; j < imgWidth; j++)
                {
                    var index = indexes[i * imgWidth + j];

                    var color = pixels[index];

                    bitmap.SetPixel(j, i, color);
                }
            }

            return bitmap;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

            g.Clear(Color.Black);

            g.DrawImage(GenerateImage(), 0, 0, WIDTH, HEIGHT);

            base.OnPaint(e);
        }
    }
}
