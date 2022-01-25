using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SortingAlgorithm;

namespace SortingVisualization
{
    public class Visualizator : Form
    {
        static int SortingSpeed = 1;
        static string SortingTitle = "";

        const int WIDTH = 700;
        const int HEIGHT = 500;

        public static int[] arr { get; set; }
        public static int arrSize { get; set; }
        public static int arrMax { get; set; }
        public static int[] coloredIndexes { get; set; } = new int[2] { -1, -1 };

        Timer timer = new Timer();

        public Visualizator()
        {
            this.Size = new Size(WIDTH, HEIGHT);
            this.StartPosition = FormStartPosition.CenterScreen;

            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, true);

            timer.Enabled = true;
            timer.Interval = 1000 / SortingSpeed;
            timer.Tick += new EventHandler((s, e) => { this.Invalidate(); });
        }

        static void Main(string[] args)
        {
            var graphicsTask = new Task(() => Application.Run(new Visualizator()));
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

            var elements = (int)Math.Pow(2, 2);

            while (true)
            {
                SortingSpeed = Math.Min(elements / 2, 1000);

                var original = SortingAlgorithm.SortingAlgorithm.GetRandomArr(elements);

                arrSize = original.Count();
                arrMax = original.Max();
                arr = new int[arrSize];

                Array.Copy(original, arr, arr.Count());

                allDone = true;
                System.Threading.Thread.Sleep(1000);
                allDone = false;

                sortingAlgorithms.ForEach(s => {

                    SortingTitle = s.Name + $" ({elements} elements)\n{SortingSpeed} sorts per second";

                    Array.Copy(original, arr, arr.Count());

                    coloredIndexes = new int[] { -1, -1 };

                    var instance = (SortingAlgorithm.SortingAlgorithm)Activator.CreateInstance(s, 1000 / SortingSpeed);

                    instance.Sort(arr, coloredIndexes);

                    done = true;

                    System.Threading.Thread.Sleep(1500);

                    done = false;
                });

                elements *= 2;
            }
        }

        Font font = new Font("Arial", 11);
        Pen whitePen = new Pen(Brushes.White);
        Pen redPen = new Pen(Brushes.Red);

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

            var elementWidth = Width * .95f / arrSize;
            var elementHeight = Height * .8f / arrMax;

            g.FillRectangle(Brushes.Black, 0, 0, this.Width, this.Height);

            var widthOffset = elementWidth * 0.1f;

            g.DrawString(SortingTitle, font, Brushes.White, 10, 10);

            if (allDone)
                timer.Interval = 1000 / SortingSpeed;

            var rectangles = new RectangleF[arrSize];

            for (int i = 0; i < arrSize; i++)
            {
                //var brush = done || coloredIndexes[0] == i ? Brushes.Red
                //    : Brushes.White;

                rectangles[i] = new RectangleF(
                    x: Width * .015f + i * elementWidth + 0 * widthOffset,
                    y: Height * .9f - elementHeight * arr[i],
                    width: elementWidth - 0 * widthOffset,
                    height: elementHeight * arr[i]
                    );

                //g.FillRectangle(
                //    brush,
                //    x: Width * .015f + i * elementWidth + 0 * widthOffset,
                //    y: Height * .9f - elementHeight * arr[i],
                //    width: elementWidth - 0 * widthOffset,
                //    height: elementHeight * arr[i]);
            }

            var brush = done ? Brushes.LightGreen : Brushes.White;

            g.FillRectangles(brush, rectangles);

            if(!done && coloredIndexes[0] >= 0)
                g.FillRectangle(Brushes.Red, 
                    new RectangleF(
                        x: Width * .015f + coloredIndexes[0] * elementWidth + 0 * widthOffset,
                        y: Height * .9f - elementHeight * arr[coloredIndexes[0]],
                        width: elementWidth - 0 * widthOffset,
                        height: elementHeight * arr[coloredIndexes[0]]
                    ));

            base.OnPaint(e);
        }
    }
}
