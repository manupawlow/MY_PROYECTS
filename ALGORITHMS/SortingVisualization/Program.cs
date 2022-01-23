using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SortingVisualization
{
    class Program
    {
        static void Main(string[] args)
        {
            var visualizator = new Visualizator()
            var graphicsTask = new Task(() => Application.Run(new Visualizator()));
            var logicTask = new Task(() => MainLogic());

            graphicsTask.Start();
            logicTask.Start();

            var tasks = new List<Task>() { graphicsTask, logicTask };

            Task.WaitAll(tasks.ToArray());
        }
    }
}
