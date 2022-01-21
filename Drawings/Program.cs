using Drawings.Drawings;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Drawings
{
    public class Visualizator : Form
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Program main...");

            var drawing = new AxisStar();

            var drawTask = new Task(() => Application.Run(drawing));
            var logicTask = new Task(() => drawing.MainLogic());

            logicTask.Start();

            drawTask.Start();

            drawTask.Wait();
        }

    }
}
