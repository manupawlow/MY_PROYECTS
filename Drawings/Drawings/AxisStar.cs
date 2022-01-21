using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Drawings.Drawings
{
    public class AxisStar : Draw
    {
        int FRAME_RATE = 500;

        const int INIT_WIDTH = 700;
        const int INIT_HEIGHT = 700;

        private bool Ready { get; set; } = false;

        public AxisStar()
        {
            this.Size = new Size(INIT_WIDTH, INIT_HEIGHT);
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        int Axis = 5;
        int PointsInAxis = 5;

        Queue<List<(PointF, PointF)>> LinesBuffer;

        public override void MainLogic()
        {
            LinesBuffer = new Queue<List<(PointF, PointF)>>();

            while (true)
            {
                while (LinesBuffer.Count() > 2)
                    System.Threading.Thread.Sleep(FRAME_RATE);

                var angleBetweenAxis = 360 / Axis * (Math.PI / 180.0);
                var Lines = new List<(PointF, PointF)>();

                //DRAWING CUADRANTS
                for (int ax = 1; ax <= Axis; ax++)
                {
                    var angle = angleBetweenAxis * (ax - 1);

                    for (int i = 1; i <= PointsInAxis; i++)
                    {
                        var r1 = i;
                        var r2 = PointsInAxis - i + 1;

                        var x1 = (float)(r1 * Math.Cos(angle));
                        var y1 = (float)(r1 * Math.Sin(angle));

                        var x2 = (float)(r2 * Math.Cos(angle + angleBetweenAxis));
                        var y2 = (float)(r2 * Math.Sin(angle + angleBetweenAxis));

                        x1 = Math.Abs(x1) < 0.1 ? 0 : x1;
                        y1 = Math.Abs(y1) < 0.1 ? 0 : y1;
                        x2 = Math.Abs(x2) < 0.1 ? 0 : x2;
                        y2 = Math.Abs(y2) < 0.1 ? 0 : y2;

                        var point1 = new PointF(x1, y1);
                        var point2 = new PointF(x2, y2);

                        Lines.Add((point1, point2));
                    }
                }

                LinesBuffer.Enqueue(Lines);

                Ready = true;

                PointsInAxis++;
                //Axis++;
                
                this.Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;

            g.FillRectangle(Brushes.Black, 0, 0, this.Width, this.Height);

            if (LinesBuffer.Count() > 1)
            {
                var origin = new Point(this.Width / 2, this.Height / 2);

                var pointWidth = this.Width / (PointsInAxis * 2);
                var pointHeight = this.Height / (PointsInAxis * 2);

                LinesBuffer.Dequeue().ForEach(l =>
                {
                    var p1 = ScalePoint(l.Item1, origin, pointWidth, pointHeight);
                    var p2 = ScalePoint(l.Item2, origin, pointWidth, pointHeight);

                    g.DrawLine(
                        pen: new Pen(Color.White, 1),
                        pt1: p1,
                        pt2: p2);
                });
            }

            base.OnPaint(e);
        }

        private PointF ScalePoint(PointF p, PointF origin, int pointWidth, int pointHeight) => new PointF(p.X * pointWidth + origin.X, p.Y * pointHeight + origin.Y);
    }
}
