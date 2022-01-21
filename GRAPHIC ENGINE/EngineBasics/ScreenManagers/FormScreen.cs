using GraphicsEngine;
using GraphicsEngine.Graphics;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EngineBasics.ScreenManagers
{
    public class FormScreen : IScreenManager
    {
        public FormConsole Form { get; set; }
        public Task DrawThread { get; set; }

        List<int> Ids { get; set; }

        public FormScreen(int width, int height)
        {
            Form = new FormConsole(width, height);

            Ids = new List<int>();
        }

        public int Height() => Form.Height;
        public int Width() => Form.Width;

        public void ClearScreen()
        {
            Form.Triangles.Clear();
            Form.Lines.Clear();
            Ids.Clear();
        }
        int counter = 1;
        public void DrawLine(int x1, int y1, int x2, int y2, EngineColor color)
        {
            var newLine = new _Line(new PointF(x1, y1), new PointF(x2, y2), Color.FromArgb(color.A, color.R, color.G, color.B), counter++);

            if (!Ids.Any(id => id == newLine.Id))
            {
                Form.Lines.Add(newLine);
                Ids.Add(newLine.Id);
            }
        }

        public void FillTriangle(int x1, int y1, int x2, int y2, int x3, int y3, EngineColor color)
        {
            Form.Triangles.Add(new _Triangle(new PointF(x1, y1), new PointF(x2, y2), new PointF(x3, y3), Color.FromArgb(color.A, color.R, color.G, color.B)));
        }

        public void Init()
        {
            DrawThread = new Task(() => Application.Run(Form));

            DrawThread.Start();
        }

        public void Render()
        {
            Form.Invalidate();
        }

        public bool IsClosed() => DrawThread.IsCanceled || DrawThread.IsCompleted;
    }

    public class FormConsole : Form
    {
        public List<_Triangle> Triangles { get; set; }
        public List<_Line> Lines { get; set; }

        public FormConsole(int w, int h)
        {
            Size = new Size(w, h);
            StartPosition = FormStartPosition.CenterScreen;

            Triangles = new List<_Triangle>();
            Lines = new List<_Line>();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighSpeed;

            g.Clear(Color.Black);

            var triangles = new _Triangle[Triangles.Count()];
            Triangles.CopyTo(triangles);

            var lines = new _Line[Lines.Count()];
            Lines.CopyTo(lines);

            foreach (var t in triangles)
            {
                g.FillPolygon(
                    brush: new SolidBrush(t.Color),
                    points: new PointF[] { t.P1, t.P2, t.P3 }
                );

                //g.DrawString(counter.ToString(), new Font("arial", 15), new SolidBrush(Color.Red), t.P1);

                //counter++;
            }

            foreach (var line in lines)
            {
                g.DrawLine(
                    pen: new Pen(line.Color, 1),
                    pt1: line.P1,
                    pt2: line.P2);
            }

            //Parallel.ForEach(lines, line => {
            //    g.DrawLine(
            //        pen: pen,//new Pen(line.Color, 1),
            //        pt1: line.P1,
            //        pt2: line.P2);
            //});

            base.OnPaint(e);
        }
    }

}
