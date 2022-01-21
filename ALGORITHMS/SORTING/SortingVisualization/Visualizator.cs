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
        static int SortingSpeed = 50;
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

            timer.Enabled = true;
            timer.Interval = SortingSpeed;
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

        private static void MainLogic()
        {
            Random rnd = new Random();

            var original = SortingAlgorithm.SortingAlgorithm.GetInOrderArr(50, descending: !true)
                //.OrderBy(x => rnd.Next()).ToArray()
                ;

            arrSize = original.Count();
            arrMax = original.Max();
            arr = new int[arrSize];

            Array.Copy(original, arr, arr.Count());

            System.Threading.Thread.Sleep(500);

            var sortingAlgorithms = new List<Type>() 
            {
                typeof(MergeSort),
                typeof(InsertionSort),
                typeof(HeapSort),
                typeof(SelectionSort),
                typeof(QuickSort),
                typeof(BubbleSort),
            };

            while (true)
            {
                sortingAlgorithms.ForEach(s => {

                    System.Threading.Thread.Sleep(1500);

                    SortingTitle = s.Name;

                    Array.Copy(original, arr, arr.Count());

                    coloredIndexes = new int[] { -1, -1 };

                    var instance = (SortingAlgorithm.SortingAlgorithm)Activator.CreateInstance(s, SortingSpeed);

                    instance.Sort(arr, coloredIndexes);
                });
            }

            //Array.Copy(original, arr, arr.Count());

            //sortingAlgoritm = new BubbleSort(SortingSpeed);
            //sortingAlgoritm.Sort(arr, coloredIndexes);

            //System.Threading.Thread.Sleep(1000);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

            var elementWidth = this.Width * .95 / arrSize;
            var elementHeight = this.Height * .8 / arrMax;

            g.FillRectangle(Brushes.Black, 0, 0, this.Width, this.Height);

            var widthOffset = elementWidth * 0.1;

            g.DrawString(SortingTitle, new Font("Arial", 11), Brushes.White, 10, 10);

            for (int i = 0; i < arrSize; i++)
            {
                var brush = coloredIndexes[0] == i ? Brushes.LightGreen 
                    : coloredIndexes[1] == i ? Brushes.LightBlue 
                    : Brushes.White;

                g.FillRectangle(
                    brush,
                    x: (int)(i * elementWidth + widthOffset), 
                    y: (int)(this.Height * .9) - (int)(elementHeight * arr[i]), 
                    width: (int)(elementWidth - widthOffset), 
                    height: (int)(elementHeight * arr[i]));
            }

            base.OnPaint(e);
        }
    }
}
